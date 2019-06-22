using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Services
{
    public class ServiceWorkerBuilder : IServiceWorkerBuilder
    {
        public ServiceWorkerBuilder(
            IOptions<PwaOptions> pwaOptionsAccessor,
            IWorkboxCacheSuffixProvider workboxCacheSuffixProvider,
            IConfigureServiceWorkerReloading configureServiceWorkerReloading,
            IConfigureWorkboxPreCache configureWorkboxPreCache,
            IConfigureWorkboxNetworkOnlyRoutes configureWorkboxNetworkOnlyRoutes,
            IConfigureWorkboxCacheFirstRoutes configureWorkboxCacheFirstRoutes,
            IConfigureWorkboxNetworkFirstRoutes configureWorkboxNetworkFirstRoutes,
            IConfigureWorkboxCatchHandler configureWorkboxCatchHandler,
            IEnumerable<IAddCodeToServiceWorker> addCodeToServiceWorkers
            )
        {
            _options = pwaOptionsAccessor.Value;
            _workboxCacheSuffixProvider = workboxCacheSuffixProvider;
            _configureServiceWorkerReloading = configureServiceWorkerReloading;
            _configureWorkboxPreCache = configureWorkboxPreCache;
            _configureWorkboxNetworkOnlyRoutes = configureWorkboxNetworkOnlyRoutes;
            _configureWorkboxCacheFirstRoutes = configureWorkboxCacheFirstRoutes;
            _configureWorkboxNetworkFirstRoutes = configureWorkboxNetworkFirstRoutes;
            _configureWorkboxCatchHandler = configureWorkboxCatchHandler;
            _addCodeToServiceWorkers = addCodeToServiceWorkers;
        }

        private readonly PwaOptions _options;
        private readonly IWorkboxCacheSuffixProvider _workboxCacheSuffixProvider;
        private readonly IConfigureServiceWorkerReloading _configureServiceWorkerReloading;
        private readonly IConfigureWorkboxPreCache _configureWorkboxPreCache;
        private readonly IConfigureWorkboxNetworkOnlyRoutes _configureWorkboxNetworkOnlyRoutes;
        private readonly IConfigureWorkboxCacheFirstRoutes _configureWorkboxCacheFirstRoutes;
        private readonly IConfigureWorkboxNetworkFirstRoutes _configureWorkboxNetworkFirstRoutes;
        private readonly IConfigureWorkboxCatchHandler _configureWorkboxCatchHandler;
        private readonly IEnumerable<IAddCodeToServiceWorker> _addCodeToServiceWorkers;



        public async Task<string> Build(HttpContext context)
        {
            var sw = new StringBuilder();

            sw.Append("importScripts('" + _options.WorkBoxUrl + "');");

            sw.Append("if (workbox) {");

            if(_options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log(`Workbox is loaded`);");
                sw.Append("workbox.setConfig({ debug: true });");
               //sw.Append("workbox.core.setLogLevel(workbox.core.LOG_LEVELS.debug);");

            }
            

            var cacheSuffix = await _workboxCacheSuffixProvider.GetWorkboxCacheSuffix();

            if (context.User.Identity.IsAuthenticated)
            {
                sw.Append("workbox.core.setCacheNameDetails({");
                sw.Append("prefix: 'web-app-auth',");
                sw.Append("suffix: '" + cacheSuffix + "',");
                sw.Append("precache: 'auth-user-precache',");
                sw.Append("runtime: 'auth-user-runtime-cache'");
                sw.Append("});");


            }
            else
            {
                sw.Append("workbox.core.setCacheNameDetails({");
                sw.Append("prefix: 'web-app-anon',");
                sw.Append("suffix: '" + cacheSuffix + "',");
                sw.Append("precache: 'unauth-user-precache',");
                sw.Append("runtime: 'unauth-user-runtime-cache'");
                sw.Append("});");
            }

            _configureServiceWorkerReloading.AppendToServiceWorkerScript(sw, _options, context);

            await _configureWorkboxPreCache.AppendToServiceWorkerScript(sw, _options, context);

            await _configureWorkboxNetworkOnlyRoutes.AppendToServiceWorkerScript(sw, _options, context);

            await _configureWorkboxCacheFirstRoutes.AppendToServiceWorkerScript(sw, _options, context);

            await _configureWorkboxNetworkFirstRoutes.AppendToServiceWorkerScript(sw, _options, context);


            foreach(var p in _addCodeToServiceWorkers)
            {
                await p.AppendToServiceWorkerScript(sw, _options, context);
            }
            

            await _configureWorkboxCatchHandler.AppendToServiceWorkerScript(sw, _options, context);

            sw.Append("} else {");

            if (_options.EnableServiceWorkerConsoleLog)
            {
                sw.Append("console.log(`Workbox didn't load`);");
            }
             
            sw.Append("}");
            

            return sw.ToString();
        }

        
        


        



        
        

    }
}
