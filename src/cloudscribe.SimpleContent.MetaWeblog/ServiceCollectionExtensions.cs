using cloudscribe.MetaWeblog;
using cloudscribe.SimpleContent.MetaWeblog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMetaWeblogForSimpleContent(
            this IServiceCollection services,
            IConfiguration configuration = null
            )
        {
            services.TryAddScoped<IMetaWeblogSecurity, MetaWeblogSecurity>();
            services.TryAddScoped<IMetaWeblogService, MetaWeblogService>();
            services.TryAddScoped<MetaWeblogModelMapper, MetaWeblogModelMapper>();

            services.AddCloudscribeMetaWeblog(configuration);
            
            return services;
        }
    }
}
