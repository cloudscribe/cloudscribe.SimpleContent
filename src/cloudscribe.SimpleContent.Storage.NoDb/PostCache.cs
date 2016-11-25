// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-11-24
// Last Modified:           2016-11-25
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PostCache
    {
        public PostCache(
            IMemoryCache cache,
            IOptions<PostCacheOptions> optionsAccessor = null
            )
        {
            if (cache == null) { throw new ArgumentNullException(nameof(cache)); }
            this.cache = cache;

            options = optionsAccessor?.Value;
            if (options == null) options = new PostCacheOptions(); 
        }

        private IMemoryCache cache;

        private PostCacheOptions options;

        public List<Post> GetAllPosts(
            string projectId
            )
        {
            var cacheKey = GetListCacheKey(projectId);
            List<Post> result = (List<Post>)cache.Get(cacheKey);

            return result;

        }

        public Dictionary<string, int> GetCategories(string projectId)
        {
            var cacheKey = GetCategoriesCacheKey(projectId);
            Dictionary<string, int> result = (Dictionary<string, int>)cache.Get(cacheKey);
            return result;
        }

        public Dictionary<string, int> GetArchiveList(string projectId)
        {
            var cacheKey = GetArchiveListCacheKey(projectId);
            Dictionary<string, int> result = (Dictionary<string, int>)cache.Get(cacheKey);
            return result;
        }

        public void AddToCache(List<Post> postList, string projectId)
        {
            var cacheKey = GetListCacheKey(projectId);
            cache.Set(
                cacheKey,
                postList,
                new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromSeconds(options.CacheDurationInSeconds))
                 );
        }

        public void AddCategoriesToCache(Dictionary<string, int> categoryList, string projectId)
        {
            var cacheKey = GetCategoriesCacheKey(projectId);
            cache.Set(
                cacheKey,
                categoryList,
                new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromSeconds(options.CacheDurationInSeconds))
                 );
        }

        public void AddArchiveListToCache(Dictionary<string, int> archiveList, string projectId)
        {
            var cacheKey = GetArchiveListCacheKey(projectId);
            cache.Set(
                cacheKey,
                archiveList,
                new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromSeconds(options.CacheDurationInSeconds))
                 );
        }

        public void ClearListCache(string projectId)
        {
            var cacheKey = GetListCacheKey(projectId);
            cache.Remove(cacheKey);

            cacheKey = GetCategoriesCacheKey(projectId);
            cache.Remove(cacheKey);

            cacheKey = GetArchiveListCacheKey(projectId);
            cache.Remove(cacheKey);
        }

        public string GetListCacheKey(string projectId)
        {
            return projectId + "-postlist";
        }

        public string GetCategoriesCacheKey(string projectId)
        {
            return projectId + "-categorylist";
        }

        public string GetArchiveListCacheKey(string projectId)
        {
            return projectId + "-archivelist";
        }
    }
}
