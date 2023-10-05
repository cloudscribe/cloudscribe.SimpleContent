using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace sourceDev.WebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env
            )
        {
            _configuration = configuration;
            _environment = env;

            _sslIsAvailable = _configuration.GetValue<bool>("AppSettings:UseSsl");
            _enableWebpackMiddleware = _configuration.GetValue<bool>("DevOptions:EnableWebpackMiddleware");
        }

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly bool _sslIsAvailable;
        private bool _enableWebpackMiddleware;


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            //// **** VERY IMPORTANT *****
            // This is a custom extension method in Config/DataProtection.cs
            // These settings require your review to correctly configur data protection for your environment
            services.SetupDataProtection(_configuration, _environment);

            services.AddAuthorization(options =>
            {
                //https://docs.asp.net/en/latest/security/authorization/policies.html
                //** IMPORTANT ***
                //This is a custom extension method in Config/Authorization.cs
                //That is where you can review or customize or add additional authorization policies
                options.SetupAuthorizationPolicies();

            });

            //// **** IMPORTANT *****
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupDataStorage(_configuration, _environment);


            //*** Important ***
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupCloudscribeFeatures(_configuration);

            //*** Important ***
            // This is a custom extension method in Config/Localization.cs
            services.SetupLocalization(_configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = cloudscribe.Core.Identity.SiteCookieConsent.NeedsConsent;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.ConsentCookie.Name = "cookieconsent_status";
                options.Secure = CookieSecurePolicy.Always;
            });

            if (_sslIsAvailable)
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(180);
                });
            }

            services.Configure<Microsoft.AspNetCore.Mvc.CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            //*** Important ***
            // This is a custom extension method in Config/RoutingAndMvc.cs
            services.SetupMvc(_sslIsAvailable);

            if(!_environment.IsDevelopment())
            {
                var httpsPort = _configuration.GetValue<int>("AppSettings:HttpsPort");
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = httpsPort;
                });
            }


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            // When running behind a proxy, the request to the proxy may be made via https,
            // but the proxy may be configured to talk to cloudscribe by http only. In that case,
            // we want our httpContext to contain the correct client IP address (rather than 127.0.0.1)
            // and the correct protocol (https).
            // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-3.1#configure-apache
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/oops/error");
                if(_sslIsAvailable)
                {
                    app.UseHsts();
                }
            }
            if(_sslIsAvailable)
            {
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseCloudscribeCommonStaticFiles();
            app.UseCookiePolicy();

            //app.UseRouting();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore();


#pragma warning disable MVC1005 // Cannot use UseMvc with Endpoint Routing.
// workaround for
//https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/466
 app.UseMvc(routes =>
            {
                var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
                //*** IMPORTANT ***
                // this is in Config/RoutingAndMvc.cs
                // you can change or add routes there
                routes.UseCustomRoutes(useFolders, _configuration);
            });
#pragma warning restore MVC1005 // Cannot use UseMvc with Endpoint Routing.

//             app.UseEndpoints(endpoints =>
//             {
//                 var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
//                 //*** IMPORTANT ***
//                 // this is in Config/RoutingAndMvc.cs
//                 // you can change or add routes there
//                 endpoints.UseCustomRoutes(useFolders);
//             });

        }



    }
}