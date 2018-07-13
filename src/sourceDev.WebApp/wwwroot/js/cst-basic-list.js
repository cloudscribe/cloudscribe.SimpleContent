
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

    self.handleSortItemChanged = function (sortVal) {
        console.log(sortVal);
        self.sortItems();
        //ko.cleanNode(document.getElementById("itemsEditList"))
        //ko.applyBindings(self.Items, document.getElementById("itemsEditList"))

        //self.Items.valueHasMutated();
        //self.sortItems();
        //var tmp = self.Items();
        //self.Items([]);
        //tmp.sort(function (a, b) {
        //    if (a.Sort < b.Sort) { return -1; }
        //    if (a.Sort > b.Sort) { return 1; }
        //    return 0;
        //});
        //self.Items(tmp);

    }
    
    self.Items = ko.observableArray(ko.utils.arrayMap(initialData, function (item) {
        //console.log(item);
        var item = new ImageItem(item.Title, item.Description, item.ResizedUrl, item.FullSizeUrl, item.ThumbnailUrl, item.LinkUrl, item.Sort);
        item.Sort.subscribe(self.handleSortItemChanged);
        return item;
    }));

    self.addItem = function (title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort) {
        var item = new ImageItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort);
        item.Sort.subscribe(self.handleSortItemChanged);
        self.Items.push(item)
    }

    self.newItemFullSizeUrl = ko.observable(null);
    self.newItemResizedUrl = ko.observable(null);
    self.newItemThumbnailUrl = ko.observable(null);
    //self.hasNewItem = ko.computed(function () {
    //    return document.getElementById("imgUrlResized").value.length > 0;
    //});

    self.addNewItem = function () {
        self.addItem('', '', self.newItemFullSizeUrl(), self.newItemResizedUrl(), self.newItemThumbnailUrl())
    }

    self.removeItem = function (item) { self.Items.remove(item) }

    self.test = function () {
        self.addItem('Fungus', 'Fungus amoung us', '/media/images/img_1150.jpg', '/media/images/img_1150-ws.jpg', '', '', 9);
    }

    self.test2 = function () {
        console.log(self.newItemResizedUrl());
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