using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPost
    {
        string Author { get; set; }
        string BlogId { get; set; }
        List<string> Categories { get; set; }
        List<IComment> Comments { get; set; }
        string Content { get; set; }
        string Id { get; set; }
        bool IsPublished { get; set; }
        DateTime LastModified { get; set; }
        string MetaDescription { get; set; }
        /// <summary>
        /// the idea is to serialize an object with list of metadata and meta links
        /// whenever this model is modified it is reserialized here then
        /// we can generate the output html and store it on MetaHtml
        /// </summary>
        string MetaJson { get; set; }
        /// <summary>
        /// an automatically generated html meta data
        /// to be generated from the model seriaized in MetaJson
        /// </summary>
        string MetaHtml { get; set; }
        DateTime PubDate { get; set; }
        string Slug { get; set; }
        string Title { get; set; }
        string CorrelationKey { get; set; }
        bool IsFeatured { get; set; }
        string ImageUrl { get; set; }
        string ThumbnailUrl { get; set; }
    }
}