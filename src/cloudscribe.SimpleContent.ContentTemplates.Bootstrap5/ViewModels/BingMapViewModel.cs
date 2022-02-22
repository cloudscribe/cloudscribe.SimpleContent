using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class BingMapViewModel
    {
        [Required(ErrorMessage = "You must provide a Bing Map API key")]
        public string ApiKey { get; set; }

        [Required(ErrorMessage = "You must provide a location name")]
        public string LocationName { get; set; }

        [Required(ErrorMessage = "You must provide a location address")]
        public string LocationAddress { get; set; }

        public string ContentAbove { get; set; }
        public string ContentBelow { get; set; }

    }
}
