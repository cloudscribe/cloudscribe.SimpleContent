using cloudscribe.MetaWeblog;
using cloudscribe.MetaWeblog.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeMetaWeblog(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {
            if (configuration != null)
            {
                services.Configure<ApiOptions>(configuration);
            }
            else
            {
                services.TryAddSingleton<ApiOptions, ApiOptions>();
            }

            services.TryAddScoped<IMetaWeblogRequestParser, MetaWeblogRequestParser>();
            services.TryAddScoped<IMetaWeblogRequestProcessor, MetaWeblogRequestProcessor>();
            services.TryAddScoped<IMetaWeblogResultFormatter, MetaWeblogResultFormatter>();
            services.TryAddScoped<IMetaWeblogRequestValidator, MetaWeblogRequestValidator>();

            return services;
        }
    }
}
