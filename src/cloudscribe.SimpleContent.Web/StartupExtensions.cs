using cloudscribe.DateTimeUtils;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.SimpleContent.Web;
using cloudscribe.SimpleContent.Web.Design;
using cloudscribe.SimpleContent.Web.Mvc;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.TagHelpers;
using cloudscribe.SimpleContent.Web.Templating;
using cloudscribe.Versioning;
using cloudscribe.Web.Common.Razor;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.SiteMap;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
       
        public static IServiceCollection AddSimpleContentMvc(
            this IServiceCollection services,
            IConfiguration configuration
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

            services.TryAddScoped<IMarkdownProcessor, MarkdownProcessor>();
            services.TryAddScoped<IContentProcessor, ContentProcessor>();

            services.TryAddScoped<TeaserCache>();
            services.TryAddScoped<ITeaserService, TeaserService>();
            
            services.TryAddScoped<IProjectEmailService, ProjectEmailService>();
            services.TryAddScoped<ViewRenderer, ViewRenderer>();
            
            services.TryAddScoped<IPageNavigationCacheKeys, PageNavigationCacheKeys>();
            services.AddScoped<INavigationTreeBuilder, PagesNavigationTreeBuilder>();
            services.AddScoped<ISiteMapNodeService, BlogSiteMapNodeService>();
            services.AddScoped<IFindCurrentNode, NavigationBlogNodeFinder>();

            services.TryAddScoped<IRoleSelectorProperties, NotImplementedRoleSelectorProperties>();

            services.AddScoped<IVersionProvider, VersionInfo>();
            services.AddScoped<IVersionProvider, DataStorageVersionInfo>();

            if (configuration != null)
            {   
                services.Configure<IconCssClasses>(configuration.GetSection("IconCssClasses"));
                services.Configure<PageEditOptions>(configuration.GetSection("PageEditOptions"));
                services.Configure<BlogEditOptions>(configuration.GetSection("BlogEditOptions"));
                services.Configure<SimpleContentIconConfig>(configuration.GetSection("SimpleContentIconConfig"));
                services.Configure<SimpleContentThemeConfig>(configuration.GetSection("SimpleContentThemeConfig"));
                services.Configure<ContentTemplateConfig>(configuration.GetSection("ContentTemplateConfig"));
            }
            else
            {
                // not doing anything just configuring the default
                services.Configure<IconCssClasses>(c => {  });
                services.Configure<PageEditOptions>(c => { });
                services.Configure<BlogEditOptions>(c => { });
                services.Configure<SimpleContentIconConfig>(c => { });
                services.Configure<SimpleContentThemeConfig>(c => {  });
                services.Configure<ContentTemplateConfig>(c => { });
            }
            
            services.TryAddScoped<ISimpleContentThemeHelper, DefaultSimpleContentThemeHelper>();
            
            services.AddMediatR(typeof(PageService).Assembly);
            
            services.AddScoped<IParseModelFromForm, DefaultModelFormParser>();
            services.AddScoped<IValidateTemplateModel, DefaultTemplateModelValidator>();
            
            services.TryAddScoped<IPageUrlResolver, PageUrlResolver>();
            services.TryAddScoped<IBlogUrlResolver, BlogUrlResolver>();

            services.AddSingleton<IModelSerializer, JsonModelSerializer>();
            services.AddSingleton<IContentTemplateProvider, ConfigContentTemplateProvider>();
            services.TryAddSingleton<IContentTemplateService, ContentTemplateService>();

            services.TryAddScoped<ITimeZoneIdResolver, DefaultTimeZoneIdResolver>();

            return services;
        }

        

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
               template: "editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageEditWithTemplateRouteName,
               template: "editwithtemplate/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithTemplate" }
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
               template: "deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageHistoryRouteName,
               template: "history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
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
              template: "{sitefolder}/editpage/{slug?}"
              , defaults: new { controller = "Page", action = "Edit" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageEditWithTemplateRouteName,
              template: "{sitefolder}/editwithtemplate/{slug?}"
              , defaults: new { controller = "Page", action = "EditWithTemplate" }
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
              template: "{sitefolder}/deletepage/{id}"
              , defaults: new { controller = "Page", action = "Delete" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.FolderPageHistoryRouteName,
               template: "{sitefolder}/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
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
               template: prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageEditWithTemplateRouteName,
               template: prefix + "/editwithtemplate/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithTemplate" }
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
               template: prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               );

            routes.MapRoute(
               name: ProjectConstants.PageHistoryRouteName,
               template: prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
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
               template: "{sitefolder}/" + prefix + "/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageEditRouteName,
               template: "{sitefolder}/" + prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageEditWithTemplateRouteName,
               template: "{sitefolder}/" + prefix + "/editwithtemplate/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithTemplate" }
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
               template: "{sitefolder}/" + prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageHistoryRouteName,
               template: "{sitefolder}/" + prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
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
               name: ProjectConstants.NewPostRouteName,
               template: firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               );

            routes.MapRoute(
               name: ProjectConstants.PostEditWithTemplateRouteName,
               template: firstSegment + "editwithtemplate/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithTemplate" }
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
               name: ProjectConstants.PostHistoryRouteName,
               template: firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
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
               name: ProjectConstants.FolderNewPostRouteName,
               template: "{sitefolder}/" + firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostEditWithTemplateRouteName,
               template: "{sitefolder}/" + firstSegment + "editwithtemplate/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithTemplate" }
               , constraints: new { name = siteFolderConstraint }
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
              name: ProjectConstants.FolderMostRecentPostRouteName,
              template: "{sitefolder}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              , constraints: new { name = siteFolderConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.FolderPostHistoryRouteName,
               template: "{sitefolder}/" + firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
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


    }
}
