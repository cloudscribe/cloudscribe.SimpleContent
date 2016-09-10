// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-09-10
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class PostCommands : IPostCommands
    {
        public PostCommands(SimpleContentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private SimpleContentDbContext dbContext;

        public async Task Create(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
           )
        {
            if (post == null) throw new ArgumentException("post must not be null");
            //if (string.IsNullOrEmpty(projectId)) throw new ArgumentException("projectId must be provided");

            var p = PostEntity.FromIPost(post);

            if (string.IsNullOrEmpty(p.Id)) { p.Id = Guid.NewGuid().ToString(); }

            if (string.IsNullOrEmpty(p.BlogId)) p.BlogId = projectId;
            post.LastModified = DateTime.UtcNow;
            
            dbContext.Posts.Add(p);

            //need to add PostCategorys
            foreach (var c in p.Categories)
            {
                dbContext.PostCategories.Add(new PostCategory
                {
                    ProjectId = projectId,
                    PostEntityId = p.Id,
                    Value = c
                });
            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task Update(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
           )
        {
            if (post == null) throw new ArgumentException("post must not be null");
            if (string.IsNullOrEmpty(post.Id)) throw new ArgumentException("can only update an existing post with a populated Id");

            //if (string.IsNullOrEmpty(projectId)) throw new ArgumentException("projectId must be provided");
            var p = PostEntity.FromIPost(post);

            p.LastModified = DateTime.UtcNow;

            //need to delete and re add PostCategorys
            await DeleteCategoriesByPost(projectId, p.Id, true, cancellationToken).ConfigureAwait(false);
            foreach (var c in p.Categories)
            {
                dbContext.PostCategories.Add(new PostCategory
                {
                    ProjectId = projectId,
                    PostEntityId = p.Id,
                    Value = c
                });
            }


            bool tracking = dbContext.ChangeTracker.Entries<PostEntity>().Any(x => x.Entity.Id == p.Id);
            if (!tracking)
            {
                dbContext.Posts.Update(p);
            }
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        private async Task DeleteCategoriesByPost(
            string projectId,
            string postId,
            bool saveChanges,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from l in dbContext.PostCategories
                        where (
                        l.ProjectId == projectId
                        && l.PostEntityId == postId
                        )
                        select l;

            dbContext.PostCategories.RemoveRange(query);
            if (saveChanges)
            {
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

            }


        }

        public async Task Delete(
            string projectId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var itemToRemove = await dbContext.Posts.SingleOrDefaultAsync(
               x => x.Id == postId && x.BlogId == projectId
               , cancellationToken)
               .ConfigureAwait(false);

            if (itemToRemove == null) throw new InvalidOperationException("Post not found");

            dbContext.Posts.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public Task HandlePubDateAboutToChange(
            string projectId,
            IPost post,
            DateTime newPubDate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // no need to implement anything here
            // this was needed for NoDb because storing posts in year month folders required moving the file if
            // the pubdate year or month changed

            return Task.FromResult(0);
        }

    }
}
