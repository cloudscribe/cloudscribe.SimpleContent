// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2018-07-04
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class PostQueries : IPostQueries
    {
        public PostQueries(ISimpleContentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly ISimpleContentDbContext _dbContext;

        private const string PostContentType = "Post";

        /// <summary>
        /// this query is only used for the google site map so we don't need to get comments
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="includeUnpublished"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<IPost>> GetPosts(
            string blogId,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentTime = DateTime.UtcNow;
            var query = _dbContext.Posts
                .Where(p =>
                    p.BlogId == blogId
                    &&  (includeUnpublished || (p.IsPublished == true && p.PubDate <= currentTime))
                      )
                      .OrderByDescending(p => p.PubDate ?? p.LastModified);

            var list = await query
                .AsNoTracking()
                .ToListAsync<IPost>();
            
            return list;
        }

        public async Task<PagedPostResult> GetPosts(
            string blogId,
            string category,
            bool includeUnpublished,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;
            var currentTime = DateTime.UtcNow;

            var result = new PagedPostResult();

            var query = _dbContext.Posts
                .Include(p => p.PostComments)
                .Where(x =>
                x.BlogId == blogId
                && (includeUnpublished || (x.IsPublished == true && x.PubDate <= currentTime))
                && (string.IsNullOrEmpty(category) || x.CategoriesCsv.Contains(category)) 
                )
                .OrderByDescending(x => x.PubDate ?? x.LastModified)
                ;

            var posts = await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IPost>(cancellationToken)
                .ConfigureAwait(false);

            int totalPosts = await GetCount(blogId, category, includeUnpublished, cancellationToken).ConfigureAwait(false);
            
            result.Data = posts;
            result.TotalItems = totalPosts;

            return result;
        }

        public async Task<int> GetCount(
            string blogId,
            string category,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentTime = DateTime.UtcNow;

            if(includeUnpublished)
            {
                return await _dbContext.Posts
                .CountAsync<PostEntity>(x =>
                x.BlogId == blogId
                && (string.IsNullOrEmpty(category) || x.CategoriesCsv.Contains(category))
                );
            }
            else
            {
                return await _dbContext.Posts
                .CountAsync<PostEntity>(x =>
                x.BlogId == blogId
                && (x.IsPublished == true && x.PubDate <= currentTime)
                && (string.IsNullOrEmpty(category) || x.CategoriesCsv.Contains(category))
                );
            }
            
        }

        public async Task<List<IPost>> GetRecentPosts(
            string blogId,
            int numberToGet,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentTime = DateTime.UtcNow;
            
            var query = _dbContext.Posts
               // .Include(p => p.Comments) //think this is only used to populate a list in OLW so don't need the comments
                .Where(p =>
                p.BlogId == blogId
                && p.IsPublished == true
                && p.PubDate <= currentTime)
                .OrderByDescending(p => p.PubDate)
                ;

            return await query
                .AsNoTracking()
                .Take(numberToGet)
                .ToListAsync<IPost>()
                .ConfigureAwait(false);

        }

        public async Task<List<IPost>> GetFeaturedPosts(
            string blogId,
            int numberToGet,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentTime = DateTime.UtcNow;
            var query = _dbContext.Posts
                .Where(p =>
                p.BlogId == blogId
                && p.IsPublished
                && p.IsFeatured
                && p.PubDate <= currentTime)
                .OrderByDescending(p => p.PubDate)
                ;

            return await query
                .AsNoTracking()
                .Take(numberToGet)
                .ToListAsync<IPost>()
                .ConfigureAwait(false);

        }

        public async Task<PagedPostResult> GetPosts(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            int pageNumber = 1,
            int pageSize = 10,
            bool includeUnpublished = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            IQueryable<PostEntity> query;
            var currentTime = DateTime.UtcNow;
            
            if (day > 0 && month > 0)
            {
                query = _dbContext.Posts
                     .Include(p => p.PostComments)
                    .Where(x =>
                     x.BlogId == blogId
                     && x.PubDate.HasValue
                     &&  x.PubDate.Value.Year == year
                     && x.PubDate.Value.Month == month
                     && x.PubDate.Value.Day == day
                     && (includeUnpublished || (x.IsPublished == true && x.PubDate <= currentTime))
                )
                .OrderByDescending(p => p.PubDate ?? p.LastModified)
                ;
            }
            else if (month > 0)
            {
                query = _dbContext.Posts
                     .Include(p => p.PostComments)
                     .Where(x =>
                        x.BlogId == blogId
                        && x.PubDate.HasValue
                        && x.PubDate.Value.Year == year
                        && x.PubDate.Value.Month == month
                        && (includeUnpublished || (x.IsPublished == true  && x.PubDate <= currentTime))
                       )
                      .OrderByDescending(p => p.PubDate ?? p.LastModified)
                      ;

            }
            else
            {
                query = _dbContext.Posts
                     .Include(p => p.PostComments)
                     .Where(x =>
                         x.BlogId == blogId
                         && x.PubDate.HasValue
                         && x.PubDate.Value.Year == year
                       ).OrderByDescending(p => p.PubDate ?? p.LastModified)
                       ;
            }
            
            int offset = (pageSize * pageNumber) - pageSize;
            var posts = await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<IPost>();

            var result = new PagedPostResult();
            result.Data = posts;
            result.TotalItems = await GetCount(blogId, year, month, day, includeUnpublished, cancellationToken).ConfigureAwait(false);

            return result;
            
        }

        public async Task<int> GetCount(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            bool includeUnpublished = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            IQueryable<PostEntity> query;
            var currentTime = DateTime.UtcNow;

            if (day > 0 && month > 0)
            {
                query =  _dbContext.Posts.Where(x =>
                    x.BlogId == blogId
                    && x.PubDate.HasValue
                    && x.PubDate.Value.Year == year
                    && x.PubDate.Value.Month == month
                    && x.PubDate.Value.Day == day
                    && (includeUnpublished || (x.IsPublished == true && x.PubDate <= currentTime))
                    )
                    ;
            }
            else if (month > 0)
            {
                query = _dbContext.Posts.Where(x =>
                    x.BlogId == blogId
                    && x.PubDate.HasValue
                    && x.PubDate.Value.Year == year
                    && x.PubDate.Value.Month == month
                    && (includeUnpublished || (x.IsPublished == true && x.PubDate <= currentTime))
                    )
                    ;

            }
            else
            {
                query = _dbContext.Posts.Where(x =>
                    x.BlogId == blogId
                    && x.PubDate.HasValue
                    && x.PubDate.Value.Year == year
                    )
                    ;
            }

            return await query.CountAsync<PostEntity>().ConfigureAwait(false);

        }


        public async Task<IPost> GetPost(
            string blogId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _dbContext.Posts
                     .Include(p => p.PostComments)
                     .Where(p => p.Id == postId && p.BlogId == blogId)
                     ;

            var post = await query.AsNoTracking().SingleOrDefaultAsync<PostEntity>().ConfigureAwait(false);
            
            return post;
        }

        public async Task<PostResult> GetPostBySlug(
            string blogId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = new PostResult();

            var query = _dbContext.Posts
                     .Include(p => p.PostComments)
                     .Where(p => p.Slug == slug && p.BlogId == blogId)
                     ;

            var post = await query.AsNoTracking().SingleOrDefaultAsync<PostEntity>().ConfigureAwait(false);
            
            result.Post = await query.AsNoTracking().SingleOrDefaultAsync<PostEntity>().ConfigureAwait(false);

            if (result.Post != null)
            {
                var cutoff = result.Post.PubDate;

                result.PreviousPost = await _dbContext.Posts
                    .AsNoTracking()
                    .Where(p =>
                    p.BlogId == blogId
                    && p.PubDate < cutoff
                    && p.IsPublished == true
                    )
                    .OrderByDescending(p => p.PubDate ?? p.LastModified)
                    .Take(1)
                    .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

                result.NextPost = await _dbContext.Posts
                    .AsNoTracking()
                    .Where(p =>
                    p.BlogId == blogId
                    && p.PubDate > cutoff
                    && p.IsPublished == true
                    )
                    .OrderBy(p => p.PubDate ?? p.LastModified)
                    .Take(1)
                    .FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            }

            return result;
        }

        public async Task<IPost> GetPostByCorrelationKey(
            string blogId,
            string correlationKey,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _dbContext.Posts
                     .Include(p => p.PostComments)
                     .Where(p => p.CorrelationKey == correlationKey && p.BlogId == blogId)
                     ;

            var post = await query.AsNoTracking().FirstOrDefaultAsync<PostEntity>().ConfigureAwait(false);

            return post;
        }

        public async Task<bool> SlugIsAvailable(
            string blogId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var isInUse = await _dbContext.Posts.AnyAsync(
                p => p.Slug == slug && p.BlogId == blogId, 
                cancellationToken
                ).ConfigureAwait(false);

            return !isInUse;
        }


        public async Task<Dictionary<string, int>> GetCategories(
            string blogId,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = new Dictionary<string, int>();

            var currentTime = DateTime.UtcNow;

            var posts = await _dbContext.Posts
                .AsNoTracking()
                .Where(x =>
                    (x.BlogId.Equals(blogId))
                    //&& (includeUnpublished || (x.IsPublished == true && x.PubDate <= DateTime.UtcNow))
                    )
                            
                    .ToListAsync(cancellationToken);

            var categories = await _dbContext.PostCategories
                .AsNoTracking()
                .Where(x =>
                    (x.ProjectId.Equals(blogId))
                            
                    )
                            
                    .ToListAsync(cancellationToken);
            
            var list = from y in posts
                        join x in categories
                        on y.Id equals x.PostEntityId
                       where (
                            (x.ProjectId.Equals(blogId))
                            && (includeUnpublished || (y.IsPublished && y.PubDate <= currentTime))
                            )
                       select x
                        ;


            var grouped = from x in list
                        group x by  x.Value
                        into grp
                        select 
                          new { cat = grp.Key, count = grp.Count() }  
                        ;
            
            foreach (var category in grouped)
            {
                if (!result.ContainsKey(category.cat))
                {
                    result.Add(category.cat, category.count);
                }
            }
            
            var sorted = new SortedDictionary<string, int>(result);

            return sorted.OrderBy(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) as Dictionary<string, int>;
        }


        public async Task<Dictionary<string, int>> GetArchives(
            string blogId,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentTime = DateTime.UtcNow;

            var result = new Dictionary<string, int>();

            var query = _dbContext.Posts
                        .Where(x =>
                            (x.BlogId.Equals(blogId))
                            && (includeUnpublished || (x.IsPublished == true && x.PubDate <= currentTime))
                            )
                        ;

            var list = await query
                .AsNoTracking()
               .ToListAsync(cancellationToken)
               .ConfigureAwait(false);



            var grouped = from p in list
                          group p by new { month = p.PubDate?.Month ?? p.LastModified.Month, year = p.PubDate?.Year ?? p.LastModified.Year } into d
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

            var sorted = new SortedDictionary<string, int>(result);

            return sorted.OrderByDescending(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) as Dictionary<string, int>;
  
        }
        
    }
}
