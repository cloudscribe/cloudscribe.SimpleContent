using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddJsonFile("app-tenants-users.json");
            builder.AddJsonFile("app-content-project-settings.json");
            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true);
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            ConfigureAuthPolicy(services);

            services.Configure<cloudscribe.Web.SimpleAuth.Models.SimpleAuthSettings>(Configuration.GetSection("SimpleAuthSettings"));
            services.Configure<MultiTenancyOptions>(Configuration.GetSection("MultiTenancy"));

            services.AddMultitenancy<SiteSettings, CachingSiteResolver>();
            services.AddScoped<cloudscribe.Web.SimpleAuth.Models.IUserLookupProvider, SiteUserLookupProvider>();
            services.AddScoped<cloudscribe.Web.SimpleAuth.Models.IAuthSettingsResolver, SiteAuthSettingsResolver>();
            services.AddCloudscribeSimpleAuth();

            services.AddScoped<cloudscribe.SimpleContent.Models.IProjectQueries, cloudscribe.SimpleContent.Storage.NoDb.ConfigProjectQueries>();
            services.AddNoDbStorageForSimpleContent();

            services.AddCloudscribeNavigation(Configuration.GetSection("NavigationOptions"));
            services.Configure<List<ProjectSettings>>(Configuration.GetSection("ContentProjects"));
            services.AddScoped<IProjectSettingsResolver, SiteProjectSettingsResolver>();
            services.AddScoped<IProjectSecurityResolver, cloudscribe.SimpleContent.Security.SimpleAuth.ProjectSecurityResolver>();
            services.AddCloudscribeCommmon();
            services.AddSimpleContent();

            services.AddMetaWeblogForSimpleContent(Configuration.GetSection("MetaWeblogApiOptions"));

            services.AddSimpleContentRssSyndiction();


            // Add MVC services to the services container.
            services.Configure<MvcOptions>(options =>
            {
                // options.InputFormatters.Add(new Xm)
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
                    // if you download the cloudscribe.Web.Navigation Views and put them in your views folder
                    // then you don't need this line and can customize the views
                    options.AddEmbeddedViewsForNavigation();


                    options.AddEmbeddedViewsForSimpleAuth();

                    // If you download and install the views below your view folder you don't need this method and you can customize the views.
                    // You can get the views from https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/src/cloudscribe.SimpleContent.Blog.Web/Views
                    options.AddEmbeddedViewsForSimpleContent();


                    options.ViewLocationExpanders.Add(new SiteViewLocationExpander());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Web.SimpleAuth.Models.SimpleAuthSettings> authSettingsAccessor
            )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMultitenancy<SiteSettings>();

            app.UsePerTenant<SiteSettings>((ctx, builder) =>
            {
                var authCookieOptions = new CookieAuthenticationOptions();
                authCookieOptions.AuthenticationScheme = ctx.Tenant.AuthenticationScheme;
                authCookieOptions.LoginPath = new PathString("/login");
                authCookieOptions.AccessDeniedPath = new PathString("/");
                authCookieOptions.AutomaticAuthenticate = true;
                authCookieOptions.AutomaticChallenge = true;
                authCookieOptions.CookieName = ctx.Tenant.AuthenticationScheme;
                builder.UseCookieAuthentication(authCookieOptions);

            });

            app.UseMvc(routes =>
            {
                routes.AddStandardRoutesForSimpleContent();

                routes.MapRoute(
                    name: "def",
                    template: "{controller}/{action}"
                    );

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureAuthPolicy(IServiceCollection services)
        {
            //https://docs.asp.net/en/latest/security/authorization/policies.html

            services.AddAuthorization(options =>
            {
                // this policy currently means any user with a blogId claim can edit
                // would require somthing more for multi tenant blogs
                options.AddPolicy(
                    "BlogEditPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("blogId");
                    }
                 );
				 
				 options.AddPolicy(
                    "PageEditPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators");
                    });

                // add other policies here 

            });

        }
    }
}
