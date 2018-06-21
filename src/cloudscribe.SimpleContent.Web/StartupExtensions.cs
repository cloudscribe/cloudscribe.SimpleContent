using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.SimpleContent.Web;
using cloudscribe.SimpleContent.Web.Config;
using cloudscribe.SimpleContent.Web.Design;
using cloudscribe.SimpleContent.Web.Mvc;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.TagHelpers;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Common.Setup;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.SiteMap;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        

        public static IRouteBuilder AddStandardRoutesForSimpleContent(this IRouteBuilder routes)
        {
            routes.AddBlogRoutesForSimpleContent();
            routes.AddDefaultPageRouteForSimpleContent();


            return routes;
        }

        public static IRouteBuilder AddSimpleContentStaticResourceRoutes(this IRouteBuilder routes)
        {
            routes.MapRoute(
               name: "csscsrjs",
               template: "csscsr/js/{*slug}"
               , defaults: new { controller = "csscsr", action = "js" }
               );

            routes.MapRoute(
               name: "csscsrcss",
               template: "csscsr/css/{*slug}"
               , defaults: new { controller = "csscsr", action = "css" }
               );

            return routes;
        }

        public static IRouteBuilder AddDefaultPageRouteForSimpleContent(this IRouteBuilder routes)
        {
            routes.MapRoute(
               name: ProjectConstants.NewPageRouteName,
               template: "newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageEditRouteName,
               template: "edit/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageDevelopRouteName,
               template: "development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageTreeRouteName,
               template: "tree"
               , defaults: new { controller = "Page", action = "Tree" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageDeleteRouteName,
               template: "delete/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               );

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
              name: ProjectConstants.FolderNewPageRouteName,
              template: "{sitefolder}/newpage/{parentSlug?}"
              , defaults: new { controller = "Page", action = "NewPage" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageEditRouteName,
              template: "{sitefolder}/edit/{slug?}"
              , defaults: new { controller = "Page", action = "Edit" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageDevelopRouteName,
              template: "{sitefolder}/development/{slug}"
              , defaults: new { controller = "Page", action = "Development" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageTreeRouteName,
              template: "{sitefolder}/tree"
              , defaults: new { controller = "Page", action = "Tree" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageDeleteRouteName,
              template: "{sitefolder}/delete/{id}"
              , defaults: new { controller = "Page", action = "Delete" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.FolderPageIndexRouteName,
               template: "{sitefolder}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { name = siteFolderConstraint }
               );



            return routes;
        }

        public static IRouteBuilder AddCustomPageRouteForSimpleContent(this IRouteBuilder routes, string prefix)
        {
            routes.MapRoute(
               name: ProjectConstants.NewPageRouteName,
               template: prefix + "/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageEditRouteName,
               template: prefix + "/edit/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageDevelopRouteName,
               template: prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageTreeRouteName,
               template: prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageDeleteRouteName,
               template: prefix + "/delete/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageIndexRouteName,
               template: prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               );



            return routes;
        }

        public static IRouteBuilder AddCustomPageRouteForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            string prefix

            )
        {
            routes.MapRoute(
               name: ProjectConstants.FolderNewPageRouteName,
               template: "{sitefolder}/" + prefix + "/new/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageEditRouteName,
               template: "{sitefolder}/" + prefix + "/edit/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageDevelopRouteName,
               template: "{sitefolder}/" + prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageTreeRouteName,
               template: "{sitefolder}/" + prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageDeleteRouteName,
               template: "{sitefolder}/" + prefix + "/delete/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageIndexRouteName,
               template: "{sitefolder}/" + prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { name = siteFolderConstraint }
               );



            return routes;
        }

        private static string GetSegmentTemplate(string providedStartSegment)
        {
            string segmentResult = "";
            if (!string.IsNullOrEmpty(providedStartSegment))
            {
                if (providedStartSegment != string.Empty)
                {
                    if (!providedStartSegment.EndsWith("/"))
                    {
                        segmentResult = providedStartSegment + "/";
                    }
                }
            }

            return segmentResult;
        }

        public static IRouteBuilder AddBlogRoutesForSimpleContent(this IRouteBuilder routes, string startSegment = "blog")
        {
            string firstSegment = GetSegmentTemplate(startSegment);

            routes.MapRoute(
                   name: ProjectConstants.BlogCategoryRouteName,
                   template: firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   );


            routes.MapRoute(
                  ProjectConstants.BlogArchiveRouteName,
                  firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  //new { controller = "Blog", action = "Archive" },
                  new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                  ProjectConstants.PostWithDateRouteName,
                  firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
               name: ProjectConstants.PostEditRouteName,
               template: firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               );

            routes.MapRoute(
               name: ProjectConstants.PostDeleteRouteName,
               template: firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               );

            routes.MapRoute(
              name: ProjectConstants.MostRecentPostRouteName,
              template: firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              );

            routes.MapRoute(
               name: ProjectConstants.PostWithoutDateRouteName,
               template: firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               );

            routes.MapRoute(
               name: ProjectConstants.BlogIndexRouteName,
               template: firstSegment
               , defaults: new { controller = "Blog", action = "Index" }
               );

            return routes;
        }

        public static IRouteBuilder AddBlogRoutesForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            string startSegment = "blog"
            )
        {
            string firstSegment = GetSegmentTemplate(startSegment);

            routes.MapRoute(
                   name: ProjectConstants.FolderBlogCategoryRouteName,
                   template: "{sitefolder}/" + firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   , constraints: new { name = siteFolderConstraint }
                   );

            routes.MapRoute(
                  ProjectConstants.FolderBlogArchiveRouteName,
                  "{sitefolder}/" + firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { name = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                  ProjectConstants.FolderPostWithDateRouteName,
                  "{sitefolder}/" + firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { name = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
               name: ProjectConstants.FolderPostEditRouteName,
               template: "{sitefolder}/" + firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostDeleteRouteName,
               template: "{sitefolder}/" + firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderNewPostRouteName,
               template: "{sitefolder}/" + firstSegment + "new"
               , defaults: new { controller = "Blog", action = "New" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
              name: ProjectConstants.FolderMostRecentPostRouteName,
              template: "{sitefolder}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.FolderPostWithoutDateRouteName,
               template: "{sitefolder}/" + firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderBlogIndexRouteName,
               template: "{sitefolder}/" + firstSegment + ""
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { name = siteFolderConstraint }
               );


            return routes;
        }


        public static IServiceCollection AddSimpleContentMvc(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {
            services.AddSimpleContentCommon(configuration);

            services.AddScoped<IVersionProvider, ControllerVersionInfo>();

            return services;
        }


        public static IServiceCollection AddSimpleContentCommon(
            this IServiceCollection services,
            IConfiguration configuration = null
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

            //services.TryAddScoped<IHtmlProcessor, HtmlProcessor>();
            services.TryAddScoped<IMarkdownProcessor, MarkdownProcessor>();
            services.TryAddScoped<IContentProcessor, ContentProcessor>();

            services.TryAddScoped<TeaserCache>();
            services.TryAddScoped<ITeaserService, TeaserService>();

            
            services.TryAddScoped<IProjectEmailService, ProjectEmailService>();
            services.TryAddScoped<ViewRenderer, ViewRenderer>();

            services.TryAddScoped<IPageRouteHelper, DefaultPageRouteHelper>();
            services.TryAddScoped<IPageNavigationCacheKeys, PageNavigationCacheKeys>();
            services.AddScoped<INavigationTreeBuilder, PagesNavigationTreeBuilder>();
            //services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();
            services.AddScoped<ISiteMapNodeService, BlogSiteMapNodeService>();
            services.AddScoped<IFindCurrentNode, NavigationBlogNodeFinder>();

            services.TryAddScoped<IRoleSelectorProperties, NotImplementedRoleSelectorProperties>();

            services.AddScoped<IVersionProvider, VersionInfo>();
            services.AddScoped<IVersionProvider, DataStorageVersionInfo>();

            // registering an IOptions<IconCssClasses> that canbe injected into views
            if (configuration != null)
            {
                // To override the css icon classes create a json config section representing the IconCssClass
                services.Configure<IconCssClasses>(configuration.GetSection("IconCssClasses"));

                services.Configure<PageEditOptions>(configuration.GetSection("PageEditOptions"));
                services.Configure<SimpleContentConfig>(configuration.GetSection("SimpleContentConfig"));

                services.Configure<SimpleContentIconConfig>(configuration.GetSection("SimpleContentIconConfig"));
                services.Configure<SimpleContentThemeConfig>(configuration.GetSection("SimpleContentThemeConfig"));

                services.Configure<ContentTemplateConfig>(configuration.GetSection("ContentTemplateConfig"));


                //services.AddScoped<CoreThemeHelper>();
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

                services.Configure<SimpleContentIconConfig>(c =>
                {
                    // not doing anything just configuring the default
                });

                services.Configure<SimpleContentThemeConfig>(c =>
                {
                    // not doing anything just configuring the default
                });
            }

            services.TryAddScoped<IContentTemplateProvider, ConfigContentTemplateProvider>();
            services.AddScoped<ContentTemplateService>();

            services.TryAddScoped<ISimpleContentThemeHelper, DefaultSimpleContentThemeHelper>();

            services.AddMediatR(typeof(PageService).Assembly);

            services.AddScoped<IModelSerializer, JsonModelSerializer>();

            return services;
        }

        [Obsolete("this method is deprecated, please use services.AddSimpleContentMvc instead.")]
        public static IServiceCollection AddSimpleContent(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {

            services.AddSimpleContentCommon(configuration);

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
