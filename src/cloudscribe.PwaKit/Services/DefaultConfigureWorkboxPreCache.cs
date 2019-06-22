using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureWorkboxPreCache : IConfigureWorkboxPreCache
    {
        public DefaultConfigureWorkboxPreCache(
            IEnumerable<IPreCacheItemProvider> preCacheProviders
            )
        {
            _preCacheProviders = preCacheProviders;
        }

        private readonly IEnumerable<IPreCacheItemProvider> _preCacheProviders;

        public async Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
        {
            var items = new List<PreCacheItem>();
            foreach (var provider in _preCacheProviders)
            {
                var i = await provider.GetItems();
                items.AddRange(i);
            }

            if (items.Count == 0) { return; }

            var comma = "";

            sw.Append("workbox.precaching.precacheAndRoute([");
            foreach (var item in items)
            {
                sw.Append(comma);
                if (!string.IsNullOrEmpty(item.Revision))
                {
                    sw.Append("{");
                    sw.Append("\"url\": \"" + item.Url + "\",");
                    sw.Append("\"revision\": \"" + item.Revision + "\"");

                    sw.Append("}");
                }
                else
                {
                    sw.Append("'" + item.Url + "'");
                }

                comma = ",";
            }

            sw.Append("]);");

        }

    }

    
}
