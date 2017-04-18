using cloudscribe.SimpleContent.Models;
using System;

namespace cloudscribe.SimpleContent.Storage.EFCore.Models
{
    public class PageResourceEntity : IPageResource
    {
        public PageResourceEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string ContentId
        {
            get { return pageEntityId; }
            set { pageEntityId = value; }
        }

        // not part of IPageResource
        private string pageEntityId;
        public string PageEntityId
        {
            get { return pageEntityId; }
            set { pageEntityId = value; }
        }

        public int Sort { get; set; } = 1;

        public string Type { get; set; }
        public string Environment { get; set; }
        public string Url { get; set; }

        public static PageResourceEntity FromIPageResource(IPageResource r)
        {
            var resource = new PageResourceEntity();
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
