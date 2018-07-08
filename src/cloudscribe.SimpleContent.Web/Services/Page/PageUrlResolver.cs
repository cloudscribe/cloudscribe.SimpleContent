using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageUrlResolver : IPageUrlResolver
    {
        public PageUrlResolver(
            IHttpContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            IContentProcessor contentProcessor,
            IPageRoutes pageRoutes
            )
        {
            _contextAccessor = contextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _contentProcessor = contentProcessor;
            _pageRoutes = pageRoutes;
        }

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccesor;
        private readonly IContentProcessor _contentProcessor;
        private readonly IPageRoutes _pageRoutes;

        public Task<string> ResolvePageUrl(IPage page)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            var result = urlHelper.RouteUrl(_pageRoutes.PageRouteName, new { slug = page.Slug });
            return Task.FromResult(result);
        }

        public async Task ConvertToRelativeUrls(IPage page, IProjectSettings projectSettings)
        {
            var httpContext = _contextAccessor.HttpContext;
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            var imageAbsoluteBaseUrl = urlHelper.Content("~" + projectSettings.LocalMediaVirtualPath);
            if (httpContext != null)
            {
                imageAbsoluteBaseUrl = httpContext.Request.AppBaseUrl() + projectSettings.LocalMediaVirtualPath;
            }

            page.DraftContent = await _contentProcessor.ConvertMediaUrlsToRelative(
                projectSettings.LocalMediaVirtualPath,
                imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
                page.DraftContent);

            // olw also adds hard coded style to images
            page.DraftContent = _contentProcessor.RemoveImageStyleAttribute(page.DraftContent);

            // open live writer passes in posts with absolute urls
            // we want to change them to relative to keep the files portable
            // to a different root url
            page.Content = await _contentProcessor.ConvertMediaUrlsToRelative(
                projectSettings.LocalMediaVirtualPath,
                imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
                page.Content);

            // olw also adds hard coded style to images
            page.Content = _contentProcessor.RemoveImageStyleAttribute(page.Content);
        }

    }
    
}
