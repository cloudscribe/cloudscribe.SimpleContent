using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Interfaces
{
    public interface IGenerateServiceWorkerInitScript
    {
        Task<string> BuildSwInitScript(HttpContext context, IUrlHelper urlHelper);
    }
}
