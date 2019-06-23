using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureServiceWorkerReloading : IConfigureServiceWorkerReloading
    {
        public void AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
        {
            //https://developers.google.com/web/tools/workbox/reference-docs/latest/workbox.core

            //Force a service worker to become active, instead of waiting. This is normally used in conjunction with clientsClaim()
            sw.Append("workbox.core.skipWaiting();");
            sw.Append("workbox.core.clientsClaim();");


            //https://developers.google.com/web/tools/workbox/reference-docs/latest/workbox.core

            //Force a service worker to become active, instead of waiting. This is normally used in conjunction with clientsClaim()
            //sw.Append("workbox.skipWaiting();");
            //sw.Append("workbox.clientsClaim();");

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

            if(options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('activate event fired');");
            }

            

            sw.Append("});");

            //new in 4.0.x
            sw.Append("workbox.precaching.cleanupOutdatedCaches();");

        }

    }
}
