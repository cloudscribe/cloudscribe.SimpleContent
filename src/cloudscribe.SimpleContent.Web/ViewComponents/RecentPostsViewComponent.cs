using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.ViewModels;
using cloudscribe.Web.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web
{
    public class RecentPostsViewComponent : ViewComponent
    {
        public RecentPostsViewComponent(
            IProjectService projectService,
            IPostQueries postQueries,
            IContentProcessor contentProcessor,
            ITimeZoneHelper timeZoneHelper
            )
        {
            _projectService = projectService;
            _postQueries = postQueries;
            _timeZoneHelper = timeZoneHelper;
            _contentProcessor = contentProcessor;
        }

        private IProjectService _projectService;
        private IPostQueries _postQueries;
        private ITimeZoneHelper _timeZoneHelper;
        private IContentProcessor _contentProcessor;


        public async Task<IViewComponentResult> InvokeAsync(string viewName = "RecentPosts", int numberToShow = 5)
        {
            var model = new RecentPostsViewModel(_contentProcessor);
            var settings = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            var list = await _postQueries.GetRecentPosts(settings.Id, numberToShow);
            model.ProjectSettings = settings;
            model.Posts = list;
            model.TimeZoneHelper = _timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;

            return View(viewName, model);
        }

    }
}
