using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class OfflinePageCacheItemProvider : IPreCacheItemProvider
    {
        public OfflinePageCacheItemProvider(
            IOfflinePageUrlProvider offlinePageUrlProvider
            )
        {
            _offlinePageUrlProvider = offlinePageUrlProvider;
        }

        private readonly IOfflinePageUrlProvider _offlinePageUrlProvider;
        

        public Task<List<PreCacheItem>> GetItems()
        {
            var result = new List<PreCacheItem>();
            
            var offlinePage = new PreCacheItem()
            {
                Url = _offlinePageUrlProvider.GetOfflineUrl()
               
            };

            result.Add(offlinePage);
            
            return Task.FromResult(result);
        }



    }
}
