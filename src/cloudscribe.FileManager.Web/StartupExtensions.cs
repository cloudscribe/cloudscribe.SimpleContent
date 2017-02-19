using cloudscribe.FileManager.Web.Models;
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
            services.TryAddScoped<IImageResizer, ImageResizerService>();

            // Angular's default header name for sending the XSRF token.
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            return services;
        }
    }
}
