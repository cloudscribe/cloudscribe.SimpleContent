using System.Collections.Generic;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class CarouselWithContent
    {
        public CarouselWithContent()
        {
            Items = new List<CarouselItem>();

            //Items.Add(new CarouselItem()
            //{
            //    SizedImageUrl = "/media/images/dudes1-550x412.jpeg",
            //    FullSizeImageUrl = "/media/images/dudes1.jpeg",
            //    Caption = "Starsky and Hutch!",
            //    Description = "Some dudes on bikes in Mexico"
            //}
            //);
            
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
