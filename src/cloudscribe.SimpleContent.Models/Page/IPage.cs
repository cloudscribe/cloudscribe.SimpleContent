using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPage : IContentItem
    {
        string Author { get; set; }
        List<string> Categories { get; set; }
        List<IComment> Comments { get; set; }
        
        bool IsPublished { get; set; }
        DateTime LastModified { get; set; }
       
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

        bool ShowCategories { get; set; }
        bool ShowComments { get; set; }
        bool ShowHeading { get; set; }
        bool ShowLastModified { get; set; }
        bool ShowPubDate { get; set; }
        
        string ExternalUrl { get; set; }
        string CorrelationKey { get; set; } 
        
        string ViewRoles { get; set; }
        string MenuFilters { get; set; }
        /// <summary>
        /// If true then the page is just a parent for other pages in the menu
        /// the content will not be rendered, only a child page menu will be shown
        /// </summary>
        bool MenuOnly { get; set; }

        bool ShowMenu { get; set; }
        bool DisableEditor { get; set; }

        List<PageResource> Resources { get; set; }

        // new fields 2018-06-20
        DateTime CreatedUtc { get; set; }
        string CreatedByUser { get; set; }
        string LastModifiedByUser { get; set; }
        string DraftContent { get; set; }
        string DraftAuthor { get; set; }
        DateTime? DraftPubDate { get; set; }

        string TemplateKey { get; set; }
        string SerializedModel { get; set; }
        string DraftSerializedModel { get; set; }
        
        string Serializer { get; set; }

    }
}