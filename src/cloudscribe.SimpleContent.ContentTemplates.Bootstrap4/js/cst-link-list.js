
ko.observable.fn.silentUpdate = function (value) {
    this.notifySubscribers = function () { };
    this(value);
    this.notifySubscribers = function () {
        ko.subscribable.fn.notifySubscribers.apply(this, arguments);
    };
};

// http://knockoutjs.com/documentation/extenders.html
ko.extenders.required = function (target, overrideMessage) {
    target.hasError = ko.observable();
    target.validationMessage = ko.observable();
    function validate(newValue) {
        target.hasError(newValue ? false : true);
        target.validationMessage(newValue ? "" : overrideMessage || "This field is required");
    }
    validate(target());
    target.subscribe(validate);
    return target;
};

ko.extenders.isValidUrl = function(target, overrideMessage) {
    target.hasError = ko.observable();
    target.validationMessage = ko.observable();
    var _testEle = null;
    function validate(newValue) {
        if (newValue) {

            if (!self._testEle) {
                _testEle = document.createElement('input');
                _testEle.setAttribute('type', 'url');
            }
            _testEle.value = newValue;
            var isValid = _testEle.validity.valid;
            if (isValid) {
                target.hasError(false);
                target.validationMessage("");
            }
            else {
                target.hasError(true);
                target.validationMessage("Must be a valid url");
            }
        }
        else {
            target.hasError(newValue ? false : true);
            target.validationMessage(newValue ? "" : "Url is required");
        }  
    }
    validate(target());
    target.subscribe(validate);
    return target;
};


//class to represent a list item
function ListItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort) {
    var self = this;

    self.Title = ko.observable(decodeEncodedJson(title)).extend({ required: "Title is required" });
    self.Description = ko.observable(decodeEncodedJson(description));
    self.FullSizeUrl = ko.observable(fullSizeUrl);
    self.ResizedUrl = ko.observable(resizedUrl);
    self.ThumbnailUrl = ko.observable(thumbnailUrl);
    self.LinkUrl = ko.observable(linkUrl).extend({ required: "Link Url is required", isValidUrl: "Must be a valid url" });
    self.Sort = ko.observable(sort);

    self.incrementSort = function () {
        self.Sort(self.Sort() + 3);
    }
    self.decrementSort = function () {
        var newSort = self.Sort() - 3;
        if (newSort < 0) { newSort = 0; }
        self.Sort(newSort);
    }
    self._testEle = null;

    self.isValidUrl = function (url) {
        if (!self._testEle) {
            self._testEle = document.createElement('input');
            self._testEle.setAttribute('type', 'url');
        }
        self._testEle.value = url;
        var result = self._testEle.validity.valid;
        return result;
    }
}

function ItemListViewModel(initialData) {
    var self = this;
    self.hiddenField = document.getElementById("ItemsJson");

    self.handleSortItemChanged = function (sortVal) {
        self.sortItems(); 
        var sort = 1;
        for (i = 0; i < self.Items().length; i++) {
            var item = self.Items()[i];
            item.Sort.silentUpdate(sort); //avoid infinite loop of event by silent update
            sort += 2;
           
        }
        self.sortItems(); 
    }
    
    self.Items = ko.observableArray(ko.utils.arrayMap(initialData, function (item) {
        var item = new ListItem(item.Title, item.Description, item.FullSizeUrl, item.ResizedUrl, item.ThumbnailUrl, item.LinkUrl, item.Sort);
        item.Sort.subscribe(self.handleSortItemChanged);
        return item;
    }));
    
    self.addItem = function (title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort) {
        var item = new ListItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort);
        item.Sort.subscribe(self.handleSortItemChanged);
        self.Items.push(item)
    }

    self.newItemTitle = ko.observable(null).extend({ required: "Title is required" });
    self.newItemDescription = ko.observable(null);
    self.newItemFullSizeUrl = ko.observable(null);
    self.newItemResizedUrl = ko.observable(null);
    self.newItemThumbnailUrl = ko.observable(null);
    self.newItemLinkUrl = ko.observable(null).extend({ isValidUrl: null });

    self.newItemSort = function () {
        if (self.Items().length === 0) { return 1; }
        var result = Math.max.apply(Math, self.Items().map(function (o) { return o.Sort(); }))
        return result + 2;
    }
   
    self.addNewItem = function () {
        self.addItem(self.newItemTitle(), self.newItemDescription(), self.newItemFullSizeUrl(), self.newItemResizedUrl(), self.newItemThumbnailUrl(), self.newItemLinkUrl(), self.newItemSort());
        self.newItemTitle(null);
        self.newItemDescription(null);
        self.newItemFullSizeUrl(null);
        self.newItemResizedUrl(null);
        self.newItemThumbnailUrl(null);
        self.newItemLinkUrl(null);
        window.cloudscribeDropAndCrop.clearAllItems();
    }

    self.removeItem = function (item) { self.Items.remove(item) }

    self.getCssClass = function (index) {
        if (index === 0) { return "carousel-item active"; }
        return "carousel-item";
    }
    
    self.dropZoneSuccess = function (file, serverResponse) {
        //console.log(serverResponse);
        self.newItemFullSizeUrl(serverResponse[0].originalUrl);
        self.newItemResizedUrl(serverResponse[0].resizedUrl);
        self.newItemThumbnailUrl(serverResponse[0].thumbUrl)
    }

    window.DropZoneSuccessHandler = self.dropZoneSuccess;

    self.handleCropSave = function (resizedUrl) {
        self.newItemResizedUrl(resizedUrl);
    }
    window.HandleCropResult = self.handleCropSave;

    self.serverFileSelected = function (url) {
        self.newItemResizedUrl(url);
    }
    window.ServerFileSelected = self.serverFileSelected;

    self.filteredItems = ko.computed(function () {
        return ko.utils.arrayFilter(self.Items(), function (item) {
            return item.LinkUrl.hasError() == false && item.Title.hasError() == false;
        });
    });
    
    self.currentListState = ko.computed(function () {
        return encodeURIComponent(ko.toJSON(self.filteredItems()));
    });
    self.sortItems = function () {
        self.Items.sort(function (a, b) {
            if (a.Sort() < b.Sort()) { return -1; }
            if (a.Sort() > b.Sort()) { return 1; }
            return 0;
        })
    }

    self._testEle = null;

    self.isValidNewItemLink = ko.computed(function() {
        if (self.newItemTitle.hasError()) { return false; }
        if (self.newItemLinkUrl.hasError()) { return false; }
        return true;
    });
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
