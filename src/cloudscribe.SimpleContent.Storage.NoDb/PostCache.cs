using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using cloudscribe.SimpleContent.Models;
using System.Threading;

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
            string cacheKey
            )
        {
            List<Post> result = (List<Post>)cache.Get(cacheKey);

            return result;

        }

        public void AddToCache(List<Post> postList, string cacheKey)
        {
            cache.Set(
                cacheKey,
                postList,
                new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromSeconds(options.CacheDurationInSeconds))
                 );
        }

        public void ClearListCache(string projectId)
        {
            var cacheKey = GetListCacheKey(projectId);
            cache.Remove(cacheKey);
        }

        public string GetListCacheKey(string projectId)
        {
            return projectId + "-postlist";
        }
    }
}
