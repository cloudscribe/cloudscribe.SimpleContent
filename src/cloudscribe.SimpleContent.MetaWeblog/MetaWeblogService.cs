// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-08-01
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.MetaWeblog;
using cloudscribe.MetaWeblog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.MetaWeblog
{
    public class MetaWeblogService : IMetaWeblogService
    {

        public MetaWeblogService(
            IProjectService projectService,
            IBlogService blogService = null,
            IPageService pageService = null)
        {
            this.projectService = projectService;
            this.blogService = blogService ?? new NotImplementedBlogService();
            this.pageService = pageService ?? new NotImplementedPageService();
            this.mapper = new MetaWeblogModelMapper();
            
        }

        private IProjectService projectService;
        private IBlogService blogService;
        private IPageService pageService;
        private MetaWeblogModelMapper mapper;
        
        public async Task<string> NewPost(
            string blogId,
            string userName,
            string password,
            PostStruct newPost,
            bool publish,
            string authorDisplayName
            )
        {
            var post = mapper.GetPostFromStruct(newPost);
            
            post.BlogId = blogId;
            post.Id = Guid.NewGuid().ToString();
            post.Author = authorDisplayName;
            post.IsPublished = publish;
            
            await blogService.Create(
                blogId, 
                userName,
                password,
                post, 
                publish
                ).ConfigureAwait(false);
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
            var existing = await blogService.GetPost(
                blogId, 
                postId,
                userName,
                password
                ).ConfigureAwait(false);
            
            if (existing == null) { return false; }

            var update = mapper.GetPostFromStruct(post);

            existing.Title = update.Title;
            existing.MetaDescription = update.MetaDescription;
            existing.Content = update.Content;
            
            // TODO: does OLW enable changing pubdate?
            //if (existing.PubDate != update.PubDate)
            //{
            //    await blogService.HandlePubDateAboutToChange(existing, update.PubDate).ConfigureAwait(false);
            //    existing.PubDate = update.PubDate;
            //}
            
            

            if (!string.Equals(existing.Slug, update.Slug, StringComparison.OrdinalIgnoreCase))
            {
                // slug changed make sure the new slug is available
                var requestedSlug = ContentUtils.CreateSlug(update.Slug);
                var available = await blogService.SlugIsAvailable(blogId, requestedSlug).ConfigureAwait(false);

                if (available)
                {
                    existing.Slug = requestedSlug;
                }
            }
               
            existing.Categories = update.Categories;
            existing.IsPublished = publish;

            await blogService.Update(
                blogId, 
                userName,
                password,
                existing, 
                publish).ConfigureAwait(false);

            return true;
        }

        public async Task<PostStruct> GetPost(
            string blogId,
            string postId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            
            var existing = await blogService.GetPost(
                blogId, 
                postId,
                userName,
                password
                ).ConfigureAwait(false);
            
            if (existing == null) { return new PostStruct(); }

            var commentsOpen = await blogService.CommentsAreOpen(existing, false);
            var postUrl = await blogService.ResolvePostUrl(existing);

            var postStruct = mapper.GetStructFromPost(existing,postUrl, commentsOpen);
            // OLW/WLW need the image urls to be fully qualified - actually not sure this is true
            // I seem to remember that being the case with WLW
            // but when I open a recent post in olw that orignated from the web client
            // it shows the image correctly in olw
            // we persist them as relative so we need to convert them back
            // before passing posts back to metaweblogapi

            return postStruct;

        }

        public async Task<List<CategoryStruct>> GetCategories(
            string blogId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var cats = await blogService.GetCategories(
                blogId, 
                userName, 
                password
                ).ConfigureAwait(false);
            
            List<CategoryStruct> output = new List<CategoryStruct>();
            foreach(var c in cats)
            {
                CategoryStruct s = new CategoryStruct { title = c.Key };
                output.Add(s);
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
            
            var blogPosts = await blogService.GetRecentPosts(
                blogId,
                userName,
                password,
                numberOfPosts
                ).ConfigureAwait(false);

            var structs = new List<PostStruct>();

            foreach(Post p in blogPosts)
            {
                var commentsOpen = await blogService.CommentsAreOpen(p, false);
                var postUrl = await blogService.ResolvePostUrl(p);

                var s = mapper.GetStructFromPost(p,postUrl, commentsOpen);

                // OLW/WLW need the image urls to be fully qualified - actually not sure this is true
                // I seem to remember that being the case with WLW
                // but when I open a recent post in olw that orignated from the web client
                // it shows the image correctly in olw
                // we persist them as relative so we need to convert them back
                // before passing posts back to metaweblogapi

                structs.Add(s);
            }

            return structs;

        }

        public async Task<MediaInfoStruct> NewMediaObject(
            string blogId,
            string userName,
            string password,
            MediaObjectStruct mediaObject)
        {
            string extension = Path.GetExtension(mediaObject.name);
            string fileName = Guid.NewGuid().ToString() + extension;
            await blogService.SaveMedia(
                blogId, 
                userName,
                password,
                mediaObject.bytes, 
                fileName
                ).ConfigureAwait(false);

            var mediaUrl = await blogService.ResolveMediaUrl(fileName); ;
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
            await blogService.Delete(
                blogId, 
                postId,
                userName,
                password);

            return true;
        }
        
        public async Task<List<BlogInfoStruct>> GetUserBlogs(
            string key, 
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var result = new List<BlogInfoStruct>();

            //var blog= await projectService.GetProjectSettings(
            //    permissions.BlogId
            //    );
            //if(blog != null)
            //{
            //    var url = await blogService.ResolveBlogUrl(blog).ConfigureAwait(false);
            //    var b = mapper.GetStructFromBlog(blog, url);
            //    result.Add(b);
            //}

            var userBlogs = await projectService.GetUserProjects(userName, password)
                .ConfigureAwait(false);

            foreach (ProjectSettings blog in userBlogs)
            {
                var url = await blogService.ResolveBlogUrl(blog).ConfigureAwait(false);
                var b = mapper.GetStructFromBlog(blog, url);
                result.Add(b);
            }

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

            var pages = await pageService.GetAllPages(
                blogId,
                userName,
                password
                ).ConfigureAwait(false);

            foreach (var p in pages)
            {
                //var commentsOpen = await blogService.CommentsAreOpen(p, false);
                var postUrl = await pageService.ResolvePageUrl(p);

                var s = mapper.GetStructFromPage(p, postUrl, false);

                // OLW/WLW need the image urls to be fully qualified - actually not sure this is true
                // I seem to remember that being the case with WLW
                // but when I open a recent post in olw that orignated from the web client
                // it shows the image correctly in olw
                // we persist them as relative so we need to convert them back
                // before passing posts back to metaweblogapi

                list.Add(s);
            }

            //return structs;

            //var list = new List<PageStruct>();

            return list;
        }

        public async Task<List<PageStruct>> GetPageList(
            string blogId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var list = new List<PageStruct>();

            var pages = await pageService.GetAllPages(
                blogId,
                userName,
                password
                ).ConfigureAwait(false);
            
            foreach (var p in pages)
            {
                //var commentsOpen = await blogService.CommentsAreOpen(p, false);
                var postUrl = await pageService.ResolvePageUrl(p);

                var s = mapper.GetStructFromPage(p, postUrl, false);

                // OLW/WLW need the image urls to be fully qualified - actually not sure this is true
                // I seem to remember that being the case with WLW
                // but when I open a recent post in olw that orignated from the web client
                // it shows the image correctly in olw
                // we persist them as relative so we need to convert them back
                // before passing posts back to metaweblogapi

                list.Add(s);
            }

            //return structs;

            //var list = new List<PageStruct>();

            return list;
            
        }

        public async Task<PageStruct> GetPage(
            string blogId, 
            string pageId,
            string userName,
            string password,
            CancellationToken cancellationToken)
        {
            var existing = await pageService.GetPage(
                blogId, 
                pageId,
                userName,
                password
                ).ConfigureAwait(false);

            if (existing == null) { return new PageStruct(); }

            //var commentsOpen = await blogService.CommentsAreOpen(existing, false);
            var commentsOpen = false;
            var pageUrl = await pageService.ResolvePageUrl(existing);

            var pageStruct = mapper.GetStructFromPage(existing, pageUrl, commentsOpen);
            // OLW/WLW need the image urls to be fully qualified - actually not sure this is true
            // I seem to remember that being the case with WLW
            // but when I open a recent post in olw that orignated from the web client
            // it shows the image correctly in olw
            // we persist them as relative so we need to convert them back
            // before passing posts back to metaweblogapi

            return pageStruct;
        }

        public async Task<string> NewPage(
            string blogId,
            string userName,
            string password,
            PageStruct newPage, 
            bool publish)
        {
            
            var page = mapper.GetPageFromStruct(newPage);

            page.ProjectId = blogId;
            page.Id = Guid.NewGuid().ToString();
            //page.Author = authorDisplayName;
            page.IsPublished = publish;

            await pageService.Create(
                blogId, 
                userName,
                password,
                page, 
                publish
                ).ConfigureAwait(false);

            return page.Id;
            
        }

        public async Task<bool> EditPage(
            string blogId, 
            string pageId,
            string userName,
            string password,
            PageStruct page, 
            bool publish)
        {
            var existing = await pageService.GetPage(
                blogId, 
                pageId,
                userName,
                password
                ).ConfigureAwait(false);
            if(existing == null) { return false; }

            var update = mapper.GetPageFromStruct(page);
            existing.Content = update.Content;
            existing.PageOrder = update.PageOrder;
            existing.ParentId = update.ParentId;
            existing.Title = update.Title;
            
            await pageService.Update(
                blogId, 
                userName,
                password,
                existing, 
                publish
                ).ConfigureAwait(false);

            return true;
        }

        public Task<bool> DeletePage(
            string blogId, 
            string pageId,
            string userName,
            string password
            )
        {
            //TODO: implement
            //return await blogService.Delete(blogId, postId);
            throw new NotImplementedException();
        }
    }
}
