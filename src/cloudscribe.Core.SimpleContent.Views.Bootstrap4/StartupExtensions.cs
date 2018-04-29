using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static RazorViewEngineOptions AddCloudscribeCoreSimpleContentIntegrationBootstrap4Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(StartupExtensions).GetTypeInfo().Assembly,
                    "cloudscribe.Core.SimpleContent.Views.Bootstrap4"
                ));

            return options;
        }

    }
}
