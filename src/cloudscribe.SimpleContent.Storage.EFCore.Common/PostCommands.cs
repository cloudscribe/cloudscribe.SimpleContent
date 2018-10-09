// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2018-10-09
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
    public class PostCommands : IPostCommands, IPostCommandsSingleton
    {
        public PostCommands(ISimpleContentDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly ISimpleContentDbContextFactory _contextFactory;

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

            using (var db = _contextFactory.CreateContext())
            {
                db.Posts.Add(p);

                //need to add PostCategorys
                foreach (var c in p.Categories)
                {
                    if (string.IsNullOrEmpty(c)) continue;
                    var t = c.Trim();
                    if (string.IsNullOrEmpty(t)) continue;

                    db.PostCategories.Add(new PostCategory
                    {
                        ProjectId = projectId,
                        PostEntityId = p.Id,
                        Value = t
                    });
                }

                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
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

            using (var db = _contextFactory.CreateContext())
            {
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
                    
                    db.PostCategories.Add(cat);
                   
                }


                bool tracking = db.ChangeTracker.Entries<PostEntity>().Any(x => x.Entity.Id == p.Id);
                if (!tracking)
                {
                    db.Comments.RemoveRange(db.Comments.Where(x => x.PostEntityId == p.Id));
                    p.PostComments.Clear();
                    db.Posts.Update(p);
                }

                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (post.Comments.Count > 0)
                {
                    //p.Comments = post.Comments;
                    foreach (var r in post.Comments)
                    {
                        var pc = new PostComment();
                        if (!string.IsNullOrEmpty(r.Id))
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

                    rowsAffected = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            
        }

        private async Task DeleteCategoriesByPost(
            string projectId,
            string postId,
            bool saveChanges,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var db = _contextFactory.CreateContext())
            {
                var query = from l in db.PostCategories
                            where (
                            l.ProjectId == projectId
                            && l.PostEntityId == postId
                            )
                            select l;

                db.PostCategories.RemoveRange(query);
                if (saveChanges)
                {
                    int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            
        }

        private async Task DeleteCommentsByPost(
            string projectId,
            string postId,
            bool saveChanges,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            using (var db = _contextFactory.CreateContext())
            {
                var query = from l in db.Comments
                            where (
                            l.ProjectId == projectId
                            && l.PostEntityId == postId
                            )
                            select l;

                db.Comments.RemoveRange(query);
                if (saveChanges)
                {
                    int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }

            
        }

        public async Task Delete(
            string projectId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {

            using (var db = _contextFactory.CreateContext())
            {
                var itemToRemove = await db.Posts.SingleOrDefaultAsync(
               x => x.Id == postId && x.BlogId == projectId
               , cancellationToken)
               .ConfigureAwait(false);

                if (itemToRemove == null) throw new InvalidOperationException("Post not found");

                await DeleteCommentsByPost(projectId, postId, false);
                await DeleteCategoriesByPost(projectId, postId, false);

                db.Posts.Remove(itemToRemove);
                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }
        
    }
}
