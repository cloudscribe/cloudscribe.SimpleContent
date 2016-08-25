using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public class Page
    {
        public Page()
        {
            Categories = new List<string>();
            Comments = new List<Comment>();
        }

        public string Id { get; set; } = string.Empty;

        public string ProjectId { get; set; } = string.Empty;

        public string ParentId { get; set; } = string.Empty;

        public string ParentTitle { get; set; } = string.Empty;

        public int PageOrder { get; set; } = 3;

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

        public string ViewRoles { get; set; } = string.Empty;

        public List<string> Categories { get; set; }
        public List<Comment> Comments { get; set; }

        public bool ShowHeading { get; set; } = true;
        public bool ShowPubDate { get; set; } = false;
        public bool ShowLastModified { get; set; } = false;
        public bool ShowCategories { get; set; } = false;
        public bool ShowComments { get; set; } = false;
    }
}
