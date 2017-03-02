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
        /// <summary>
        /// If true then the page is just a parent for other pages in the menu
        /// the content will not be rendered, only a child page menu will be shown
        /// </summary>
        bool MenuOnly { get; set; }

        bool ShowMenu { get; set; }
    }
}