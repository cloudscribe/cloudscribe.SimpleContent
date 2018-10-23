using cloudscribe.SimpleContent.Models;
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

        public async Task ConvertMediaToRelativeUrls(IPage page)
        {
            var baseUrl = string.Concat(
                        _contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        _contextAccessor.HttpContext.Request.Host.ToUriComponent()
                        );

            page.DraftContent = await _contentProcessor.ConvertMediaUrlsToRelative(
                baseUrl, 
                page.DraftContent);

            // olw also adds hard coded style to images
            page.DraftContent = _contentProcessor.RemoveImageStyleAttribute(page.DraftContent);

            // open live writer passes in posts with absolute urls
            // we want to change them to relative to keep the files portable
            // to a different root url
            page.Content = await _contentProcessor.ConvertMediaUrlsToRelative(
                baseUrl, 
                page.Content);

            // olw also adds hard coded style to images
            page.Content = _contentProcessor.RemoveImageStyleAttribute(page.Content);
        }

        public Task ConvertMediaToAbsoluteUrls(IPage page, IProjectSettings projectSettings)
        {
            string baseUrl = projectSettings.CdnUrl;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = string.Concat(
                        _contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        _contextAccessor.HttpContext.Request.Host.ToUriComponent()
                        );
            }

            page.Content = _contentProcessor.ConvertUrlsToAbsolute(baseUrl, page.Content);
            page.DraftContent = _contentProcessor.ConvertUrlsToAbsolute(baseUrl, page.DraftContent);

            return Task.CompletedTask;
        }

    }
    
}
