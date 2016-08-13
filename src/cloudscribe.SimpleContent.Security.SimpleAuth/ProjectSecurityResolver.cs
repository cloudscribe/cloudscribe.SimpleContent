// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-08-13
// 

using cloudscribe.SimpleContent.Web;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.SimpleAuth.Services;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace cloudscribe.SimpleContent.Security.SimpleAuth
{
    public class ProjectSecurityResolver : IProjectSecurityResolver
    {
        public ProjectSecurityResolver(
            SignInManager signInManager,
            IAuthorizationService authorizationService
            )
        {
            this.signInManager = signInManager;
            this.authorizationService = authorizationService;
        }

        private SignInManager signInManager;
        private IAuthorizationService authorizationService;

        public async Task<ProjectSecurityResult> ValidatePermissions(
            string projectId,
            string userName,
            string providedPassword,
            CancellationToken cancellationToken)
        {
            var displayName = string.Empty;
            var isAuthenticated = false;
            var canEditPosts = false;
            var canEditPages = false;

            var authUser = signInManager.GetUser(userName);

            if (authUser != null)
            {
                isAuthenticated = signInManager.ValidatePassword(authUser, providedPassword);
            }
            
            if (isAuthenticated)
            {
                var claimsPrincipal = signInManager.GetClaimsPrincipal(authUser);
                if(string.IsNullOrEmpty(projectId))
                {
                    projectId = claimsPrincipal.GetProjectId();
                }
                if (!string.IsNullOrEmpty(projectId))
                {
                    canEditPosts = await claimsPrincipal.CanEditBlog(projectId, authorizationService); 
                    canEditPages = await claimsPrincipal.CanEditPages(projectId, authorizationService);                 
                }
                
                displayName = claimsPrincipal.GetUserDisplayName();
            }
            
            var blogSecurity = new ProjectSecurityResult(displayName, projectId, isAuthenticated, canEditPosts, canEditPages);

            return blogSecurity;

        }

        

    }
}
