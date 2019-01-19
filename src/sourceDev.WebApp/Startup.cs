using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace sourceDev.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
            _sslIsAvailable = _configuration.GetValue<bool>("AppSettings:UseSsl");
            _enableWebpackMiddleware = _configuration.GetValue<bool>("DevOptions:EnableWebpackMiddleware");
        }

        private IHostingEnvironment _environment;
        private IConfiguration _configuration;
        private bool _sslIsAvailable;
        private bool _enableWebpackMiddleware;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddGlimpse();

            services.SetupDataProtection(_configuration, _environment);
            
            //services.AddMemoryCache();

            services.AddAuthorization(options =>
            {
                //https://docs.asp.net/en/latest/security/authorization/policies.html
                //** IMPORTANT ***
                //This is a custom extension method in Config/Authorization.cs
                //That is where you can review or customize or add additional authorization policies
                options.SetupAuthorizationPolicies();

            });
            
            services.SetupDataStorage(_configuration);

            services.SetupCloudscribeFeatures(_configuration);
            
            services.SetupLocalization(_configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = cloudscribe.Core.Identity.SiteCookieConsent.NeedsConsent;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.ConsentCookie.Name = "cookieconsent_status";
            });

            services.Configure<Microsoft.AspNetCore.Mvc.CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.SetupMvc(_configuration, _sslIsAvailable);


            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            var useMiniProfiler = _configuration.GetValue<bool>("DevOptions:EnableMiniProfiler");
            if(useMiniProfiler)
            {
                app.UseMiniProfiler();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
                if(_enableWebpackMiddleware)
                {
                    app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                    {
                        HotModuleReplacement = true
                        ,ReactHotModuleReplacement = true
                    });
                }
                
            }
            else
            {
                app.UseExceptionHandler("/oops/error");
                if(_sslIsAvailable)
                {
                    app.UseHsts();
                }
            }

            if (_sslIsAvailable)
            {
                app.UseHttpsRedirection();
            }


            app.UseStaticFiles();
            app.UseCloudscribeCommonStaticFiles();
            app.UseCookiePolicy();

            //app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore(
                    loggerFactory,
                    multiTenantOptions,
                    _sslIsAvailable);

            app.UseMvc(routes =>
            {
                var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
                //*** IMPORTANT ***
                // this is in Config/RoutingAndMvc.cs
                // you can change or add routes there
                routes.UseCustomRoutes(useFolders, _configuration);
            });
            
            
        }

        
    }
}
