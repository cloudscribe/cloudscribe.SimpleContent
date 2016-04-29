// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-04-25
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class NoDbPostRepository : IPostRepository
    {
        public NoDbPostRepository(
            IBasicCommands<Post> postCommands,
            IBasicQueries<Post> postQueries,
            ILogger<NoDbPostRepository> logger)
        {
            commands = postCommands;
            query = postQueries;
            log = logger;
        }

        private IBasicCommands<Post> commands;
        private IBasicQueries<Post> query;
        private ILogger log;


        public async Task HandlePubDateAboutToChange(Post post, DateTime newPubDate)
        {
            // because with the filesystem storage we are storing posts in a year folder
            // if the year changes we need to delete the old file and save the updated post to the
            // new year folder
            //await filePersister.DeletePostFile(post.BlogId, post.Id, post.PubDate).ConfigureAwait(false);
            await commands.DeleteAsync(post.BlogId, post.Id).ConfigureAwait(false);
        }

        public async Task Save(
            string blogId,
            Post post,
            bool isNew)
        {
            bool result = false;

            post.LastModified = DateTime.UtcNow;
            
            if (string.IsNullOrEmpty(post.Id)) { post.Id = Guid.NewGuid().ToString(); }

            if (isNew) // New post
            {
                //var posts = await query.GetAllAsync(
                //    blogId,
                //    CancellationToken.None).ConfigureAwait(false);
                //posts.Insert(0, post);
                //posts.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));

                result = await commands.CreateAsync(blogId, post.Id, post).ConfigureAwait(false);
            }
            else
            {
                result = await commands.UpdateAsync(blogId, post.Id, post).ConfigureAwait(false);
            }

          
        }

        public async Task<bool> Delete(string blogId, string postId)
        {
            var post = await query.FetchAsync(blogId, postId, CancellationToken.None).ConfigureAwait(false);
            if (post != null)
            {
                var allPosts = await GetAllPosts(blogId, CancellationToken.None).ConfigureAwait(false);
                await commands.DeleteAsync(blogId, postId).ConfigureAwait(false);
                allPosts.Remove(post);
                return true;
                //Blog.ClearStartPageCache();
            }
            return false;

        }
        
        public async Task<List<Post>> GetAllPosts(
            string blogId,
            CancellationToken cancellationToken)
        {
            //TODO: caching
            //if (HttpRuntime.Cache["posts"] == null)

            var l = await query.GetAllAsync(blogId, cancellationToken).ConfigureAwait(false);
            var list = l.ToList();
            //if (list.Count > 0)
            //{
            //    list.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));
            //    //HttpRuntime.Cache.Insert("posts", list);
            //}

            return list;

            //if (HttpRuntime.Cache["posts"] != null)
            //{
            //    return (List<Post>)HttpRuntime.Cache["posts"];
            //}
            //return new List<Post>();
        }

        public async Task<List<Post>> GetVisiblePosts(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var list = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);

            list = list.Where(p =>
                      (
                      (
                      p.IsPublished
                      && p.PubDate <= DateTime.UtcNow
                      )
                      || userIsBlogOwner)
                      ).ToList<Post>();

            if (list.Count > 0)
            {
                list.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));
                //HttpRuntime.Cache.Insert("posts", list);
            }

            return list;
        }

        public async Task<List<Post>> GetVisiblePosts(
            string blogId,
            string category,
            bool userIsBlogOwner,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var posts = await GetVisiblePosts(blogId, userIsBlogOwner, cancellationToken);

            if (!string.IsNullOrEmpty(category))
            {
                //var i = posts as IEnumerable<Post>;

                posts = posts.Where(
                    p => p.Categories.Any(
                        c => string.Equals(c, category, StringComparison.OrdinalIgnoreCase))
                        ).ToList<Post>();

            }

            if (pageSize > 0)
            {

                var offset = 0;
                if (pageNumber > 1) { offset = pageSize * (pageNumber - 1); }
                posts = posts.Skip(offset).Take(pageSize).ToList<Post>();


            }

            return posts;
        }

        public async Task<int> GetCount(
            string blogId,
            string category,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var posts = await GetVisiblePosts(blogId, userIsBlogOwner, cancellationToken);
            
            if (!string.IsNullOrEmpty(category))
            {
                posts = posts.Where(
                    p => p.Categories.Any(
                        c => string.Equals(c, category, StringComparison.OrdinalIgnoreCase))
                        ).ToList<Post>();
            }
            
            return posts.Count();
        }

        public async Task<List<Post>> GetRecentPosts(
            string blogId,
            int numberToGet,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var posts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);

            return posts.Take(numberToGet).ToList<Post>();

        }

        public async Task<List<Post>> GetPosts(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var posts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);

            if (day > 0 && month > 0)
            {
                posts = posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                && x.PubDate.Day == day
                )
                .ToList<Post>();
            }
            else if (month > 0)
            {
                posts = posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                )
                .ToList<Post>();

            }
            else
            {
                posts = posts.Where(
                x => x.PubDate.Year == year
                )
                .ToList<Post>();
            }

            if (pageSize > 0)
            {
                var offset = 0;
                if (pageNumber > 1) { offset = pageSize * (pageNumber - 1); }
                posts = posts.Skip(offset).Take(pageSize).ToList<Post>();

            }

            return posts;

            //return posts.Where(
            //    x => x.PubDate.Year == year
            //    )
            //    .Take(pageSize).ToList<Post>();

        }

        public async Task<int> GetCount(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var posts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);

            if (day > 0 && month > 0)
            {
                return posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                && x.PubDate.Day == day
                )
                .Count();
            }
            else if (month > 0)
            {
                return posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                )
                .Count();

            }

            return posts.Where(
                x => x.PubDate.Year == year
                )
                .Count();

        }

        public async Task<Post> GetPost(
            string blogId,
            string postId,
            CancellationToken cancellationToken)
        {
            return await query.FetchAsync(blogId, postId, cancellationToken).ConfigureAwait(false);
            //var allPosts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);
            //return allPosts.FirstOrDefault(p => p.Id == postId);

        }

        public async Task<Post> GetPostBySlug(
            string blogId,
            string slug,
            CancellationToken cancellationToken)
        {
            var allPosts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);
            return allPosts.FirstOrDefault(p => p.Slug == slug);

        }

        public async Task<bool> SlugIsAvailable(
            string blogId,
            string slug,
            CancellationToken cancellationToken)
        {
            var allPosts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);

            var isInUse = allPosts.Any(
                p => string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));

            return !isInUse;
        }

        public async Task<Dictionary<string, int>> GetCategories(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, int>();

            var visiblePosts = await GetVisiblePosts(
                blogId,
                userIsBlogOwner,
                cancellationToken).ConfigureAwait(false);

            foreach (var category in visiblePosts.SelectMany(post => post.Categories))
            {
                if (!result.ContainsKey(category))
                {
                    result.Add(category, 0);
                }

                result[category] = result[category] + 1;
            }

            // TODO: cache this 
            var sorted = new SortedDictionary<string, int>(result);

            return sorted.OrderBy(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) as Dictionary<string, int>;
        }

        public async Task<Dictionary<string, int>> GetArchives(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, int>();

            // since we are storing the files on disk grouped by year and month folders
            // we could derive this list from the file system
            // currently we are loading all posts into memory but for a blog with years of frequent posts
            // maybe we don't need to keep all the old posts in memory we could load the recent year(s) by default and
            // load the old years on demand if requests come in for them
            // at any rate I think the way of retrieving posts needs more review and thought
            // about efficient strategies to use the minimum resources needed

            var visiblePosts = await GetVisiblePosts(
                blogId,
                userIsBlogOwner,
                cancellationToken).ConfigureAwait(false);

            var grouped = from p in visiblePosts
                          group p by new { month = p.PubDate.Month, year = p.PubDate.Year } into d
                          select new
                          {
                              key = d.Key.year.ToString() + "/" + d.Key.month.ToString("00")
                              ,
                              count = d.Count()
                          };

            foreach (var item in grouped)
            {
                result.Add(item.key, item.count);
            }

            return result;

            //foreach (var category in visiblePosts.SelectMany(post => post.Categories))
            //{
            //    if (!result.ContainsKey(category))
            //    {
            //        result.Add(category, 0);
            //    }

            //    result[category] = result[category] + 1;
            //}

            // TODO: cache this 
            //var sorted = new SortedDictionary<string, int>(result);

            //return sorted.OrderBy(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) as Dictionary<string, int>;
        }

    }
}
