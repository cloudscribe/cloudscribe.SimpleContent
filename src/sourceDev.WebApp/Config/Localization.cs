using cloudscribe.Core.Web.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Localization
    {
        public static IServiceCollection SetupLocalization(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            // https://github.com/cloudscribe/cloudscribe.Web.Localization
            // https://www.cloudscribe.com/localization
            services.Configure<GlobalResourceOptions>(config.GetSection("GlobalResourceOptions"));
            services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();

            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            //you should only add cultures that you really plan to support
            //if you translate the resx files or other files please share them back with us, email zip to info@cloudscribe.com
            var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    //new CultureInfo("en-GB"),
                    //new CultureInfo("cy"),  // deliberately not cy-GB
                    new CultureInfo("fr-FR"),
                    new CultureInfo("cy-GB"),
                    new CultureInfo("it-IT"),
                    //new CultureInfo("ar-AR"),
                    //new CultureInfo("es-ES"),

                };

            //this comes from cloudscribe core
            var routeSegmentLocalizationProvider = new UrlSegmentRequestCultureProvider(supportedCultures.ToList());

            services.Configure<RequestLocalizationOptions>(options =>
            {


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

                options.RequestCultureProviders.Insert(0, routeSegmentLocalizationProvider);

            });

            return services;
        }

    }
}
