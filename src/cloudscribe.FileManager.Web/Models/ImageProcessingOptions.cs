namespace cloudscribe.FileManager.Web.Models
{
    public class ImageProcessingOptions
    {
        public string ImageDefaultVirtualSubPath { get; set; } = "/media/images";

        //public string ImageWebSizeVirtualSubPath { get; set; } = "/media/images/websize";

        //public string ImageThumbnailVirtualSubPath { get; set; } = "/media/images/thumbnails";

        public bool AutoResize { get; set; } = true;

        public bool KeepOriginalImages { get; set; } = true;

        public int WebSizeImageMaxWidth { get; set; } = 550;

        public int WebSizeImageMaxHeight { get; set; } = 550;

        //public int ThumbnailImageMaxWidth { get; set; } = 75;

        //public int ThumbnailImageMaxHeight { get; set; } = 75;
    }
}
