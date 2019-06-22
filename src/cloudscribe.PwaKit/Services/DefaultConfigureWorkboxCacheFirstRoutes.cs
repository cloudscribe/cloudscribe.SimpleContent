using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureWorkboxCacheFirstRoutes : IConfigureWorkboxCacheFirstRoutes
    {
        public DefaultConfigureWorkboxCacheFirstRoutes(
            IOptions<PwaOptions> pwaOptionsAccessor
            )
        {
            _options = pwaOptionsAccessor.Value;
        }

        private readonly PwaOptions _options;

        public Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
        {
            sw.Append("const cacheFirstMatcher = ({url, event}) => {");
            sw.Append("var re = new RegExp(/(.*)\\.(?:png|gif|jpg|jpeg)/);");
            sw.Append("var match = re.test(url.href);");

            //sw.Append("console.log(event);");
            //sw.Append("console.log(url);");
            sw.Append("if(match) {");

            if(options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('cacheFirstMatcher returning true for ' + url.href);");
            }

            sw.Append("return true; }");

            if(options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('cacheFirstMatcher returning false for url ' + url.href);");
            }
            

            sw.Append("return false;");


            sw.Append("};");

            sw.Append("workbox.routing.registerRoute(");
            //sw.Append("/(.*)\\.(?:png|gif|jpg)/,");
            sw.Append("cacheFirstMatcher,");
            sw.Append("new workbox.strategies.CacheFirst({");
            sw.Append("cacheName: 'images-cache-" + _options.CacheIdSuffix + "',");
            sw.Append("plugins: [");
            sw.Append("new workbox.expiration.Plugin({");
            sw.Append("maxEntries: 2000,");
            sw.Append("maxAgeSeconds: 30 * 24 * 60 * 60,"); // 30 Days
            sw.Append("})");
            sw.Append("]");
            sw.Append("})");
            sw.Append(");");


            return Task.CompletedTask;
        }


    }
}
