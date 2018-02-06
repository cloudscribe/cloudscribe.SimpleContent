// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2017-10-05
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.SimpleContent.Services
{
    public class BlogService : IBlogService
    {
        public BlogService(
            IProjectService projectService,
            IProjectSecurityResolver security,
            IPostQueries postQueries,
            IPostCommands postCommands,
            IMediaProcessor mediaProcessor,
            IContentProcessor contentProcessor,
            //IHtmlProcessor htmlProcessor,
            IBlogRoutes blogRoutes,
            PostEvents eventHandlers,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            IHttpContextAccessor contextAccessor = null)
        {
            this.security = security;
            this.postQueries = postQueries;
            this.postCommands = postCommands;
            context = contextAccessor?.HttpContext;
            this.mediaProcessor = mediaProcessor;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccesor = actionContextAccesor;
            this.projectService = projectService;
            //this.htmlProcessor = htmlProcessor;
            _contentProcessor = contentProcessor;
            this.blogRoutes = blogRoutes;
            this.eventHandlers = eventHandlers;
        }

        private IProjectService projectService;
        private IProjectSecurityResolver security;
        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;
        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionContextAccesor;
        private IPostQueries postQueries;
        private IPostCommands postCommands;
        private IMediaProcessor mediaProcessor;
        private IProjectSettings settings = null;
        private IContentProcessor _contentProcessor;
        //private IHtmlProcessor htmlProcessor;
        private IBlogRoutes blogRoutes;
        private PostEvents eventHandlers;

        private async Task EnsureBlogSettings()
        {
            if(settings != null) { return; }
            settings = await projectService.GetCurrentProjectSettings().ConfigureAwait(false);    
        }
        
        public async Task<List<IPost>> GetPosts(bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetPosts(
                settings.Id,
                includeUnpublished,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PagedPostResult> GetPosts(
            string category,
            int pageNumber,
            bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetPosts(
                settings.Id,
                category,
                includeUnpublished,
                pageNumber,
                settings.PostsPerPage,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCount(string category, bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetCount(
                settings.Id,
                category,
                includeUnpublished,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCount(
            string projectId,
            int year,
            int month = 0,
            int day = 0,
            bool includeUnpublished = false
            )
        {
            return await postQueries.GetCount(
                projectId,
                year,
                month,
                day,
                includeUnpublished,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPost>> GetRecentPosts(int numberToGet)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetRecentPosts(
                settings.Id,
                numberToGet,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPost>> GetFeaturedPosts(int numberToGet)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetFeaturedPosts(
                settings.Id,
                numberToGet,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPost>> GetRecentPosts(
            string projectId, 
            string userName,
            string password,
            int numberToGet)
        {
            var permission = await security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if(!permission.CanEditPosts)
            {
                return new List<IPost>(); // empty
            }

            
            return await postQueries.GetRecentPosts(
                projectId,
                numberToGet,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PagedPostResult> GetPosts(
            string projectId, 
            int year, 
            int month = 0, 
            int day = 0, 
            int pageNumber = 1, 
            int pageSize = 10,
            bool includeUnpublished = false)
        {
            return await postQueries.GetPosts(projectId, year, month, day, pageNumber, pageSize, includeUnpublished).ConfigureAwait(false);
        }

        public async Task Create(
            string projectId, 
            string userName,
            string password,
            IPost post, 
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
            
            await InitializeNewPosts(projectId, post, publish);

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var imageAbsoluteBaseUrl = urlHelper.Content("~" + settings.LocalMediaVirtualPath);
            if(context != null)
            {
                imageAbsoluteBaseUrl = context.Request.AppBaseUrl() + settings.LocalMediaVirtualPath;
            }

            // open live writer passes in posts with absolute urls
            // we want to change them to relative to keep the files portable
            // to a different root url
            post.Content = await _contentProcessor.ConvertMediaUrlsToRelative(
                settings.LocalMediaVirtualPath,
                imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
                post.Content);
            // olw also adds hard coded style to images
            post.Content = _contentProcessor.RemoveImageStyleAttribute(post.Content);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (post.PubDate == nonPublishedDate)
            {
                post.PubDate = DateTime.UtcNow;
            }

            await postCommands.Create(settings.Id, post).ConfigureAwait(false);
            await eventHandlers.HandleCreated(settings.Id, post).ConfigureAwait(false);
        }

        public async Task Update(
            string projectId,
            string userName,
            string password,
            IPost post,
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
            post.Content = await _contentProcessor.ConvertMediaUrlsToRelative(
                settings.LocalMediaVirtualPath,
                imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
                post.Content);
            // olw also adds hard coded style to images
            post.Content = _contentProcessor.RemoveImageStyleAttribute(post.Content);
            
            var nonPublishedDate = new DateTime(1, 1, 1);
            if (post.PubDate == nonPublishedDate)
            {
                post.PubDate = DateTime.UtcNow;
            }

            await eventHandlers.HandlePreUpdate(settings.Id, post.Id).ConfigureAwait(false);
            await postCommands.Update(settings.Id, post).ConfigureAwait(false);
            await eventHandlers.HandleUpdated(settings.Id, post).ConfigureAwait(false);
        }

        public async Task Create(IPost post)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            //this is no longer needed, we once used bootstrapwysiwyg which passed images as base64 content
            // but we don't use that anymore. now we have ckeditor and filemanager integration
            //post.Content = await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
            //    settings.LocalMediaVirtualPath,
            //    post.Content
            //    ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if(post.PubDate == nonPublishedDate)
            {
                post.PubDate = DateTime.UtcNow;
            }

            await postCommands.Create(settings.Id, post).ConfigureAwait(false);
            await eventHandlers.HandleCreated(settings.Id, post).ConfigureAwait(false);
        }

        public async Task Update(IPost post)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            //this is no longer needed, we once used bootstrapwysiwyg which passed images as base64 content
            // but we don't use that anymore. now we have ckeditor and filemanager integration
            //post.Content = await mediaProcessor.ConvertBase64EmbeddedImagesToFilesWithUrls(
            //    settings.LocalMediaVirtualPath,
            //    post.Content
            //    ).ConfigureAwait(false);

            var nonPublishedDate = new DateTime(1, 1, 1);
            if (post.PubDate == nonPublishedDate)
            {
                post.PubDate = DateTime.UtcNow;
            }

            await eventHandlers.HandlePreUpdate(settings.Id, post.Id).ConfigureAwait(false);
            await postCommands.Update(settings.Id, post).ConfigureAwait(false);
            await eventHandlers.HandleUpdated(settings.Id, post).ConfigureAwait(false);
        }

        public async Task HandlePubDateAboutToChange(IPost post, DateTime newPubDate)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            await postCommands.HandlePubDateAboutToChange(settings.Id, post, newPubDate);
        }

        private async Task InitializeNewPosts(string projectId, IPost post, bool publish)
        {
            if(publish)
            {
                post.PubDate = DateTime.UtcNow;
            }

            if(string.IsNullOrEmpty(post.Slug))
            {
                var slug = CreateSlug(post.Title);
                var available = await SlugIsAvailable(slug);
                if (available)
                {
                    post.Slug = slug;
                }

            }
        }

        

        public async Task<string> ResolvePostUrl(IPost post)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            string postUrl;
            if (settings.IncludePubDateInPostUrls)
            {
                postUrl = urlHelper.RouteUrl(blogRoutes.PostWithDateRouteName,
                    new
                    {
                        year = post.PubDate.Year,
                        month = post.PubDate.Month.ToString("00"),
                        day = post.PubDate.Day.ToString("00"),
                        slug = post.Slug
                    });
            }
            else
            {
                postUrl = urlHelper.RouteUrl(blogRoutes.PostWithoutDateRouteName,
                    new { slug = post.Slug });
            }

            return postUrl;
            
        }

        

        public Task<string> ResolveBlogUrl(IProjectSettings blog)
        {
            //await EnsureBlogSettings().ConfigureAwait(false);

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var result = urlHelper.Action("Index", "Blog");

            return Task.FromResult(result);
        }


        public async Task<IPost> GetPost(string postId)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetPost(
                settings.Id,
                postId,
                CancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<IPost> GetPost(
            string projectId, 
            string postId,
            string userName,
            string password
            )
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
            
            return await postQueries.GetPost(
                projectId,
                postId,
                CancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<PostResult> GetPostBySlug(string slug)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetPostBySlug(
                settings.Id,
                slug,
                CancellationToken)
                .ConfigureAwait(false);

        }

        public string CreateSlug(string title)
        {
            return ContentUtils.CreateSlug(title);
        }

        public async Task<bool> SlugIsAvailable(string slug)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.SlugIsAvailable(
                settings.Id,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> SlugIsAvailable(string projectId, string slug)
        {
            
            return await postQueries.SlugIsAvailable(
                projectId,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }

        

        public async Task Delete(string postId)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            await eventHandlers.HandlePreDelete(settings.Id, postId).ConfigureAwait(false);
            await postCommands.Delete(settings.Id, postId).ConfigureAwait(false);

        }

        public async Task Delete(
            string projectId, 
            string postId,
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
                return; //TODO: exception here?
            }

            await eventHandlers.HandlePreDelete(projectId, postId).ConfigureAwait(false);
            await postCommands.Delete(projectId, postId).ConfigureAwait(false);

        }

        public async Task<Dictionary<string, int>> GetCategories(bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await postQueries.GetCategories(
                settings.Id,
                includeUnpublished,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Dictionary<string, int>> GetCategories(
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
                return new Dictionary<string, int>(); //empty
            }
            var settings = await projectService.GetProjectSettings(projectId).ConfigureAwait(false);

            return await postQueries.GetCategories(
                settings.Id,
                permission.CanEditPosts,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Dictionary<string, int>> GetArchives(bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            
            return await postQueries.GetArchives(
                settings.Id,
                includeUnpublished,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> CommentsAreOpen(IPost post, bool userCanEdit)
        {
            
            await EnsureBlogSettings().ConfigureAwait(false);

            if(settings.DaysToComment == -1) { return true; }
            if(settings.DaysToComment == 0) { return false; }
            if (userCanEdit) { return true; }

            var result = post.PubDate > DateTime.UtcNow.AddDays(-settings.DaysToComment);
            return result;
        }

        /// <summary>
        /// this is only used for processing images added via metaweblog api
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> ResolveMediaUrl(string fileName)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            return await mediaProcessor.ResolveMediaUrl(settings.LocalMediaVirtualPath, fileName).ConfigureAwait(false);
        }

        /// <summary>
        /// this is only used for processing images added via metaweblog api
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task SaveMedia(
            string projectId, 
            string userName,
            string password,
            byte[] bytes, string 
            fileName)
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

            await mediaProcessor.SaveMedia(settings.LocalMediaVirtualPath, fileName, bytes).ConfigureAwait(false);
        }

        
    }
}
