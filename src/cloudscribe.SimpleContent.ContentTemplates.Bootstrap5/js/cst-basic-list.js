
ko.observable.fn.silentUpdate = function (value) {
    this.notifySubscribers = function () { };
    this(value);
    this.notifySubscribers = function () {
        ko.subscribable.fn.notifySubscribers.apply(this, arguments);
    };
};
//class to represent a list item
function ListItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort, altText) {
    var self = this;

    self.Title = ko.observable(decodeEncodedJson(title));
    self.Description = ko.observable(decodeEncodedJson(description));
    self.FullSizeUrl = ko.observable(fullSizeUrl);
    self.ResizedUrl = ko.observable(resizedUrl);
    self.ThumbnailUrl = ko.observable(thumbnailUrl);
    self.LinkUrl = ko.observable(linkUrl);
    self.Sort = ko.observable(sort);
    self.AltText = ko.observable(altText);

    self.incrementSort = function () {
        self.Sort(self.Sort() + 3);
    };

    self.decrementSort = function () {
        var newSort = self.Sort() - 3;
        if (newSort < 0) { newSort = 0; }
        self.Sort(newSort);
    };
}

function ItemListViewModel(initialData) {
    var self = this;
    self.hiddenField = document.getElementById("ItemsJson");

    self.handleSortItemChanged = function (sortVal) {
        self.sortItems();
        var sort = 1;
        for (i = 0; i < self.Items().length; i++) {
            var item = self.Items()[i];
            item.Sort.silentUpdate(sort); //avoid infinite loop of subscription by silent update
            sort += 2;
        }
        self.sortItems();
    };

    self.Items = ko.observableArray(ko.utils.arrayMap(initialData, function (item) {
        var thisItem = new ListItem(item.Title, item.Description, item.FullSizeUrl, item.ResizedUrl, item.ThumbnailUrl, item.LinkUrl, item.Sort, item.AltText);
        thisItem.Sort.subscribe(self.handleSortItemChanged);
        return thisItem;
    }));

    self.addItem = function (title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort, altText) {
        var item = new ListItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort, altText);
        item.Sort.subscribe(self.handleSortItemChanged);
        self.Items.push(item);
        window.thisPage = window.thisPage || {};
        window.thisPage.hasUnsavedChanges = true;
    };

    self.newItemTitle = ko.observable(null);
    self.newItemDescription = ko.observable(null);
    self.newItemFullSizeUrl = ko.observable(null);
    self.newItemResizedUrl = ko.observable(null);
    self.newItemThumbnailUrl = ko.observable(null);
    self.newItemLinkUrl = ko.observable(null);
    self.newItemAltText = ko.observable(null);


    self.newItemSort = function () {
        if (self.Items().length === 0) { return 1; }
        var result = Math.max.apply(Math, self.Items().map(function (o) { return o.Sort(); }));
        return result + 2;
    };

    self.addNewItem = function () {
        //console.log(self.newItemSort());
        self.addItem(self.newItemTitle(), self.newItemDescription(), self.newItemFullSizeUrl(), self.newItemResizedUrl(), self.newItemThumbnailUrl(), self.newItemLinkUrl(), self.newItemSort(), self.newItemAltText());
        self.newItemTitle(null);
        self.newItemDescription(null);
        self.newItemFullSizeUrl(null);
        self.newItemResizedUrl(null);
        self.newItemThumbnailUrl(null);
        self.newItemLinkUrl(null);
        self.newItemAltText(null);
        if (self.imageEditor) {
            window.cloudscribeDropAndCrop.clearOneZoneItems(self.imageEditor.dropZoneDiv.id);
        }

        //window.cloudscribeDropAndCrop.clearAllItems();
    };

    self.removeItem = function (item) {
        self.Items.remove(item);
        window.thisPage = window.thisPage || {};
        window.thisPage.hasUnsavedChanges = true;
    };

    self.getCssClass = function (index) {
        if (index === 0) { return "carousel-item active"; }
        return "carousel-item";
    };

    self.dropZoneSuccess = function (file, serverResponse, imageEditor) {
        //console.log('dropzone success');
        //console.log(serverResponse);
        self.imageEditor = imageEditor;
        self.newItemFullSizeUrl(serverResponse[0].originalUrl);
        if (serverResponse[0].resizedUrl) {
            self.newItemResizedUrl(serverResponse[0].resizedUrl);
        } else {
            self.newItemResizedUrl(serverResponse[0].originalUrl);
        }

        self.newItemThumbnailUrl(serverResponse[0].thumbUrl);
    };

    window.DropZoneSuccessHandler = self.dropZoneSuccess;

    self.handleCropSave = function (resizedUrl) {
        self.newItemResizedUrl(resizedUrl);
    };
    window.HandleCropResult = self.handleCropSave;

    self.serverFileSelected = function (url) {
        //console.log('server file selected ' + url);
        self.newItemResizedUrl(url);
    };
    window.ServerFileSelected = self.serverFileSelected;

    self.currentListState = ko.computed(function () {
        return encodeURIComponent(ko.toJSON(self.Items));
    });
    self.sortItems = function () {
        self.Items.sort(function (a, b) {
            if (a.Sort() < b.Sort()) { return -1; }
            if (a.Sort() > b.Sort()) { return 1; }
            return 0;
        });
    };
}

function decodeEncodedJson(encodedJson) {
    if (encodedJson === null) { return encodedJson; }
    if (encodedJson === undefined) { return encodedJson; }
    var x = encodedJson.replace(/\+/g, " ");
    return decodeURIComponent(x);
}

document.addEventListener("DOMContentLoaded", function () {
    var configElement = document.getElementById("ItemsConfig");
    var initialData = JSON.parse(decodeEncodedJson(configElement.value));
    //console.log(initialData);

    ko.applyBindings(new ItemListViewModel(initialData));
});

