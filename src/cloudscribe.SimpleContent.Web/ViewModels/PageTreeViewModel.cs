using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PageTreeViewModel
    {
        //public string PageViewUrlTemplate { get; set; }
        //public string PageEditUrlTemplate { get; set; }

        public string TreeServiceUrl { get; set; }
        public string SortUrl { get; set; }
        public string MoveUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string ViewUrl { get; set; }
        public string EditUrl { get; set; }
    }
}
