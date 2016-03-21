// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-03-20
// 

using cloudscribe.SimpleContent.Common;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.SimpleAuth.Services;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Security.SimpleAuth
{
    public class ProjectSecurityResolver : IProjectSecurityResolver
    {
        public ProjectSecurityResolver(SignInManager signInManager)
        {
            this.signInManager = signInManager;
        }

        private SignInManager signInManager;

        public Task<ProjectSecurityResult> ValidatePermissions(
            string projectId,
            string userName,
            string providedPassword,
            CancellationToken cancellationToken)
        {
            var displayName = string.Empty;
            var isAllowed = false;
            var isBlogOwner = false;

            var authUser = signInManager.GetUser(userName);

            if (authUser != null)
            {
                isAllowed = signInManager.ValidatePassword(authUser, providedPassword);
            }
            
            if (isAllowed)
            {
                var claimsPrincipal = signInManager.GetClaimsPrincipal(authUser);
                if(string.IsNullOrEmpty(projectId))
                {
                    projectId = claimsPrincipal.GetProjectId();
                }
                isBlogOwner = claimsPrincipal.CanEditProject(projectId);
                displayName = claimsPrincipal.GetDisplayName();
            }
            
            var blogSecurity = new ProjectSecurityResult(displayName, projectId, isAllowed, isBlogOwner);

            return Task.FromResult(blogSecurity);

        }

        

    }
}
