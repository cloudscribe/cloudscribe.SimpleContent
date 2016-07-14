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

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IRouteBuilder AddStandardRoutesForSimpleContent(this IRouteBuilder routes)
        {
            routes.AddBlogRoutesForSimpleContent();
            
            routes.MapRoute(
               name: ProjectConstants.PageIndexRouteName,
               template: "{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
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

        public static IServiceCollection AddSimpleContent(
            this IServiceCollection services
            )
        {
            services.TryAddScoped<IBlogService, BlogService>();
            services.TryAddScoped<IPageService, PageService>();
            services.TryAddScoped<IProjectService, ProjectService>();
            services.TryAddScoped<IProjectSettingsResolver, DefaultProjectSettingsResolver>();
            services.TryAddScoped<IMediaProcessor, FileSystemMediaProcessor>();

            services.TryAddScoped<HtmlProcessor, HtmlProcessor>();
            services.TryAddScoped<IProjectEmailService, ProjectEmailService>();
            services.TryAddScoped<ViewRenderer, ViewRenderer>();

            services.AddScoped<INavigationTreeBuilder, PagesNavigationTreeBuilder>();
            services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();
            services.AddScoped<ISiteMapNodeService, BlogSiteMapNodeService>();

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
