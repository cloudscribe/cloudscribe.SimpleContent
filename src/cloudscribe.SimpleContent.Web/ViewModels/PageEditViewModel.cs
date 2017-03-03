using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PageEditViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "ProjectId is required")]
        public string ProjectId { get; set; } = string.Empty;

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

        
        public string Content { get; set; } = string.Empty;

        public string ViewRoles { get; set; } = string.Empty;

        public string PubDate { get; set; } = string.Empty;

        
        public bool IsPublished { get; set; } = true;

        public bool ShowHeading { get; set; } = true;

        public string ExternalUrl { get; set; } = string.Empty;

        public bool ShowMenu { get; set; } = false;

        public bool MenuOnly { get; set; } = false;

        public string DropFileUrl { get; set; } = "/filemanager/upload";

        public string FileBrowseUrl { get; set; } = "/filemanager/ckfiledialog?type=file";

        public string ImageBrowseUrl { get; set; } = "/filemanager/ckfiledialog?type=image";



    }
}
