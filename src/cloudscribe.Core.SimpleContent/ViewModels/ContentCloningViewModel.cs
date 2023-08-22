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

        public Boolean ClonePages { get; set; } = true;
        public Boolean ClonePageTree { get; set; } = true;
        public Boolean CloneMediaFiles { get; set; } = false;
        public Boolean RewriteContentUrls { get; set; } = true;
        public Boolean CloneBlog { get; set; } = true;
        public Boolean CloneBlogPosts { get; set; } = true;
        public Boolean CloneAboutContent { get; set; } = true;
        public Boolean CloneFeedSettings { get; set; } = true;
        public Boolean CloneContentHistory { get; set; } = false;


        public string CloneFromSiteId { get; set; } = string.Empty;
        public string CloneFromSiteName { get; set; } = string.Empty;
        public int CloneFromPageCount { get; set; } = 0;
        public int CloneFromPostCount { get; set; } = 0;

        public List<SiteDetails> CloneFromSites { get; set; } = new List<SiteDetails>();

        public string CloneToSiteId { get; set; } = string.Empty;
        public string CloneToSiteName { get; set; } = string.Empty;
        public int CloneToPageCount { get; set; } = 0;
        public int CloneToPostCount { get; set; } = 0;

        public List<SiteDetails> CloneToSites { get; set; } = new List<SiteDetails>();

        public bool CloneAllowed { get; set; } = false;

        public string Command { get; set; } = string.Empty;

        public class SiteDetails
        {
            public string SiteId { get; set; }
            public string SiteName { get; set; }
        }

    }
}