using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using Microsoft.AspNetCore.Http;
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
            IEnumerable<IPreCacheItemProvider> preCacheProviders,
            IOfflinePageUrlProvider offlinePageUrlProvider
            )
        {
            _options = pwaOptionsAccessor.Value;
            _preCacheProviders = preCacheProviders;
            _offlinePageUrlProvider = offlinePageUrlProvider;
        }

        private readonly PwaOptions _options;
        private readonly IEnumerable<IPreCacheItemProvider> _preCacheProviders;
        private readonly IOfflinePageUrlProvider _offlinePageUrlProvider;

        public async Task<string> Build(HttpContext context)
        {
            var sw = new StringBuilder();

            sw.Append("importScripts('" + _options.WorkBoxUrl + "');");

            sw.Append("if (workbox) {");
            sw.Append("console.log(`Yay! Workbox is loaded`);");

#if DEBUG

            sw.Append("workbox.setConfig({ debug: true });");
            sw.Append("workbox.core.setLogLevel(workbox.core.LOG_LEVELS.debug);");
#endif

            if(context.User.Identity.IsAuthenticated)
            {
                sw.Append("workbox.core.setCacheNameDetails({");
                sw.Append("prefix: 'web-app-auth',");
                sw.Append("suffix: 'v1',");
                sw.Append("precache: 'auth-user-precache',");
                sw.Append("runtime: 'auth-user-runtime-cache'");
                sw.Append("});");


            }
            else
            {
                sw.Append("workbox.core.setCacheNameDetails({");
                sw.Append("prefix: 'web-app-anon',");
                sw.Append("suffix: 'v1',");
                sw.Append("precache: 'unauth-user-precache',");
                sw.Append("runtime: 'unauth-user-runtime-cache'");
                sw.Append("});");
            }

            //https://developers.google.com/web/tools/workbox/reference-docs/latest/workbox.core

            //Force a service worker to become active, instead of waiting. This is normally used in conjunction with clientsClaim()
            sw.Append("workbox.skipWaiting();");
            sw.Append("workbox.clientsClaim();");

            //https://github.com/GoogleChrome/workbox/issues/1407
            //cleanup old caches
            sw.Append("let currentCacheNames = Object.assign(");
            sw.Append("{ precacheTemp: workbox.core.cacheNames.precache + \"-temp\" },");
            sw.Append("workbox.core.cacheNames");
            sw.Append(");");

            sw.Append("self.addEventListener(\"activate\", function(event) {");

            sw.Append("event.waitUntil(");
            sw.Append("caches.keys().then(function(cacheNames) {");
            sw.Append("let validCacheSet = new Set(Object.values(currentCacheNames));");
            sw.Append("return Promise.all(");
            sw.Append("cacheNames");
            sw.Append(".filter(function(cacheName) {");
            sw.Append("return !validCacheSet.has(cacheName);");
            sw.Append("})");
            sw.Append(".map(function(cacheName) {");
            sw.Append("return caches.delete(cacheName);");
            sw.Append("})");
            sw.Append(");");

            sw.Append("})");
            sw.Append(");");

            sw.Append("console.log('activate event fired');");

            sw.Append("});");


            await AddPreCaching(sw);

            AddNewtworkOnlyRoutes(sw);

            AddCacheFirstRoutes(sw);

            AddNewtworkFirstRoutes(sw);

            AddCatchHandler(sw);







            sw.Append("} else {");
            sw.Append("console.log(`Boo! Workbox didn't load`);");
            sw.Append("}");



            return sw.ToString();
        }

        private void AddNewtworkOnlyRoutes(StringBuilder sw)
        {
            sw.Append("const networkOnlyMatchFunction = ({url, event}) => {");
            
            sw.Append("if(event.request.method == 'POST') {");
            sw.Append("console.log('networkOnlyMatchFunction returning true for url ' + url.href);");
            sw.Append("return true;");
            sw.Append("}");

            sw.Append("if(url.href.indexOf('/account/') > -1) { return true; }");


            sw.Append("console.log('networkOnlyMatchFunction returning false for url ' + url.href);");

            sw.Append("return false;");
            sw.Append("};");

            sw.Append("workbox.routing.registerRoute(");
            sw.Append("networkOnlyMatchFunction,");
            sw.Append("new workbox.strategies.NetworkOnly()");
            sw.Append(");");

        }

        private void AddCacheFirstRoutes(StringBuilder sw)
        {
            sw.Append("const cacheFirstMatcher = ({url, event}) => {");
            sw.Append("var re = new RegExp(/(.*)\\.(?:png|gif|jpg|jpeg)/);");
            sw.Append("var match = re.test(url.href);");

            //sw.Append("console.log(event);");
            //sw.Append("console.log(url);");
            sw.Append("if(match) { console.log('cacheFirstMatcher returning true for ' + url.href); return true; }");


            sw.Append("console.log('cacheFirstMatcher returning false for url ' + url.href);");

            sw.Append("return false;");


            sw.Append("};");

            sw.Append("workbox.routing.registerRoute(");
            //sw.Append("/(.*)\\.(?:png|gif|jpg)/,");
            sw.Append("cacheFirstMatcher,");
            sw.Append("workbox.strategies.cacheFirst({");
            sw.Append("cacheName: 'images-cache-" + _options.CacheIdSuffix + "',");
            sw.Append("plugins: [");
            sw.Append("new workbox.expiration.Plugin({");
            sw.Append("maxEntries: 2000,");
            sw.Append("maxAgeSeconds: 30 * 24 * 60 * 60,"); // 30 Days
            sw.Append("})");
            sw.Append("]");
            sw.Append("})");
            sw.Append(");");


        }


        private void AddCatchHandler(StringBuilder sw)
        {
            var offlineUrl = _offlinePageUrlProvider.GetOfflineUrl();

            sw.Append("workbox.routing.setCatchHandler(({event}) => {");
            // The FALLBACK_URL entries must be added to the cache ahead of time, either via runtime
            // or precaching.
            // If they are precached, then call workbox.precaching.getCacheKeyForURL(FALLBACK_URL)
            // to get the correct cache key to pass in to caches.match().
            //
            // Use event, request, and url to figure out how to respond.
            // One approach would be to use request.destination, see
            // https://medium.com/dev-channel/service-worker-caching-strategies-based-on-request-types-57411dd7652c
            sw.Append("switch (event.request.destination) {");
            sw.Append("case 'document':");
            sw.Append("return caches.match('" + offlineUrl + "');");
            sw.Append("break;");
            
            sw.Append("case 'image':");
            //sw.Append("return caches.match(FALLBACK_IMAGE_URL);");
            sw.Append("return new Response('<svg role=\"img\" aria-labelledby=\"offline-title\" viewBox=\"0 0 400 300\" xmlns=\"http://www.w3.org/2000/svg\"><title id=\"offline-title\">Offline</title><g fill=\"none\" fill-rule=\"evenodd\"><path fill=\"#D8D8D8\" d=\"M0 0h400v300H0z\"/><text fill=\"#9B9B9B\" font-family=\"Helvetica Neue,Arial,Helvetica,sans-serif\" font-size=\"72\" font-weight=\"bold\"><tspan x=\"93\" y=\"172\">offline</tspan></text></g></svg>', { headers: { 'Content-Type': 'image/svg+xml' } });");
            sw.Append("break;");

            //sw.Append("case 'font':");
            //sw.Append("return caches.match(FALLBACK_FONT_URL);");
            //sw.Append("break;");

            sw.Append("default:");
            sw.Append("return Response.error();");

            sw.Append("}");

            sw.Append("});");

        }



        private void AddNewtworkFirstRoutes(StringBuilder sw)
        {
            


            sw.Append("const networkFirstHandler = workbox.strategies.networkFirst({");
            sw.Append("cacheName: 'network-first-content-cache-" + _options.CacheIdSuffix + "',");
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

            // Return true if the route should match
            sw.Append("console.log('networkFirstMatchFunction returning true for ' + url.href);");
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

            //vanilla
            //sw.Append("workbox.routing.registerRoute(");
            //sw.Append("networkFirstMatchFunction,");
            //sw.Append("new workbox.strategies.NetworkFirst()");
            //sw.Append(");");


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
