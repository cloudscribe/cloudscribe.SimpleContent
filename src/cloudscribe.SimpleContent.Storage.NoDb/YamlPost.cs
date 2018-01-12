﻿using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
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

        [YamlMember(Order = 1)]
        public string Id { get; set; }

        [YamlMember(Order = 2)]
        public string BlogId { get; set; }

        [YamlMember(Order = 3)]
        public string CorrelationKey { get; set; } = string.Empty;

        [YamlMember(Order = 4)]
        public string Slug { get; set; }

        [YamlMember(Order = 5)]
        public string ContentType { get; set; } = "html";

        [YamlMember(Order = 6)]
        public string Title { get; set; }

        [YamlMember(Order = 7)]
        public string Author { get; set; }
        
        [YamlMember(Order = 8)]
        public string MetaDescription { get; set; }

        [YamlMember(Order = 9)]
        public string MetaJson { get; set; }
        [YamlMember(Order = 10)]
        public string MetaHtml { get; set; }
        
        [YamlMember(Order = 11)]
        public bool IsPublished { get; set; }

        [YamlMember(Order = 12)]
        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        [YamlMember(Order = 13)]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        [YamlMember(Order = 14)]
        public bool IsFeatured { get; set; }

        [YamlMember(Order = 15)]
        public List<string> Categories { get; set; }

        [YamlMember(Order = 16, Alias = "Comments")]
        public List<Comment> TheComments { get; set; }

        [YamlMember(Order = 17, Alias = "Comments")]
        public string TeaserOverride { get; set; }
        [YamlMember(Order = 18, Alias = "Comments")]
        public bool SuppressAutoTeaser { get; set; }




        [YamlIgnore]
        public List<IComment> Comments { get; set; }

        [YamlIgnore]
        public string Content { get; set; }

        // not currently used but could be later
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
