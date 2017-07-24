using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPage
    {
        string Author { get; set; }
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
        int PageOrder { get; set; }
        string ParentId { get; set; }
        string ParentSlug { get; set; }
        string ProjectId { get; set; }
        DateTime PubDate { get; set; }
        bool ShowCategories { get; set; }
        bool ShowComments { get; set; }
        bool ShowHeading { get; set; }
        bool ShowLastModified { get; set; }
        bool ShowPubDate { get; set; }
        string Slug { get; set; }
        string ExternalUrl { get; set; }
        string CorrelationKey { get; set; } 
        string Title { get; set; }
        string ViewRoles { get; set; }
        string MenuFilters { get; set; }
        /// <summary>
        /// If true then the page is just a parent for other pages in the menu
        /// the content will not be rendered, only a child page menu will be shown
        /// </summary>
        bool MenuOnly { get; set; }

        bool ShowMenu { get; set; }

        List<PageResource> Resources { get; set; }
    }
}