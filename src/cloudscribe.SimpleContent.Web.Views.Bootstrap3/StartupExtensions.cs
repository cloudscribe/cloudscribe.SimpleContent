using cloudscribe.SimpleContent.Web.Views.Bootstrap3;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        
        public static RazorViewEngineOptions AddCloudscribeSimpleContentBootstrap3Views(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Bootstrap3).GetTypeInfo().Assembly,
                    "cloudscribe.SimpleContent.Web.Views.Bootstrap3"
                ));

            return options;
        }
    }
}
