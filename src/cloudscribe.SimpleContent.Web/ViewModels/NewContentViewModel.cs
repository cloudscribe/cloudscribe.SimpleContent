using cloudscribe.Pagination.Models;
using cloudscribe.SimpleContent.Models;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class NewContentViewModel
    {
        public NewContentViewModel()
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

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int CountOfTemplates { get; set; }

        public string SearchRouteName { get; set; }
        public string PostActionName { get; set; }

    }
}
