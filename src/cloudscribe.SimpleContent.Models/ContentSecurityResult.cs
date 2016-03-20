// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-02-16
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public class ContentSecurityResult
    {
        public ContentSecurityResult(
            string displayName,
            string projectId,
            bool isAllowed, 
            bool isBlogOwner
            )
        {
            this.displayName = displayName;
            this.projectId = projectId;
            this.isAllowed = isAllowed;
            this.isBlogOwner = isBlogOwner;
        }

        private string displayName = string.Empty;
        private string projectId = string.Empty;
        private bool isAllowed = false;
        private bool isBlogOwner = false;

        public string DisplayName
        {
            get { return displayName; }
        }

        public string ProjectId
        {
            get { return projectId; }
        }

        public bool IsAllowed
        {
            get { return isAllowed; }
        }

        public bool IsBlogOwner
        {
            get { return isBlogOwner; }
        }
    }
}
