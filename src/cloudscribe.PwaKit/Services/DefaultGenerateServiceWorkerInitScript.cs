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

            if(_options.SetupInstallPrompt)
            {
                script.Append("let deferredPrompt;");
                script.Append("let divPrompt = document.getElementById('divPwaInstallPrompt');");
                script.Append("let btnInstall = document.getElementById('btnPwaInstall');");
                script.Append("if(divPrompt && btnInstall) {");


                script.Append("window.addEventListener('beforeinstallprompt', (e) => {");
                // Prevent Chrome 67 and earlier from automatically showing the prompt
                script.Append("e.preventDefault();");
                // Stash the event so it can be triggered later.
                script.Append("deferredPrompt = e;");

                //update the UI notify the user
                script.Append("divPrompt.classList.add('show');");
                script.Append("divPrompt.style.display ='block';");

                script.Append("});");
                

                script.Append("btnInstall.addEventListener('click', (e) => {");

                script.Append("document.getElementById('divPwaInstallPrompt').classList.remove('show');");
                script.Append("divPrompt.style.display ='none';");
                // Show the prompt
                script.Append("deferredPrompt.prompt();");
                // Wait for the user to respond to the prompt
                script.Append("deferredPrompt.userChoice");
                script.Append(".then((choiceResult) => {");
                script.Append("if (choiceResult.outcome === 'accepted') {");
                script.Append("console.log('User accepted the A2HS prompt');");
                script.Append("} else {");
                script.Append("console.log('User dismissed the A2HS prompt');");
                script.Append("}");
                script.Append("deferredPrompt = null;");
                script.Append("});");
                script.Append("});");


                script.Append("}");


            }
            
            script.Append("if ('serviceWorker' in navigator) {");
            script.Append("window.addEventListener('load', () => {");

            if (_options.ReloadPageOnServiceWorkerUpdate)
            {
                script.Append("var refreshing;");
                script.Append("navigator.serviceWorker.addEventListener('controllerchange', function(event) {");
                if (_options.EnableServiceWorkerConsoleLog)
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
                script.Append("window.location.reload();");

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
