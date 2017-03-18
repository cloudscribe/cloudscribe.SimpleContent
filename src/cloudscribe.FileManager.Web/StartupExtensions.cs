using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

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
            services.TryAddScoped<IFileExtensionValidationRegexBuilder, FileExtensionValidationRegexBuilder>();
            services.TryAddScoped<IMediaPathResolver, DefaultMediaPathResolver>();

            // Angular's default header name for sending the XSRF token.
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            if(configuration != null)
            {
                services.Configure<FileManagerIcons>(configuration.GetSection("FileManagerIcons"));
            }

            return services;
        }

        public static IRouteBuilder AddCloudscribeFileManagerRoutes(this IRouteBuilder routes)
        {
            routes.MapRoute(
               name: "filemanagerjs",
               template: "filemanager/js/{*slug}"
               , defaults: new { controller = "FileManager", action = "js" }
               );

            routes.MapRoute(
               name: "filemanagercss",
               template: "filemanager/css/{*slug}"
               , defaults: new { controller = "FileManager", action = "css" }
               );

            routes.MapRoute(
               name: "filemanagerfonts",
               template: "filemanager/font/{*slug}"
               , defaults: new { controller = "FileManager", action = "font" }
               );

            return routes;
        }


        

    }
}
namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmbeddedViews
    {
        public static RazorViewEngineOptions AddBootstrap3EmbeddedViewsForFileManager(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(EmbeddedViews).GetTypeInfo().Assembly,
                    "cloudscribe.FileManager.Web"
                ));

            return options;
        }
    }
}
