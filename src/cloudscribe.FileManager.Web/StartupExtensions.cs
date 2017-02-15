using cloudscribe.FileManager.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeFileManager(
            this IServiceCollection services
            )
        {
            services.TryAddScoped<FileManagerService>();

            return services;
        }
    }
}
