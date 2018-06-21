using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class NewPageViewModel
    {
        public NewPageViewModel()
        {
            Templates = new List<ContentTemplate>();
        }


        public string ParentSlug { get; set; }
        public string ContentType { get; set; }
        public List<ContentTemplate> Templates { get; set; }

        [Required (ErrorMessage = "Template is required")]
        public string SelectedTemplate { get; set; }

        [Required(ErrorMessage = "Page heading is required")]
        public string PageTitle { get; set; }


    }
}
