// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-03-29
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.SimpleContent.Services
{
    public class PageService : IPageService
    {
        public PageService(
            IProjectService projectService,
            IProjectSecurityResolver security,
            IPageRepository pageRepository,
            IMediaProcessor mediaProcessor,
            IUrlHelper urlHelper,
            IHttpContextAccessor contextAccessor = null)
        {

            this.projectService = projectService;
            this.security = security;
            pageRepo = pageRepository;
            context = contextAccessor?.HttpContext;
            this.mediaProcessor = mediaProcessor;
            this.urlHelper = urlHelper;
            htmlProcessor = new HtmlProcessor();
        }

        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;
        private IUrlHelper urlHelper;
        private IProjectSecurityResolver security;
        private IPageRepository pageRepo;
        private IMediaProcessor mediaProcessor;
        private IProjectService projectService;
        private ProjectSettings settings = null;
        private HtmlProcessor htmlProcessor;

        private async Task<bool> EnsureProjectSettings()
        {
            if (settings != null) { return true; }
            settings = await projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            if (settings != null) { return true; }
            return false;
        }

        
        public Task<string> ResolvePageUrl(Page page)
        {
            //await EnsureBlogSettings().ConfigureAwait(false);
            
            var result = urlHelper.Action("Home", "About", new { slug = page.Slug });

            return Task.FromResult(result);
        }

        public async Task<bool> PageSlugIsAvailable(string slug)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await pageRepo.SlugIsAvailable(
                settings.ProjectId,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> PageSlugIsAvailable(string projectId, string slug)
        {
            return await pageRepo.SlugIsAvailable(
                projectId,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task Save(
            string projectId,
            string userName,
            string password,
            Page page,
            bool isNew,
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

            if (isNew)
            {
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
            }

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

            await pageRepo.Save(projectId, page, isNew).ConfigureAwait(false);
        }

        public async Task Save(
            Page page,
            bool isNew,
            bool publish)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            

            if (isNew)
            {
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
            }

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

            await pageRepo.Save(settings.ProjectId, page, isNew).ConfigureAwait(false);
        }

        public async Task DeletePage(string projectId, string pageId)
        {
            await pageRepo.Delete(projectId, pageId).ConfigureAwait(false);

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

            return await pageRepo.GetPage(
                projectId,
                pageId,
                CancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<Page> GetPage(string pageId)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            return await pageRepo.GetPage(
                settings.ProjectId,
                pageId,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Page> GetPageBySlug(string projectId, string slug)
        {
            return await pageRepo.GetPageBySlug(
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

            return await pageRepo.GetAllPages(
                projectId,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<Page>> GetRootPages()
        {
            await EnsureProjectSettings();
            return await pageRepo.GetRootPages(
                settings.ProjectId,
                CancellationToken
                ).ConfigureAwait(false);
        }

        public async Task<List<Page>> GetChildPages(string pageId)
        {
            await EnsureProjectSettings();
            return await pageRepo.GetChildPages(
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
            return await pageRepo.SlugIsAvailable(
                settings.ProjectId,
                slug,
                CancellationToken
                ).ConfigureAwait(false);
        }

    }
}
