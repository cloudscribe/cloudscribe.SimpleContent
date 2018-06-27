using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PageEditWithTemplateViewModel
    {
        public string ProjectId { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public object TemplateModel { get; set; }

        public ContentTemplate Template { get; set; }
        
        public string ParentId { get; set; } = "0";
        public string ParentSlug { get; set; } = string.Empty;

        public string CorrelationKey { get; set; } = string.Empty;

        public int PageOrder { get; set; } = 3;

        [Required(ErrorMessage = "Page Heading is required")]
        [StringLength(255, ErrorMessage = "Page Heading has a maximum length of 255 characters")]
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;

        public string ViewRoles { get; set; } = string.Empty;

        public DateTime? PubDate { get; set; }

        [RequiredWhen("SaveMode", "PublishLater", ErrorMessage = "A Date is required to publish later.")]
        public DateTime? NewPubDate { get; set; }

        public DateTime? DraftPubDate { get; set; }

        public bool IsPublished { get; set; } = true;

        public bool ShowHeading { get; set; } = true;

        public string DisqusShortname { get; set; }

        public bool ShowComments { get; set; } = false;

        public bool ShowMenu { get; set; } = false;

        [StringLength(500, ErrorMessage = "The menu filters has a maximun length of 500 characters")]
        public string MenuFilters { get; set; }

        public string SaveMode { get; set; } //SaveDraft, PublishNow, PublishLater buttomn values
    }
}
