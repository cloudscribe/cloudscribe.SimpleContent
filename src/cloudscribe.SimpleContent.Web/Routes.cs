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

        public static IRouteBuilder AddCulturePageRouteForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint cultureConstraint
            )
        {
            routes.MapRoute(
               name: ProjectConstants.CultureNewPageRouteName,
               template: "{culture}/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageEditRouteName,
               template: "{culture}/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageEditWithTemplateRouteName,
               template: "{culture}/editwithtemplate/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithTemplate" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageDevelopRouteName,
               template: "{culture}/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageTreeRouteName,
               template: "{culture}/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageDeleteRouteName,
               template: "{culture}/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageHistoryRouteName,
               template: "{culture}/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageIndexRouteName,
               template: "{culture}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { culture = cultureConstraint }
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
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageEditRouteName,
              template: "{sitefolder}/editpage/{slug?}"
              , defaults: new { controller = "Page", action = "Edit" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageEditWithTemplateRouteName,
              template: "{sitefolder}/editwithtemplate/{slug?}"
              , defaults: new { controller = "Page", action = "EditWithTemplate" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageDevelopRouteName,
              template: "{sitefolder}/development/{slug}"
              , defaults: new { controller = "Page", action = "Development" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageTreeRouteName,
              template: "{sitefolder}/tree"
              , defaults: new { controller = "Page", action = "Tree" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.FolderPageDeleteRouteName,
              template: "{sitefolder}/deletepage/{id}"
              , defaults: new { controller = "Page", action = "Delete" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.FolderPageHistoryRouteName,
               template: "{sitefolder}/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageIndexRouteName,
               template: "{sitefolder}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );



            return routes;
        }

        public static IRouteBuilder AddCulturePageRouteForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            IRouteConstraint cultureConstraint
            )
        {
            routes.MapRoute(
              name: ProjectConstants.CultureFolderNewPageRouteName,
              template: "{sitefolder}/{culture}/newpage/{parentSlug?}"
              , defaults: new { controller = "Page", action = "NewPage" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.CultureFolderPageEditRouteName,
              template: "{sitefolder}/{culture}/editpage/{slug?}"
              , defaults: new { controller = "Page", action = "Edit" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.CultureFolderPageEditWithTemplateRouteName,
              template: "{sitefolder}/{culture}/editwithtemplate/{slug?}"
              , defaults: new { controller = "Page", action = "EditWithTemplate" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.CultureFolderPageDevelopRouteName,
              template: "{sitefolder}/{culture}/development/{slug}"
              , defaults: new { controller = "Page", action = "Development" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.CultureFolderPageTreeRouteName,
              template: "{sitefolder}/{culture}/tree"
              , defaults: new { controller = "Page", action = "Tree" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapRoute(
              name: ProjectConstants.CultureFolderPageDeleteRouteName,
              template: "{sitefolder}/{culture}/deletepage/{id}"
              , defaults: new { controller = "Page", action = "Delete" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageHistoryRouteName,
               template: "{sitefolder}/{culture}/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageIndexRouteName,
               template: "{sitefolder}/{culture}/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );



            return routes;
        }

        public static IRouteBuilder AddCustomPageRouteForSimpleContent(
            this IRouteBuilder routes,
            string prefix)
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

        public static IRouteBuilder AddCultureCustomPageRouteForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint cultureConstraint,
            string prefix)
        {
            routes.MapRoute(
               name: ProjectConstants.CultureNewPageRouteName,
               template: "{culture}/" + prefix + " / newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageEditRouteName,
               template: "{culture}/" + prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { culture = cultureConstraint }
               );


            routes.MapRoute(
               name: ProjectConstants.CulturePageEditWithTemplateRouteName,
               template: "{culture}/" + prefix + "/editwithtemplate/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithTemplate" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageDevelopRouteName,
               template: "{culture}/" + prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageTreeRouteName,
               template: "{culture}/" + prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageDeleteRouteName,
               template: "{culture}/" + prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageHistoryRouteName,
               template: "{culture}/" + prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePageIndexRouteName,
               template: "{culture}/" + prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { culture = cultureConstraint }
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
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageEditRouteName,
               template: "{sitefolder}/" + prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageEditWithTemplateRouteName,
               template: "{sitefolder}/" + prefix + "/editwithtemplate/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithTemplate" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageDevelopRouteName,
               template: "{sitefolder}/" + prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageTreeRouteName,
               template: "{sitefolder}/" + prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageDeleteRouteName,
               template: "{sitefolder}/" + prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageHistoryRouteName,
               template: "{sitefolder}/" + prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPageIndexRouteName,
               template: "{sitefolder}/" + prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );



            return routes;
        }

        public static IRouteBuilder AddCultureCustomPageRouteForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            IRouteConstraint cultureConstraint,
            string prefix

            )
        {
            routes.MapRoute(
               name: ProjectConstants.CultureFolderNewPageRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/newpage/{parentSlug?}"
               , defaults: new { controller = "Page", action = "NewPage" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageEditRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/editpage/{slug?}"
               , defaults: new { controller = "Page", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageEditWithTemplateRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/editwithtemplate/{slug?}"
               , defaults: new { controller = "Page", action = "EditWithTemplate" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageDevelopRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/development/{slug}"
               , defaults: new { controller = "Page", action = "Development" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageTreeRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/tree"
               , defaults: new { controller = "Page", action = "Tree" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageDeleteRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/deletepage/{id}"
               , defaults: new { controller = "Page", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageHistoryRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/history/{slug?}"
               , defaults: new { controller = "Page", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPageIndexRouteName,
               template: "{sitefolder}/{culture}/" + prefix + "/{slug=none}"
               , defaults: new { controller = "Page", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
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

        public static IRouteBuilder AddBlogRoutesForSimpleContent(
            this IRouteBuilder routes,
            string startSegment = "blog")
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

        public static IRouteBuilder AddCultureBlogRoutesForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint cultureConstraint,
            string startSegment = "blog")
        {
            string firstSegment = GetSegmentTemplate(startSegment);

            routes.MapRoute(
                   name: ProjectConstants.CultureBlogCategoryRouteName,
                   template: "{culture}/" + firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   , constraints: new { culture = cultureConstraint }
                   );


            routes.MapRoute(
                  ProjectConstants.CultureBlogArchiveRouteName,
                  "{culture}/" + firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                  ProjectConstants.CulturePostWithDateRouteName,
                  "{culture}/" + firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
               name: ProjectConstants.CultureNewPostRouteName,
               template: "{culture}/" + firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePostEditWithTemplateRouteName,
               template: "{culture}/" + firstSegment + "editwithtemplate/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithTemplate" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePostEditRouteName,
               template: "{culture}/" + firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePostDeleteRouteName,
               template: "{culture}/" + firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
              name: ProjectConstants.CultureMostRecentPostRouteName,
              template: "{culture}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              );

            routes.MapRoute(
               name: ProjectConstants.CulturePostHistoryRouteName,
               template: "{culture}/" + firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CulturePostWithoutDateRouteName,
               template: "{culture}/" + firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureBlogIndexRouteName,
               template: "{culture}/" + firstSegment
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { culture = cultureConstraint }
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
                   , constraints: new { sitefolder = siteFolderConstraint }
                   );

            routes.MapRoute(
                  ProjectConstants.FolderBlogArchiveRouteName,
                  "{sitefolder}/" + firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { sitefolder = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                  ProjectConstants.FolderPostWithDateRouteName,
                  "{sitefolder}/" + firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { sitefolder = siteFolderConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
               name: ProjectConstants.FolderNewPostRouteName,
               template: "{sitefolder}/" + firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostEditWithTemplateRouteName,
               template: "{sitefolder}/" + firstSegment + "editwithtemplate/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithTemplate" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostEditRouteName,
               template: "{sitefolder}/" + firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostDeleteRouteName,
               template: "{sitefolder}/" + firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
              name: ProjectConstants.FolderMostRecentPostRouteName,
              template: "{sitefolder}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              , constraints: new { sitefolder = siteFolderConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.FolderPostHistoryRouteName,
               template: "{sitefolder}/" + firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderPostWithoutDateRouteName,
               template: "{sitefolder}/" + firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.FolderBlogIndexRouteName,
               template: "{sitefolder}/" + firstSegment + ""
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint }
               );


            return routes;
        }

        public static IRouteBuilder AddCultureBlogRoutesForSimpleContent(
            this IRouteBuilder routes,
            IRouteConstraint siteFolderConstraint,
            IRouteConstraint cultureConstraint,
            string startSegment = "blog"
            )
        {
            string firstSegment = GetSegmentTemplate(startSegment);

            routes.MapRoute(
                   name: ProjectConstants.CultureFolderBlogCategoryRouteName,
                   template: "{sitefolder}/{culture}/" + firstSegment + "category/{category=''}/{pagenumber=1}"
                   , defaults: new { controller = "Blog", action = "Category" }
                   , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
                   );

            routes.MapRoute(
                  ProjectConstants.CultureFolderBlogArchiveRouteName,
                  "{sitefolder}/{culture}/" + firstSegment + "{year}/{month}/{day}",
                  new { controller = "Blog", action = "Archive", month = "00", day = "00" },
                  new { sitefolder = siteFolderConstraint, culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
                  ProjectConstants.CultureFolderPostWithDateRouteName,
                  "{sitefolder}/{culture}/" + firstSegment + "{year}/{month}/{day}/{slug}",
                  new { controller = "Blog", action = "PostWithDate" },
                  new { sitefolder = siteFolderConstraint, culture = cultureConstraint, year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
                );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderNewPostRouteName,
               template: "{sitefolder}/{culture}/" + firstSegment + "newpost"
               , defaults: new { controller = "Blog", action = "NewPost" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPostEditWithTemplateRouteName,
               template: "{sitefolder}/{culture}/" + firstSegment + "editwithtemplate/{slug}"
               , defaults: new { controller = "Blog", action = "EditWithTemplate" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPostEditRouteName,
               template: "{sitefolder}/{culture}/" + firstSegment + "edit/{slug?}"
               , defaults: new { controller = "Blog", action = "Edit" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPostDeleteRouteName,
               template: "{sitefolder}/{culture}/" + firstSegment + "delete/{id?}"
               , defaults: new { controller = "Blog", action = "Delete" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
              name: ProjectConstants.CultureFolderMostRecentPostRouteName,
              template: "{sitefolder}/{culture}/" + firstSegment + "mostrecent"
              , defaults: new { controller = "Blog", action = "MostRecent" }
              , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
              );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPostHistoryRouteName,
               template: "{sitefolder}/{culture}/" + firstSegment + "history/{slug?}"
               , defaults: new { controller = "Blog", action = "History" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderPostWithoutDateRouteName,
               template: "{sitefolder}/{culture}/" + firstSegment + "{slug}"
               , defaults: new { controller = "Blog", action = "PostNoDate" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );

            routes.MapRoute(
               name: ProjectConstants.CultureFolderBlogIndexRouteName,
               template: "{sitefolder}/{culture}/" + firstSegment + ""
               , defaults: new { controller = "Blog", action = "Index" }
               , constraints: new { sitefolder = siteFolderConstraint, culture = cultureConstraint }
               );


            return routes;
        }

    }
}
