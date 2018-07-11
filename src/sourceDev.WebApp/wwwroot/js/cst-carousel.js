
//console.log('hello world');
//class to represent an image
function ImageItem(sizedUrl, fullSizeUrl, caption, description, sort) {
    var self = this;
    self.SizedImageUrl = ko.observable(sizedUrl);
    self.FullSizeImageUrl = ko.observable(fullSizeUrl);
    self.Caption = ko.observable(caption);
    self.Description = ko.observable(description);
    self.Sort = ko.observable(sort);
}
function CarouselViewModel(initialData) {
    var self = this;
    self.hiddenField = document.getElementById("ItemsJson");
    
    self.Items = ko.observableArray(ko.utils.arrayMap(initialData, function (item) {
        //console.log(item);
        return new ImageItem(item.SizedImageUrl, item.FullSizeImageUrl,item.Caption, item.Description, item.Sort);
    }));

    self.addItem = function (sizedUrl, fullSizeUrl, caption, description, sort) {
        self.Items.push(new ImageItem(sizedUrl, fullSizeUrl, caption, description, sort))
    }

    self.test = function () {
        self.addItem('/media/images/img_1150-ws.jpg', '/media/images/img_1150.jpg', 'Fungus', 'Fungus amoung us', 9);
    }


}

var initialData = JSON.parse(decodeURIComponent(document.getElementById("ItemsJson").value));
//console.log(initialData);

ko.applyBindings(new CarouselViewModel(initialData));