using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web
{
    public class RecentPostsViewComponent : ViewComponent
    {
        public RecentPostsViewComponent(
            IProjectService projectService,
            IPostQueries postQueries
            )
        {
            this.projectService = projectService;
            this.postQueries = postQueries;
        }

        private IProjectService projectService;
        private IPostQueries postQueries;


        public async Task<IViewComponentResult> InvokeAsync(string viewName = "RecentPosts", int numberToShow = 5)
        {
            var model = new RecentPostsViewModel();
            var settings = await projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            var list = await postQueries.GetRecentPosts(settings.Id, numberToShow);
            model.ProjectSettings = settings;
            model.Posts = list;

            return View(viewName, model);
        }

    }
}
