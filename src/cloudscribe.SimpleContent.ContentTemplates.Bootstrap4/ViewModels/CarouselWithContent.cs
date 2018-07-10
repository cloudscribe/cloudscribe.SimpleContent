using System.Collections.Generic;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class CarouselWithContent
    {
        public CarouselWithContent()
        {
            Items = new List<CarouselItem>();
        }

        public string ContentAbove { get; set; }
        public string ContentBelow { get; set; }
        public List<CarouselItem> Items { get; set; }

        

    }

    public class CarouselItem
    {
        public string SizedImageUrl { get; set; }
        public string FullSizeImageUrl { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }

        public int Sort { get; set; } = 3;
    }
}
