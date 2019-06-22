using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureWorkboxCatchHandler : IConfigureWorkboxCatchHandler
    {
        public DefaultConfigureWorkboxCatchHandler(
            IOfflinePageUrlProvider offlinePageUrlProvider
            )
        {
            _offlinePageUrlProvider = offlinePageUrlProvider;
        }

        private readonly IOfflinePageUrlProvider _offlinePageUrlProvider;

        public Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
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

            return Task.CompletedTask;
        }


    }
}
