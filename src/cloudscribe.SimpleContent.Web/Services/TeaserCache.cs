using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class TeaserCache
    {
        public TeaserCache(
            IMemoryCache cache,
            IOptions<TeaserCacheOptions> optionsAccessor = null
            )
        {
            if (cache == null) { throw new ArgumentNullException(nameof(cache)); }
            _cache = cache;

            _options = optionsAccessor?.Value;
            if (_options == null) _options = new TeaserCacheOptions();
        }

        private IMemoryCache _cache;

        private TeaserCacheOptions _options;

        public string GetTeaser(string postId)
        {
            return (string)_cache.Get(postId);
        }

        public void AddToCache(string teaser, string postId)
        {
            
            _cache.Set(
                postId,
                teaser,
                new MemoryCacheEntryOptions()
                 .SetSlidingExpiration(TimeSpan.FromSeconds(_options.CacheDurationInSeconds))
                 );
        }

    }
}
