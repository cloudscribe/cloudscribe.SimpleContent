using System;
using System.Collections.Generic;
using System.Text;
using cloudscribe.PwaKit.Integration.Navigation;
using cloudscribe.PwaKit.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddPwaKitNavigationIntegration(
           this IServiceCollection services,
           IConfiguration config
           )
        {
            services.AddScoped<IPreCacheItemProvider, NavigationPreCacheItemProvider>();

            return services;
        }

    }
}
