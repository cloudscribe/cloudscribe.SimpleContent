// Copyright (c) Idox Software Ltd All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See License.txt in the project root for license information.
// Author:					Simon Annetts, Idox Software Ltd
// Created:					2023-08-17
// Last Modified:			2023-08-17
//

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.SimpleContent.Integration.ViewModels
{

    public class ContentCloningViewModel
    {
        public ContentCloningViewModel()
        {

        }
        //this could be passed in from the Site Settings List page
        public string SiteId { get; set; } = string.Empty;

        public Boolean CloneContentSettings { get; set; } = true;
        public Boolean ClonePages { get; set; } = true;
        public Boolean RewriteContentUrls { get; set; } = false;
        public Boolean CloneBlogPosts { get; set; } = true;


        public string CloneFromSiteId { get; set; } = null;
        public string CloneFromSiteName { get; set; } = string.Empty;
        public int CloneFromPageCount { get; set; } = 0;
        public int CloneFromPostCount { get; set; } = 0;

        public List<SiteDetails> CloneFromSites { get; set; } = new List<SiteDetails>();

        public string CloneToSiteId { get; set; } = null;
        public string CloneToSiteName { get; set; } = string.Empty;
        public int CloneToPageCount { get; set; } = 0;
        public int CloneToPostCount { get; set; } = 0;

        public List<SiteDetails> CloneToSites { get; set; } = new List<SiteDetails>();

        public Boolean AllowCloneToSiteSelection { get; set; } = true;

        public bool CloneAllowed { get; set; } = false;

        public string Command { get; set; } = string.Empty;

        public class SiteDetails
        {
            public string SiteId { get; set; }
            public string SiteIdentifier { get; set; }
        }

    }
}