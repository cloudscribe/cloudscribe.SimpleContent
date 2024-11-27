using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class YamlPage : IPage
    {
        public YamlPage()
        {
            Categories = new List<string>();
            Comments = new List<IComment>();
            Resources = new List<PageResource>();
            //TheComments = new List<Comment>();
        }

        [YamlMember(Order = 1)]
        public string Id { get; set; } = string.Empty;

        [YamlMember(Order = 2)]
        public string CorrelationKey { get; set; } = string.Empty;

        [YamlMember(Order = 3)]
        public string ProjectId { get; set; } = string.Empty;

        [YamlMember(Order = 4)]
        public string Slug { get; set; } = string.Empty;

        [YamlMember(Order = 5)]
        public string ExternalUrl { get; set; } = string.Empty;

        [YamlMember(Order = 6)]
        public string ContentType { get; set; } = "html";

        [YamlMember(Order = 7)]
        public string ViewRoles { get; set; } = string.Empty;

        [YamlMember(Order = 8)]
        public string ParentId { get; set; } = string.Empty;

        [YamlMember(Order = 9)]
        public string ParentSlug { get; set; } = string.Empty;

        [YamlMember(Order = 10)]
        public int PageOrder { get; set; } = 3;

        [YamlMember(Order = 11)]
        public string Title { get; set; } = string.Empty;

        [YamlMember(Order = 12)]
        public string Author { get; set; } = string.Empty;

        [YamlMember(Order = 13)]
        public string MetaDescription { get; set; } = string.Empty;

        [YamlMember(Order = 14)]
        public string MetaJson { get; set; }

        [YamlMember(Order = 15)]
        public string MetaHtml { get; set; }

        [YamlMember(Order = 16)]
        public bool IsPublished { get; set; } = true;

        [YamlMember(Order = 17)]
        public DateTime? PubDate { get; set; } = DateTime.UtcNow;

        [YamlMember(Order = 18)]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        [YamlMember(Order = 19)]
        public bool MenuOnly { get; set; } = false;

        [YamlMember(Order = 20)]
        public bool ShowMenu { get; set; } = false;

        [YamlMember(Order = 21)]
        public bool ShowHeading { get; set; } = true;

        [YamlMember(Order = 22)]
        public bool ShowPubDate { get; set; } = false;

        [YamlMember(Order = 23)]
        public bool ShowLastModified { get; set; } = false;

        [YamlMember(Order = 24)]
        public bool ShowCategories { get; set; } = false;

        [YamlMember(Order = 25)]
        public bool ShowComments { get; set; } = false;

        [YamlMember(Order = 26)]
        public string MenuFilters { get; set; }

        [YamlMember(Order = 27)]
        public bool DisableEditor { get; set; } = false;

        [YamlMember(Order = 28)]
        public List<PageResource> Resources { get; set; }

        [YamlMember(Order = 29)]
        public List<string> Categories { get; set; }

        //[YamlMember(Order = 30, Alias = "Comments")]
        //public List<Comment> TheComments { get; set; }


        // new fields 2018-06-20
        [YamlMember(Order = 30)]
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        [YamlMember(Order = 31)]
        public string CreatedByUser { get; set; }

        [YamlMember(Order = 32)]
        public string LastModifiedByUser { get; set; }

        [YamlMember(Order = 33)]
        public string DraftContent { get; set; }

        [YamlMember(Order = 34)]
        public string DraftAuthor { get; set; }

        [YamlMember(Order = 35)]
        public DateTime? DraftPubDate { get; set; }

        [YamlMember(Order = 36)]
        public string TemplateKey { get; set; }

        [YamlMember(Order = 37)]
        public string SerializedModel { get; set; }

        [YamlMember(Order = 38)]
        public string DraftSerializedModel { get; set; }

        [YamlMember(Order = 39)]
        public string Serializer { get; set; }

        [YamlMember(Order = 40)]
        public bool? ShowCreatedBy { get; set; }

        [YamlMember(Order = 41)]
        public bool? ShowCreatedDate { get; set; }

        [YamlMember(Order = 42)]
        public bool? ShowLastModifiedBy { get; set; }

        [YamlMember(Order = 43)]
        public bool? ShowLastModifiedDate { get; set; }

        [YamlIgnore]
        public string Content { get; set; } = string.Empty;
        [YamlIgnore]
        public List<IComment> Comments { get; set; }


    }
}
