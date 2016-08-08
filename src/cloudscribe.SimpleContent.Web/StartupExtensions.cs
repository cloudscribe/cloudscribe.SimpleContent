using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.SimpleContent.Services;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.SiteMap;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using cloudscribe.SimpleContent.Web.Services;
using Microsoft.Extensions.Configuration;
using cloudscribe.SimpleContent.Web.Design;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IRouteBuilder AddStandardRoutesForSimpleContent(this IRouteBuilder routes)
        {
            routes.AddBlogRoutesForSimpleContent();
            routes.AddDefaultPageRouteForSimpleContent();
            
            return routes;
        }

        public static IRouteBuilder AddDefaultPageRouteForSimpleContent(this IRouteBuilder routes)
        {
           
            routes.MapRoute(
               name: ProjectConstants.PageIndexRouteName,
               template: "{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               );

            return routes;
        }

        public static IRouteBuilder AddDefaultPageRouteForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint siteFolderConstraint
            )
        {

            routes.MapRoute(
               name: ProjectConstants.FolderPageIndexRouteName,
               template: "{sitefolder}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { name = siteFolderConstraint }
               );

            return routes;
        }

        public static IRouteBuilder AddBlogRoutesForSimpleContent(this IRouteBuilder routes)
        {
            routes.MapRoute(
                   name: ProjectConstants.BlogCategoryRouteName,
                   template: "blog/category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   );


            routes.MapRoute(
                  ProjectConstants.BlogArchiveRouteName,
                  "blog/{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  //new { controller = "Blog", action = "Archive" },
                  new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                  ProjectConstants.PostWithDateRouteName,
                  "blog/{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
               name: ProjectConstants.NewPostRouteName,
               template: "blog/new"
               , defaults: new { controller = "Blog", action = "New" }
               );

            routes.MapRoute(
               name: ProjectConstants.PostWithoutDateRouteName,
               template: "blog/{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               );

            routes.MapRoute(
               name: ProjectConstants.BlogIndexRouteName,
               template: "blog/"
               , defaults: new { controller = "Blog", action = "Index" }
               );
            
            return routes;
        }

        public static IRouteBuilder AddBlogRoutesForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint siteFolderConstraint
            )
        {
            routes.MapRoute(
                   name: ProjectConstants.FolderBlogCategoryRouteName,
                   template: "{sitefolder}/blog/category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   , constraints: new { name = siteFolderConstraint }
                   );

            routes.MapRoute(
                  ProjectConstants.FolderBlogArchiveRouteName,
                  "{sitefolder}/blog/{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { name = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                  ProjectConstants.FolderPostWithDateRouteName,
                  "{sitefolder}/blog/{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { name = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
               name: ProjectConstants.FolderNewPostRouteName,
               template: "{sitefolder}/blog/new"
               , defaults: new { controller = "Blog", action = "New" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostWithoutDateRouteName,
               template: "{sitefolder}/blog/{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderBlogIndexRouteName,
               template: "{sitefolder}/blog/"
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { name = siteFolderConstraint }
               );


            return routes;
        }

        public static IServiceCollection AddSimpleContent(
            this IServiceCollection services,
            IConfigurationRoot configuration = null
            )
        {

            services.TryAddScoped<IBlogRoutes, DefaultBlogRoutes>();
            services.TryAddScoped<IBlogService, BlogService>();
            services.TryAddScoped<IPageService, PageService>();
            services.TryAddScoped<IProjectService, ProjectService>();
            services.TryAddScoped<IProjectSettingsResolver, DefaultProjectSettingsResolver>();
            services.TryAddScoped<IMediaProcessor, FileSystemMediaProcessor>();

            services.TryAddScoped<HtmlProcessor, HtmlProcessor>();
            services.TryAddScoped<IProjectEmailService, ProjectEmailService>();
            services.TryAddScoped<ViewRenderer, ViewRenderer>();

            services.TryAddScoped<IPageRouteHelper, DefaultPageRouteHelper>();
            services.AddScoped<INavigationTreeBuilder, PagesNavigationTreeBuilder>();
            services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();
            services.AddScoped<ISiteMapNodeService, BlogSiteMapNodeService>();

            // registering an IOptions<IconCssClasses> that canbe injected into views
            if (configuration != null)
            {
                // To override the css icon classes create a json config section representing the IconCssClass
                services.Configure<IconCssClasses>(configuration.GetSection("IconCssClasses"));
            }
            else
            {
                services.Configure<IconCssClasses>(c =>
                {
                    // not doing anything just configuring the default
                });    
            }

            return services;
        }

        /// <summary>
        /// This method adds an embedded file provider to the RazorViewOptions to be able to load the Blog/Pages related views.
        /// If you download and install the views below your view folder you don't need this method and you can customize the views.
        /// You can get the views from https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/src/cloudscribe.SimpleContent.Blog.Web/Views
        /// </summary>
        /// <param name="options"></param>
        /// <returns>RazorViewEngineOptions</returns>
        public static RazorViewEngineOptions AddEmbeddedViewsForSimpleContent(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(BlogController).GetTypeInfo().Assembly,
                    "cloudscribe.SimpleContent.Web"
                ));

            return options;
        }
    }
}
