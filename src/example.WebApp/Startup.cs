
using cloudscribe.Web.SimpleAuth.Services;
using cloudscribe.Web.SimpleAuth.Models;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using cloudscribe.Web.Pagination;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.MetaWeblog;
using cloudscribe.SimpleContent.Storage.Xml;
using cloudscribe.SimpleContent.Storage.Json;
using cloudscribe.SimpleContent.Services;
using cloudscribe.MetaWeblog;
using cloudscribe.MetaWeblog.Models;
using Glimpse;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Formatters.Xml;
using Microsoft.Data.Entity;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;




namespace example.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddJsonFile("app-tenants-users.json");

            builder.AddJsonFile("app-content-project-settings.json");

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true);

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGlimpse();

            ConfigureAuthPolicy(services);

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            // single tenant
            services.Configure<SimpleAuthSettings>(Configuration.GetSection("SimpleAuthSettings"));
            //services.AddScoped<IUserLookupProvider, DefaultUserLookupProvider>(); 
            //services.AddScoped<IAuthSettingsResolver, DefaultAuthSettingsResolver>();
            //services.Configure<List<SimpleAuthUser>>(Configuration.GetSection("Users"));

            // multi tenant
            services.Configure<MultiTenancyOptions>(Configuration.GetSection("MultiTenancy"));
            services.AddMultitenancy<SiteSettings, CachingSiteResolver>();
            services.AddScoped<IUserLookupProvider, SiteUserLookupProvider>();
            services.AddScoped<IAuthSettingsResolver, SiteAuthSettingsResolver>();
            
            // common
            services.AddScoped<IPasswordHasher<SimpleAuthUser>, PasswordHasher<SimpleAuthUser>>();
            services.AddScoped<SignInManager, SignInManager>();


            services.AddScoped<IProjectSettingsRepository, cloudscribe.SimpleContent.Storage.Json.ProjectSettingsRepository>();
            services.Configure<List<ProjectSettings>>(Configuration.GetSection("ContentProjects"));
            services.AddScoped<IProjectService, ProjectService>();
            
            services.AddXmlBlogStorage();

            services.AddJsonPageStorage();
            
            //services.AddScoped<IProjectSettingsResolver, DefaultProjectSettingsResolver>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPageService, PageService>();

            services.AddScoped<IMediaProcessor, FileSystemMediaProcessor>();
            
            services.AddScoped<IMetaWeblogService, MetaWeblogService>();
            services.AddScoped<IMetaWeblogRequestParser, MetaWeblogRequestParser>();
            services.AddScoped<IMetaWeblogRequestProcessor, MetaWeblogRequestProcessor>();
            services.AddScoped<IMetaWeblogResultFormatter, MetaWeblogResultFormatter>();
            services.AddScoped<IMetaWeblogRequestValidator, MetaWeblogRequestValidator>();
            services.AddScoped<MetaWeblogModelMapper, MetaWeblogModelMapper>();
            services.AddScoped<IProjectSettingsResolver, SiteProjectSettingsResolver>();

            services.AddScoped<IProjectSecurityResolver, cloudscribe.SimpleContent.Security.SimpleAuth.ProjectSecurityResolver>();
            services.Configure<ApiOptions>(Configuration.GetSection("MetaWeblogApiOptions"));
            services.AddScoped<IMetaWeblogSecurity, MetaWeblogSecurity>();
            
            services.AddScoped<NavigationTreeBuilderService, NavigationTreeBuilderService>();
            //services.TryAddScoped<ITreeCache, MemoryTreeCache>();
            services.AddScoped<INavigationTreeBuilder, XmlNavigationTreeBuilder>();
            services.AddScoped<INavigationTreeBuilder, PagesNavigationTreeBuilder>();

            services.Configure<NavigationOptions>(options =>
            {
                //options.RootTreeBuilderName = "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder";
            });

            
            services.AddScoped<INodeUrlPrefixProvider, DefaultNodeUrlPrefixProvider>();
            services.AddScoped<INavigationNodePermissionResolver, NavigationNodePermissionResolver>();
            //services.Configure<NavigationOptions>(Configuration.GetSection("NavigationOptions"));
            

            // Add MVC services to the services container.
            services.Configure<MvcOptions>(options =>
            {
               // options.InputFormatters.Add(new Xm)
            });

            services.AddMvc();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new SiteViewLocationExpander());
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



        // Configure is called after ConfigureServices is called.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IOptions<SimpleAuthSettings> authSettingsAccessor)
        {
            app.UseGlimpse();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                
                //app.UseDatabaseErrorPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");
            }

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            app.UseMultitenancy<SiteSettings>();

            // Add cookie-based authentication to the request pipeline
            
            app.UsePerTenant<SiteSettings>((ctx, builder) =>
            {
                builder.UseCookieAuthentication(options =>
                {
                    options.AuthenticationScheme = ctx.Tenant.AuthenticationScheme;
                    options.LoginPath = new PathString("/login");
                    options.AccessDeniedPath = new PathString("/");
                    options.AutomaticAuthenticate = true;
                    options.AutomaticChallenge = true;

                    options.CookieName = ctx.Tenant.AuthenticationScheme;
                });

                
            });

            // Add MVC to the request pipeline.
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

            });
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
