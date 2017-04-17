using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageResource
    {
        string Id { get; set; }
        /// <summary>
        /// the page id
        /// </summary>
        string ContentId { get; set; } 
        /// <summary>
        /// css or js
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// any, dev, or prod
        /// </summary>
        string Environment { get; set; }
        string Url { get; set; }
    }

    public class PageResource : IPageResource
    {
        public string Id { get; set; }
        public string ContentId { get; set; }

        public string Type { get; set; }
        public string Environment { get; set; }
        public string Url { get; set; }
    }
}
