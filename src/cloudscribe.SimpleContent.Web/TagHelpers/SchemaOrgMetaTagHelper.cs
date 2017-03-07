// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-03-06
// Last Modified:			2017-03-06
// 

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Threading.Tasks;
using System.Text;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    [HtmlTargetElement("schema-org-meta")]
    public class SchemaOrgMetaTagHelper : TagHelper
    {
       
        private const string ItemPropAttributeName = "itemprop";

        private const string ItemTypeAttributeName = "itemtype";

        private const string ItemScopeAttributeName = "itemscope";
        private const string ContentAttributeName = "content";

        [HtmlAttributeName(ItemPropAttributeName)]
        public string ItemProp { get; set; } 

        [HtmlAttributeName(ItemTypeAttributeName)]
        public string ItemType { get; set; } 

        [HtmlAttributeName(ContentAttributeName)]
        public string Content { get; set; } 

        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ItemProp))
            {
                output.SuppressOutput();
                return;
            }

            if (string.IsNullOrEmpty(Content))
            {
                output.SuppressOutput();
                return;
            }
           
            

            output.TagName = "meta";    // Replaces <schema-org-meta> with <meta> tag
            output.TagMode = TagMode.SelfClosing;
            output.Attributes.Add(ItemPropAttributeName, ItemProp);
            
            if(!string.IsNullOrEmpty(ItemType))
            {
                output.Attributes.Add(ItemScopeAttributeName, "");

                output.Attributes.Add(ItemTypeAttributeName, ItemType);

            }
            output.Attributes.Add(ContentAttributeName, Content);
            //output.Attributes.Add( new TagHelperAttribute("type", new HtmlString("application/ld+json")));

            


        }
    }
}
