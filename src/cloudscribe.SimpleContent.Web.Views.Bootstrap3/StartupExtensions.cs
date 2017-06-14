using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Web.Views.Bootstrap3;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        [Obsolete("AddBootstrap3EmbeddedViewsForSimpleContent is deprecated, please use AddCloudscribeSimpleContentBootstrap3Views instead.")]
        public static RazorViewEngineOptions AddBootstrap3EmbeddedViewsForSimpleContent(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(Bootstrap3).GetTypeInfo().Assembly,
                    "cloudscribe.SimpleContent.Web.Views.Bootstrap3"
                ));

            return options;
        }

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
