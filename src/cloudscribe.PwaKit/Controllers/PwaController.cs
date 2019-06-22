using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Controllers
{
    public class PwaController : Controller
    {
        public PwaController(
            IServiceWorkerBuilder serviceWorkerBuilder,
            IPwaRouteNameProvider serviceWorkerRouteNameProvider,
            IOptions<PwaOptions> pwaOptionsAccessor
            )
        {
            _serviceWorkerBuilder = serviceWorkerBuilder;
            _serviceWorkerRouteNameProvider = serviceWorkerRouteNameProvider;
            _options = pwaOptionsAccessor.Value;
        }

        private readonly IServiceWorkerBuilder _serviceWorkerBuilder;
        private readonly IPwaRouteNameProvider _serviceWorkerRouteNameProvider;
        private readonly PwaOptions _options;


        public async Task<IActionResult> ServiceWorker()
        {
            Response.ContentType = "application/javascript; charset=utf-8";

            //TODO: 
            //Response.Headers[HeaderNames.CacheControl] = $"max-age={_options.ServiceWorkerCacheControlMaxAge}";

            var sw = await _serviceWorkerBuilder.Build(HttpContext);


            return Content(sw);

        }

        public IActionResult ServiceWorkerInit()
        {
            Response.ContentType = "application/javascript; charset=utf-8";

            var url = Url.RouteUrl(_serviceWorkerRouteNameProvider.GetServiceWorkerRouteName());

            var script = new StringBuilder();
            script.Append("if ('serviceWorker' in navigator) {");
            script.Append("window.addEventListener('load', () => {");

            if (_options.ReloadPageOnServiceWorkerUpdate)
            {
                script.Append("var refreshing;");
                script.Append("navigator.serviceWorker.addEventListener('controllerchange', function(event) {");
                script.Append("console.log('Controller loaded');");
                script.Append("if (refreshing) return;");
                script.Append("refreshing = true;");
                script.Append("if(!window.location.href.indexOf('account') > -1) {");
                //this causes login to fail
                script.Append("console.log('reloading page because service worker updated');");
                //script.Append("window.location.reload();");
                script.Append("}");
                script.Append("});");
            }

            var scope = _serviceWorkerRouteNameProvider.GetServiceWorkerScope();

            script.Append("navigator.serviceWorker.register('" + url + "',{scope: '" + scope + "'})");
            script.Append(".then(registration => {");

            script.Append("console.log(`Service Worker registered! Scope: ${registration.scope}`);");

            //this gets into an infinite loop of reloading
            //if (_options.ReloadPageOnServiceWorkerUpdate)
            //{
            //    script.Append("if (!navigator.serviceWorker.controller) {");
            //    script.Append("return;");
            //    script.Append("}");

            //    script.Append("registration.addEventListener('updatefound', function () {");
            //    script.Append("const newWorker = registration.installing;");
            //    script.Append("var refreshing;");
            //    script.Append("newWorker.addEventListener('statechange', () => {");
            //    script.Append("if (newWorker.state == 'activated') {");
            //    script.Append("if (refreshing) return;");
            //    script.Append("window.location.reload();");
            //    script.Append("refreshing = true;");
            //    script.Append("}");
            //    script.Append("});");
            //    script.Append("});");

            //}


            script.Append(" })");
            script.Append(".catch(err => {");
            script.Append("console.log(`Service Worker registration failed: ${err}`);");
            script.Append("});");
            script.Append("});");
            script.Append("}");

            //var script = "'serviceWorker'in navigator&&navigator.serviceWorker.register('" + url + "')";

            return Content(script.ToString());

        }

        public IActionResult Offline()
        {

            return View();
        }



    }
}
