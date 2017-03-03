// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-03-03
// Last Modified:			2017-03-03
// 

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System.Text;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    public class SchemaOrgBlogPostTagHelper : TagHelper
    {
        public SchemaOrgBlogPostTagHelper(
            IBlogService blogService
            )
        {
            this.blogService = blogService;
        }

        private IBlogService blogService;

        private const string PostAttributeName = "post";

        [HtmlAttributeName(PostAttributeName)]
        public BlogViewModel BlogModel { get; set; } = null;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (BlogModel == null)
            {
                output.SuppressOutput();
                return;
            }
            var post = BlogModel.TmpPost ??  BlogModel.CurrentPost;

            if (post == null)
            {
                output.SuppressOutput();
                return;
            }

            //BlogModel.

            output.TagName = "script";    // Replaces <google-analytics> with <script> tag
            output.Attributes.Add("type", "application/ld+json");

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("\"@context\":\"http://schema.org\",");
            sb.AppendLine("\"@type\":\"BlogPosting\"");


            sb.AppendLine("}");

            output.Content.SetHtmlContent(sb.ToString());


        }
    }
}
