using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using cloudcsribe.Core.SimpleContent.Bootstrap3;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static RazorViewEngineOptions AddCloudscribeCoreSimpleContentIntegrationBootstrap3Views(this RazorViewEngineOptions options)
        {
            // 2017-06-14 for some reason this is not working, the views are not found as if they are not embedded
            // I copied the views into cloudscribe.SimpleContent.Web.Views.Bootstrap3 and it works from there

            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Bootstrap3Views).GetTypeInfo().Assembly,
                    "cloudscribe.Core.SimpleContent.Boostrap3"
                ));

            return options;
        }
    }
}
