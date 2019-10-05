using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    public static class Routes
    {
        //public static IRouteBuilder AddStandardRoutesForSimpleContent(this IRouteBuilder routes)
        //{
        //    routes.AddBlogRoutesForSimpleContent();
        //    routes.AddDefaultPageRouteForSimpleContent();


        //    return routes;
        //}

        public static IEndpointRouteBuilder AddSimpleContentStaticResourceRoutes(this IEndpointRouteBuilder routes)
        {
            routes.MapControllerRoute(
               name: "csscsrjs",
               pattern: "csscsr/js/{*slug}"
               , defaults: new { controller = "csscsr", action = "js" }
               );

            routes.MapControllerRoute(
               name: "csscsrcss",
               pattern: "csscsr/css/{*slug}"
               , defaults: new { controller = "csscsr", action = "css" }
               );

            return routes;
        }

        public static IEndpointRouteBuilder AddDefaultPageRouteForSimpleContent(this IEndpointRouteBuilder routes)
        {
            routes.MapControllerRoute(
               name: ProjectConstants.NewPageRouteName,
               pattern: "newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageEditRouteName,
               pattern: "editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageEditWithTemplateRouteName,
               pattern: "editwithpattern/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithpattern" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageDevelopRouteName,
               pattern: "development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageTreeRouteName,
               pattern: "tree"
               , defaults: new { controller = "Page", action = "Tree" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageDeleteRouteName,
               pattern: "deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageHistoryRouteName,
               pattern: "history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageIndexRouteName,
               pattern: "{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageCanEditRouteName,
               pattern: "page/canedit"
               , defaults: new { controller = "Page", action = "CanEdit" }
               );




            return routes;
        }

        public static IEndpointRouteBuilder AddCulturePageRouteForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint cultureConstraint
            )
        {
            routes.MapControllerRoute(
               name: ProjectConstants.CultureNewPageRouteName,
               pattern: "{culture}/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageEditRouteName,
               pattern: "{culture}/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageEditWithTemplateRouteName,
               pattern: "{culture}/editwithpattern/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithpattern" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageDevelopRouteName,
               pattern: "{culture}/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageTreeRouteName,
               pattern: "{culture}/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageDeleteRouteName,
               pattern: "{culture}/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageHistoryRouteName,
               pattern: "{culture}/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageIndexRouteName,
               pattern: "{culture}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
              name: ProjectConstants.CulturePageCanEditRouteName,
              pattern: "{culture}/page/canedit"
              , defaults: new { controller = "Page", action = "CanEdit" }
              , constraints: new { culture = cultureConstraint }
              );



            return routes;
        }

        public static IEndpointRouteBuilder AddDefaultPageRouteForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint siteFolderConstraint
            )
        {
            routes.MapControllerRoute(
              name: ProjectConstants.FolderNewPageRouteName,
              pattern: "{sitefolder}/newpage/{parentSlug?}"
              , defaults: new { controller = "Page", action = "NewPage" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.FolderPageEditRouteName,
              pattern: "{sitefolder}/editpage/{slug?}"
              , defaults: new { controller = "Page", action = "Edit" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.FolderPageEditWithTemplateRouteName,
              pattern: "{sitefolder}/editwithpattern/{slug?}"
              , defaults: new { controller = "Page", action = "EditWithpattern" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.FolderPageDevelopRouteName,
              pattern: "{sitefolder}/development/{slug}"
              , defaults: new { controller = "Page", action = "Development" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.FolderPageTreeRouteName,
              pattern: "{sitefolder}/tree"
              , defaults: new { controller = "Page", action = "Tree" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.FolderPageDeleteRouteName,
              pattern: "{sitefolder}/deletepage/{id}"
              , defaults: new { controller = "Page", action = "Delete" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageHistoryRouteName,
               pattern: "{sitefolder}/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageIndexRouteName,
               pattern: "{sitefolder}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageCanEditRouteName,
               pattern: "{sitefolder}/page/canedit"
               , defaults: new { controller = "Page", action = "CanEdit" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );




            return routes;
        }

        public static IEndpointRouteBuilder AddCulturePageRouteForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            IRouteConstraint cultureConstraint
            )
        {
            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderNewPageRouteName,
              pattern: "{sitefolder}/{culture}/newpage/{parentSlug?}"
              , defaults: new { controller = "Page", action = "NewPage" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderPageEditRouteName,
              pattern: "{sitefolder}/{culture}/editpage/{slug?}"
              , defaults: new { controller = "Page", action = "Edit" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderPageEditWithTemplateRouteName,
              pattern: "{sitefolder}/{culture}/editwithpattern/{slug?}"
              , defaults: new { controller = "Page", action = "EditWithpattern" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderPageDevelopRouteName,
              pattern: "{sitefolder}/{culture}/development/{slug}"
              , defaults: new { controller = "Page", action = "Development" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderPageTreeRouteName,
              pattern: "{sitefolder}/{culture}/tree"
              , defaults: new { controller = "Page", action = "Tree" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderPageDeleteRouteName,
              pattern: "{sitefolder}/{culture}/deletepage/{id}"
              , defaults: new { controller = "Page", action = "Delete" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageHistoryRouteName,
               pattern: "{sitefolder}/{culture}/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageIndexRouteName,
               pattern: "{sitefolder}/{culture}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderPageCanEditRouteName,
              pattern: "{sitefolder}/{culture}/page/canedit"
              , defaults: new { controller = "Page", action = "CanEdit" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );




            return routes;
        }

        public static IEndpointRouteBuilder AddCustomPageRouteForSimpleContent(
            this IEndpointRouteBuilder routes,
            string prefix)
        {
            routes.MapControllerRoute(
               name: ProjectConstants.NewPageRouteName,
               pattern: prefix + "/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageEditRouteName,
               pattern: prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageEditWithTemplateRouteName,
               pattern: prefix + "/editwithpattern/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithpattern" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageDevelopRouteName,
               pattern: prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageTreeRouteName,
               pattern: prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageDeleteRouteName,
               pattern: prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageHistoryRouteName,
               pattern: prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageIndexRouteName,
               pattern: prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PageCanEditRouteName,
               pattern: prefix + "/page/canedit"
               , defaults: new { controller = "Page", action = "CanEdit" }
               );



            return routes;
        }

        public static IEndpointRouteBuilder AddCultureCustomPageRouteForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint cultureConstraint,
            string prefix)
        {
            routes.MapControllerRoute(
               name: ProjectConstants.CultureNewPageRouteName,
               pattern: "{culture}/" + prefix + " / newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageEditRouteName,
               pattern: "{culture}/" + prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { culture = cultureConstraint }
               );


            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageEditWithTemplateRouteName,
               pattern: "{culture}/" + prefix + "/editwithpattern/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithpattern" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageDevelopRouteName,
               pattern: "{culture}/" + prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageTreeRouteName,
               pattern: "{culture}/" + prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageDeleteRouteName,
               pattern: "{culture}/" + prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageHistoryRouteName,
               pattern: "{culture}/" + prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageIndexRouteName,
               pattern: "{culture}/" + prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePageCanEditRouteName,
               pattern: "{culture}/" + prefix + "/page/canedit"
               , defaults: new { controller = "Page", action = "CanEdit" }
               , constraints: new { culture = cultureConstraint }
               );



            return routes;
        }


        public static IEndpointRouteBuilder AddCustomPageRouteForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            string prefix

            )
        {
            routes.MapControllerRoute(
               name: ProjectConstants.FolderNewPageRouteName,
               pattern: "{sitefolder}/" + prefix + "/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageEditRouteName,
               pattern: "{sitefolder}/" + prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageEditWithTemplateRouteName,
               pattern: "{sitefolder}/" + prefix + "/editwithpattern/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithpattern" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageDevelopRouteName,
               pattern: "{sitefolder}/" + prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageTreeRouteName,
               pattern: "{sitefolder}/" + prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageDeleteRouteName,
               pattern: "{sitefolder}/" + prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageHistoryRouteName,
               pattern: "{sitefolder}/" + prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageIndexRouteName,
               pattern: "{sitefolder}/" + prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPageCanEditRouteName,
               pattern: "{sitefolder}/" + prefix + "/page/canedit"
               , defaults: new { controller = "Page", action = "CanEdit" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );



            return routes;
        }

        public static IEndpointRouteBuilder AddCultureCustomPageRouteForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            IRouteConstraint cultureConstraint,
            string prefix

            )
        {
            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderNewPageRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageEditRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageEditWithTemplateRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/editwithpattern/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithpattern" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageDevelopRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageTreeRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageDeleteRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageHistoryRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageIndexRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPageCanEditRouteName,
               pattern: "{sitefolder}/{culture}/" + prefix + "/page/canedit"
               , defaults: new { controller = "Page", action = "CanEdit" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );



            return routes;
        }

        private static string GetSegmentpattern(string providedStartSegment)
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

        public static IEndpointRouteBuilder AddBlogRoutesForSimpleContent(
            this IEndpointRouteBuilder routes,
            string startSegment = "blog")
        {
            string firstSegment = GetSegmentpattern(startSegment);

            routes.MapControllerRoute(
                   name: ProjectConstants.BlogCategoryRouteName,
                   pattern: firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   );


            routes.MapControllerRoute(
                  ProjectConstants.BlogArchiveRouteName,
                  firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  //new { controller = "Blog", action = "Archive" },
                  new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
                  ProjectConstants.PostWithDateRouteName,
                  firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
               name: ProjectConstants.NewPostRouteName,
               pattern: firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PostEditWithTemplateRouteName,
               pattern: firstSegment + "editwithpattern/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithpattern" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PostEditRouteName,
               pattern: firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PostDeleteRouteName,
               pattern: firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               );

            routes.MapControllerRoute(
              name: ProjectConstants.MostRecentPostRouteName,
              pattern: firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              );

            routes.MapControllerRoute(
               name: ProjectConstants.PostHistoryRouteName,
               pattern: firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.PostWithoutDateRouteName,
               pattern: firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.BlogIndexRouteName,
               pattern: firstSegment
               , defaults: new { controller = "Blog", action = "Index" }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.BlogCanEditRouteName,
               pattern: firstSegment + "blog/canedit"
               , defaults: new { controller = "Blog", action = "CanEdit" }
               );

            return routes;
        }

        public static IEndpointRouteBuilder AddCultureBlogRoutesForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint cultureConstraint,
            string startSegment = "blog")
        {
            string firstSegment = GetSegmentpattern(startSegment);

            routes.MapControllerRoute(
                   name: ProjectConstants.CultureBlogCategoryRouteName,
                   pattern: "{culture}/" + firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   , constraints: new { culture = cultureConstraint }
                   );


            routes.MapControllerRoute(
                  ProjectConstants.CultureBlogArchiveRouteName,
                  "{culture}/" + firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
                  ProjectConstants.CulturePostWithDateRouteName,
                  "{culture}/" + firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureNewPostRouteName,
               pattern: "{culture}/" + firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePostEditWithTemplateRouteName,
               pattern: "{culture}/" + firstSegment + "editwithpattern/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithpattern" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePostEditRouteName,
               pattern: "{culture}/" + firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePostDeleteRouteName,
               pattern: "{culture}/" + firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureMostRecentPostRouteName,
              pattern: "{culture}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePostHistoryRouteName,
               pattern: "{culture}/" + firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CulturePostWithoutDateRouteName,
               pattern: "{culture}/" + firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureBlogIndexRouteName,
               pattern: "{culture}/" + firstSegment
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureBlogCanEditRouteName,
               pattern: "{culture}/" + firstSegment + "blog/canedit"
               , defaults: new { controller = "Blog", action = "CanEdit" }
               , constraints: new { culture = cultureConstraint }
               );

            return routes;
        }

        public static IEndpointRouteBuilder AddBlogRoutesForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            string startSegment = "blog"
            )
        {
            string firstSegment = GetSegmentpattern(startSegment);

            routes.MapControllerRoute(
                   name: ProjectConstants.FolderBlogCategoryRouteName,
                   pattern: "{sitefolder}/" + firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   , constraints: new { sitefolder = siteFolderConstraint }
                   );

            routes.MapControllerRoute(
                  ProjectConstants.FolderBlogArchiveRouteName,
                  "{sitefolder}/" + firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { sitefolder = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
                  ProjectConstants.FolderPostWithDateRouteName,
                  "{sitefolder}/" + firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { sitefolder = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderNewPostRouteName,
               pattern: "{sitefolder}/" + firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPostEditWithTemplateRouteName,
               pattern: "{sitefolder}/" + firstSegment + "editwithpattern/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithpattern" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPostEditRouteName,
               pattern: "{sitefolder}/" + firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPostDeleteRouteName,
               pattern: "{sitefolder}/" + firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
              name: ProjectConstants.FolderMostRecentPostRouteName,
              pattern: "{sitefolder}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPostHistoryRouteName,
               pattern: "{sitefolder}/" + firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderPostWithoutDateRouteName,
               pattern: "{sitefolder}/" + firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderBlogIndexRouteName,
               pattern: "{sitefolder}/" + firstSegment + ""
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.FolderBlogCanEditRouteName,
               pattern: "{sitefolder}/" + firstSegment + "blog/canedit"
               , defaults: new { controller = "Blog", action = "CanEdit" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );


            return routes;
        }

        public static IEndpointRouteBuilder AddCultureBlogRoutesForSimpleContent(
            this IEndpointRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            IRouteConstraint cultureConstraint,
            string startSegment = "blog"
            )
        {
            string firstSegment = GetSegmentpattern(startSegment);

            routes.MapControllerRoute(
                   name: ProjectConstants.CultureFolderBlogCategoryRouteName,
                   pattern: "{sitefolder}/{culture}/" + firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
                   );

            routes.MapControllerRoute(
                  ProjectConstants.CultureFolderBlogArchiveRouteName,
                  "{sitefolder}/{culture}/" + firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { sitefolder = siteFolderConstraint, culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
                  ProjectConstants.CultureFolderPostWithDateRouteName,
                  "{sitefolder}/{culture}/" + firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { sitefolder = siteFolderConstraint, culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderNewPostRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPostEditWithTemplateRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + "editwithpattern/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithpattern" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPostEditRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPostDeleteRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
              name: ProjectConstants.CultureFolderMostRecentPostRouteName,
              pattern: "{sitefolder}/{culture}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPostHistoryRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderPostWithoutDateRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderBlogIndexRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + ""
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapControllerRoute(
               name: ProjectConstants.CultureFolderBlogCanEditRouteName,
               pattern: "{sitefolder}/{culture}/" + firstSegment + "blog/canedit"
               , defaults: new { controller = "Blog", action = "CanEdit" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );


            return routes;
        }

    }
}
