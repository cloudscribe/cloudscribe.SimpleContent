// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-03-21
// 


namespace cloudscribe.SimpleContent.Models
{
    public class ProjectSecurityResult
    {
        public ProjectSecurityResult(
            string displayName,
            string projectId,
            bool isAllowed, 
            bool isProjectOwner
            )
        {
            this.displayName = displayName;
            this.projectId = projectId;
            this.isAllowed = isAllowed;
            this.isProjectOwner = isProjectOwner;
        }

        private string displayName = string.Empty;
        private string projectId = string.Empty;
        private bool isAllowed = false;
        private bool isProjectOwner = false;

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

        public bool IsProjectOwner
        {
            get { return isProjectOwner; }
        }
    }
}
