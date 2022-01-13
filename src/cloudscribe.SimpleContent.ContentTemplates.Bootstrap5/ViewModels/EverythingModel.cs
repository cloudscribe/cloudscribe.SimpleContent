using cloudscribe.SimpleContent.ContentTemplates.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class EverythingModel
    {
        public EverythingModel()
        {
            Items = new List<ListItemModel>();
        }

        public List<ListItemModel> Items { get; set; }

        public string TopSectionsLayout { get; set; } = "EverythingTopColumnsPartial";
        public string BottomSectionsLayout { get; set; } = "EverythingBottomColumnsPartial";

        public string GalleryLayout { get; set; } = "EverythingCardsPartial";

        [Required(ErrorMessage = "Carousel Interval is required.")]
        public int CarouselIntervalInMilliseconds { get; set; } = 5000;

        //top sections

        public string SectionOneHeading { get; set; }
        public string SectionOneContent { get; set; }
        public string SectionOneResizedUrl { get; set; }
        public string SectionOneFullSizeUrl { get; set; }
        public string SectionOneAltText { get; set; }


        public string SectionTwoHeading { get; set; }
        public string SectionTwoContent { get; set; }
        public string SectionTwoResizedUrl { get; set; }
        public string SectionTwoFullSizeUrl { get; set; }
        public string SectionTwoAltText { get; set; }

        public string SectionThreeHeading { get; set; }
        public string SectionThreeContent { get; set; }
        public string SectionThreeResizedUrl { get; set; }
        public string SectionThreeFullSizeUrl { get; set; }
        public string SectionThreeAltText { get; set; }

        public string SectionFourHeading { get; set; }
        public string SectionFourContent { get; set; }
        public string SectionFourResizedUrl { get; set; }
        public string SectionFourFullSizeUrl { get; set; }
        public string SectionFourAltText { get; set; }

        //bottom sections

        public string SectionFiveHeading { get; set; }
        public string SectionFiveContent { get; set; }
        public string SectionFiveResizedUrl { get; set; }
        public string SectionFiveFullSizeUrl { get; set; }
        public string SectionFiveAltText { get; set; }


        public string SectionSixHeading { get; set; }
        public string SectionSixContent { get; set; }
        public string SectionSixResizedUrl { get; set; }
        public string SectionSixFullSizeUrl { get; set; }
        public string SectionSixAltText { get; set; }

        public string SectionSevenHeading { get; set; }
        public string SectionSevenContent { get; set; }
        public string SectionSevenResizedUrl { get; set; }
        public string SectionSevenFullSizeUrl { get; set; }
        public string SectionSevenAltText { get; set; }

        public string SectionEightHeading { get; set; }
        public string SectionEightContent { get; set; }
        public string SectionEightResizedUrl { get; set; }
        public string SectionEightFullSizeUrl { get; set; }
        public string SectionEightAltText { get; set; }

    }
}
