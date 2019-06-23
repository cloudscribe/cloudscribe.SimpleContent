using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureWorkboxNetworkOnlyRoutes : IConfigureWorkboxNetworkOnlyRoutes
    {

        public Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
        {

            sw.Append("const networkOnlyMatchFunction = ({url, event}) => {");

            sw.Append("if(event.request.method == 'POST') {");

            if(options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('networkOnlyMatchFunction returning true for url ' + url.href);");
            }
            

            sw.Append("return true;");
            sw.Append("}");

            sw.Append("if(url.href.indexOf('/account/') > -1) { return true; }");

            if (options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log('networkOnlyMatchFunction returning false for url ' + url.href);");
            }

            sw.Append("return false;");
            sw.Append("};");

            sw.Append("workbox.routing.registerRoute(");
            sw.Append("networkOnlyMatchFunction,");
            sw.Append("new workbox.strategies.NetworkOnly()");
            sw.Append(");");


            return Task.CompletedTask;
        }
    }
}
