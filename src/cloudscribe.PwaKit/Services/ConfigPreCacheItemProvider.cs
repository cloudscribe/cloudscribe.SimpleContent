using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class ConfigPreCacheItemProvider : IPreCacheItemProvider
    {
        public ConfigPreCacheItemProvider(
            IOptions<PwaPreCacheItems> optionsAccessor
            )
        {
            _options = optionsAccessor.Value;
        }

        private readonly PwaPreCacheItems _options;

        public Task<List<PreCacheItem>> GetItems()
        {
            return Task.FromResult(_options.Assets);
        }

    }
}
