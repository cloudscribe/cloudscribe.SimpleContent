using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Interfaces
{
    public interface INetworkOnlyUrlProvider
    {
        Task<List<string>> GetNetworkOnlyUrls(PwaOptions options, HttpContext context);
    }
}
