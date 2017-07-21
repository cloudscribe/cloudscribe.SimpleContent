// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2017-07-18
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
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
        public PostCommands(ISimpleContentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private ISimpleContentDbContext dbContext;

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
                if (string.IsNullOrEmpty(c)) continue;
                var t = c.Trim();
                if (string.IsNullOrEmpty(t)) continue;

                dbContext.PostCategories.Add(new PostCategory
                {
                    ProjectId = projectId,
                    PostEntityId = p.Id,
                    Value = t
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

            var cats = p.CategoriesCsv.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

            //need to delete and re add PostCategories
            await DeleteCategoriesByPost(projectId, post.Id, true, cancellationToken).ConfigureAwait(false);

            
            foreach (var c in cats)
            {
                if (string.IsNullOrEmpty(c)) continue;
                var t = c.Trim();
                if (string.IsNullOrEmpty(t)) continue;

                var cat = new PostCategory
                {
                    ProjectId = projectId,
                    PostEntityId = p.Id,
                    Value = t
                };

                //var trackingCat = dbContext.ChangeTracker.Entries<PostCategory>().Any(x => x.Entity.PostEntityId == p.Id && x.Entity.Value.ToLower() == t.ToLower());
                //if(trackingCat)
                //{
                //    dbContext.PostCategories.Update(cat);
                //}
                //else
                //{
                    dbContext.PostCategories.Add(cat);
                //}
                
            }


            bool tracking = dbContext.ChangeTracker.Entries<PostEntity>().Any(x => x.Entity.Id == p.Id);
            if (!tracking)
            {
                dbContext.Comments.RemoveRange(dbContext.Comments.Where(x => x.PostEntityId == p.Id));
                p.PostComments.Clear();
                dbContext.Posts.Update(p);
            }
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (post.Comments.Count > 0)
            {
                //p.Comments = post.Comments;
                foreach (var r in post.Comments)
                {
                    var pc = new PostComment();
                    if(!string.IsNullOrEmpty(r.Id))
                    {
                        pc.Id = r.Id;
                    }
                    pc.Author = r.Author;
                    pc.Content = r.Content;
                    pc.Email = r.Email;
                    pc.Ip = r.Ip;
                    pc.IsAdmin = r.IsAdmin;
                    pc.IsApproved = r.IsApproved;
                    pc.ProjectId = projectId;
                    pc.PubDate = r.PubDate;
                    pc.UserAgent = r.UserAgent;
                    pc.Website = r.Website;
                    pc.PostEntityId = p.Id;

                    //dbContext.Comments.Add(pc);
                    p.PostComments.Add(pc);
                }

                rowsAffected = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

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

        private async Task DeleteCommentsByPost(
            string projectId,
            string postId,
            bool saveChanges,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from l in dbContext.Comments
                        where (
                        l.ProjectId == projectId
                        && l.PostEntityId == postId
                        )
                        select l;

            dbContext.Comments.RemoveRange(query);
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

            await DeleteCommentsByPost(projectId, postId, false);
            await DeleteCategoriesByPost(projectId, postId, false);

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
