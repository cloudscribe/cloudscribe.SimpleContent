// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-07
// Last Modified:           2016-07-13
// 

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPostRepository : IPostQueries, IPostCommands
    {
       
    }

    public interface IPostCommands
    {
        Task Delete(string blogId, string postId);

        Task Save(
            string blogId,
            Post post,
            bool isNew);
    }

    public interface IPostQueries
    {
        //TODO: can we remove this? do we ever reall yneed to get all of them?
        //Task<List<Post>> GetAllPosts(
        //    string blogId,
        //    CancellationToken cancellationToken);

        Task<Dictionary<string, int>> GetCategories(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken
            );

        Task<Dictionary<string, int>> GetArchives(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken);

        Task<Post> GetPost(
            string blogId,
            string postId,
            CancellationToken cancellationToken
            );

        Task<PostResult> GetPostBySlug(
            string blogId,
            string slug,
            CancellationToken cancellationToken
            );

        Task<bool> SlugIsAvailable(
            string blogId,
            string slug,
            CancellationToken cancellationToken
            );

        Task<List<Post>> GetRecentPosts(
            string blogId,
            int numberToGet,
            CancellationToken cancellationToken);

        Task<List<Post>> GetVisiblePosts(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken);

        Task<PagedResult<Post>> GetVisiblePosts(
            string blogId,
            string category,
            bool userIsBlogOwner,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<PagedResult<Post>> GetPosts(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetCount(
            string blogId,
            string category,
            bool userIsBlogOwner,
            CancellationToken cancellationToken);

        Task<int> GetCount(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            CancellationToken cancellationToken = default(CancellationToken));

        Task HandlePubDateAboutToChange(Post post, DateTime newPubDate);
    }
}
