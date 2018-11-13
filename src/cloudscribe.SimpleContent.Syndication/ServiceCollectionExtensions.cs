using cloudscribe.SimpleContent.Syndication;
using cloudscribe.Syndication.Models.Rss;
using cloudscribe.Versioning;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleContentRssSyndiction(this IServiceCollection services)
        {
            services.TryAddScoped<IChannelProvider, RssChannelProvider>();
            services.AddScoped<IVersionProvider, VersionProvider>();
            return services;
        }
    }
}
