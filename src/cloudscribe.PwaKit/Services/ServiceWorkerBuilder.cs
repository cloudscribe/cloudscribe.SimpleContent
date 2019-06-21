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
            





            sw.Append("} else {");
            sw.Append("console.log(`Boo! Workbox didn't load`);");
            sw.Append("}");



            return sw.ToString();
        }

        private void AddNewtworkOnlyRoutes(StringBuilder sw)
        {
            sw.Append("const postMatchFunction = ({url, event}) => {");
            
            sw.Append("if(event.request.method == 'POST') {");
            sw.Append("console.log('postMatchFunction returning true for url ' + url);");
            sw.Append("return true;");
            sw.Append("}");

            sw.Append("if(url.href.indexOf('/account/') > -1) { return true; }");

            sw.Append("return false;");
            sw.Append("};");

            sw.Append("workbox.routing.registerRoute(");
            sw.Append("postMatchFunction,");
            sw.Append("new workbox.strategies.NetworkOnly()");
            sw.Append(");");

        }

        private void AddCacheFirstRoutes(StringBuilder sw)
        {
            sw.Append("const imageMatcher = ({url, event}) => {");
            sw.Append("var re = new RegExp(/(.*)\\.(?:png|gif|jpg|jpeg)/);");
            sw.Append("var match = re.test(url.href);");

            sw.Append("console.log(event);");
            sw.Append("console.log(url);");
            sw.Append("if(match) { console.log('imageMatcher returning true for ' + url); return true; }");
            

            sw.Append("return false;");


            sw.Append("};");

            sw.Append("workbox.routing.registerRoute(");
            //sw.Append("/(.*)\\.(?:png|gif|jpg)/,");
            sw.Append("imageMatcher,");
            sw.Append("workbox.strategies.cacheFirst({");
            sw.Append("cacheName: 'images-cache-" + _options.CacheIdSuffix + "',");
            sw.Append("plugins: [");
            sw.Append("new workbox.expiration.Plugin({");
            sw.Append("maxEntries: 50,");
            sw.Append("maxAgeSeconds: 30 * 24 * 60 * 60,"); // 30 Days
            sw.Append("})");
            sw.Append("]");
            sw.Append("})");
            sw.Append(");");


        }



        private void AddNewtworkFirstRoutes(StringBuilder sw)
        {
            sw.Append("const articleHandler = workbox.strategies.networkFirst({");
            sw.Append("cacheName: 'main-content-cache-" + _options.CacheIdSuffix + "',");
            sw.Append("plugins: [");
            sw.Append("new workbox.expiration.Plugin({");
            sw.Append(" maxEntries: 50,");
            sw.Append("})");
            sw.Append("]");
            sw.Append("}); ");

            sw.Append("const mainMatchFunction = ({url, event}) => {");
            

            sw.Append("if(String(url).indexOf(\"serviceworkerinit\") > -1) {");
            sw.Append("return false;");
            sw.Append("}");

            // Return true if the route should match
            sw.Append("console.log('mainMatchFunction returning true for ' + url);");
            sw.Append("return true;");
            sw.Append("};");

            var offlineUrl = _offlinePageUrlProvider.GetOfflineUrl();

            //sw.Append("workbox.routing.registerRoute(/(.*)/, args => {");

            sw.Append("workbox.routing.registerRoute(mainMatchFunction, args => {");
            sw.Append("return articleHandler.handle(args).then(response => {");
            sw.Append("if (!response) {");
            sw.Append("return caches.match('" + offlineUrl + "');");
            sw.Append("} else if (response.status === 404) {");
            sw.Append("return caches.match('" + offlineUrl + "');");
            sw.Append("}");
            sw.Append("return response;");
            sw.Append("});");
            sw.Append("});");


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
