using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureWorkboxNetworkFirstRoutes : IConfigureWorkboxNetworkFirstRoutes
    {
        public DefaultConfigureWorkboxNetworkFirstRoutes(
            IOfflinePageUrlProvider offlinePageUrlProvider,
            IWorkboxCacheSuffixProvider workboxCacheSuffixProvider
            )
        {
            _offlinePageUrlProvider = offlinePageUrlProvider;
            _workboxCacheSuffixProvider = workboxCacheSuffixProvider;
        }

        private readonly IOfflinePageUrlProvider _offlinePageUrlProvider;
        private readonly IWorkboxCacheSuffixProvider _workboxCacheSuffixProvider;


        public async Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
        {
            var cacheSuffix = await _workboxCacheSuffixProvider.GetWorkboxCacheSuffix();

            sw.Append("const networkFirstHandler = new workbox.strategies.NetworkFirst({");
            sw.Append("cacheName: 'network-first-content-cache-" + cacheSuffix + "',");
            sw.Append("plugins: [");
            sw.Append("new workbox.expiration.Plugin({");
            sw.Append(" maxEntries: 2000,");
            sw.Append("maxAgeSeconds: 30 * 24 * 60 * 60,"); //30 days

            sw.Append("})");
            sw.Append("]");
            sw.Append("}); ");

            sw.Append("const networkFirstMatchFunction = ({url, event}) => {");


            sw.Append("if(url.href.indexOf(\"serviceworkerinit\") > -1) {");
            sw.Append("return false;");
            sw.Append("}");

            

            if(options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('networkFirstMatchFunction returning true for ' + url.href);");
            }
            // Return true if the route should match, we are basically matching all requests except to the initserviceworker script
            sw.Append("return true;");
            sw.Append("};");

            var offlineUrl = _offlinePageUrlProvider.GetOfflineUrl();

            sw.Append("workbox.routing.registerRoute(networkFirstMatchFunction, args => {");
            sw.Append("return networkFirstHandler.handle(args).then(response => {");
            sw.Append("if (!response) {");
            sw.Append("return caches.match('" + offlineUrl + "');");
            sw.Append("} else if (response.status === 404) {");
            sw.Append("return caches.match('" + offlineUrl + "');");
            sw.Append("}");

            //sw.Append("console.log('network first returning response');");
            //sw.Append("console.log(response.url);");

            sw.Append("return response;");
            sw.Append("});");
            sw.Append("});");

           


            
        }


    }
}
