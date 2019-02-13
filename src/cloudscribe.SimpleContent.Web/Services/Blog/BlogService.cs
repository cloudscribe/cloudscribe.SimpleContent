// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-08-20
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using Microsoft.Extensions.Logging;
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
            IPostQueries postQueries,
            IPostCommands postCommands,
            IContentProcessor contentProcessor,
            IBlogUrlResolver blogUrlResolver,
            PostEvents eventHandlers,
            IContentHistoryCommands historyCommands,
            ILogger<BlogService> logger
           )
        {
            _postQueries = postQueries;
            _postCommands = postCommands;
            _projectService = projectService;
            _contentProcessor = contentProcessor;
            _blogUrlResolver = blogUrlResolver;
            _eventHandlers = eventHandlers;
            _historyCommands = historyCommands;
            _log = logger;
        }

        private readonly IProjectService _projectService;
        private readonly IPostQueries _postQueries;
        private readonly IPostCommands _postCommands;
        private IProjectSettings _settings = null;
        private readonly IContentProcessor _contentProcessor;
        private readonly IBlogUrlResolver _blogUrlResolver;
        private readonly PostEvents _eventHandlers;
        private readonly IContentHistoryCommands _historyCommands;
        private readonly ILogger _log;

        private async Task EnsureBlogSettings()
        {
            if(_settings != null) { return; }
            _settings = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);    
        }

        public async Task PublishReadyDrafts(CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings();
            var drafts = await _postQueries.GetPostsReadyForPublish(_settings.Id, cancellationToken);
            foreach (var post in drafts)
            {
                post.Content = post.DraftContent;
                post.Author = post.DraftAuthor;
                post.PubDate = post.DraftPubDate.Value;
                post.SerializedModel = post.DraftSerializedModel;
                post.IsPublished = true;

                post.DraftAuthor = null;
                post.DraftContent = null;
                post.DraftSerializedModel = null;
                post.DraftPubDate = null;

                await Update(post);

                await _eventHandlers.HandlePublished(_settings.Id, post).ConfigureAwait(false);
                await _historyCommands.DeleteDraftHistory(_settings.Id, post.Id).ConfigureAwait(false);
                
                _log.LogDebug($"auto published draft for post {post.Title}");
            }
        }

        public async Task<List<IPost>> GetPosts(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetPosts(
                _settings.Id,
                includeUnpublished,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PagedPostResult> GetPosts(
            string category,
            int pageNumber,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetPosts(
                _settings.Id,
                category,
                includeUnpublished,
                pageNumber,
                _settings.PostsPerPage,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCount(string category, bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetCount(
                _settings.Id,
                category,
                includeUnpublished,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCount(
            string projectId,
            int year,
            int month = 0,
            int day = 0,
            bool includeUnpublished = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return await _postQueries.GetCount(
                projectId,
                year,
                month,
                day,
                includeUnpublished,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPost>> GetRecentPosts(int numberToGet, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetRecentPosts(
                _settings.Id,
                numberToGet,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<IPost>> GetFeaturedPosts(int numberToGet, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetFeaturedPosts(
                _settings.Id,
                numberToGet,
                cancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<PagedPostResult> GetPosts(
            string projectId, 
            int year, 
            int month = 0, 
            int day = 0, 
            int pageNumber = 1, 
            int pageSize = 10,
            bool includeUnpublished = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return await _postQueries.GetPosts(projectId, year, month, day, pageNumber, pageSize, includeUnpublished, cancellationToken).ConfigureAwait(false);
        }

        public async Task FirePublishEvent(IPost post)
        {
            await _eventHandlers.HandlePublished(post.BlogId, post);
        }

        public async Task FireUnPublishEvent(IPost post)
        {
            await _eventHandlers.HandleUnPublished(post.BlogId, post);
        }

        public async Task Create(IPost post)
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            //if(convertToRelativeUrls)
            //{
            //    await _blogUrlResolver.ConvertToRelativeUrls(post, _settings).ConfigureAwait(false);
            //}

            if (string.IsNullOrEmpty(post.Slug))
            {
                var slug = CreateSlug(post.Title);
                var available = await SlugIsAvailable(slug);
                while (!available)
                {
                    slug = slug + "-";
                    available = await SlugIsAvailable(slug);
                }
                if (available)
                {
                    post.Slug = slug;
                }
            }

            await _postCommands.Create(_settings.Id, post).ConfigureAwait(false);
            await _eventHandlers.HandleCreated(_settings.Id, post).ConfigureAwait(false);
        }
        
        public async Task Update(IPost post)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            await _eventHandlers.HandlePreUpdate(_settings.Id, post.Id).ConfigureAwait(false);

            //if (convertToRelativeUrls)
            //{
            //    await _blogUrlResolver.ConvertToRelativeUrls(post, _settings).ConfigureAwait(false);
            //}

            await _postCommands.Update(_settings.Id, post).ConfigureAwait(false);
            await _eventHandlers.HandleUpdated(_settings.Id, post).ConfigureAwait(false);
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
        
        public async Task<IPost> GetPost(string postId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetPost(
                _settings.Id,
                postId,
                cancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<PostResult> GetPostBySlug(string slug, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetPostBySlug(
                _settings.Id,
                slug,
                cancellationToken)
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
                CancellationToken.None)
                .ConfigureAwait(false);
        }
        
        public async Task<bool> SlugIsAvailable(string projectId, string slug)
        { 
            return await _postQueries.SlugIsAvailable(
                projectId,
                slug,
                CancellationToken.None)
                .ConfigureAwait(false);
        }

        

        public async Task Delete(string postId)
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            await _eventHandlers.HandlePreDelete(_settings.Id, postId).ConfigureAwait(false);
            await _postCommands.Delete(_settings.Id, postId).ConfigureAwait(false);

        }
        
        public async Task<Dictionary<string, int>> GetCategories(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);

            return await _postQueries.GetCategories(
                _settings.Id,
                includeUnpublished,
                cancellationToken)
                .ConfigureAwait(false);
        }
        
        public async Task<Dictionary<string, int>> GetArchives(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureBlogSettings().ConfigureAwait(false);
            
            return await _postQueries.GetArchives(
                _settings.Id,
                includeUnpublished,
                cancellationToken)
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
        
        ///// <summary>
        ///// this is only used for processing images added via metaweblog api
        ///// </summary>
        ///// <param name="projectId"></param>
        ///// <param name="bytes"></param>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public async Task SaveMedia(
        //    string projectId, 
        //    byte[] bytes, string 
        //    fileName)
        //{          
        //    var settings = await _projectService.GetProjectSettings(projectId).ConfigureAwait(false);
        //    await _mediaProcessor.SaveMedia(settings.LocalMediaVirtualPath, fileName, bytes).ConfigureAwait(false);
        //}

        
    }
}
