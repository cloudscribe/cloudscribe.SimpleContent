// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2018-10-23
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.MetaWeblog;
using cloudscribe.MetaWeblog.Models;
using cloudscribe.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace cloudscribe.SimpleContent.MetaWeblog
{
    public class MetaWeblogService : IMetaWeblogService
    {

        public MetaWeblogService(
            IProjectService projectService,
            IProjectSecurityResolver security,
            IContentHistoryCommands contentHistoryCommands,
            IPageUrlResolver pageUrlResolver,
            IBlogUrlResolver blogUrlResolver,
            IMediaProcessor mediaProcessor,
            ITimeZoneHelper timeZoneHelper,
            ILogger<MetaWeblogService> logger,
            IBlogService blogService = null,
            IPageService pageService = null
            )
        {
            _projectService = projectService;
            _security = security;
            _contentHistoryCommands = contentHistoryCommands;
            _pageUrlResolver = pageUrlResolver;
            _blogUrlResolver = blogUrlResolver;
            _timeZoneHelper = timeZoneHelper;
            _blogService = blogService ?? new NotImplementedBlogService();
            _pageService = pageService ?? new NotImplementedPageService();
            _mediaProcessor = mediaProcessor;
            _mapper = new MetaWeblogModelMapper();
            _log = logger;
            
        }

        private readonly IProjectService _projectService;
        private readonly IProjectSecurityResolver _security;
        private readonly IPageUrlResolver _pageUrlResolver;
        private readonly IBlogUrlResolver _blogUrlResolver;
        private readonly IMediaProcessor _mediaProcessor;
        private readonly IContentHistoryCommands _contentHistoryCommands;
        private readonly IBlogService _blogService;
        private readonly IPageService _pageService;
        private readonly MetaWeblogModelMapper _mapper;
        private readonly ITimeZoneHelper _timeZoneHelper;
        private readonly ILogger _log;

        public async Task<string> NewPost(
            string blogId,
            string userName,
            string password,
            PostStruct newPost,
            bool publish,
            string authorDisplayName
            )
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                _log.LogWarning($"rejecting new post because user {userName} cannot edit posts");
                return null;
            }

            var post = _mapper.GetPostFromStruct(newPost);
            post.BlogId = blogId;
            post.Id = Guid.NewGuid().ToString();
            post.Author = authorDisplayName;
            post.CreatedByUser = permission.DisplayName;
            var utcPubDate = _timeZoneHelper.ConvertToUtc(newPost.postDate, permission.TimeZoneId);
            
            if (publish)
            {
                if(utcPubDate.Year == 1)
                {
                    //invalid because not supplied
                    utcPubDate = DateTime.UtcNow;
                }

                if(utcPubDate <= DateTime.UtcNow)
                {
                    post.IsPublished = true;
                    post.PubDate = utcPubDate;
                }
                else
                {
                    //future date needs to be draft, it will auto publish after pub date
                    post.DraftAuthor = post.Author;
                    post.DraftContent = post.Content;
                    post.DraftPubDate = utcPubDate;
                    post.IsPublished = false;
                    post.PubDate = null;
                    post.Content = null;
                }
                
                
            }
            else
            {
                post.DraftAuthor = post.Author;
                post.DraftContent = post.Content;
                if(utcPubDate > DateTime.UtcNow)
                {
                    post.DraftPubDate = utcPubDate;
                }
                
                post.Content = null;
                post.PubDate = null;
            }
            
            await _blogUrlResolver.ConvertMediaToRelativeUrls(post).ConfigureAwait(false);

            await _blogService.Create(post).ConfigureAwait(false);
            if (publish)
            {
                await _blogService.FirePublishEvent(post).ConfigureAwait(false);
            }

            return post.Id; 
        }

        public async Task<bool> EditPost(
            string blogId,
            string postId,
            string userName,
            string password,
            PostStruct post,
            bool publish)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                _log.LogWarning($"rejecting new post because user {userName} cannot edit posts");
                return false;
            }

            var existing = await _blogService.GetPost(postId).ConfigureAwait(false);
            
            if (existing == null)
            {
                _log.LogError($"post not found for id {postId}");
                return false;
            }
            
            if(!string.IsNullOrWhiteSpace(existing.TemplateKey))
            {
                //_log.LogError($"post {postId} uses a content template and cannot be edited via metaweblog api");
                throw new InvalidOperationException("templated posts cannot be edited via metaweblog api");
            }
            if (existing.ContentType != ProjectConstants.HtmlContentType)
            {
                //_log.LogError($"post {postId} uses a content type {existing.ContentType} and cannot be edited via metaweblog api");
                throw new InvalidOperationException("posts that have content type other than html cannot be edited via metaweblog api");
            }

            var history = existing.CreateHistory(userName);

            var update = _mapper.GetPostFromStruct(post);

            existing.Title = update.Title;
            existing.MetaDescription = update.MetaDescription;
            existing.LastModified = DateTime.UtcNow;
            existing.LastModifiedByUser = permission.DisplayName;
           
            if (!string.Equals(existing.Slug, update.Slug, StringComparison.OrdinalIgnoreCase))
            {
                // slug changed make sure the new slug is available
                var requestedSlug = ContentUtils.CreateSlug(update.Slug);
                var available = await _blogService.SlugIsAvailable(blogId, requestedSlug).ConfigureAwait(false);

                if (available)
                {
                    existing.Slug = requestedSlug;
                }
            }
               
            existing.Categories = update.Categories;
            var utcPubDate = _timeZoneHelper.ConvertToUtc(post.postDate, permission.TimeZoneId);

            if (publish)
            {
                if (utcPubDate <= DateTime.UtcNow)
                {
                    existing.Content = update.Content;
                    existing.IsPublished = true;
                    if (utcPubDate.Year != 1) // no date specified
                    {
                        existing.PubDate = utcPubDate;
                    }
                    if(!existing.PubDate.HasValue)
                    {
                        existing.PubDate = DateTime.UtcNow;
                    }
                        
                    existing.DraftAuthor = null;
                    existing.DraftContent = null;
                    existing.DraftPubDate = null;
                }
                else
                {
                    existing.DraftContent = update.Content;
                    existing.DraftPubDate = utcPubDate;
                    existing.DraftAuthor = update.Author;
                }       
            }
            else
            {
                existing.DraftContent = update.Content;
                existing.DraftAuthor = update.Author;
                if(utcPubDate > DateTime.UtcNow)
                {
                    existing.DraftPubDate = utcPubDate;
                }
                
            }

            await _blogUrlResolver.ConvertMediaToRelativeUrls(existing).ConfigureAwait(false);
            await _blogService.Update(existing).ConfigureAwait(false);
            await _contentHistoryCommands.Create(blogId, history).ConfigureAwait(false);

            if(publish)
            {
                await _blogService.FirePublishEvent(existing).ConfigureAwait(false);
                await _contentHistoryCommands.DeleteDraftHistory(blogId, history.ContentId).ConfigureAwait(false);
            }

            return true;
        }

        public async Task<PostStruct> GetPost(
            string blogId,
            string postId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if (!permission.CanEditPosts)
            {
                _log.LogWarning($"user {userName} cannot edit posts");
                return new PostStruct(); 
            }

            var existing = await _blogService.GetPost(postId, cancellationToken).ConfigureAwait(false);
            
            if (existing == null) { return new PostStruct(); }

            

            var commentsOpen = await _blogService.CommentsAreOpen(existing, false).ConfigureAwait(false);
            var project = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            if(project == null)
            {
                _log.LogError($"could not resolve project settings");
                return new PostStruct();
            }
            await _blogUrlResolver.ConvertMediaToAbsoluteUrls(existing, project).ConfigureAwait(false);

            var postUrl = await _blogUrlResolver.ResolvePostUrl(existing, project).ConfigureAwait(false);
            var postStruct = _mapper.GetStructFromPost(existing,postUrl, commentsOpen);
            return postStruct;
        }

        public async Task<List<CategoryStruct>> GetCategories(
            string blogId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            List<CategoryStruct> output = new List<CategoryStruct>();

            if(permission.CanEditPosts)
            {
                var cats = await _blogService.GetCategories(permission.CanEditPosts, cancellationToken).ConfigureAwait(false);
                foreach (var c in cats)
                {
                    CategoryStruct s = new CategoryStruct { title = c.Key };
                    output.Add(s);
                }
            }
            else
            {
                _log.LogWarning($"user {userName} cannot edit posts");
            }
            
            return output;
        }

        public async Task<List<PostStruct>> GetRecentPosts(
            string blogId,
            string userName,
            string password,
            int numberOfPosts,
            CancellationToken cancellationToken)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            var structs = new List<PostStruct>();

            if(permission.CanEditPosts)
            {
                var blogPosts = await _blogService.GetRecentPosts(numberOfPosts, cancellationToken).ConfigureAwait(false);
                var project = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
                if(project == null)
                {
                    _log.LogError($"could not resolve project settings");
                    return structs;
                }
                
                foreach (var p in blogPosts)
                {
                    var commentsOpen = await _blogService.CommentsAreOpen(p, false).ConfigureAwait(false);
                    var postUrl = await _blogUrlResolver.ResolvePostUrl(p, project).ConfigureAwait(false);
                    var s = _mapper.GetStructFromPost(p, postUrl, commentsOpen);
                    structs.Add(s);
                }
            }
            else
            {
                _log.LogWarning($"user {userName} cannot edit posts");
            }
            
            return structs;
        }

        public async Task<MediaInfoStruct> NewMediaObject(
            string blogId,
            string userName,
            string password,
            MediaObjectStruct mediaObject)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if(!permission.CanEditPosts)
            {
                _log.LogWarning($"user {userName} cannot edit posts");
                return new MediaInfoStruct();
            }

            string extension = Path.GetExtension(mediaObject.name);
            string fileName = Path.GetFileName(mediaObject.name).ToLowerInvariant().Replace("_thumb", "-wlw");
            var project = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            if(project == null)
            {
                _log.LogError("failed to resolve proejct settings");
                return new MediaInfoStruct();
            }

            //https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/345
            var currentTime = DateTime.UtcNow;
            var storageVirtualPath = "/media/images/" + currentTime.Year.ToInvariantString()
                + "/" + currentTime.Month.ToString("00")
                + "/" + currentTime.Day.ToString("00")
                + "/"
                ;
            
            await _mediaProcessor.SaveMedia(storageVirtualPath, fileName, mediaObject.bytes).ConfigureAwait(false);

            var mediaUrl = await _mediaProcessor.ResolveMediaUrl(storageVirtualPath, fileName).ConfigureAwait(false);
            var result = new MediaInfoStruct() { url = mediaUrl };

            return result;
        }

        public async Task<bool> DeletePost(
            string blogId, 
            string postId,
            string userName,
            string password
            )
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if(!permission.CanEditPosts)
            {
                _log.LogWarning($"user {userName} cannot edit posts");
                return false;
            }

            var post = await _blogService.GetPost(postId).ConfigureAwait(false);
            if(post == null)
            {
                _log.LogError($"post was null for id {postId}");

                return false;
            }

            var history = post.CreateHistory(userName);
            await _contentHistoryCommands.Create(blogId, history).ConfigureAwait(false);

            await _blogService.Delete(postId).ConfigureAwait(false);

            return true;
        }
        
        public async Task<List<BlogInfoStruct>> GetUserBlogs(
            string key, 
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var result = new List<BlogInfoStruct>();

            var permission = await _security.ValidatePermissions(
                string.Empty,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);
            
            if (!permission.CanEditPosts)
            {
                _log.LogWarning($"user {userName} cannot edit posts");
                return result; //empty
            }

            var project = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            if(project == null)
            {
                _log.LogError($"could not resolve project settings");
                return result; //empty
            }

            var url = await _blogUrlResolver.ResolveBlogUrl(project).ConfigureAwait(false);
            var b = _mapper.GetStructFromBlog(project, url);
            result.Add(b);
            
            return result;
        }

        public Task<string> NewCategory(
            string blogId, 
            string category,
            string userName,
            string password
            )
        {
            //TODO: implement

            throw new NotImplementedException();
        }

        public async Task<List<PageStruct>> GetPages(
            string blogId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var list = new List<PageStruct>();

            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if(!permission.CanEditPages)
            {
                _log.LogWarning($"user {userName} cannot edit pages");
                return list;
            }

            var pages = await _pageService.GetAllPages(blogId, cancellationToken).ConfigureAwait(false);

            foreach (var p in pages)
            {
                if (!string.IsNullOrWhiteSpace(p.ViewRoles)) { continue; }

                var pageUrl = await _pageUrlResolver.ResolvePageUrl(p).ConfigureAwait(false);
                var s = _mapper.GetStructFromPage(p, pageUrl, false);
                list.Add(s);
            }
            
            return list;
        }

        public async Task<List<PageStruct>> GetPageList(
            string blogId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var list = new List<PageStruct>();

            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if (!permission.CanEditPages)
            {
                _log.LogWarning($"user {userName} cannot edit pages");
                return list;
            }

            var pages = await _pageService.GetAllPages(blogId, cancellationToken).ConfigureAwait(false);
            
            foreach (var p in pages)
            {

                if(!string.IsNullOrWhiteSpace(p.ViewRoles)) { continue; }

                var pageUrl = await _pageUrlResolver.ResolvePageUrl(p).ConfigureAwait(false);
                var s = _mapper.GetStructFromPage(p, pageUrl, false);
                list.Add(s);
            }
            
            return list;
        }

        public async Task<PageStruct> GetPage(
            string blogId, 
            string pageId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if(!permission.CanEditPages)
            {
                _log.LogWarning($"user {userName} cannot edit pages");
                return new PageStruct();
            }

            var existing = await _pageService.GetPage(pageId, cancellationToken).ConfigureAwait(false);

            if (existing == null) { return new PageStruct(); }

            var commentsOpen = false;
            var pageUrl = await _pageUrlResolver.ResolvePageUrl(existing).ConfigureAwait(false);
            var project = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            await _pageUrlResolver.ConvertMediaToAbsoluteUrls(existing, project).ConfigureAwait(false);

            var pageStruct = _mapper.GetStructFromPage(existing, pageUrl, commentsOpen);
            return pageStruct;
        }

        public async Task<string> NewPage(
            string blogId,
            string userName,
            string password,
            PageStruct newPage, 
            bool publish)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if (!permission.CanEditPages)
            {
                _log.LogWarning($"user {userName} cannot edit pages");
                return null;
            }

            var page = _mapper.GetPageFromStruct(newPage);

            page.ProjectId = blogId;
            page.Id = Guid.NewGuid().ToString();
            page.CreatedByUser = permission.DisplayName;
            page.LastModifiedByUser = permission.DisplayName;
            if(!string.IsNullOrWhiteSpace(page.ParentId))
            {
                var parent = await _pageService.GetPage(page.ParentId).ConfigureAwait(false);
                if(parent != null)
                {
                    page.ParentSlug = parent.Slug;
                } 
            }
            else
            {
                page.ParentSlug = null;
            }
            if (string.IsNullOrWhiteSpace(newPage.pageOrder))
            {
                page.PageOrder = await _pageService.GetNextChildPageOrder(page.ParentSlug).ConfigureAwait(false);
            }

            var utcPubDate = _timeZoneHelper.ConvertToUtc(newPage.pageDate, permission.TimeZoneId);

            if (publish)
            {
                if (utcPubDate.Year == 1)
                {
                    //invalid because not supplied
                    utcPubDate = DateTime.UtcNow;
                }

                if (utcPubDate < DateTime.UtcNow)
                {
                    page.IsPublished = true;
                    page.PubDate = utcPubDate;
                }
                else
                {
                    //future date needs to be draft, it will auto publish after pub date
                    page.DraftAuthor = page.Author;
                    page.DraftContent = page.Content;
                    page.DraftPubDate = utcPubDate;
                    page.IsPublished = false;
                    page.PubDate = null;
                    page.Content = null;
                }
                
            }
            else
            {
                page.DraftAuthor = page.Author;
                page.DraftContent = page.Content;
                if (utcPubDate > DateTime.UtcNow)
                {
                    page.DraftPubDate = utcPubDate;
                }
                page.Content = null;
                page.IsPublished = false;
                page.PubDate = null;

            }

            await _pageUrlResolver.ConvertMediaToRelativeUrls(page).ConfigureAwait(false);
            
            await _pageService.Create(page).ConfigureAwait(false);
            _pageService.ClearNavigationCache();
            if (publish)
            {
                await _pageService.FirePublishEvent(page).ConfigureAwait(false);
            }

            return page.Id;
            
        }

        public async Task<bool> EditPage(
            string blogId, 
            string pageId,
            string userName,
            string password,
            PageStruct pageStruct, 
            bool publish)
        {
            var permission = await _security.ValidatePermissions(
                blogId,
                userName,
                password,
                CancellationToken.None
                ).ConfigureAwait(false);

            if (!permission.CanEditPages)
            {
                _log.LogWarning($"user {userName} cannot edit pages");
                return false;
            }

            var page = await _pageService.GetPage(pageId).ConfigureAwait(false);
            if (page == null)
            {
                _log.LogError($"page not found for id {pageId}");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(page.TemplateKey))
            {
                throw new InvalidOperationException("templated pages cannot be edited via metaweblog api");  
            }
            if (page.ContentType != ProjectConstants.HtmlContentType)
            {
                throw new InvalidOperationException("non html content cannot be edited via metaweblog api");
            }

            var history = page.CreateHistory(permission.DisplayName);
            var update = _mapper.GetPageFromStruct(pageStruct);
            var utcPubDate = _timeZoneHelper.ConvertToUtc(pageStruct.pageDate, permission.TimeZoneId);

            if (publish)
            {
                if (utcPubDate < DateTime.UtcNow)
                {
                    page.IsPublished = true;
                    if (utcPubDate.Year != 1) // no date specified
                    {
                        page.PubDate = utcPubDate;
                    }
                    if(!page.PubDate.HasValue)
                    {
                        page.PubDate = DateTime.UtcNow;
                    }
                    page.Content = update.Content;
                    page.DraftAuthor = null;
                    page.DraftContent = null;
                    page.DraftPubDate = null;
                }
                else
                {
                    //future date needs to be draft, it will auto publish after pub date
                    page.DraftAuthor = update.Author;
                    page.DraftContent = update.Content;
                    page.DraftPubDate = utcPubDate;
                   
                }
                
            }
            else
            {
                page.DraftAuthor = update.Author;
                page.DraftContent = update.Content;
                if (utcPubDate > DateTime.UtcNow)
                {
                    page.DraftPubDate = utcPubDate;
                }
            }
            
            page.PageOrder = update.PageOrder;
            page.ParentId = update.ParentId;
            if (!string.IsNullOrWhiteSpace(page.ParentId))
            {
                var parent = await _pageService.GetPage(page.ParentId);
                if (parent != null)
                {
                    page.ParentSlug = parent.Slug;
                }
            }
            else
            {
                page.ParentSlug = null;
            }
            page.Title = update.Title;
            page.LastModifiedByUser = permission.DisplayName;

            await _contentHistoryCommands.Create(blogId, history).ConfigureAwait(false);

            await _pageUrlResolver.ConvertMediaToRelativeUrls(page).ConfigureAwait(false);

            await _pageService.Update(page).ConfigureAwait(false);

            _pageService.ClearNavigationCache();
            if (publish)
            {
                await _pageService.FirePublishEvent(page).ConfigureAwait(false);
                await _contentHistoryCommands.DeleteDraftHistory(blogId, history.ContentId).ConfigureAwait(false);
            }

            return true;
        }

        public Task<bool> DeletePage(
            string blogId, 
            string pageId,
            string userName,
            string password
            )
        {
            //var permission = await _security.ValidatePermissions(
            //    blogId,
            //    userName,
            //    password,
            //    CancellationToken.None
            //    ).ConfigureAwait(false);

            //if (!permission.CanEditPages)
            //{
            //    _log.LogWarning($"user {userName} cannot edit pages");
            //    return false;
            //}

            //TODO: implement
            //return await blogService.Delete(blogId, postId);
            throw new NotImplementedException();
        }
    }
}
