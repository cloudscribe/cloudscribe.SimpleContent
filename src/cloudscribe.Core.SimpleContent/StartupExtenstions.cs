using cloudscribe.Core.SimpleContent;
using cloudscribe.Core.SimpleContent.Integration;
using cloudscribe.Core.SimpleContent.Integration.Mvc.Controllers;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Design;
using cloudscribe.SimpleContent.Web.TagHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace cloudscribe.Core.SimpleContent
{
    public static class Extensions
    {
        public static RazorViewEngineOptions AddEmbeddedViewsForCloudscribeCoreSimpleContentIntegration(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(ContentSettingsController).GetTypeInfo().Assembly,
                    "cloudscribe.Core.SimpleContent"
                ));

            return options;
        }
    }

}


namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtenstions
    {
        public const string FolderPostWithDateRouteName = "folderpostwithdate";
        public const string FolderPostWithoutDateRouteName = "folderpostwithoutdate";
        public const string FolderBlogCategoryRouteName = "folderblogcategory";
        public const string FolderBlogArchiveRouteName = "folderblogarchive";
        public const string FolderBlogIndexRouteName = "folderblogindex";
        public const string FolderNewPostRouteName = "foldernewpost";
        public const string FolderPageIndexRouteName = "folderpageindex";

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
            services.TryAddScoped<IProjectEmailService, CoreProjectEmailService>();
            services.AddScoped<ISimpleContentThemeHelper, SiteSimpleContentThemeHelper>();

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

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    options.AddEmbeddedViewsForCloudscribeCoreSimpleContentIntegration();

                });

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

            return options;
        }




    }
}
