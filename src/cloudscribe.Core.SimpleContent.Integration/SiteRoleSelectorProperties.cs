// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-26
// Last Modified:			2016-08-26
// 


using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Web.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class SiteRoleSelectorProperties
    {
        public SiteRoleSelectorProperties(SiteSettings currentSite)
        {
            this.currentSite = currentSite;
            Attributes = new Dictionary<string, string>();
            RouteParams = new Dictionary<string, string>();
            RequiredScriptPaths = new List<string>();

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

        public Dictionary<string, string> Attributes { get; private set; }

        public Dictionary<string, string> RouteParams { get; private set; }


        public List<string> RequiredScriptPaths { get; private set; }


        public string CsvTargetElementId
        {
            get { return string.Empty; }
        }

    }
}
