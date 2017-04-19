using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class AddPageResourceViewModel
    {
        public string Slug { get; set; }
        public int Sort { get; set; }
        /// <summary>
        /// css or js
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// any, dev, or prod
        /// </summary>
        public string Environment { get; set; }
        [Required(ErrorMessage ="Url is required")]
        //[DataType(DataType.Url)]
        [RegularExpression(@"^(http(s)?://)?(\/?)[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-‌​\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage ="Please provide a valid url or relative url")]
        public string Url { get; set; }
    }
}
