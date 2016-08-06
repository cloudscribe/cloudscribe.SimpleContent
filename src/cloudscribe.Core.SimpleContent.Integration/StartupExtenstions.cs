using cloudscribe.Core.SimpleContent.Integration;
using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtenstions
    {
        public static IServiceCollection AddCloudscribeCoreIntegrationForSimpleContent(
            this IServiceCollection services
            )
        {
            services.AddScoped<IBlogRoutes, MultiTenantBlogRoutes>();

            return services;
        }

    }
}
