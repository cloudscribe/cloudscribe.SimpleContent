using cloudscribe.Core.SimpleContent.Integration;
using cloudscribe.Core.SimpleContent.Integration.Controllers;
using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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


        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeCoreSimpleContentIntegration(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(ContentSettingsController).GetTypeInfo().Assembly,
                    "cloudscribe.Core.SimpleContent.Integration"
                ));

            return options;
        }

    }
}
