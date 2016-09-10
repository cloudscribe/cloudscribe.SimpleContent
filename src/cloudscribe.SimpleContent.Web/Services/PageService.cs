// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-09-08
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace cloudscribe.SimpleContent.Services
{
    public class PageService : IPageService
    {
        public PageService(
            IProjectService projectService,
            IProjectSecurityResolver security,
            IPageQueries pageQueries,
            IPageCommands pageCommands,
            IMediaProcessor mediaProcessor,
            IUrlHelperFactory urlHelperFactory,
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
            htmlProcessor = new HtmlProcessor();
            this.cache = cache;
            this.cacheKeys = cacheKeys;
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
        private HtmlProcessor htmlProcessor;
        private IMemoryCache cache;
        private IPageNavigationCacheKeys cacheKeys;

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

        public async Task<bool> PageSlugIsAvailable(string slug)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await pageQueries.SlugIsAvailable(
                settings.Id,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> PageSlugIsAvailable(string projectId, string slug)
        {
            return await pageQueries.SlugIsAvailable(
                projectId,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
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
                var available = await PageSlugIsAvailable(slug);
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

            await pageCommands.Update(projectId, page).ConfigureAwait(false);
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
                var available = await PageSlugIsAvailable(slug);
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
            
            await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
                settings.LocalMediaVirtualPath,
                page
                ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (page.PubDate == nonPublishedDate)
            {
                page.PubDate = DateTime.UtcNow;
            }

            await pageCommands.Create(settings.Id, page).ConfigureAwait(false);
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
            
            await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
                settings.LocalMediaVirtualPath,
                page
                ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (page.PubDate == nonPublishedDate)
            {
                page.PubDate = DateTime.UtcNow;
            }

            await pageCommands.Update(settings.Id, page).ConfigureAwait(false);
        }

        public async Task DeletePage(string projectId, string pageId)
        {
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

        public async Task<bool> SlugIsAvailable(
            string projectId,
            string slug
            )
        {
            await EnsureProjectSettings();
            return await pageQueries.SlugIsAvailable(
                settings.Id,
                slug,
                CancellationToken
                ).ConfigureAwait(false);
        }

    }
}
