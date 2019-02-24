using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class SimpleGalleryViewModel : ListWithContentModel
    {
        public string Layout { get; set; } = "GalleryWithContentRenderCardsPartial";

        [Required(ErrorMessage = "Carousel Interval is required.")]
        public int CarouselIntervalInMilliseconds { get; set; } = 5000;
    }
}
