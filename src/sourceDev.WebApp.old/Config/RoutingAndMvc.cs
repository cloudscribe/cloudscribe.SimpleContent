﻿using cloudscribe.Web.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {
        /// <summary>
        /// this is the 2.x way which still works
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="useFolders"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IRouteBuilder UseCustomRoutes(
            this IRouteBuilder
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
                routes.MapRoute(
                   name: "foldererrorhandler",
                   template: "{sitefolder}/oops/error/{statusCode?}",
                   defaults: new { controller = "Oops", action = "Error" },
                   constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                );

                routes.MapRoute(
                      name: "apifoldersitemap-localized",
                      template: "{sitefolder}/{culture}/api/sitemap"
                      , defaults: new { controller = "FolderSiteMap", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapRoute(
                       name: "apifoldersitemap",
                       template: "{sitefolder}/api/sitemap"
                       , defaults: new { controller = "FolderSiteMap", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapRoute(
                      name: "folderserviceworker",
                      template: "{sitefolder}/serviceworker"
                      , defaults: new { controller = "Pwa", action = "ServiceWorker" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                      );

                routes.MapRoute(
                      name: "apifoldermetaweblog-localized",
                      template: "{sitefolder}/{culture}/api/metaweblog"
                      , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapRoute(
                       name: "apifoldermetaweblog",
                       template: "{sitefolder}/api/metaweblog"
                       , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapRoute(
                       name: "apifolderrss-localized",
                       template: "{sitefolder}/{culture}/api/rss"
                       , defaults: new { controller = "FolderRss", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                       );

                routes.MapRoute(
                       name: "apifolderrss",
                       template: "{sitefolder}/api/rss"
                       , defaults: new { controller = "FolderRss", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                if (useCustomRoutes)
                {
                    routes.MapRoute(
                    name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}"
                        , defaults: new { controller = "Home", action = "Index" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                    routes.AddCultureCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true), "docs");

                    routes.AddCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "docs");
                }
                else
                {
                    routes.MapRoute(
                        name: "foldersitemap-localized",
                        template: "{sitefolder}/{culture}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                        );

                    routes.MapRoute(
                        name: "foldersitemap",
                        template: "{sitefolder}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                    routes.MapRoute(
                           name: "folderdefault-localized",
                           template: "{sitefolder}/{culture}/{controller}/{action}/{id?}",
                           defaults: new { controller = "Home" },
                           constraints: new { sitefolder = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) });

                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}"
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
                routes.AddCultureCustomPageRouteForSimpleContent(new CultureSegmentRouteConstraint(), "docs");
                routes.AddCustomPageRouteForSimpleContent("docs");
            }


            routes.MapRoute(
               name: "errorhandler",
               template: "oops/error/{statusCode?}",
               defaults: new { controller = "Oops", action = "Error" }
               );

            routes.MapRoute(
                       name: "api-sitemap-culture",
                       template: "{culture}/api/sitemap"
                       , defaults: new { controller = "CultureSiteMap", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                       name: "api-rss-culture",
                       template: "{culture}/api/rss"
                       , defaults: new { controller = "CultureRss", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                       name: "api-metaweblog-culture",
                       template: "{culture}/api/metaweblog"
                       , defaults: new { controller = "CultureMetaweblog", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            //routes.AddPwaDefaultRoutes(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());

            routes.MapRoute(
                name: "sitemap-localized",
                template: "sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                , constraints: new { culture = new CultureSegmentRouteConstraint() }
                );

            routes.MapRoute(
                name: "sitemap",
                template: "sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                );

            var useHomeIndexAsDefault = config.GetValue<bool>("DevOptions:UseHomeIndexAsDefault");
            if (useHomeIndexAsDefault)
            {
                routes.MapRoute(
                    name: "default-localized",
                    template: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}"
                    , defaults: new { controller = "Home", action = "Index" }
                    );
            }
            else
            {
                routes.MapRoute(
                    name: "default-localized",
                    template: "{culture}/{controller}/{action}/{id?}",
                    defaults: null,
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}"
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

        /// <summary>
        /// this is the new 3.0 endpoint routing but it has bugs that break folder tenants and culture segment route constraints
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="useFolders"></param>
        /// <param name="config"></param>
        /// <returns></returns>
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
                options.EnableEndpointRouting = false;

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
                options.LowercaseQueryStrings = false;
            });

            var boostrapVersion = config.GetValue<int>("DevOptions:BootstrapVersion");

            services.AddMvc()
                .AddRazorOptions(options =>
                { 
                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());

                })
               // .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                ;

            return services;
        }
    }
}
