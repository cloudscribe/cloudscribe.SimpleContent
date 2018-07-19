using System;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentItem
    {
        string Id { get; set; }
        string ContentType { get; set; }
        string Slug { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        DateTime? PubDate { get; set; }
        string MetaDescription { get; set; }

    }
}
