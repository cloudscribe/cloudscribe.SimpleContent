// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-07-08
// 


using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web;
using cloudscribe.SimpleContent.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class PageService : IPageService
    {
        public PageService(
            IProjectService projectService,
            IPageQueries pageQueries,
            IPageCommands pageCommands,
            PageEvents eventHandlers,
            IMediaProcessor mediaProcessor,
            IContentProcessor contentProcessor,
            IPageUrlResolver pageUrlResolver,
            IMemoryCache cache,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer,
            IPageNavigationCacheKeys cacheKeys
            )
        {

            _projectService = projectService;
            _pageQueries = pageQueries;
            _pageCommands = pageCommands;
            _mediaProcessor = mediaProcessor;
            _pageUrlResolver = pageUrlResolver;
            _contentProcessor = contentProcessor;
            _cache = cache;
            _cacheKeys = cacheKeys;
            _eventHandlers = eventHandlers;
            _sr = localizer;
        }
        
        private readonly IPageQueries _pageQueries;
        private readonly IPageCommands _pageCommands;
        private readonly IMediaProcessor _mediaProcessor;
        private readonly IProjectService _projectService;
        private readonly IContentProcessor _contentProcessor;
        private readonly IMemoryCache _cache;
        private readonly IPageNavigationCacheKeys _cacheKeys;
        private readonly PageEvents _eventHandlers;
        private readonly IPageUrlResolver _pageUrlResolver;
        private readonly IStringLocalizer _sr;

        private IProjectSettings _settings = null;

        private async Task EnsureProjectSettings()
        {
            if (_settings != null) { return; }
            _settings = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            
        }

        public void ClearNavigationCache()
        {  
            _cache.Remove(_cacheKeys.PageTreeCacheKey);
            _cache.Remove(_cacheKeys.XmlTreeCacheKey);
            _cache.Remove(_cacheKeys.JsonTreeCacheKey);
        }
        
        public async Task<bool> SlugIsAvailable(string slug)
        {
            await EnsureProjectSettings();
            return await _pageQueries.SlugIsAvailable(
                _settings.Id,
                slug,
                CancellationToken.None
                ).ConfigureAwait(false);
        }
        
        public async Task Create(IPage page, bool convertToRelativeUrls = false)
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            
            if (string.IsNullOrEmpty(page.Slug))
            {
                var slug = ContentUtils.CreateSlug(page.Title);
                var available = await SlugIsAvailable(slug);
                if (available)
                {
                    page.Slug = slug;
                }
            }

            if(convertToRelativeUrls)
            {
                await _pageUrlResolver.ConvertToRelativeUrls(page, _settings).ConfigureAwait(false);
            }
            
            await _pageCommands.Create(_settings.Id, page).ConfigureAwait(false);
            await _eventHandlers.HandleCreated(_settings.Id, page).ConfigureAwait(false);
           
        }

        public async Task Update(IPage page, bool convertToRelativeUrls = false)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            if (convertToRelativeUrls)
            {
                await _pageUrlResolver.ConvertToRelativeUrls(page, _settings).ConfigureAwait(false);
            }

            await _eventHandlers.HandlePreUpdate(_settings.Id, page.Id).ConfigureAwait(false);
            await _pageCommands.Update(_settings.Id, page).ConfigureAwait(false);
            await _eventHandlers.HandleUpdated(_settings.Id, page).ConfigureAwait(false);
        }

        public async Task DeletePage(string pageId)
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            await _eventHandlers.HandlePreDelete(_settings.Id, pageId).ConfigureAwait(false);

            // we have a loosely coupled raltionship of pages not enforced in the db
            // so we have to consider how to handle child pages belonging to a page that is about to be deleted
            // it seems dangerous to cascade the delete to child pages
            // in most cases a delete decisioon should be made on each page
            // the possibility of accidently deleting multiple pages would be high
            // so for now we will just orphan the children to become root pages
            // which can result in a bad user experience if a bunch of pages suddenly appear in the root
            // we will show a warning that indicates the child pages will become root pages
            // and that they should delete child pages before deleting the parent page if that is the intent
            await HandleChildPagesBeforeDelete(pageId);

            await _pageCommands.Delete(_settings.Id, pageId).ConfigureAwait(false);
            

        }

        private async Task HandleChildPagesBeforeDelete(string pageId)
        {
            var children = await GetChildPages(pageId);
            foreach(var c in children)
            {
                // rebase to root 
                c.ParentId = "0";
                c.ParentSlug = string.Empty;
                await _pageCommands.Update(_settings.Id, c).ConfigureAwait(false);
            }
        }
        
        public async Task<IPage> GetPage(string pageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await _pageQueries.GetPage(
                _settings.Id,
                pageId,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IPage> GetPageBySlug(string slug, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await _pageQueries.GetPageBySlug(
                _settings.Id,
                slug,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPage>> GetAllPages(string projectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _pageQueries.GetAllPages(
                projectId,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPage>> GetRootPages(CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureProjectSettings();
            return await _pageQueries.GetRootPages(
                _settings.Id,
                cancellationToken
                ).ConfigureAwait(false);
        }

        public async Task<List<IPage>> GetChildPages(string pageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureProjectSettings();
            return await _pageQueries.GetChildPages(
                _settings.Id,
                pageId,
                cancellationToken
                ).ConfigureAwait(false);
        }

        public async Task<int> GetNextChildPageOrder(string pageSlug, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureProjectSettings();
            var pageId = "0"; // root level pages have this parent id
            var page = await _pageQueries.GetPageBySlug(_settings.Id, pageSlug);
            if (page != null)
            {
                pageId = page.Id;
            }
            
            var countOfChildren =  await _pageQueries.GetChildPageCount(
                _settings.Id,
                pageId,
                true,
                cancellationToken
                ).ConfigureAwait(false);

            return (countOfChildren * 3) + 2;
        }

        public async Task<PageActionResult> Move(PageMoveModel model)
        {
            PageActionResult result;


            if (string.IsNullOrEmpty(model.MovedNode) || string.IsNullOrEmpty(model.TargetNode) || (model.MovedNode == "-1") || (model.TargetNode == "-1") || (string.IsNullOrEmpty(model.Position)))
            {
                result = new PageActionResult(false, "bad request, failed to move page");
                return result;
            }

            var movedNode = await GetPage(model.MovedNode);
            var targetNode = await GetPage(model.TargetNode);

            if ((movedNode == null) || (targetNode == null))
            {
                result = new PageActionResult(false, "bad request, page or target page not found");
                return result;
            }

            if(movedNode.Slug == _settings.DefaultPageSlug)
            {
                result = new PageActionResult(false, _sr["Moving the default/home page is not allowed"]);
                return result;
            }

            switch (model.Position)
            {
                case "inside":
                    // this case is when moving to a new parent node that doesn't have any children yet
                    // target is the new parent
                    // or when momving to the first position of the current parent
                    movedNode.ParentId = targetNode.Id;
                    movedNode.ParentSlug = targetNode.Slug;
                    movedNode.PageOrder = 0;
                    await _pageCommands.Update(movedNode.ProjectId, movedNode);
                    await SortChildPages(targetNode.Id);

                    break;

                case "before":
                    // put this page before the target page beneath the same parent as the target
                    if (targetNode.ParentId != movedNode.ParentId)
                    {
                        movedNode.ParentId = targetNode.ParentId;
                        movedNode.ParentSlug = targetNode.ParentSlug;
                        movedNode.PageOrder = targetNode.PageOrder - 1;
                        await _pageCommands.Update(movedNode.ProjectId, movedNode);
                        await SortChildPages(targetNode.ParentId);

                    }
                    else
                    {
                        //parent did not change just sort
                        // set sort and re-sort
                        movedNode.PageOrder = targetNode.PageOrder - 1;
                        await _pageCommands.Update(movedNode.ProjectId, movedNode);
                        await SortChildPages(targetNode.ParentId);

                    }

                    break;

                case "after":
                default:
                    // put this page after the target page beneath the same parent as the target
                    if (targetNode.ParentId != movedNode.ParentId)
                    {
                        movedNode.ParentId = targetNode.ParentId;
                        movedNode.ParentSlug = targetNode.ParentSlug;
                        movedNode.PageOrder = targetNode.PageOrder + 1;
                        await _pageCommands.Update(movedNode.ProjectId, movedNode);
                        await SortChildPages(targetNode.ParentId);
                    }
                    else
                    {
                        //parent did not change just sort
                        movedNode.PageOrder = targetNode.PageOrder + 1;
                        await _pageCommands.Update(movedNode.ProjectId, movedNode);
                        await SortChildPages(targetNode.ParentId);

                    }

                    break;
            }

            ClearNavigationCache();

            result = new PageActionResult(true, "operation succeeded");

            return result;
        }

        public async Task SortChildPages(string pageId)
        {
            var children = await GetChildPages(pageId);
            int i = 1;
            foreach(var child in children)
            {
                child.PageOrder = i;
                await _pageCommands.Update(child.ProjectId, child);
                i += 2;
            }
        }

        public async Task<PageActionResult> SortChildPagesAlpha(string pageId)
        {
            var children = await GetChildPages(pageId);
            var sorted = children.OrderBy(p => p.Title);
            int i = 1;
            foreach (var child in sorted)
            {
                child.PageOrder = i;
                await _pageCommands.Update(child.ProjectId, child);
                i += 2;
            }

            ClearNavigationCache();
            return new PageActionResult(true, "operation succeeded");

        }

        public async Task<string> GetPageTreeJson(ClaimsPrincipal user, Func<IPage, string> urlResolver, string node = "root", CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureProjectSettings();
            

            List<IPage> list;
            if(node == "root")
            {
                list = await GetRootPages();
            }
            else
            {
                list = await GetChildPages(node);
            }
            
            var comma = string.Empty;
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var p in list)
            {
                sb.Append(comma);
                await BuildPageJson(user, sb, p, urlResolver, cancellationToken);
                comma = ",";
            }
            sb.Append("]");

            return sb.ToString();
        }

        private async Task BuildPageJson(ClaimsPrincipal user, StringBuilder script, IPage page, Func<IPage, string> urlResolver, CancellationToken cancellationToken = default(CancellationToken))
        {
            var childPagesCount = await _pageQueries.GetChildPageCount(page.ProjectId, page.Id, true, cancellationToken); 
            script.Append("{");
            script.Append("\"id\":" + "\"" + page.Id + "\"");
            script.Append(",\"slug\":" + "\"" + page.Slug + "\"");
            script.Append(",\"label\":\"" + Encode(page.Title) + "\"");
            script.Append(",\"url\":\"" + urlResolver(page) + "\"");
            script.Append(",\"parentId\":" + "\"" + page.ParentId + "\"");
            script.Append(",\"childcount\":" + childPagesCount.ToString());
            script.Append(",\"children\":[");
            script.Append("]");
            if (childPagesCount > 0)
            {
                script.Append(",\"load_on_demand\":true");
            }
            
            if(!string.IsNullOrEmpty(page.ViewRoles) && !user.IsInRoles(page.ViewRoles))
            {
                script.Append(",\"canEdit\":false");
                script.Append(",\"canCreateChild\":false");
                script.Append(",\"canDelete\":false");
            }
            else
            {
                script.Append(",\"canEdit\":true");
                script.Append(",\"canCreateChild\":true");
                if (page.Slug == _settings.DefaultPageSlug)
                {
                    script.Append(",\"canDelete\":false"); // don't allow delete the home/default page from the ui
                }
                else
                {
                    script.Append(",\"canDelete\":true");
                }
            }
            
            script.Append(",\"pubstatus\":\"" + GetPublishingStatus(page) + "\"");
            
            script.Append("}");

        }

        private string GetPublishingStatus(IPage page)
        {
            if(!string.IsNullOrEmpty(page.ExternalUrl)) return _sr["Link Only"];

            if (!page.IsPublished) return _sr["Unpublished"];
            if (page.PubDate > DateTime.UtcNow) return _sr["Future Published"];
            if (string.IsNullOrEmpty(page.ViewRoles) || page.ViewRoles.Contains("All Users")) return _sr["Public"];
            return _sr["Protected"];
        }

        public async Task FirePublishEvent(IPage page)
        {
            await _eventHandlers.HandlePublished(page.ProjectId, page);
        }

        //private string ResolveUrl(IPage page, IUrlHelper urlHelper)
        //{
        //    if(page.Slug == this._settings.DefaultPageSlug)
        //    {
        //        return urlHelper.RouteUrl(_pageRoutes.PageRouteName);
        //    }

        //    return urlHelper.RouteUrl(_pageRoutes.PageRouteName, new { slug = page.Slug });
        //}

        private string Encode(string input)
        {

            //return JsonEscape(HttpUtility.HtmlDecode(input));
            return JsonEscape(input);

        }

        private static string JsonEscape(string s)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            //sb.Append("\"");

            return sb.ToString();
        }

    }
}
