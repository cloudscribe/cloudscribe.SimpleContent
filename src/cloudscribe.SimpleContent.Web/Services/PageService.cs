// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2017-04-21
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

        public async Task<bool> SlugIsAvailable(string projectId,string slug)
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
                var available = await SlugIsAvailable(projectId, slug);
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
                var available = await SlugIsAvailable(this.settings.Id, slug);
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
            
            page.Content = await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
                settings.LocalMediaVirtualPath,
                page.Content
                ).ConfigureAwait(false);

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
            
            page.Content = await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
                settings.LocalMediaVirtualPath,
                page.Content
                ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (page.PubDate == nonPublishedDate)
            {
                page.PubDate = DateTime.UtcNow;
            }

            await eventHandlers.HandlePreUpdate(settings.Id, page.Id).ConfigureAwait(false);
            await pageCommands.Update(settings.Id, page).ConfigureAwait(false);
            await eventHandlers.HandleUpdated(settings.Id, page).ConfigureAwait(false);
        }

        public async Task DeletePage(string projectId, string pageId)
        {
            await eventHandlers.HandlePreDelete(projectId, pageId).ConfigureAwait(false);
            await pageCommands.Delete(projectId, pageId).ConfigureAwait(false);
            

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

        public async Task<IPage> GetPageBySlug(string projectId, string slug)
        {
            return await pageQueries.GetPageBySlug(
                projectId,
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

        public async Task<string> GetPageTreeJson(string node = "root")
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
                await BuildPageJson(sb, p, urlHelper);
                comma = ",";
            }
            sb.Append("]");

            return sb.ToString();
        }

        private async Task BuildPageJson(StringBuilder script, IPage page, IUrlHelper urlHelper)
        {
            var childPages = await GetChildPages(page.Id); // TODO: this is not effecient
            script.Append("{");
            script.Append("\"id\":" + "\"" + page.Id + "\"");
            script.Append(",\"slug\":" + "\"" + page.Slug + "\"");
            script.Append(",\"label\":\"" + Encode(page.Title) + "\"");
            script.Append(",\"url\":\"" + ResolveUrl(page, urlHelper) + "\"");
            //script.Append(",\"isRoot\":false");
            script.Append(",\"parentId\":" + "\"" + page.ParentId + "\"");
            script.Append(",\"childcount\":" + childPages.Count.ToString());
            script.Append(",\"children\":[");
            script.Append("]");
            if (childPages.Count > 0)
            {
                script.Append(",\"load_on_demand\":true");
            }

            //if (canEdit)
            //{
                script.Append(",\"canEdit\":true");
            //}
            //else
            //{
            //    script.Append(",\"canEdit\":false");
            //}


            //if (canDelete)
            //{
                script.Append(",\"canDelete\":true");
            //}
            //else
            //{
             //   script.Append(",\"canDelete\":false");
            //}

            //if (canCreateChildPages)
            //{
                script.Append(",\"canCreateChild\":true");
           // }
            //else
            //{
           //     script.Append(",\"canCreateChild\":false");
           // }

            if (string.IsNullOrEmpty(page.ViewRoles) || page.ViewRoles.Contains("All Users"))
            {
                script.Append(",\"protection\":\"Public\"");
            }
            else
            {
                script.Append(",\"protection\":\"Protected \"");
            }

            script.Append("}");

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
