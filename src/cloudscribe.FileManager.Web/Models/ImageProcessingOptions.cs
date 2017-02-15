namespace cloudscribe.FileManager.Web.Models
{
    public class ImageProcessingOptions
    {
        public string ImageOriginalSizeVirtualSubPath { get; set; } = "/media/images/origsize/";

        public string ImageWebSizeVirtualSubPath { get; set; } = "/media/images/websize/";

        public string ImageThumbnailVirtualSubPath { get; set; } = "/media/images/thumbnails/";

        public bool AutoResize { get; set; } = true;

        public bool KeepOriginalImages { get; set; } = true;

        public int ResizeImageMaxWidth { get; set; } = 550;

        public int ResizeImageMaxHeight { get; set; } = 550;
    }
}
