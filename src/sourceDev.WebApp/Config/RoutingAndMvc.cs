using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {
        public static IRouteBuilder UseCustomRoutes(
            this IRouteBuilder 
            routes, bool useFolders,
            IConfiguration config)
        {
            var useCustomRoutes = config.GetValue<bool>("DevOptions:UseCustomRoutes");

            if (useFolders)
            {
                routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
            }

            routes.AddBlogRoutesForSimpleContent();
            routes.AddSimpleContentStaticResourceRoutes();
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
                       name: "apifoldersitemap",
                       template: "{sitefolder}/api/sitemap"
                       , defaults: new { controller = "FolderSiteMap", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapRoute(
                       name: "apifoldermetaweblog",
                       template: "{sitefolder}/api/metaweblog"
                       , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
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

                    routes.AddCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "docs");
                }
                else
                {
                    
                    routes.MapRoute(
                        name: "foldersitemap",
                        template: "{sitefolder}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                    routes.MapRoute(
                    name: "folderdefault",
                    template: "{sitefolder}/{controller}/{action}/{id?}"
                    , defaults: new { controller = "Home" }
                    , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );

                    routes.AddDefaultPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());

                    //routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(),"");
                }


            }

            if (useCustomRoutes)
            {
                routes.AddCustomPageRouteForSimpleContent("docs");
            }


            routes.MapRoute(
               name: "errorhandler",
               template: "oops/error/{statusCode?}",
               defaults: new { controller = "Oops", action = "Error" }
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
                name: "default",
                template: "{controller}/{action}"
                , defaults: new { controller = "Home", action = "Index" }
                );
            }
            else
            {
                routes.MapRoute(
                name: "default",
                template: "{controller}/{action}"
                //, defaults: new { controller = "Home", action = "Index" }
                );
            }


            if (!useCustomRoutes)
            {
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
                    //options.AddCloudscribeViewLocationFormats();


                    //options.AddCloudscribeCommonEmbeddedViews();
                    
                    

                    switch(boostrapVersion)
                    {
                        case 4:

                            //options.AddCloudscribeFileManagerBootstrap4Views();
                            //options.AddCloudscribeNavigationBootstrap4Views();
                            //options.AddCloudscribeCoreBootstrap4Views();
                            //options.AddCloudscribeCoreSimpleContentIntegrationBootstrap4Views();
                            //options.AddCloudscribeSimpleContentBootstrap4Views();
                           // options.AddCloudscribeLoggingBootstrap4Views();

                            break;

                        case 3:
                        default:

                            //options.AddCloudscribeFileManagerBootstrap3Views();
                            //options.AddCloudscribeNavigationBootstrap3Views();
                            //options.AddCloudscribeCoreBootstrap3Views();
                            //options.AddCloudscribeCoreSimpleContentIntegrationBootstrap3Views();
                            //options.AddCloudscribeSimpleContentBootstrap3Views();

                            //options.AddCloudscribeLoggingBootstrap3Views();

                            break;
                    }

                    

                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());

                });

            return services;
        }
    }
}
