using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static RazorViewEngineOptions AddCloudscribeSimpleContentBootstrap4Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(StartupExtensions).GetTypeInfo().Assembly,
                    "cloudscribe.SimpleContent.Web.Views.Bootstrap4"
                ));

            return options;
        }
    }
}
