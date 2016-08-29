using cloudscribe.Core.SimpleContent.Integration;
using cloudscribe.Core.SimpleContent.Integration.Controllers;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.TagHelpers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

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
            this IServiceCollection services
            )
        {
            services.AddScoped<MediaFolderHelper, MediaFolderHelper>();
            services.AddScoped<IBlogRoutes, MultiTenantBlogRoutes>();
            services.AddScoped<IPageNavigationCacheKeys, SiteNavigationCacheKeys>();
            services.AddScoped<IRoleSelectorProperties, SiteRoleSelectorProperties>();

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
