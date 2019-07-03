using cloudscribe.Core.SimpleContent;
using cloudscribe.Core.SimpleContent.Integration;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Design;
using cloudscribe.SimpleContent.Web.TagHelpers;
using cloudscribe.Web.Navigation.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;



namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtenstions
    {
        public static IServiceCollection AddCloudscribeCoreIntegrationForSimpleContent(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {
            services.AddScoped<IProjectSettingsResolver, SiteProjectSettingsResolver>();
            services.AddScoped<IProjectSecurityResolver, ProjectSecurityResolver>();
            services.TryAddScoped<IMediaProcessor, SiteFileSystemMediaProcessor>();
            services.AddScoped<IBlogRoutes, MultiTenantBlogRoutes>();
            services.AddScoped<IPageRoutes, MultiTenantPageRoutes>();
            services.AddScoped<IRoleSelectorProperties, SiteRoleSelectorProperties>();
            services.TryAddScoped<IAuthorNameResolver, AuthorNameResolver>();
            services.TryAddScoped<IProjectEmailService, CoreProjectEmailService>();
            services.AddScoped<ISimpleContentThemeHelper, SiteSimpleContentThemeHelper>();

            services.AddScoped<ITreeCacheKeyResolver, SiteNavigationCacheKeyResolver>();
            

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


        

        public static AuthorizationOptions AddCloudscribeCoreSimpleContentIntegrationDefaultPolicies(this AuthorizationOptions options)
        {
            options.AddPolicy("BlogViewPolicy", policy =>
                policy.RequireAssertion(context =>
                {
                    return true; //allow anonymous
                })
                );

            

            options.AddPolicy(
                    "BlogEditPolicy",
                    authBuilder =>
                    {
                        //authBuilder.RequireClaim("blogId");
                        authBuilder.RequireRole("Administrators", "Content Administrators");
                    }
                 );

            options.AddPolicy(
                "PageEditPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Administrators", "Content Administrators");
                });

            options.AddPolicy(
                    "ViewContentHistoryPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators", "Content Administrators");
                    }
                 );

            return options;
        }




    }
}
