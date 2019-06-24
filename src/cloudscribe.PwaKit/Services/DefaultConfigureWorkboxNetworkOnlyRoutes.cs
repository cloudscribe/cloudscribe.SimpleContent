using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureWorkboxNetworkOnlyRoutes : IConfigureWorkboxNetworkOnlyRoutes
    {

        public DefaultConfigureWorkboxNetworkOnlyRoutes(
            IEnumerable<INetworkOnlyUrlProvider> networkOnlyUrlProviders
            )
        {
            _networkOnlyUrlProviders = networkOnlyUrlProviders;
        }


        private readonly IEnumerable<INetworkOnlyUrlProvider> _networkOnlyUrlProviders;
        
        public async Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
        {

            sw.Append("const networkOnlyMatchFunction = ({url, event}) => {");

            sw.Append("if(event.request.method == 'POST') {");

            if(options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('networkOnlyMatchFunction returning true for url ' + url.href + ' for POST');");
            }
            

            sw.Append("return true;");
            sw.Append("}");

            var urls = await GetNetworkOnlyUrls(options, context);

            sw.Append("var networkOnlyUrls = [");

            var comma = "";
            foreach(var url in urls)
            {
                if (string.IsNullOrEmpty(url)) continue;

                sw.Append(comma);
                sw.Append("\"" + url + "\"");

                comma = ",";
            }
            
            sw.Append("];");

            sw.Append("if(networkOnlyUrls.indexOf(url.href) > -1) {");

            if (options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('networkOnlyMatchFunction returning true for url ' + url.href);");
            }

            sw.Append("return true; }");


            //if (options.EnableServiceWorkerConsoleLog)
            //{
            //    sw.Append("console.log('networkOnlyMatchFunction returning false for url ' + url.href);");
            //}

            sw.Append("return false;");
            sw.Append("};");

            sw.Append("workbox.routing.registerRoute(");
            sw.Append("networkOnlyMatchFunction,");
            sw.Append("new workbox.strategies.NetworkOnly()");
            sw.Append(");");


            
        }

        private async Task<List<string>> GetNetworkOnlyUrls(PwaOptions options, HttpContext context)
        {
            var result = new List<string>();

            foreach(var provider in _networkOnlyUrlProviders)
            {
                var list = await provider.GetNetworkOnlyUrls(options, context);
                result.AddRange(list);
            }


            return result;

        }
    }
}
