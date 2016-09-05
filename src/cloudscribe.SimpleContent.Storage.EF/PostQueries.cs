// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-09-05
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class PostQueries
    {
        public PostQueries(SimpleContentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private SimpleContentDbContext dbContext;

        private const string PostContentType = "Post";

        /// <summary>
        /// this query is only used for the google site map so we don't need to get comments
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="includeUnpublished"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Post>> GetPosts(
            string blogId,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var query = dbContext.Posts
                .Where(p =>
                    p.BlogId == blogId
                    &&  (includeUnpublished || (p.IsPublished && p.PubDate <= DateTime.UtcNow))
                      )
                      .OrderByDescending(p => p.PubDate);

            var list = await query
                .AsNoTracking()
                .ToListAsync<Post>();
            
            return list;
        }

        public async Task<PagedResult<Post>> GetPosts(
            string blogId,
            string category,
            bool includeUnpublished,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;
            var currentTime = DateTime.UtcNow;

            var result = new PagedResult<Post>();

            var query = dbContext.Posts
                .Where(x =>
                x.BlogId == blogId
                && (includeUnpublished || (x.IsPublished && x.PubDate <= currentTime))
                && (string.IsNullOrEmpty(category) || (EF.Property<string>(x, "CategoryCsv").Contains(category))) // will this work?
                )
                .Select(x => new Post
                {
                    // note that this will have to be updated if there are any new properties added to Post
                    Id = x.Id,
                    Categories = EF.Property<string>(x, "CategoryCsv").Split(new char[] { ',' }, 
                        StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToLower()).ToList(), // get from shadow property
                    Author = x.Author,
                    BlogId = x.BlogId,
                    Content = x.Content,
                    IsPublished = x.IsPublished,
                    LastModified = x.LastModified,
                    MetaDescription = x.MetaDescription,
                    PubDate = x.PubDate,
                    Slug = x.Slug,
                    Title = x.Title
                    //, Comments = is is possible to sub query here without navigation property
                }
                 
                )
                .OrderByDescending(x => x.PubDate)
                ;

            
            List<Post> posts = await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<Post>(cancellationToken)
                .ConfigureAwait(false);

            int totalPosts = await GetCount(blogId, category, includeUnpublished, cancellationToken).ConfigureAwait(false);

            

            // TODO: get list of post comments for the same range and update the posts?



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
            var currentTime = DateTime.UtcNow;

            var count = await dbContext.Posts
                .CountAsync(x =>
                x.BlogId == blogId
                && (includeUnpublished || (x.IsPublished && x.PubDate <= currentTime))
                && (string.IsNullOrEmpty(category) || (EF.Property<string>(x, "CategoryCsv").Contains(category))) // will this work?
                );
            
            return count;
        }






        public async Task<Dictionary<string, int>> GetCategories(
            string blogId,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var result = new Dictionary<string, int>();

            var query = dbContext.TagItems
                .Where(t => t.ProjectId == blogId && t.ContentType == PostContentType)
                .Select(t => new
                {
                    Cat = t,
                    ItemCount = t.ContentId.Count()
                }).AsNoTracking()
                ;

            //var catQuery = (from x in dbContext.TagItems
            //     where (x.ProjectId == blogId && x.ContentType == PostContentType)
            //     group x by new { ProjectId = x.ProjectId, x.ContentType, x.TagValue } into hh
            //     select new {

            //     }

            //    )
            //    ;

            var list = await query
               .ToListAsync(cancellationToken)
               .ConfigureAwait(false);


            foreach (var category in list)
            {
                //var c = category.Cat.TagValue;
                if (!result.ContainsKey(category.Cat.TagValue))
                {
                    result.Add(category.Cat.TagValue, category.ItemCount);
                }

                //result[c] = result[c] + 1;
            }

            // TODO: cache this 
            var sorted = new SortedDictionary<string, int>(result);

            return sorted.OrderBy(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) as Dictionary<string, int>;
        }


        #region IDisposable Support

        private void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion

    }
}
