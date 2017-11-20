using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet;
using YamlDotNet.Serialization;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class YamlPost : IPost
    {
        public YamlPost()
        {
            Categories = new List<string>();
            Comments = new List<IComment>();
            TheComments = new List<Comment>();
        }

        //[YamlMember()]
        public string Id { get; set; }

        public string BlogId { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// This field is a place to store a surrogate key if needed.
        /// For example in a multi-tenant multi-lanaguage setup, it could be used
        /// to correlate posts between the different language sites
        /// to implement a language switcher. ie the corresponding post in the other
        /// site could be found by looking it up by the correlationkey
        /// </summary>
        public string CorrelationKey { get; set; } = string.Empty;

        public string Author { get; set; }

        public string Slug { get; set; }

        public string MetaDescription { get; set; }
        public string MetaJson { get; set; }
        public string MetaHtml { get; set; }

        [YamlIgnore]
        public string Content { get; set; }

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }

        public List<string> Categories { get; set; }

        //[YamlMember(serializeAs:typeof(List<Comment>))]
        [YamlIgnore]
        public List<IComment> Comments { get; set; }

        public List<Comment> TheComments { get; set; }
        public bool IsFeatured { get; set; }

        // not currently used but could be later
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        public string ContentType { get; set; } = "html";

    }
}
