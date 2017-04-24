using System;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageResource
    {
        string Id { get; set; }
        /// <summary>
        /// the page id
        /// </summary>
        string ContentId { get; set; } 
        int Sort { get; set; }
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
        public PageResource()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string ContentId { get; set; }
        public int Sort { get; set; } = 1;

        public string Type { get; set; }
        public string Environment { get; set; }
        public string Url { get; set; }

        public static PageResource FromIPageResource(IPageResource r)
        {
            var resource = new PageResource();
            resource.ContentId = r.ContentId;
            resource.Environment = r.Environment;
            resource.Id = r.Id;
            resource.Sort = r.Sort;
            resource.Type = r.Type;
            resource.Url = r.Url;

            return resource;

        }
    }
}
