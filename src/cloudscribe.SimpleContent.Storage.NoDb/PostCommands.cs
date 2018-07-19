// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2018-06-28
// 

using cloudscribe.SimpleContent.Models;
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
            )
        {
            _cache = cache;
            _commands = postCommands;
            _query = postQueries;
            _keyGenerator = keyGenerator;
        }

        private PostCache _cache;
        private IBasicCommands<Post> _commands;
        private IBasicQueries<Post> _query;
        private IKeyGenerator _keyGenerator;
       
        

        public async Task Create(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
           )
        {
            var p = Post.FromIPost(post);

            p.LastModified = DateTime.UtcNow;

            p.Id = _keyGenerator.GenerateKey(p);
            
            await _commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);
            _cache.ClearListCache(projectId);
        }

        public async Task Update(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
           )
        {
            var p = Post.FromIPost(post);
            p.LastModified = DateTime.UtcNow;

            //since we store posts in year month folders we need to check if pubdate changed and move the file if needed by delete and re-reate.
            var currentVersion = await _query.FetchAsync(projectId, p.Id);
            if(currentVersion.PubDate != p.PubDate)
            {
                await _commands.DeleteAsync(projectId, p.Id).ConfigureAwait(false);
                await _commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);
            }
            else
            {
                await _commands.UpdateAsync(projectId, p.Id, p).ConfigureAwait(false);
            }
           
            _cache.ClearListCache(projectId);

        }

        //private async Task HandlePubDateAboutToChange(
        //    string projectId,
        //    IPost post,
        //    DateTime newPubDate,
        //    CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    if (post.PubDate.HasValue)
        //    {
        //        if ((post.PubDate.Value.Month == newPubDate.Month) && (post.PubDate.Value.Year == newPubDate.Year))
        //        {
        //            // we store posts in /year/month folders, if that didn't change no need to do anything
        //            return;
        //        }
        //    }


        //    // because with the filesystem storage we are storing posts in a year/month folder
        //    // if the year or month changes we need to delete the old file and save the updated post to the
        //    // new year/month folder
        //    var p = Post.FromIPost(post);

        //    await _commands.DeleteAsync(projectId, p.Id).ConfigureAwait(false);
        //    p.PubDate = newPubDate;
        //    await _commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);
        //}


        public async Task Delete(
            string projectId, 
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var post = await _query.FetchAsync(projectId, postId, cancellationToken).ConfigureAwait(false);
            if (post != null)
            {
                //var allPosts = await GetAllPosts(projectId, CancellationToken.None).ConfigureAwait(false);
                await _commands.DeleteAsync(projectId, postId).ConfigureAwait(false);
                //allPosts.Remove(post);

                _cache.ClearListCache(projectId);
            }
            
        }

        private async Task<List<Post>> GetAllPosts(
            string projectId,
            CancellationToken cancellationToken)
        {

            var list = _cache.GetAllPosts(projectId);
            if (list != null) return list;

            var l = await _query.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            list = l.ToList();
            _cache.AddToCache(list, projectId);

            return list;
            
        }
        
    }
}
