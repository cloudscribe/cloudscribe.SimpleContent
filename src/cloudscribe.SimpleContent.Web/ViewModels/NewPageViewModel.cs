using cloudscribe.SimpleContent.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class NewPageViewModel
    {
        public NewPageViewModel()
        {
            Templates = new List<ContentTemplate>();
        }


        public string ParentSlug { get; set; }
        public int PageOrder { get; set; } = 3;
        public string ContentType { get; set; }
        public List<ContentTemplate> Templates { get; set; }

        [Required (ErrorMessage = "Template is required")]
        public string SelectedTemplate { get; set; }

        [Required(ErrorMessage = "Page heading is required")]
        public string PageTitle { get; set; }


    }
}
