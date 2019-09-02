using cloudscribe.Web.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {
        public static IEndpointRouteBuilder UseCustomRoutes(
            this IEndpointRouteBuilder 
            routes, bool useFolders,
            IConfiguration config)
        {
            var useCustomRoutes = config.GetValue<bool>("DevOptions:UseCustomRoutes");

            if (useFolders)
            {
                routes.AddCultureBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true));
                routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
            }

            routes.AddCultureBlogRoutesForSimpleContent(new CultureSegmentRouteConstraint());
            routes.AddBlogRoutesForSimpleContent();

            routes.AddSimpleContentStaticResourceRoutes();

            //TODO filemanager culture routes?

            routes.AddCloudscribeFileManagerRoutes();



            if (useFolders)
            {
                routes.MapControllerRoute(
                   name: "foldererrorhandler",
                   pattern: "{sitefolder}/oops/error/{statusCode?}",
                   defaults: new { controller = "Oops", action = "Error" },
                   constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                );

                routes.MapControllerRoute(
                      name: "apifoldersitemap-localized",
                      pattern: "{sitefolder}/{culture}/api/sitemap"
                      , defaults: new { controller = "FolderSiteMap", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapControllerRoute(
                       name: "apifoldersitemap",
                       pattern: "{sitefolder}/api/sitemap"
                       , defaults: new { controller = "FolderSiteMap", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapControllerRoute(
                      name: "folderserviceworker",
                      pattern: "{sitefolder}/serviceworker"
                      , defaults: new { controller = "Pwa", action = "ServiceWorker" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                      );

                routes.MapControllerRoute(
                      name: "apifoldermetaweblog-localized",
                      pattern: "{sitefolder}/{culture}/api/metaweblog"
                      , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapControllerRoute(
                       name: "apifoldermetaweblog",
                       pattern: "{sitefolder}/api/metaweblog"
                       , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapControllerRoute(
                       name: "apifolderrss-localized",
                       pattern: "{sitefolder}/{culture}/api/rss"
                       , defaults: new { controller = "FolderRss", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                       );

                routes.MapControllerRoute(
                       name: "apifolderrss",
                       pattern: "{sitefolder}/api/rss"
                       , defaults: new { controller = "FolderRss", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                if (useCustomRoutes)
                {
                    routes.MapControllerRoute(
                    name: "folderdefault",
                        pattern: "{sitefolder}/{controller}/{action}/{id?}"
                        , defaults: new { controller = "Home", action = "Index" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                    routes.AddCultureCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true), "docs");

                    routes.AddCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "docs");
                }
                else
                {
                    routes.MapControllerRoute(
                        name: "foldersitemap-localized",
                        pattern: "{sitefolder}/{culture}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                        );

                    routes.MapControllerRoute(
                        name: "foldersitemap",
                        pattern: "{sitefolder}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                    routes.MapControllerRoute(
                           name: "folderdefault-localized",
                           pattern: "{sitefolder}/{culture}/{controller}/{action}/{id?}",
                           defaults: new { controller = "Home" },
                           constraints: new { sitefolder = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) });

                    routes.MapControllerRoute(
                        name: "folderdefault",
                        pattern: "{sitefolder}/{controller}/{action}/{id?}"
                        , defaults: new { controller = "Home" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                    routes.AddCulturePageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true));
                    routes.AddDefaultPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());

                    //routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(),"");
                }


            }

            if (useCustomRoutes)
            {
                routes.AddCultureCustomPageRouteForSimpleContent(new CultureSegmentRouteConstraint(),"docs");
                routes.AddCustomPageRouteForSimpleContent("docs");
            }


            routes.MapControllerRoute(
               name: "errorhandler",
               pattern: "oops/error/{statusCode?}",
               defaults: new { controller = "Oops", action = "Error" }
               );

            routes.MapControllerRoute(
                       name: "api-sitemap-culture",
                       pattern: "{culture}/api/sitemap"
                       , defaults: new { controller = "CultureSiteMap", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                       name: "api-rss-culture",
                       pattern: "{culture}/api/rss"
                       , defaults: new { controller = "CultureRss", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                       name: "api-metaweblog-culture",
                       pattern: "{culture}/api/metaweblog"
                       , defaults: new { controller = "CultureMetaweblog", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            //routes.AddPwaDefaultRoutes(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());

            routes.MapControllerRoute(
                name: "sitemap-localized",
                pattern: "sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                , constraints: new { culture = new CultureSegmentRouteConstraint() }
                );

            routes.MapControllerRoute(
                name: "sitemap",
                pattern: "sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                );

            var useHomeIndexAsDefault = config.GetValue<bool>("DevOptions:UseHomeIndexAsDefault");
            if (useHomeIndexAsDefault)
            {
                routes.MapControllerRoute(
                    name: "default-localized",
                    pattern: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}"
                    , defaults: new { controller = "Home", action = "Index" }
                    );
            }
            else
            {
                routes.MapControllerRoute(
                    name: "default-localized",
                    pattern: "{culture}/{controller}/{action}/{id?}",
                    defaults: null,
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}"
                    //, defaults: new { controller = "Home", action = "Index" }
                    );
            }


            if (!useCustomRoutes)
            {
                routes.AddCulturePageRouteForSimpleContent(new CultureSegmentRouteConstraint());
                routes.AddDefaultPageRouteForSimpleContent();
            }
            //routes.AddBlogRoutesForSimpleContent("");


            return routes;
        }


        public static IServiceCollection SetupMvc(
            this IServiceCollection services,
            IConfiguration config,
            bool sslIsAvailable
            )
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            services.Configure<MvcOptions>(options =>
            {
                if (sslIsAvailable)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }


                options.CacheProfiles.Add("SiteMapCacheProfile",
                     new CacheProfile
                     {
                         Duration = 30
                     });

                options.CacheProfiles.Add("RssCacheProfile",
                     new CacheProfile
                     {
                         Duration = 100
                     });
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            var boostrapVersion = config.GetValue<int>("DevOptions:BootstrapVersion");

            services.AddMvc()
                .AddRazorOptions(options =>
                { 
                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());

                })
                
                ;

            return services;
        }
    }
}
