// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-26
// Last Modified:			2016-08-27
// 


using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Web.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class SiteRoleSelectorProperties : IRoleSelectorProperties
    {
        public SiteRoleSelectorProperties(SiteSettings currentSite)
        {
            this.currentSite = currentSite;
            
            RequiredScriptPaths = new List<string>();
            RequiredScriptPaths.Add("~/cscsr/jqueryajax");
            RequiredScriptPaths.Add("~/cscsr/roleselectorjs");

        }

        private SiteSettings currentSite;

        public string Action
        {
            get { return "Modal"; }
        }

        public string Controller
        {
            get { return "RoleAdmin"; }
        }

        public Dictionary<string, string> GetAttributes(string csvTargetElementId, string displayTargetId = "")
        {
            var result = new Dictionary<string, string>();
            result.Add("data-ajax", "true");
            result.Add("data-ajax-begin", "roleSelector.prepareModal('" + csvTargetElementId + "','" + displayTargetId + "')");
            result.Add("data-ajax-failure", "roleSelector.clearModal()");
            result.Add("data-ajax-method", "GET");
            result.Add("data-ajax-mode", "replace");
            result.Add("data-ajax-success", "roleSelector.openModal()");
            result.Add("data-ajax-update", "#roledialog");

            return result;

        }

        public Dictionary<string, string> GetRouteParams(string projectId)
        {
            var result = new Dictionary<string, string>();
            result.Add("siteId", projectId);
            return result;
        }


        public List<string> RequiredScriptPaths { get; private set; }
        
    }
}
