using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Controllers
{
    public class PwaController : Controller
    {
        public PwaController(
            IServiceWorkerBuilder serviceWorkerBuilder,
            IGenerateServiceWorkerInitScript serviceWorkerInitScriptGenerator,
            IOptions<PwaOptions> pwaOptionsAccessor
            )
        {
            _serviceWorkerBuilder = serviceWorkerBuilder;
            _serviceWorkerInitScriptGenerator = serviceWorkerInitScriptGenerator;
            _options = pwaOptionsAccessor.Value;
        }

        private readonly IServiceWorkerBuilder _serviceWorkerBuilder;
        private readonly IGenerateServiceWorkerInitScript _serviceWorkerInitScriptGenerator;
        private readonly PwaOptions _options;


        public async Task<IActionResult> ServiceWorker()
        {
            Response.ContentType = "application/javascript; charset=utf-8";

            //TODO ?: do we need to cache this,suppose to load at least every 24 hours
            // I think the browser makes head requests to see if any changes
            //Response.Headers[HeaderNames.CacheControl] = $"max-age={_options.ServiceWorkerCacheControlMaxAge}";

            var sw = await _serviceWorkerBuilder.Build(HttpContext);


            return Content(sw);

        }

        public async Task<IActionResult> ServiceWorkerInit()
        {
            var script = await _serviceWorkerInitScriptGenerator.BuildSwInitScript(HttpContext, Url);
            if(string.IsNullOrWhiteSpace(script))
            {
                return NotFound();
            }

            Response.ContentType = "application/javascript; charset=utf-8";
            
            return Content(script);

        }

        public IActionResult Offline()
        {

            return View();
        }



    }
}
