using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Url { get; set; }
    }
}
