using cloudscribe.SimpleContent.ContentTemplates.Services;
using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddContentTemplatesForSimpleContent(
           this IServiceCollection services,
           IConfiguration configuration
           )
        {
            services.AddSingleton<IContentTemplateProvider, ContentTemplateProvider>();

            return services;
        }
    }
}
