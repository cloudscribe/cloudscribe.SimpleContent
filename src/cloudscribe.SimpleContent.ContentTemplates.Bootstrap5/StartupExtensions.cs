using cloudscribe.SimpleContent.ContentTemplates.Bootstrap5;
using cloudscribe.SimpleContent.ContentTemplates.Configuration;
using cloudscribe.SimpleContent.ContentTemplates.Services;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddContentTemplatesForSimpleContent(
           this IServiceCollection services,
           IConfiguration configuration
           )
        {
            services.AddSingleton<IContentTemplateProvider, ContentTemplateProvider>();

            services.Configure<GalleryOptions>(configuration.GetSection("ContentTemplateSettings:GalleryOptions"));
            services.TryAddScoped<IGalleryOptionsProvider, ConfigGalleryOptionsProvider>();

            services.Configure<LinkListOptions>(configuration.GetSection("ContentTemplateSettings:LinkListOptions"));
            services.TryAddScoped<ILinkListOptionsProvider, ConfigLinkListOptionsProvider>();

            services.Configure<ColumnTemplateOptions>(configuration.GetSection("ContentTemplateSettings:ColumnTemplateOptions"));
            services.TryAddScoped<IColumnTemplateOptionsProvider, ConfigColumnTemplateOptionsProvider>();

            services.Configure<ImageWithContentOptions>(configuration.GetSection("ContentTemplateSettings:ImageWithContentOptions"));
            services.TryAddScoped<IImageWithContentOptionsProvider, ConfigImageWithContentOptionsProvider>();
            services.AddScoped<IVersionProvider, VersionProvider>();



            return services;
        }
    }
}
