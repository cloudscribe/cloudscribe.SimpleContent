// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2017-05-28
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using cloudscribe.SimpleContent.Models.EventHandlers;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using cloudscribe.SimpleContent.Web;

namespace cloudscribe.SimpleContent.Services
{
    public class PageService : IPageService
    {
        public PageService(
            IProjectService projectService,
            IProjectSecurityResolver security,
            IPageQueries pageQueries,
            IPageCommands pageCommands,
            PageEvents eventHandlers,
            IMediaProcessor mediaProcessor,
            IHtmlProcessor htmlProcessor,
            IUrlHelperFactory urlHelperFactory,
            IPageRoutes pageRoutes,
            IMemoryCache cache,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer,
            IPageNavigationCacheKeys cacheKeys,
            IActionContextAccessor actionContextAccesor,
            IHttpContextAccessor contextAccessor = null)
        {

            this.projectService = projectService;
            this.security = security;
            this.pageQueries = pageQueries;
            this.pageCommands = pageCommands;
            context = contextAccessor?.HttpContext;
            this.mediaProcessor = mediaProcessor;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccesor = actionContextAccesor;
            this.pageRoutes = pageRoutes;
            this.htmlProcessor = htmlProcessor;
            this.cache = cache;
            this.cacheKeys = cacheKeys;
            this.eventHandlers = eventHandlers;
            sr = localizer;
        }

        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;
        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionContextAccesor;
        private IProjectSecurityResolver security;
        private IPageQueries pageQueries;
        private IPageCommands pageCommands;
        private IMediaProcessor mediaProcessor;
        private IProjectService projectService;
        private IProjectSettings settings = null;
        private IHtmlProcessor htmlProcessor;
        private IMemoryCache cache;
        private IPageNavigationCacheKeys cacheKeys;
        private PageEvents eventHandlers;
        private IPageRoutes pageRoutes;
        private IStringLocalizer sr;

        private async Task EnsureProjectSettings()
        {
            if (settings != null) { return; }
            settings = await projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            if (settings != null) { return; }
            
        }

        public void ClearNavigationCache()
        {  
            cache.Remove(cacheKeys.PageTreeCacheKey);
            cache.Remove(cacheKeys.XmlTreeCacheKey);
            cache.Remove(cacheKeys.JsonTreeCacheKey);
        }

        public Task<string> ResolvePageUrl(IPage page)
        {
            //await EnsureBlogSettings().ConfigureAwait(false);
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
       
            var result = urlHelper.Action("Index", "Page", new { slug = page.Slug });

            return Task.FromResult(result);
        }

        //public async Task<bool> PageSlugIsAvailable(string slug)
        //{
        //    await EnsureProjectSettings().ConfigureAwait(false);

        //    return await pageQueries.SlugIsAvailable(
        //        settings.Id,
        //        slug,
        //        CancellationToken)
        //        .ConfigureAwait(false);
        //}

        //public async Task<bool> PageSlugIsAvailable(string projectId, string slug)
        //{
        //    return await pageQueries.SlugIsAvailable(
        //        projectId,
        //        slug,
        //        CancellationToken)
        //        .ConfigureAwait(false);
        //}

        public async Task<bool> SlugIsAvailable(string slug)
        {
            await EnsureProjectSettings();
            return await pageQueries.SlugIsAvailable(
                settings.Id,
                slug,
                CancellationToken
                ).ConfigureAwait(false);
        }

        public async Task Create(
            string projectId,
            string userName,
            string password,
            IPage page,
            bool publish)
        {
            var permission = await security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return; 
            }

            var settings = await projectService.GetProjectSettings(projectId).ConfigureAwait(false);
            
            if (publish)
            {
                page.PubDate = DateTime.UtcNow;
            }

            if (string.IsNullOrEmpty(page.Slug))
            {
                var slug = ContentUtils.CreateSlug(page.Title);
                var available = await SlugIsAvailable(slug);
                if (available)
                {
                    page.Slug = slug;
                }

            }
            
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var imageAbsoluteBaseUrl = urlHelper.Content("~" + settings.LocalMediaVirtualPath);
            if (context != null)
            {
                imageAbsoluteBaseUrl = context.Request.AppBaseUrl() + settings.LocalMediaVirtualPath;
            }

            // open live writer passes in posts with absolute urls
            // we want to change them to relative to keep the files portable
            // to a different root url
            page.Content = await htmlProcessor.ConvertMediaUrlsToRelative(
                settings.LocalMediaVirtualPath,
                imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
                page.Content);

            // olw also adds hard coded style to images
            page.Content = htmlProcessor.RemoveImageStyleAttribute(page.Content);

