// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-26
// Last Modified:			2016-08-26
// 

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    /// <summary>
    /// purpose to allow external components to provide a way to select roles
    /// specifically is using SimpleContent with cloudscribe.Core, we need a way to provide the controller and action and data-*
    /// attributes to open modal for role selection and a way to feed the selected roles back into SimpleContent page editor
    /// </summary>
    [HtmlTargetElement("a", Attributes = RoleSelectorAttributeName)]
    public class RoleSelectorTagHelper : AnchorTagHelper
    {
        private const string RoleSelectorAttributeName = "csc-role-selector";
        //private const string ProjectIdAttributeName = "csc-projectId";

        public RoleSelectorTagHelper(
            //IRoleSelectorProperties roleSelectorProperties,
            IHtmlGenerator generator
            ) : base(generator)
        {
            //roleSelector = roleSelectorProperties;
        }

        //private IRoleSelectorProperties roleSelector;

        [HtmlAttributeName(RoleSelectorAttributeName)]
        public IRoleSelectorProperties RoleSelectorInfo { get; set; } = null;

        

        //[HtmlAttributeName(ProjectIdAttributeName)]
        //public string ProjectId { get; set; } = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (RoleSelectorInfo == null 
                || string.IsNullOrEmpty(RoleSelectorInfo.Controller) 
                || string.IsNullOrEmpty(RoleSelectorInfo.Action)
              //  || string.IsNullOrEmpty(ProjectId)
                )
            {
                output.SuppressOutput();
                return;
            }
            
            // we don't need to render these
            var markerAttribute = new TagHelperAttribute(RoleSelectorAttributeName);
            output.Attributes.Remove(markerAttribute);
            //var projectAttribute = new TagHelperAttribute(ProjectIdAttributeName);
            //output.Attributes.Remove(projectAttribute);

            this.Action = RoleSelectorInfo.Action;
            this.Controller = RoleSelectorInfo.Controller;
            if(RoleSelectorInfo.RouteParams != null)
            {
                foreach(var param in RoleSelectorInfo.RouteParams)
                {
                    this.RouteValues.Add(param.Key, param.Value);
                }
            }

            if(RoleSelectorInfo.Attributes != null)
            {
                foreach(var att in RoleSelectorInfo.Attributes)
                {
                    output.Attributes.Add(new TagHelperAttribute(att.Key, att.Value));
                }
            }
            
            await base.ProcessAsync(context, output);
        }
    }
}
