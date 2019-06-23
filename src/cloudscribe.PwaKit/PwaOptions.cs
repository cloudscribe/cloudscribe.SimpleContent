using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.PwaKit
{
    public class PwaOptions
    {

        public PwaOptions()
        {
            AutoRegisterServiceWorker = true;
            EnableCspNonce = false;
            ServiceWorkerCacheControlMaxAgeInSeconds = 60 * 60 * 24 * 1;    // 1 days
        }


        /// <summary>
        /// The cache identifier of the service worker (can be any string).
        /// Change this property to force the service worker to reload in browsers.
        /// </summary>
        public string CacheIdSuffix { get; set; } = "v1.0";


        /// <summary>
        /// Determines if a script that registers the service worker should be injected
        /// into the bottom of the HTML page.
        /// </summary>
        public bool AutoRegisterServiceWorker { get; set; } = true;

        /// <summary>
        /// a flag that can be used to include or exclude consile.log statement in the generated script
        /// </summary>
        public bool EnableServiceWorkerConsoleLog { get; set; } 

        /// <summary>
        /// request paths to exclude auto registering the service worker ie /account/login,/account/register
        /// </summary>
        public string ExcludedAutoRegistrationPathsCsv { get; set; }

        /// <summary>
        /// Determines the value of the ServiceWorker CacheControl header Max-Age (in seconds)
        /// </summary>
        public int ServiceWorkerCacheControlMaxAgeInSeconds { get; set; }

        /// <summary>
        /// Determines whether a CSP nonce will be added via NWebSec
        /// </summary>
        public bool EnableCspNonce { get; set; }

        /// <summary>
        /// Generate code even on HTTP connection. Necessary for SSL offloading.
        /// </summary>
        public bool AllowHttp { get; set; }

        /// <summary>
        /// if true the current page will be reloaded after a service worker update
        /// this is needed for example if the page shows different things for authenticated vs unauthenticated users ie login or logout links in nav
        /// </summary>
        public bool ReloadPageOnServiceWorkerUpdate { get; set; } = true;

        public bool SetupInstallPrompt { get; set; } = true;

       

        /// <summary>
        /// the url for google workbox
        /// </summary>
        public string WorkBoxUrl { get; set; } = "https://storage.googleapis.com/workbox-cdn/releases/4.3.1/workbox-sw.js";



    }

    


}
