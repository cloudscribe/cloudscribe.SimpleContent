using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultGenerateServiceWorkerInitScript : IGenerateServiceWorkerInitScript
    {
        public DefaultGenerateServiceWorkerInitScript(
            IOptions<PwaOptions> pwaOptionsAccessor,
            IPwaRouteNameProvider pwaRouteNameProvider
            )
        {
            _options = pwaOptionsAccessor.Value;
            _pwaRouteNameProvider = pwaRouteNameProvider;
        }

        private readonly PwaOptions _options;
        private readonly IPwaRouteNameProvider _pwaRouteNameProvider;

        public Task<string> BuildSwInitScript(HttpContext context, IUrlHelper urlHelper)
        {

            var url = urlHelper.RouteUrl(_pwaRouteNameProvider.GetServiceWorkerRouteName());

            var script = new StringBuilder();
            script.Append("if ('serviceWorker' in navigator) {");
            script.Append("window.addEventListener('load', () => {");

            if (_options.ReloadPageOnServiceWorkerUpdate)
            {
                script.Append("var refreshing;");
                script.Append("navigator.serviceWorker.addEventListener('controllerchange', function(event) {");
                if(_options.EnableServiceWorkerConsoleLog)
                {
                    script.Append("console.log('Controller loaded');");
                }
                
                script.Append("if (refreshing) return;");
                script.Append("refreshing = true;");
                script.Append("if(!window.location.href.indexOf('account') > -1) {");

                
                if (_options.EnableServiceWorkerConsoleLog)
                {
                    script.Append("console.log('reloading page because service worker updated');");
                }
                //this causes login to fail
                //script.Append("window.location.reload();");

                script.Append("}");
                script.Append("});");
            }

            var scope = _pwaRouteNameProvider.GetServiceWorkerScope();

            script.Append("navigator.serviceWorker.register('" + url + "',{scope: '" + scope + "'})");
            script.Append(".then(registration => {");

            if (_options.EnableServiceWorkerConsoleLog)
            {
                script.Append("console.log(`Service Worker registered! Scope: ${registration.scope}`);");
            }
                

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

            return Task.FromResult(script.ToString());


        }


    }
}
