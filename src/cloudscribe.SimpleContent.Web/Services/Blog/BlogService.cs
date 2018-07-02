// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-06-28
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
            _security = security;
            _postQueries = postQueries;
            _postCommands = postCommands;
            context = contextAccessor?.HttpContext;
            _mediaProcessor = mediaProcessor;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _projectService = projectService;
            _contentProcessor = contentProcessor;
            _blogRoutes = blogRoutes;
            _eventHandlers = eventHandlers;
        }

        private IProjectService _projectService;
        private IProjectSecurityResolver _security;
        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;
        private IUrlHelperFactory _urlHelperFactory;
        private IActionContextAccessor _actionContextAccesor;
        private IPostQueries _postQueries;
        private IPostCommands _postCommands;
        private IMediaProcessor _mediaProcessor;
        private IProjectSettings _settings = null;
        private IContentProcessor _contentProcessor;
        private IBlogRoutes _blogRoutes;
        private PostEvents _eventHandlers;

        private async Task EnsureBlogSettings()
        {
            if(_settings != null) { return; }
            _settings = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);    
        }
        
        public async Task<List<IPost>> GetPosts(bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetPosts(
                _settings.Id,
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

            return await _postQueries.GetPosts(
                _settings.Id,
                category,
                includeUnpublished,
                pageNumber,
                _settings.PostsPerPage,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCount(string category, bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetCount(
                _settings.Id,
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
            return await _postQueries.GetCount(
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

            return await _postQueries.GetRecentPosts(
                _settings.Id,
                numberToGet,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPost>> GetFeaturedPosts(int numberToGet)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetFeaturedPosts(
                _settings.Id,
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
            var permission = await _security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if(!permission.CanEditPosts)
            {
                return new List<IPost>(); // empty
            }

            
            return await _postQueries.GetRecentPosts(
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
            return await _postQueries.GetPosts(projectId, year, month, day, pageNumber, pageSize, includeUnpublished).ConfigureAwait(false);
        }

        public async Task Create(
            string projectId, 
            string userName,
            string password,
            IPost post, 
            bool publish)
        {
            var permission = await _security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return; 
            }

            var settings = await _projectService.GetProjectSettings(projectId).ConfigureAwait(false);
            
            await InitializeNewPosts(projectId, post, publish);

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
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

            await _postCommands.Create(settings.Id, post).ConfigureAwait(false);
            await _eventHandlers.HandleCreated(settings.Id, post).ConfigureAwait(false);
        }

        public async Task Update(
            string projectId,
            string userName,
            string password,
            IPost post,
            bool publish)
        {
            var permission = await _security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return;
            }

            var settings = await _projectService.GetProjectSettings(projectId).ConfigureAwait(false);
            
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
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

            await _eventHandlers.HandlePreUpdate(settings.Id, post.Id).ConfigureAwait(false);
            await _postCommands.Update(settings.Id, post).ConfigureAwait(false);
            await _eventHandlers.HandleUpdated(settings.Id, post).ConfigureAwait(false);
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

            await _postCommands.Create(_settings.Id, post).ConfigureAwait(false);
            await _eventHandlers.HandleCreated(_settings.Id, post).ConfigureAwait(false);
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

            //var nonPublishedDate = new DateTime(1, 1, 1);
            //if (post.PubDate == nonPublishedDate)
            //{
            //    post.PubDate = DateTime.UtcNow;
            //}

            await _eventHandlers.HandlePreUpdate(_settings.Id, post.Id).ConfigureAwait(false);
            await _postCommands.Update(_settings.Id, post).ConfigureAwait(false);
            await _eventHandlers.HandleUpdated(_settings.Id, post).ConfigureAwait(false);
        }

        //public async Task HandlePubDateAboutToChange(IPost post, DateTime newPubDate)
        //{
        //    await EnsureBlogSettings().ConfigureAwait(false);

        //    await _postCommands.HandlePubDateAboutToChange(_settings.Id, post, newPubDate);
        //}

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
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            string postUrl;
            if (_settings.IncludePubDateInPostUrls)
            {
                DateTime? pubDate = post.PubDate;
                if(!pubDate.HasValue)
                {
                    pubDate = DateTime.UtcNow;
                }
                postUrl = urlHelper.RouteUrl(_blogRoutes.PostWithDateRouteName,
                    new
                    {
                        year = pubDate.Value.Year,
                        month = pubDate.Value.Month.ToString("00"),
                        day = pubDate.Value.Day.ToString("00"),
                        slug = post.Slug
                    });
            }
            else
            {
                postUrl = urlHelper.RouteUrl(_blogRoutes.PostWithoutDateRouteName,
                    new { slug = post.Slug });
            }

            return postUrl;
            
        }

        

        public Task<string> ResolveBlogUrl(IProjectSettings blog)
        {
            //await EnsureBlogSettings().ConfigureAwait(false);

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            var result = urlHelper.Action("Index", "Blog");

            return Task.FromResult(result);
        }


        public async Task<IPost> GetPost(string postId)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetPost(
                _settings.Id,
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

            var permission = await _security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return null;
            }
            
            return await _postQueries.GetPost(
                projectId,
                postId,
                CancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<PostResult> GetPostBySlug(string slug)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetPostBySlug(
                _settings.Id,
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

            return await _postQueries.SlugIsAvailable(
                _settings.Id,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> SlugIsAvailable(string projectId, string slug)
        {
            
            return await _postQueries.SlugIsAvailable(
                projectId,
                slug,
                CancellationToken)
                .ConfigureAwait(false);
        }

        

        public async Task Delete(string postId)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            await _eventHandlers.HandlePreDelete(_settings.Id, postId).ConfigureAwait(false);
            await _postCommands.Delete(_settings.Id, postId).ConfigureAwait(false);

        }

        public async Task Delete(
            string projectId, 
            string postId,
            string userName,
            string password)
        {
            var permission = await _security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return; //TODO: exception here?
            }

            await _eventHandlers.HandlePreDelete(projectId, postId).ConfigureAwait(false);
            await _postCommands.Delete(projectId, postId).ConfigureAwait(false);

        }

        public async Task<Dictionary<string, int>> GetCategories(bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetCategories(
                _settings.Id,
                includeUnpublished,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Dictionary<string, int>> GetCategories(
            string projectId, 
            string userName,
            string password)
        {
            var permission = await _security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return new Dictionary<string, int>(); //empty
            }
            var settings = await _projectService.GetProjectSettings(projectId).ConfigureAwait(false);

            return await _postQueries.GetCategories(
                settings.Id,
                permission.CanEditPosts,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Dictionary<string, int>> GetArchives(bool includeUnpublished)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            
            return await _postQueries.GetArchives(
                _settings.Id,
                includeUnpublished,
                CancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> CommentsAreOpen(IPost post, bool userCanEdit)
        {
            
            await EnsureBlogSettings().ConfigureAwait(false);

            if(_settings.DaysToComment == -1) { return true; }
            if(_settings.DaysToComment == 0) { return false; }
            if (userCanEdit) { return true; }

            var result = post.PubDate > DateTime.UtcNow.AddDays(-_settings.DaysToComment);
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
            return await _mediaProcessor.ResolveMediaUrl(_settings.LocalMediaVirtualPath, fileName).ConfigureAwait(false);
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
            var permission = await _security.ValidatePermissions(
                projectId,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                return;
            }

            var settings = await _projectService.GetProjectSettings(projectId).ConfigureAwait(false);

            await _mediaProcessor.SaveMedia(settings.LocalMediaVirtualPath, fileName, bytes).ConfigureAwait(false);
        }

        
    }
}
