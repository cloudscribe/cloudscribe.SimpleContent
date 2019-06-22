using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Interfaces
{
    /// <summary>
    /// an interface to get custom code into service worker, you can inject more than one of these
    /// </summary>
    public interface IAddCodeToServiceWorker
    {
        Task AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context);
    }
}
