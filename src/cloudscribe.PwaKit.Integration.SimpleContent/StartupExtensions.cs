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


            return services;
        }

    }
}
