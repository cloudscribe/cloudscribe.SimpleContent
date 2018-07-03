// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Created:					2018-06-20
// Last Modified:			2018-07-03
// 

using System;

namespace cloudscribe.SimpleContent.Models
{
    public class ContentHistory
    {
        public ContentHistory()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; } 

        public string ContentId { get; set; } 

        public string ProjectId { get; set; }

        public string ContentSource { get; set; } = "Page";
        public string ContentType { get; set; } = "html";

        public string Slug { get; set; }

        public bool IsDraftHx { get; set; }
        public bool WasDeleted { get; set; }
        public DateTime ArchivedUtc { get; set; } = DateTime.UtcNow;
        public string ArchivedBy { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; }
        public string CorrelationKey { get; set; }

        public string Content { get; set; }

        public DateTime? PubDate { get; set; }

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string CreatedByUser { get; set; }
        public string LastModifiedByUser { get; set; }
        public string DraftContent { get; set; }
        public string DraftAuthor { get; set; }
        public DateTime? DraftPubDate { get; set; }

        public string SerializedModel { get; set; }
        public string DraftSerializedModel { get; set; }
        
        //blog only

        public string TeaserOverride { get; set; }



    }
}