            // here we need to process any base64 embedded images
            // save them under wwwroot
            // and update the src in the post with the new url
            // since this overload of Save is only called from metaweblog
            // and metaweblog does not base64 encode the images like the browser client
            // this call may not be needed here
            //await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
            //    settings.LocalMediaVirtualPath,
            //    post
            //    ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (page.PubDate == nonPublishedDate)
            {
                page.PubDate = DateTime.UtcNow;
            }

            await pageCommands.Create(projectId, page).ConfigureAwait(false);
            await eventHandlers.HandleCreated(projectId, page).ConfigureAwait(false);
        }

        public async Task Update(
            string projectId,
            string userName,
            string password,
            IPage page,
            bool publish)
        {
            var permission = await security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return;
            }

            var settings = await projectService.GetProjectSettings(projectId).ConfigureAwait(false);
            
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var imageAbsoluteBaseUrl = urlHelper.Content("~" + settings.LocalMediaVirtualPath);
            if (context != null)
            {
                imageAbsoluteBaseUrl = context.Request.AppBaseUrl() + settings.LocalMediaVirtualPath;
            }

            // open live writer passes in posts with absolute urls
            // we want to change them to relative to keep the files portable
            // to a different root url
            page.Content = await htmlProcessor.ConvertMediaUrlsToRelative(
                settings.LocalMediaVirtualPath,
                imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
                page.Content);

            // olw also adds hard coded style to images
            page.Content = htmlProcessor.RemoveImageStyleAttribute(page.Content);

            // here we need to process any base64 embedded images
            // save them under wwwroot
            // and update the src in the post with the new url
            // since this overload of Save is only called from metaweblog
            // and metaweblog does not base64 encode the images like the browser client
            // this call may not be needed here
            //await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
            //    settings.LocalMediaVirtualPath,
            //    post
            //    ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (page.PubDate == nonPublishedDate)
            {
                page.PubDate = DateTime.UtcNow;
            }

