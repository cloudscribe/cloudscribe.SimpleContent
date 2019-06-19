using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.PwaKit
{
    public class PwaOptions
    {

        public PwaOptions()
        {
            RegisterServiceWorker = true;
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
        public bool RegisterServiceWorker { get; set; }

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
        /// the url for google workbox
        /// </summary>
        public string WorkBoxUrl { get; set; } = "https://storage.googleapis.com/workbox-cdn/releases/3.5.0/workbox-sw.js";



    }

    


}
