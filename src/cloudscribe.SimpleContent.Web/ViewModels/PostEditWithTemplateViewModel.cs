using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PostEditWithTemplateViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "ProjectId is required")]
        public string ProjectId { get; set; } = string.Empty;

        public string CorrelationKey { get; set; } = string.Empty;


        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title has a maximum length of 255 characters")]
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;

        public object TemplateModel { get; set; }

        public ContentTemplate Template { get; set; }


        public bool IsPublished { get; set; } = false;

        public string Categories { get; set; }

        public DateTime? PubDate { get; set; }

        [RequiredWhen("SaveMode", "PublishLater", ErrorMessage = "A Date is required to publish later.")]
        public DateTime? NewPubDate { get; set; }

        public DateTime? DraftPubDate { get; set; }

        public string CurrentPostUrl { get; set; }

        public string DeletePostRouteName { get; set; }

        public bool IsFeatured { get; set; } = false;

        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        public string ContentType { get; set; } = "html";

        public string TeaserOverride { get; set; }

        public bool SuppressTeaser { get; set; }

        public bool TeasersEnabled { get; set; }

        public string SaveMode { get; set; } //SaveDraft, PublishNow, PublishLater buttomn values

        public Guid? HistoryId { get; set; }
        public DateTime? HistoryArchiveDate { get; set; }
        public bool DidReplaceDraft { get; set; }
        public bool DidRestoreDeleted { get; set; }

    }
}