            await eventHandlers.HandlePreUpdate(projectId, page.Id).ConfigureAwait(false);
            await pageCommands.Update(projectId, page).ConfigureAwait(false);
            await eventHandlers.HandleUpdated(projectId, page).ConfigureAwait(false);
        }

        public async Task Create(
            IPage page,
            bool publish)
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            
            if (publish)
            {
                page.PubDate = DateTime.UtcNow;
            }

            if (string.IsNullOrEmpty(page.Slug))
            {
                var slug = ContentUtils.CreateSlug(page.Title);
                var available = await SlugIsAvailable(slug);
                if (available)
                {
                    page.Slug = slug;
                }

            }
            
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var imageAbsoluteBaseUrl = urlHelper.Content("~" + settings.LocalMediaVirtualPath);
            if (context != null)
            {
                imageAbsoluteBaseUrl = context.Request.AppBaseUrl() + settings.LocalMediaVirtualPath;
            }
            
            //this is no longer needed, we once used bootstrapwysiwyg which passed images as base64 content
            // but we don't use that anymore. now we have ckeditor and filemanager integration
            //page.Content = await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
            //    settings.LocalMediaVirtualPath,
            //    page.Content
            //    ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (page.PubDate == nonPublishedDate)
            {
                page.PubDate = DateTime.UtcNow;
            }

            await pageCommands.Create(settings.Id, page).ConfigureAwait(false);
            await eventHandlers.HandleCreated(settings.Id, page).ConfigureAwait(false);
        }

        public async Task Update(
            IPage page,
            bool publish)
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var imageAbsoluteBaseUrl = urlHelper.Content("~" + settings.LocalMediaVirtualPath);
            if (context != null)
            {
                imageAbsoluteBaseUrl = context.Request.AppBaseUrl() + settings.LocalMediaVirtualPath;
            }

            //this is no longer needed, we once used bootstrapwysiwyg which passed images as base64 content
            // but we don't use that anymore. now we have ckeditor and filemanager integration
            //page.Content = await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
            //    settings.LocalMediaVirtualPath,
            //    page.Content
            //    ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (page.PubDate == nonPublishedDate)
            {
                page.PubDate = DateTime.UtcNow;
            }

            await eventHandlers.HandlePreUpdate(settings.Id, page.Id).ConfigureAwait(false);
            await pageCommands.Update(settings.Id, page).ConfigureAwait(false);
            await eventHandlers.HandleUpdated(settings.Id, page).ConfigureAwait(false);
        }

        public async Task DeletePage(string pageId)
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            await eventHandlers.HandlePreDelete(settings.Id, pageId).ConfigureAwait(false);

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

            await pageCommands.Delete(settings.Id, pageId).ConfigureAwait(false);
            

        }

        private async Task HandleChildPagesBeforeDelete(string pageId)
        {
            var children = await GetChildPages(pageId);
            foreach(var c in children)
            {
                // rebase to root 
                c.ParentId = "0";
                c.ParentSlug = string.Empty;
                await pageCommands.Update(settings.Id, c).ConfigureAwait(false);
            }
        }

        public async Task<IPage> GetPage(
            string projectId, 
            string pageId,
            string userName,
            string password)
        {
            var permission = await security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return null;
            }

            return await pageQueries.GetPage(
                projectId,
                pageId,
                CancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<IPage> GetPage(string pageId)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await pageQueries.GetPage(
                settings.Id,
                pageId,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IPage> GetPageBySlug(string slug)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await pageQueries.GetPageBySlug(
                settings.Id,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPage>> GetAllPages(
            string projectId,
            string userName,
            string password)
        {
            var permission = await security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return new List<IPage>(); // empty
            }

            return await pageQueries.GetAllPages(
                projectId,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPage>> GetRootPages()
        {
            await EnsureProjectSettings();
            return await pageQueries.GetRootPages(
                settings.Id,
                CancellationToken
                ).ConfigureAwait(false);
        }

        public async Task<List<IPage>> GetChildPages(string pageId)
        {
            await EnsureProjectSettings();
            return await pageQueries.GetChildPages(
                settings.Id,
                pageId,
                CancellationToken
                ).ConfigureAwait(false);
        }

        public async Task<int> GetNextChildPageOrder(string pageSlug)
        {
            await EnsureProjectSettings();
            var pageId = "0"; // root level pages have this parent id
            var page = await pageQueries.GetPageBySlug(settings.Id, pageSlug);
            if (page != null)
            {
                pageId = page.Id;
            }
            
            var countOfChildren =  await pageQueries.GetChildPageCount(
                settings.Id,
                pageId,
                true,
                CancellationToken
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

            if(movedNode.Slug == settings.DefaultPageSlug)
            {
                result = new PageActionResult(false, sr["Moving the default/home page is not allowed"]);
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
                    await pageCommands.Update(movedNode.ProjectId, movedNode);
                    await SortChildPages(targetNode.Id);

                    break;

                case "before":
                    // put this page before the target page beneath the same parent as the target
                    if (targetNode.ParentId != movedNode.ParentId)
                    {
                        movedNode.ParentId = targetNode.ParentId;
                        movedNode.ParentSlug = targetNode.ParentSlug;
                        movedNode.PageOrder = targetNode.PageOrder - 1;
                        await pageCommands.Update(movedNode.ProjectId, movedNode);
                        await SortChildPages(targetNode.ParentId);

                    }
                    else
                    {
                        //parent did not change just sort
                        // set sort and re-sort
                        movedNode.PageOrder = targetNode.PageOrder - 1;
                        await pageCommands.Update(movedNode.ProjectId, movedNode);
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
                        await pageCommands.Update(movedNode.ProjectId, movedNode);
                        await SortChildPages(targetNode.ParentId);
                    }
                    else
                    {
                        //parent did not change just sort
                        movedNode.PageOrder = targetNode.PageOrder + 1;
                        await pageCommands.Update(movedNode.ProjectId, movedNode);
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
                await pageCommands.Update(child.ProjectId, child);
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
                await pageCommands.Update(child.ProjectId, child);
                i += 2;
            }

            ClearNavigationCache();
            return new PageActionResult(true, "operation succeeded");

        }

        public async Task<string> GetPageTreeJson(ClaimsPrincipal user, string node = "root")
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

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);

            var comma = string.Empty;
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var p in list)
            {
                sb.Append(comma);
                await BuildPageJson(user, sb, p, urlHelper);
                comma = ",";
            }
            sb.Append("]");

            return sb.ToString();
        }

        private async Task BuildPageJson(ClaimsPrincipal user, StringBuilder script, IPage page, IUrlHelper urlHelper)
        {
            var childPagesCount = await pageQueries.GetChildPageCount(page.ProjectId, page.Id, true, CancellationToken); 
            script.Append("{");
            script.Append("\"id\":" + "\"" + page.Id + "\"");
            script.Append(",\"slug\":" + "\"" + page.Slug + "\"");
            script.Append(",\"label\":\"" + Encode(page.Title) + "\"");
            script.Append(",\"url\":\"" + ResolveUrl(page, urlHelper) + "\"");
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
                if (page.Slug == settings.DefaultPageSlug)
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
            if(!string.IsNullOrEmpty(page.ExternalUrl)) return sr["Link Only"];

            if (!page.IsPublished) return sr["Unpublished"];
            if (page.PubDate > DateTime.UtcNow) return sr["Future Published"];
            if (string.IsNullOrEmpty(page.ViewRoles) || page.ViewRoles.Contains("All Users")) return sr["Public"];
            return sr["Protected"];
        }

        private string ResolveUrl(IPage page, IUrlHelper urlHelper)
        {
            if(page.Slug == this.settings.DefaultPageSlug)
            {
                return urlHelper.RouteUrl(pageRoutes.PageRouteName);
            }

            return urlHelper.RouteUrl(pageRoutes.PageRouteName, new { slug = page.Slug });
        }

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
