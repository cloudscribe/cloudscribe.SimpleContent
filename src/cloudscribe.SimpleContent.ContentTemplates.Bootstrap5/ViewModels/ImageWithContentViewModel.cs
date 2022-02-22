namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class ImageWithContentViewModel
    {
        public string Content { get; set; }
        public string ResizedUrl { get; set; }
        public string FullSizeUrl { get; set; }
        public string AltText { get; set; }
        public string Layout { get; set; } = "ImageOnLeft"; // or ImageOnRight
    }
}
