
//console.log('hello world');
//class to represent a list item
function ImageItem(title, description, fullSizeUrl, resizedUrl, thumbnailUrl, linkUrl, sort) {
    var self = this;

    self.Title = ko.observable(title);
    self.Description = ko.observable(description);
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

    self.test = function () {
        self.addItem('Fungus', 'Fungus amoung us', '/media/images/img_1150.jpg', '/media/images/img_1150-ws.jpg', '', '', 9);
    }


}

var initialData = JSON.parse(decodeURIComponent(document.getElementById("ItemsInitialData").value));
//console.log(initialData);

ko.applyBindings(new ItemListViewModel(initialData));