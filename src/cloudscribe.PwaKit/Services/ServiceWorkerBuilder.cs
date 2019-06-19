using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class ServiceWorkerBuilder : IServiceWorkerBuilder
    {
        public ServiceWorkerBuilder(
            IOptions<PwaOptions> pwaOptionsAccessor,
            IEnumerable<IPreCacheItemProvider> preCacheProviders
            )
        {
            _options = pwaOptionsAccessor.Value;
            _preCacheProviders = preCacheProviders;
        }

        private readonly PwaOptions _options;
        private readonly IEnumerable<IPreCacheItemProvider> _preCacheProviders;

        public async Task<string> Build()
        {
            var sw = new StringBuilder();

            sw.Append("importScripts('" + _options.WorkBoxUrl + "');");

            sw.Append("if (workbox) {");
            sw.Append("console.log(`Yay! Workbox is loaded`);");


            await AddPreCaching(sw);





            sw.Append("} else {");
            sw.Append("console.log(`Boo! Workbox didn't load`);");
            sw.Append("}");



            return sw.ToString();
        }

        private async Task AddPreCaching(StringBuilder sw)
        {
            var items = new List<PreCacheItem>();
            foreach(var provider in _preCacheProviders)
            {
                var i = await provider.GetItems();
                items.AddRange(i);
            }

            if(items.Count == 0) { return; }

            var comma = "";

            sw.Append("workbox.precaching.precacheAndRoute([");
            foreach(var item in items)
            {
                sw.Append(comma);
                if(!string.IsNullOrEmpty(item.Revision))
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
