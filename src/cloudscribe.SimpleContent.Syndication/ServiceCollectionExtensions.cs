using cloudscribe.SimpleContent.Syndication;
using cloudscribe.Syndication.Models.Rss;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleContentRssSyndiction(this IServiceCollection services)
        {
            services.TryAddScoped<IChannelProvider, RssChannelProvider>();
            return services;
        }
    }
}
