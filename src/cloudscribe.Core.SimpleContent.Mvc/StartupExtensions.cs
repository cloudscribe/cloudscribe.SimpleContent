using cloudscribe.Core.SimpleContent;
using cloudscribe.Core.SimpleContent.Integration;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeCoreIntegrationForSimpleContent(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {
            services.AddScoped<IProjectSettingsResolver, SiteProjectSettingsResolver>();
            services.AddScoped<IProjectSecurityResolver, ProjectSecurityResolver>();

            services.TryAddScoped<IMediaProcessor, SiteFileSystemMediaProcessor>();

            //services.AddScoped<MediaFolderHelper, MediaFolderHelper>();
            services.AddScoped<IBlogRoutes, MultiTenantBlogRoutes>();
            services.AddScoped<IPageRoutes, MultiTenantPageRoutes>();
            services.AddScoped<IPageNavigationCacheKeys, SiteNavigationCacheKeys>();
            services.AddScoped<IRoleSelectorProperties, SiteRoleSelectorProperties>();
            services.TryAddScoped<IAuthorNameResolver, AuthorNameResolver>();

            if (configuration != null)
            {
                services.Configure<ContentSettingsUIConfig>(configuration.GetSection("ContentSettingsUIConfig"));
            }
            else
            {
                services.Configure<ContentSettingsUIConfig>(c =>
                {
                    // not doing anything just configuring the default
                });
            }


            return services;
        }
    }
}
