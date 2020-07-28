// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					
// Last Modified:			2018-08-20
// 
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class Post : IPost
    {
        public Post()
        {
            Categories = new List<string>();
            Comments = new List<IComment>();
        }

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

        public string Content { get; set; }

        public DateTime? PubDate { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }
        
        public List<string> Categories { get; set; }
        public List<IComment> Comments { get; set; }
        public bool IsFeatured { get; set; }

        // not currently used but could be later
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        public string ContentType { get; set; } = "html";
        public bool ShowComments { get; set; } = false;

        /// <summary>
        /// If not null or whitespace, displays this teaser on blog index/listing views regardless of <see cref="TeaserMode"/> settings.
        /// </summary>
        public string TeaserOverride { get; set; }

        /// <summary>
        /// a place to store the generated teaser so it doens'yt have to be generated on every request
        /// </summary>
        public string AutoTeaser { get; set; }

        /// <summary>
        /// If true, will display an entire blog post on index/listing views regardless of <see cref="TeaserMode"/> settings.
        /// </summary>
        public bool SuppressTeaser { get; set; }


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

        public static Post FromIPost(IPost post)
        {
            var p = new Post();
            post.CopyTo(p);
            return p;
        }

    }
}
