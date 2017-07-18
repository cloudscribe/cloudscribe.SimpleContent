using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.SimpleContent.Web.Config;
using cloudscribe.SimpleContent.Web.Design;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.TagHelpers;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.SiteMap;
//using cloudscribe.FileManager.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddSimpleContent(
            this IServiceCollection services,
            IConfigurationRoot configuration = null
            )
        {
            services.TryAddScoped<IPageRoutes, DefaultPageRoutes>();
            services.TryAddScoped<IBlogRoutes, DefaultBlogRoutes>();
            services.TryAddScoped<IBlogService, BlogService>();
            services.TryAddScoped<IAuthorNameResolver, DefaultAuthorNameResolver>();
            services.TryAddScoped<PageEvents, PageEvents>();
            services.TryAddScoped<PostEvents, PostEvents>();

            services.TryAddScoped<IPageService, PageService>();
            services.TryAddScoped<IProjectService, ProjectService>();
            services.TryAddScoped<IProjectSettingsResolver, DefaultProjectSettingsResolver>();
            services.TryAddScoped<IMediaProcessor, FileSystemMediaProcessor>();

            services.TryAddScoped<IHtmlProcessor, HtmlProcessor>();
            services.TryAddScoped<IProjectEmailService, ProjectEmailService>();
            services.TryAddScoped<ViewRenderer, ViewRenderer>();

            services.TryAddScoped<IPageRouteHelper, DefaultPageRouteHelper>();
            services.TryAddScoped<IPageNavigationCacheKeys, PageNavigationCacheKeys>();
            services.AddScoped<INavigationTreeBuilder, PagesNavigationTreeBuilder>();
            services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();
            services.AddScoped<ISiteMapNodeService, BlogSiteMapNodeService>();
            services.AddScoped<IFindCurrentNode, NavigationBlogNodeFinder>();

            services.TryAddScoped<IRoleSelectorProperties, NotImplementedRoleSelectorProperties>();

            // registering an IOptions<IconCssClasses> that canbe injected into views
            if (configuration != null)
            {
                // To override the css icon classes create a json config section representing the IconCssClass
                services.Configure<IconCssClasses>(configuration.GetSection("IconCssClasses"));

                services.Configure<PageEditOptions>(configuration.GetSection("PageEditOptions"));
                services.Configure<SimpleContentConfig>(configuration.GetSection("SimpleContentConfig"));
            }
            else
            {
                services.Configure<PageEditOptions>(c =>
                {
                    // not doing anything just configuring the default
                });

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
        //[Obsolete("AddEmbeddedViewsForSimpleContent is deprecated, please use AddBootstrap3EmbeddedViewsForSimpleContent instead.")]
        //public static RazorViewEngineOptions AddEmbeddedViewsForSimpleContent(this RazorViewEngineOptions options)
        //{
        //    options.AddBootstrap3EmbeddedViewsForSimpleContent();
        //    //options.FileProviders.Add(new EmbeddedFileProvider(
        //    //        typeof(BlogController).GetTypeInfo().Assembly,
        //    //        "cloudscribe.SimpleContent.Web"
        //    //    ));

        //    return options;
        //}
    }
}
