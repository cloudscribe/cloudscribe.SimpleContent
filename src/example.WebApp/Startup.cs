using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using cloudscribe.SimpleContent.Models;

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

            builder.AddJsonFile("app-tenants-users.json");

            builder.AddJsonFile("app-content-project-settings.json");

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddGlimpse();

            ConfigureAuthPolicy(services);

            // Hosting doesn't add IHttpContextAccessor by default
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // single tenant
            services.Configure<cloudscribe.Web.SimpleAuth.Models.SimpleAuthSettings>(Configuration.GetSection("SimpleAuthSettings"));
            //services.AddScoped<IUserLookupProvider, DefaultUserLookupProvider>(); 
            //services.AddScoped<IAuthSettingsResolver, DefaultAuthSettingsResolver>();
            //services.Configure<List<SimpleAuthUser>>(Configuration.GetSection("Users"));

            // multi tenant
            services.Configure<MultiTenancyOptions>(Configuration.GetSection("MultiTenancy"));
            services.AddMultitenancy<SiteSettings, CachingSiteResolver>();
            services.AddScoped<cloudscribe.Web.SimpleAuth.Models.IUserLookupProvider, SiteUserLookupProvider>();
            services.AddScoped<cloudscribe.Web.SimpleAuth.Models.IAuthSettingsResolver, SiteAuthSettingsResolver>();
            services.AddCloudscribeSimpleAuth();

            services.AddNoDbProjectStorage();
            
            services.Configure<List<ProjectSettings>>(Configuration.GetSection("ContentProjects"));
            services.AddScoped<IProjectService, cloudscribe.SimpleContent.Services.ProjectService>();
;
            services.AddScoped<NoDb.IStringSerializer<Post>, cloudscribe.SimpleContent.Storage.NoDb.PostXmlSerializer>();
            services.AddScoped<NoDb.IStoragePathResolver<Post>, cloudscribe.SimpleContent.Storage.NoDb.PostStoragePathResolver>();
            services.AddNoDbPostStorage();

            services.AddNoDbPageStorage();

            services.AddScoped<IBlogService, cloudscribe.SimpleContent.Services.BlogService>();
            services.AddScoped<IPageService, cloudscribe.SimpleContent.Services.PageService>();

            services.AddScoped<IMediaProcessor, cloudscribe.SimpleContent.Services.FileSystemMediaProcessor>();

            services.AddMetaWeblogForSimpleContent(Configuration.GetSection("MetaWeblogApiOptions"));
            

            services.AddScoped<IProjectSettingsResolver, SiteProjectSettingsResolver>();
            services.AddScoped<cloudscribe.SimpleContent.Services.HtmlProcessor, cloudscribe.SimpleContent.Services.HtmlProcessor>();
            //services.AddScoped<cloudscribe.SimpleContent.Models.IProjectEmailService, ProjectEmailService>();
            services.AddScoped<IProjectEmailService, cloudscribe.SimpleContent.Services.ProjectEmailService>();
            services.AddScoped<cloudscribe.Web.Common.Razor.ViewRenderer, cloudscribe.Web.Common.Razor.ViewRenderer>();
            
            services.AddScoped<IProjectSecurityResolver, cloudscribe.SimpleContent.Security.SimpleAuth.ProjectSecurityResolver>();
           
            services.AddScoped<cloudscribe.Syndication.Models.Rss.IChannelProvider, cloudscribe.SimpleContent.Syndication.RssChannelProvider>();

            services.AddCloudscribeNavigation(Configuration.GetSection("NavigationOptions"));
            services.AddScoped<cloudscribe.Web.Navigation.INavigationTreeBuilder, cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder>();
            services.AddScoped<cloudscribe.Web.SiteMap.ISiteMapNodeService, cloudscribe.Web.SiteMap.NavigationTreeSiteMapNodeService>();
            services.AddScoped<cloudscribe.Web.SiteMap.ISiteMapNodeService, cloudscribe.SimpleContent.Services.BlogSiteMapNodeService>();

            //services.Configure<NavigationOptions>(options =>
            //{
            //    //options.RootTreeBuilderName = "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder";
            //});

            

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

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new SiteViewLocationExpander());
            });

            services.AddMvc()
                .AddRazorOptions(options =>
            {
                // if you download the cloudscribe.Web.Navigation Views and put them in your views folder
                // then you don't need this line and can customize the views
                options.AddEmbeddedViewsForNavigation();

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
            //app.UseGlimpse();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
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
                   name: ProjectConstants.NewPostRouteName,
                   template: "blog/new"
                   , defaults: new { controller = "Blog", action = "New" }
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

                routes.MapRoute(
                   name: ProjectConstants.PageIndexRouteName,
                   template: "{slug=none}"
                   , defaults: new { controller = "Page", action = "Index" }
                   );

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

                // add other policies here 

            });

        }
    }
}
