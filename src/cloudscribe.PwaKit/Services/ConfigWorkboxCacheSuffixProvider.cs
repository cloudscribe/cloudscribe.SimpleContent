using cloudscribe.PwaKit.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class ConfigWorkboxCacheSuffixProvider : IWorkboxCacheSuffixProvider
    {
        public ConfigWorkboxCacheSuffixProvider(
            IOptions<PwaOptions> pwaOptionsAccessor
            )
        {
            _options = pwaOptionsAccessor.Value;
        }

        private readonly PwaOptions _options;

        public Task<string> GetWorkboxCacheSuffix()
        {
            return Task.FromResult(_options.CacheIdSuffix);

        }

    }
}
