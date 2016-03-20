// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-17
// Last Modified:           2016-03-19
// 


using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public class JsonPostRepository : IPostRepository
    {
        public Task<bool> Delete(
            string projectId, 
            string postId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> GetArchives(string projectId, bool userIsBlogOwner, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //public Task<List<Post>> GetAllPosts(
        //    string projectId, 
        //    CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<Dictionary<string, int>> GetCategories(
            string projectId, 
            bool userIsBlogOwner, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(
            string projectId, 
            string category, 
            bool userIsBlogOwner, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(string projectId, int year, int month = 0, int day = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPost(
            string projectId, 
            string postId, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPostBySlug(
            string projectId, 
            string slug, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetPosts(string projectId, int numberToGet, int year, int month = 0, int day = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetPosts(string projectId, int year, int month = 0, int day = 0, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetRecentPosts(
            string projectId, 
            int numberToGet, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetVisiblePosts(
            string projectId, 
            bool userIsBlogOwner, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetVisiblePosts(
            string projectId, 
            string category, 
            bool userIsBlogOwner, 
            int pageNumber, 
            int pageSize, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HandlePubDateAboutToChange(Post post, DateTime newPubDate)
        {
            throw new NotImplementedException();
        }

        public Task Save(
            string projectId, 
            Post post, 
            bool isNew)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlugIsAvailable(
            string projectId, 
            string slug, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
