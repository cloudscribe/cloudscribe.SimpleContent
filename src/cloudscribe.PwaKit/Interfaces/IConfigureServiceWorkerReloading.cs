using Microsoft.AspNetCore.Http;
using System.Text;

namespace cloudscribe.PwaKit.Interfaces
{
    public interface IConfigureServiceWorkerReloading
    {
        void AppendToServiceWorkerScript(StringBuilder sw, PwaOptions options, HttpContext context);
    }
    
}
