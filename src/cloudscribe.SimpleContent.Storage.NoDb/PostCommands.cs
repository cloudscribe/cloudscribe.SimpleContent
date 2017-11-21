// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2017-11-21
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
    public class PostCommands : IPostCommands
    {
        public PostCommands(
            PostCache cache,
            IBasicCommands<Post> postCommands,
            IBasicQueries<Post> postQueries,
            IKeyGenerator keyGenerator
            //,ILogger<PostCommands> logger
            )
        {
            this.cache = cache;
            commands = postCommands;
            query = postQueries;
            _keyGenerator = keyGenerator;
           // log = logger;
        }

        private PostCache cache;
        private IBasicCommands<Post> commands;
        private IBasicQueries<Post> query;
        private IKeyGenerator _keyGenerator;
       // private ILogger log;

        public async Task HandlePubDateAboutToChange(
            string projectId,
            IPost post, 
            DateTime newPubDate,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            if ((post.PubDate.Month == newPubDate.Month) && (post.PubDate.Year == newPubDate.Year))
            {
                // we store posts in /year/month folders, if that didn't change no need to do anything
                return;
            }

            // because with the filesystem storage we are storing posts in a year/month folder
            // if the year or month changes we need to delete the old file and save the updated post to the
            // new year/month folder
            var p = Post.FromIPost(post);

            await commands.DeleteAsync(projectId, p.Id).ConfigureAwait(false);
            p.PubDate = newPubDate;
            await commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);
        }

        public async Task Create(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
           )
        {
            var p = Post.FromIPost(post);

            p.LastModified = DateTime.UtcNow;

            p.Id = _keyGenerator.GenerateKey(p);

            //if (string.IsNullOrEmpty(p.Id)) { p.Id = Guid.NewGuid().ToString(); }
            
            await commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);
            cache.ClearListCache(projectId);
        }

        public async Task Update(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
           )
        {
            var p = Post.FromIPost(post);

            p.LastModified = DateTime.UtcNow;
            
            await commands.UpdateAsync(projectId, p.Id, p).ConfigureAwait(false);
            cache.ClearListCache(projectId);

        }

        public async Task Delete(
            string projectId, 
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var post = await query.FetchAsync(projectId, postId, cancellationToken).ConfigureAwait(false);
            if (post != null)
            {
                //var allPosts = await GetAllPosts(projectId, CancellationToken.None).ConfigureAwait(false);
                await commands.DeleteAsync(projectId, postId).ConfigureAwait(false);
                //allPosts.Remove(post);

                cache.ClearListCache(projectId);
            }
            
        }

        private async Task<List<Post>> GetAllPosts(
            string projectId,
            CancellationToken cancellationToken)
        {

            var list = cache.GetAllPosts(projectId);
            if (list != null) return list;

            var l = await query.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            list = l.ToList();
            cache.AddToCache(list, projectId);

            return list;
            
        }
        
    }
}
