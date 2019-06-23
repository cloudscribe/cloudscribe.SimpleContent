using cloudscribe.PwaKit.Integration.CloudscribeCore;
using cloudscribe.PwaKit.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {

        public static IServiceCollection AddPwaKitCloudscribeCoreIntegration(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            
            services.AddScoped<IPwaRouteNameProvider, PwaRouteNameProvider>();
            services.AddScoped<IWorkboxCacheSuffixProvider, LastModifiedWorkboxCacheSuffixProvider>();
            
            return services;
        }

    }
}
