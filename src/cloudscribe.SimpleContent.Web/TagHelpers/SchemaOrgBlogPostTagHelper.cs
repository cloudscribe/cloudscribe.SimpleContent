// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-03-03
// Last Modified:			2017-03-06
// 

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Threading.Tasks;
using System.Text;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    [HtmlTargetElement("schema-org-blog-post")]
    public class SchemaOrgBlogPostTagHelper : TagHelper
    {
        public SchemaOrgBlogPostTagHelper(
            IBlogService blogService
            )
        {
            this.blogService = blogService;
        }

        private IBlogService blogService;

        private const string BlogModelAttributeName = "blogmodel";

        [HtmlAttributeName(BlogModelAttributeName)]
        public BlogViewModel BlogModel { get; set; } = null;

        private string GetImageUrl(IPost post, IProjectSettings project)
        {

            return project.PublisherLogoUrl;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (BlogModel == null)
            {
                output.SuppressOutput();
                return;
            }
            var post = (BlogModel.TmpPost != null) ? BlogModel.TmpPost :  BlogModel.CurrentPost;

            if (post == null)
            {
                output.SuppressOutput();
                return;
            }

            var url = await blogService.ResolvePostUrl(post).ConfigureAwait(false);

            //BlogModel.

            output.TagName = "script";    // Replaces <google-analytics> with <script> tag
            output.Attributes.Add( new TagHelperAttribute("type", new HtmlString("application/ld+json")));

            var sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("{");
            sb.AppendLine("\"@context\":\"http://schema.org\"");
            sb.AppendLine(",\"@type\":\"BlogPosting\"");
            sb.AppendLine(",\"headline\": \"" + post.Title + "\"");
            sb.AppendLine(",\"url\": \"" + url + "\"");
            var imageUrl = GetImageUrl(post, BlogModel.ProjectSettings);
            if(!string.IsNullOrEmpty(imageUrl))
            {
                sb.AppendLine(",\"image\": \"" + imageUrl + "\"");

                //sb.AppendLine(",\"image\": { ");
               // sb.AppendLine("\"@context\": \"http://schema.org\",");
               // sb.AppendLine("\"@type\": \"ImageObject\",");
               // sb.AppendLine("\"contentUrl\": \"" + imageUrl + "\"");
               // sb.AppendLine(" }");
            }

            sb.AppendLine(",\"datePublished\": \"" + post.PubDate.ToString("s", CultureInfo.InvariantCulture) + "\"");
            sb.AppendLine(",\"dateModified\": \"" + post.LastModified.ToString("s", CultureInfo.InvariantCulture) + "\"");
            if(!string.IsNullOrEmpty(post.Author))
            {
                sb.AppendLine(",\"author\": { ");
                sb.AppendLine("\"@type\": \"Person\"");
                sb.AppendLine(",\"name\": \"" + post.Author + "\"");
                sb.AppendLine(" }");
            }

            if(!string.IsNullOrEmpty(BlogModel.ProjectSettings.Publisher))
            {
                sb.AppendLine(",\"publisher\": { ");
                sb.AppendLine("\"@type\": \"Organization\"");
                sb.AppendLine(",\"name\": \"" + BlogModel.ProjectSettings.Publisher + "\"");
                if(!string.IsNullOrEmpty(BlogModel.ProjectSettings.PublisherLogoUrl))
                {
                    sb.AppendLine(",\"logo\": { ");
                    sb.AppendLine("\"@type\": \"ImageObject\",");
                    sb.AppendLine("\"url\": \"" + BlogModel.ProjectSettings.PublisherLogoUrl + "\"");

                    sb.AppendLine(" }");

                }

                sb.AppendLine("}");
            }


            sb.AppendLine("}");

            output.Content.SetHtmlContent(sb.ToString());


        }
    }
}
