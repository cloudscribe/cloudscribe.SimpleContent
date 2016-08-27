// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-26
// Last Modified:			2016-08-27
// 

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.TagHelpers
{
    /// <summary>
    /// purpose to allow external components to provide a way to select roles
    /// specifically when using SimpleContent with cloudscribe.Core, we need a way to provide the controller and action and data-*
    /// attributes to open modal for role selection and a way to feed the selected roles back into SimpleContent page editor
    /// abstracting the IRoleSelectorProperties allows us to avoid coupling dependencies since SimpleContent can be used without
    /// cloudscribe.Core. If the default NotImplementedRoleSelectorProperties is used then the selection link/button is not rendered at all
    /// </summary>
    [HtmlTargetElement("a", Attributes = "csc-role-selector,csc-projectId")]
    public class RoleSelectorTagHelper : AnchorTagHelper
    {
        private const string RoleSelectorAttributeName = "csc-role-selector";
        private const string ProjectIdAttributeName = "csc-projectId";
        private const string CsvTargetIdAttributeName = "csc-target-id";
        private const string DisplayTargetIdAttributeName = "csc-display-target-id";

        public RoleSelectorTagHelper(
            IHtmlGenerator generator
            ) : base(generator)
        {
            
        }
        
        [HtmlAttributeName(RoleSelectorAttributeName)]
        public IRoleSelectorProperties RoleSelectorInfo { get; set; } = null;
        
        [HtmlAttributeName(ProjectIdAttributeName)]
        public string ProjectId { get; set; } = string.Empty;

        [HtmlAttributeName(CsvTargetIdAttributeName)]
        public string CsvTargetElementId { get; set; } = string.Empty;

        [HtmlAttributeName(DisplayTargetIdAttributeName)]
        public string DisplayTargetElementId { get; set; } = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (RoleSelectorInfo == null 
                || string.IsNullOrEmpty(RoleSelectorInfo.Controller) 
                || string.IsNullOrEmpty(RoleSelectorInfo.Action)
                || string.IsNullOrEmpty(ProjectId)
                || string.IsNullOrEmpty(CsvTargetElementId)
                )
            {
                output.SuppressOutput();
                return;
            }
            
            // we don't need to render these
            var markerAttribute = new TagHelperAttribute(RoleSelectorAttributeName);
            output.Attributes.Remove(markerAttribute);
            var projectAttribute = new TagHelperAttribute(ProjectIdAttributeName);
            output.Attributes.Remove(projectAttribute);
            var csvAttribute = new TagHelperAttribute(CsvTargetIdAttributeName);
            output.Attributes.Remove(csvAttribute);
            var displayAttribute = new TagHelperAttribute(DisplayTargetIdAttributeName);
            output.Attributes.Remove(displayAttribute);
            //

            this.Action = RoleSelectorInfo.Action;
            this.Controller = RoleSelectorInfo.Controller;
            var routeParams = RoleSelectorInfo.GetRouteParams(ProjectId);
            if(routeParams != null)
            {
                foreach(var param in routeParams)
                {
                    this.RouteValues.Add(param.Key, param.Value);
                }
            }
            var atts = RoleSelectorInfo.GetAttributes(CsvTargetElementId, DisplayTargetElementId);
            if(atts != null)
            {
                foreach(var att in atts)
                {
                    output.Attributes.Add(new TagHelperAttribute(att.Key, att.Value));
                }
            }
            
            await base.ProcessAsync(context, output);
        }
    }
}
