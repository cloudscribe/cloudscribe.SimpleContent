using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.DataProtection;
using cloudscribe.Core.SimpleContent.Integration;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using cloudscribe.SimpleContent.Web.Controllers;

namespace example.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //builder.AddJsonFile("app-tenants-users.json");
            //builder.AddJsonFile("font-awesome-icon-map.json");


            //builder.AddJsonFile("app-content-project-settings.json");

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true, reloadOnChange: true);

            

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
           
            environment = env;
           
        }

        public IHostingEnvironment environment { get; set; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddGlimpse();

            string pathToCryptoKeys = Path.Combine(environment.ContentRootPath, "dp_keys");
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(pathToCryptoKeys));
            
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            services.AddMemoryCache();
            // we currently only use session for alerts, so we can fire an alert on the next request
            // if session is disabled this feature fails quietly with no errors
            services.AddSession();

            ConfigureAuthPolicy(services);

            services.AddOptions();

            ConfigureDataStorage(services);
            
            services.AddCloudscribeLogging();
            
            services.AddScoped<cloudscribe.Web.Navigation.Caching.ITreeCache, cloudscribe.Web.Navigation.Caching.NotCachedTreeCache>();
            
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();
            services.AddCloudscribeCore(Configuration);
            
            services.Configure<List<ProjectSettings>>(Configuration.GetSection("ContentProjects"));
            
            services.AddCloudscribeCoreIntegrationForSimpleContent();
            services.AddSimpleContent(Configuration);

            services.AddMetaWeblogForSimpleContent(Configuration.GetSection("MetaWeblogApiOptions"));

            services.AddSimpleContentRssSyndiction();

            services.AddCloudscribeFileManagerIntegration(Configuration);

            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("fr-FR"),
                    new CultureInfo("fr"),
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //  // My custom request culture logic
                //  return new ProviderCultureResult("en");
                //}));
            });

            services.Configure<MvcOptions>(options =>
            {
                //  if(environment.IsProduction())
                //  {
                options.Filters.Add(new RequireHttpsAttribute());
                //   }


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

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    options.AddCloudscribeViewLocationFormats();

                    options.AddEmbeddedViewsForNavigation();
                    options.AddEmbeddedBootstrap3ViewsForCloudscribeCore();
                    options.AddEmbeddedViewsForCloudscribeLogging();
                    options.AddBootstrap3EmbeddedViewsForSimpleContent();
                    options.AddEmbeddedViewsForCloudscribeCoreSimpleContentIntegration();

                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                })
                //.AddApplicationPart(typeof(FsMediaController).Assembly)
                    ;


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IServiceProvider serviceProvider,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor,
            cloudscribe.Logging.Web.ILogRepository logRepo
            )
        {
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            ConfigureLogging(loggerFactory, serviceProvider, logRepo);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseForwardedHeaders();
            app.UseStaticFiles();

            // custom 404 and error page - this preserves the status code (ie 404)
            //app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            app.UseMultitenancy<cloudscribe.Core.Models.SiteContext>();

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UsePerTenant<cloudscribe.Core.Models.SiteContext>((ctx, builder) =>
            {
                builder.UseCloudscribeCoreDefaultAuthentication(
                    loggerFactory,
                    multiTenantOptions,
                    ctx.Tenant);
                
            });

            UseMvc(app, multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName);

            var storage = Configuration["DevOptions:DbPlatform"];
            switch (storage)
            {
                case "NoDb":
                    CoreNoDbStartup.InitializeDataAsync(app.ApplicationServices).Wait();
                    break;

                case "ef":
                default:
                    // this creates ensures the database is created and initial data
                    CoreEFStartup.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                    // this one is only needed if using cloudscribe Logging with EF as the logging storage
                    LoggingEFStartup.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                    SimpleContentEFStartup.InitializeDatabaseAsync(app.ApplicationServices).Wait();

                    break;
            }

            

        }



        private void UseMvc(IApplicationBuilder app, bool useFolders)
        {
            var useCustomRoutes = Configuration.GetValue<bool>("DevOptions:UseCustomRoutes");
            app.UseMvc(routes =>
            {
                if (useFolders)
                {
                    routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
                }

                routes.AddBlogRoutesForSimpleContent();
                
                if (useFolders)
                {
                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" },
                        constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );
                    if(useCustomRoutes)
                    {
                        routes.AddCustomPageRouteForSimpleContent("docs", new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
                    }
                    else
                    {
                        routes.AddDefaultPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
                    }
                    
                    
                }

                if(useCustomRoutes)
                {
                    routes.AddCustomPageRouteForSimpleContent("docs");
                }
                

                routes.MapRoute(
                    name: "errorhandler",
                    template: "{controller}/{action}/{statusCode}"
                    );

                var useHomeIndexAsDefault = Configuration.GetValue<bool>("DevOptions:UseHomeIndexAsDefault");
                if(useHomeIndexAsDefault)
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


            })
            
            ;
        }

        private void ConfigureDataStorage(IServiceCollection services)
        {
            services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();

            var storage = Configuration["DevOptions:DbPlatform"];

            switch (storage)
            {
                case "NoDb":
                    services.AddCloudscribeCoreNoDbStorage();
                    // only needed if using cloudscribe logging with NoDb storage
                    services.AddCloudscribeLoggingNoDbStorage(Configuration);
                    services.AddNoDbStorageForSimpleContent();

                    break;

                case "ef":
                default:
                    var connectionString = Configuration.GetConnectionString("EntityFrameworkConnectionString");
                    services.AddCloudscribeCoreEFStorageMSSQL(connectionString);

                    // only needed if using cloudscribe logging with EF storage
                    services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);

                    services.AddCloudscribeSimpleContentEFStorageMSSQL(connectionString);



                    break;
            }
        }

        private void ConfigureAuthPolicy(IServiceCollection services)
        {
            //https://docs.asp.net/en/latest/security/authorization/policies.html

            services.AddAuthorization(options =>
            {
                options.AddCloudscribeCoreDefaultPolicies();

                options.AddCloudscribeLoggingDefaultPolicy();

                options.AddCloudscribeCoreSimpleContentIntegrationDefaultPolicies();

                options.AddPolicy(
                    "FileManagerPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators");
                    });

                // this is what the above extension adds
                //options.AddPolicy(
                //    "BlogEditPolicy",
                //    authBuilder =>
                //    {
                //        //authBuilder.RequireClaim("blogId");
                //        authBuilder.RequireRole("Administrators");
                //    }
                // );

                

                //options.AddPolicy(
                //    "PageEditPolicy",
                //    authBuilder =>
                //    {
                //        authBuilder.RequireRole("Administrators");
                //    });

                // add other policies here 

            });

        }

        private void ConfigureLogging(
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            , cloudscribe.Logging.Web.ILogRepository logRepo
            )
        {
            // a customizable filter for logging
            LogLevel minimumLevel;
            if (environment.IsProduction())
            {
                minimumLevel = LogLevel.Warning;
            }
            else
            {
                minimumLevel = LogLevel.Information;
            }


            // add exclusions to remove noise in the logs
            var excludedLoggers = new List<string>
            {
                "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
                "Microsoft.AspNetCore.Hosting.Internal.WebHost",
            };

            Func<string, LogLevel, bool> logFilter = (string loggerName, LogLevel logLevel) =>
            {
                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (excludedLoggers.Contains(loggerName))
                {
                    return false;
                }

                return true;
            };

            loggerFactory.AddDbLogger(serviceProvider, logFilter, logRepo);
        }

        
    }
}
