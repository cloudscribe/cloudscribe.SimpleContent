using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

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
            string prefix,
            IRouteConstraint siteFolderConstraint
            )
        {
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
               name: ProjectConstants.PostEditRouteName,
               template: "blog/edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               );

            routes.MapRoute(
               name: ProjectConstants.PostDeleteRouteName,
               template: "blog/delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               );

            //routes.MapRoute(
            //   name: ProjectConstants.NewPostRouteName,
            //   template: "blog/new"
            //   , defaults: new { controller = "Blog", action = "New" }
            //   );

            routes.MapRoute(
              name: ProjectConstants.MostRecentPostRouteName,
              template: "blog/mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
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
               name: ProjectConstants.FolderPostEditRouteName,
               template: "{sitefolder}/blog/edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostDeleteRouteName,
               template: "{sitefolder}/blog/delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderNewPostRouteName,
               template: "{sitefolder}/blog/new"
               , defaults: new { controller = "Blog", action = "New" }
               , constraints: new { name = siteFolderConstraint }
               );

            routes.MapRoute(
              name: ProjectConstants.FolderMostRecentPostRouteName,
              template: "{sitefolder}/blog/mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
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
    }
}
