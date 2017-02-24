using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Services;
using Microsoft.Extensions.Configuration;
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
            this IServiceCollection services,
            IConfigurationRoot configuration = null
            )
        {
            services.TryAddScoped<FileManagerService>();
            services.TryAddScoped<IImageResizer, ImageResizerService>();
            services.TryAddScoped<IFileManagerNameRules, DefaultFileManagerNameRules>();

            // Angular's default header name for sending the XSRF token.
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            if(configuration != null)
            {
                services.Configure<FileManagerIcons>(configuration.GetSection("FileManagerIcons"));
            }

            return services;
        }
    }
}
