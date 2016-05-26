using cloudscribe.SimpleContent.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// This method adds an embedded file provider to the RazorViewOptions to be able to load the Blog related views.
        /// If you download and install the views below your view folder you don't need this method and you can customize the views.
        /// You can get the views from https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/src/cloudscribe.SimpleContent.Blog.Web/Views
        /// </summary>
        /// <param name="options"></param>
        /// <returns>RazorViewEngineOptions</returns>
        public static RazorViewEngineOptions AddEmbeddedViewsForBlog(this RazorViewEngineOptions options)
        {
            options.FileProviders.Add(new EmbeddedFileProvider(
                    typeof(BlogController).GetTypeInfo().Assembly,
                    "cloudscribe.SimpleContent.Blog.Web"
                ));

            return options;
        }
    }
}
