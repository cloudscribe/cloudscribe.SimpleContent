using cloudscribe.Pagination.Models;
using cloudscribe.SimpleContent.Models;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class NewPageViewModel
    {
        public NewPageViewModel()
        {
            Templates = new PagedResult<ContentTemplate>();
        }

        public string Query { get; set; }
        public string ParentSlug { get; set; }
        public int PageOrder { get; set; } = 3;
        public string ContentType { get; set; }
        public PagedResult<ContentTemplate> Templates { get; set; }

        [Required (ErrorMessage = "Template is required")]
        public string SelectedTemplate { get; set; }

        [Required(ErrorMessage = "Page heading is required")]
        public string PageTitle { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int CountOfTemplates { get; set; }


    }
}
