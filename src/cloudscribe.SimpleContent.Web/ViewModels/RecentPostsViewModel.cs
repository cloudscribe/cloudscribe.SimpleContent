using cloudscribe.SimpleContent.Models;
using System.Collections.Generic;

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
