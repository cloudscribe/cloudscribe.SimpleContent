using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class RecentPostsViewModel
    {
        public RecentPostsViewModel()
        {
            ProjectSettings = new ProjectSettings();
            Posts = new List<IPost>();
        }

        public IProjectSettings ProjectSettings { get; set; }

        public List<IPost> Posts { get; set; }
    }
}
