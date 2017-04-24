using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class PageDevelopmentViewModel
    {
        public PageDevelopmentViewModel()
        {
            Css = new List<IPageResource>();
            Js = new List<IPageResource>();
            AddResourceViewModel = new AddPageResourceViewModel();
        }

        public string Slug { get; set; } = string.Empty;

        public List<IPageResource> Css { get; set; }
        public List<IPageResource> Js { get; set; }

        public AddPageResourceViewModel AddResourceViewModel { get; set; }
    }
}
