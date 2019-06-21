using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Interfaces
{
    public interface IServiceWorkerBuilder
    {
        Task<string> Build(HttpContext context);
    }
}
