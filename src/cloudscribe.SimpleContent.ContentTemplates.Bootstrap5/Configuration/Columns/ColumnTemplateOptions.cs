namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public class ColumnTemplateOptions
    {
        /// <summary>
        /// must be a folder starting with /media
        /// </summary>
        public string NewImagePath { get; set; } = "/media/images";

        /// <summary>
        /// thumbnails are not needed for the gallery and just use extra disk space.
        /// The gallery uses the same images for both thumbnail and full view
        /// </summary>
        public bool CreateThumbnails { get; set; } = false;

        /// <summary>
        /// the default value 1500 is recommended, even if the image is rendered smaller this ensures it looks good on retina displays
        /// The resisizing still makes the image file sizes significantly smaller.
        /// </summary>
        public int ResizeMaxWidth { get; set; } = 1500;

        /// <summary>
        /// the default value 1500 is recommended, even if the image is rendered smaller this ensures it looks good on retina displays
        /// The resisizing still makes the image file sizes significantly smaller.
        /// </summary>
        public int ResizeMaxHeight { get; set; } = 1500;

        /// <summary>
        /// original size images from modern cameras are quite large and not useful on the web so this defaults to false
        /// </summary>
        public bool KeepOriginalSizeImages { get; set; } = false;

        /// <summary>
        /// ability to browse files on the server is still subject to permissions of the user and file browse policy
        /// this just controls visibility of the button for browsing the server
        /// </summary>
        public bool EnableBrowseServer { get; set; } = true;

        /// <summary>
        /// allow the user to crop newly dropped/uploaded images
        /// </summary>
        public bool EnableCropping { get; set; } = true;

        /// <summary>
        /// in pixels
        /// </summary>
        public int CropAreaWidth { get; set; } = 690;

        /// <summary>
        /// in pixels
        /// </summary>
        public int CropAreaHeight { get; set; } = 517;

        /// <summary>
        /// the image shown before user uploads or selects a file from the server
        /// </summary>
        public string PlaceholderImageUrl { get; set; } = "/cr/images/690x517-placeholder.png";
    }
}
