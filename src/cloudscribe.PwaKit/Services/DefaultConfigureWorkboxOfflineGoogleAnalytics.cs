using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultConfigureWorkboxOfflineGoogleAnalytics : IConfigureWorkboxOfflineGoogleAnalytics
    {
        public Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context)
        {

            sw.Append("workbox.googleAnalytics.initialize();");

            return Task.CompletedTask;
        }
    }
}
