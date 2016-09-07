using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPost
    {
        string Author { get; set; }
        string BlogId { get; set; }
        List<string> Categories { get; set; }
        List<Comment> Comments { get; set; }
        string Content { get; set; }
        string Id { get; set; }
        bool IsPublished { get; set; }
        DateTime LastModified { get; set; }
        string MetaDescription { get; set; }
        DateTime PubDate { get; set; }
        string Slug { get; set; }
        string Title { get; set; }
    }
}