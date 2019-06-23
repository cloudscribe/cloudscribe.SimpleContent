using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Interfaces
{
    public interface IConfigureWorkboxOfflineGoogleAnalytics
    {
        Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context);
    }
}
