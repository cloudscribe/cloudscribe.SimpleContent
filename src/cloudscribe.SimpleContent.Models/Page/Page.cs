using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class Page : IPage
    {
        public Page()
        {
            Categories = new List<string>();
            Comments = new List<IComment>();
            Resources = new List<PageResource>();
        }

        public string Id { get; set; } = string.Empty;

        public string ProjectId { get; set; } = string.Empty;

        public string ParentId { get; set; } = string.Empty;

        public string ParentSlug { get; set; } = string.Empty;

        public int PageOrder { get; set; } = 3;

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// this is to facilitate adding menu items that link to external sites or urls
        /// </summary>
        public string ExternalUrl { get; set; } = string.Empty;

        /// <summary>
        /// This field is a place to store a surrogate key if needed.
        /// For example in a multi-tenant multi-lanaguage setup, it could be used
        /// to correlate pages between the different language sites
        /// to implement a language switcher. ie the corresponding page in the other
        /// site could be found by looking it up by the correlationkey
        /// </summary>
        public string CorrelationKey { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;
        public string MetaJson { get; set; }
        public string MetaHtml { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime? PubDate { get; set; } 

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } 

        public bool MenuOnly { get; set; } = false;

        public bool ShowMenu { get; set; } = false;

        public string ViewRoles { get; set; } = string.Empty;

        public List<string> Categories { get; set; }
        public List<IComment> Comments { get; set; }

        public bool ShowHeading { get; set; } = true;
        public bool ShowPubDate { get; set; } = false;
        public bool ShowLastModified { get; set; } = false;
        public bool ShowCategories { get; set; } = false;
        public bool ShowComments { get; set; } = false;

        public string MenuFilters { get; set; }

        public bool DisableEditor { get; set; } = false;

        public List<PageResource> Resources { get; set; }

        public string ContentType { get; set; } = "html";

        // new fields 2018-06-20
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string CreatedByUser { get; set; }
        public string LastModifiedByUser { get; set; }
        public string DraftContent { get; set; }
        public string DraftAuthor { get; set; }
        public DateTime? DraftPubDate { get; set; }

        public string TemplateKey { get; set; }
        public string SerializedModel { get; set; }
        public string DraftSerializedModel { get; set; }
        public string Serializer { get; set; }

        public static Page FromIPage(IPage page)
        {
            var p = new Page();
            page.CopyTo(p);
            return p;
        }

        public bool? ShowCreatedBy { get; set; }
        public bool? ShowCreatedDate { get; set; }
        public bool? ShowLastModifiedBy { get; set; }
        public bool? ShowLastModifiedDate { get; set; }
    }
}
