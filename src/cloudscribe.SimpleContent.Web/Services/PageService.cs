// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-08-01
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
        private ProjectSettings settings = null;
        private HtmlProcessor htmlProcessor;
        private IMemoryCache cache;

        private async Task<bool> EnsureProjectSettings()
        {
            if (settings != null) { return true; }
            settings = await projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            if (settings != null) { return true; }
            return false;
        }

        public void ClearNavigationCache()
        {
            var cacheKey = "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder";
            cache.Remove(cacheKey);
            cacheKey = "cloudscribe.Web.Navigation.XmlNavigationTreeBuilder";
            cache.Remove(cacheKey);
            cacheKey = "JsonNavigationTreeBuilder";
            cache.Remove(cacheKey);
        }

        public Task<string> ResolvePageUrl(Page page)
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
                settings.ProjectId,
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
            Page page,
            bool publish)
        {
            var permission = await security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEdit)
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
            Page page,
            bool publish)
        {
            var permission = await security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEdit)
            {
                return;
            }

            var settings = await projectService.GetProjectSettings(projectId).ConfigureAwait(false);

            //if (isNew)
            //{
            //    if (publish)
            //    {
            //        page.PubDate = DateTime.UtcNow;
            //    }

            //    if (string.IsNullOrEmpty(page.Slug))
            //    {
            //        var slug = ContentUtils.CreateSlug(page.Title);
            //        var available = await PageSlugIsAvailable(slug);
            //        if (available)
            //        {
            //            page.Slug = slug;
            //        }

            //    }
            //}

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
            Page page,
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

            await pageCommands.Create(settings.ProjectId, page).ConfigureAwait(false);
        }

        public async Task Update(
            Page page,
            bool publish)
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            
            //if (isNew)
            //{
            //    if (publish)
            //    {
            //        page.PubDate = DateTime.UtcNow;
            //    }

            //    if (string.IsNullOrEmpty(page.Slug))
            //    {
            //        var slug = ContentUtils.CreateSlug(page.Title);
            //        var available = await PageSlugIsAvailable(slug);
            //        if (available)
            //        {
            //            page.Slug = slug;
            //        }

            //    }
            //}
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

            await pageCommands.Update(settings.ProjectId, page).ConfigureAwait(false);
        }

        public async Task DeletePage(string projectId, string pageId)
        {
            await pageCommands.Delete(projectId, pageId).ConfigureAwait(false);

        }

        public async Task<Page> GetPage(
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

            if (!permission.CanEdit)
            {
                return null;
            }

            return await pageQueries.GetPage(
                projectId,
                pageId,
                CancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<Page> GetPage(string pageId)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await pageQueries.GetPage(
                settings.ProjectId,
                pageId,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Page> GetPageBySlug(string projectId, string slug)
        {
            return await pageQueries.GetPageBySlug(
                projectId,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<Page>> GetAllPages(
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

            if (!permission.CanEdit)
            {
                return new List<Page>(); // empty
            }

            return await pageQueries.GetAllPages(
                projectId,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<Page>> GetRootPages()
        {
            await EnsureProjectSettings();
            return await pageQueries.GetRootPages(
                settings.ProjectId,
                CancellationToken
                ).ConfigureAwait(false);
        }

        public async Task<List<Page>> GetChildPages(string pageId)
        {
            await EnsureProjectSettings();
            return await pageQueries.GetChildPages(
                settings.ProjectId,
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
                settings.ProjectId,
                slug,
                CancellationToken
                ).ConfigureAwait(false);
        }

    }
}
