using cloudscribe.PwaKit.Integration.SimpleContent;
using cloudscribe.PwaKit.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddPwaKitSimpleContentIntegration(
           this IServiceCollection services,
           IConfiguration config
           )
        {

            services.AddScoped<IPreCacheItemProvider, BlogPreCacheItemProvider>();


            return services;
        }

    }
}
