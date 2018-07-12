
//console.log('hello world');
//class to represent a list item
function ImageItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort) {
    var self = this;

    self.Title = ko.observable(decodeEncodedJson(title));
    self.Description = ko.observable(decodeEncodedJson(description));
    self.FullSizeUrl = ko.observable(fullSizeUrl);
    self.ResizedUrl = ko.observable(resizedUrl);
    self.ThumbnailUrl = ko.observable(thumbnailUrl);
    self.LinkUrl = ko.observable(linkUrl);
    self.Sort = ko.observable(sort);
    
}
function ItemListViewModel(initialData) {
    var self = this;
    self.hiddenField = document.getElementById("ItemsJson");
    
    self.Items = ko.observableArray(ko.utils.arrayMap(initialData, function (item) {
        //console.log(item);
        return new ImageItem(item.Title, item.Description, item.ResizedUrl, item.FullSizeUrl, item.ThumbnailUrl, item.LinkUrl, item.Sort);
    }));

    self.addItem = function (title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort) {
        self.Items.push(new ImageItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort))
    }

    self.removeItem = function (item) { self.Items.remove(item) }

    self.test = function () {
        self.addItem('Fungus', 'Fungus amoung us', '/media/images/img_1150.jpg', '/media/images/img_1150-ws.jpg', '', '', 9);
    }

    self.currentListState = ko.computed(function () {
        return encodeURIComponent(ko.toJSON(self.Items));
    });
    self.sortItems = function () {
        self.Items.sort(function (a, b) {
            if (a.Sort < b.Sort) { return -1; }
            if (a.Sort > b.Sort) { return 1; }
            return 0;
        })
    }

    self.sortedItems = ko.computed(function () {
        return self.Items.sort(function (a, b) {
            if (a.Sort < b.Sort) { return -1; }
            if (a.Sort > b.Sort) { return 1; }
            return 0;
        })
    });
   


}

function decodeEncodedJson(encodedJson) {
    var x = encodedJson.replace(/\+/g, " ");
    return decodeURIComponent(x);
}

//var initialData = JSON.parse(decodeURIComponent(document.getElementById("ItemsInitialData").value));
var initialData = JSON.parse(decodeEncodedJson(document.getElementById("ItemsInitialData").value));
//console.log(initialData);

ko.applyBindings(new ItemListViewModel(initialData));