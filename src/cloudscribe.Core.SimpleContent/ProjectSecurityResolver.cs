// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-07-11
// Last Modified:           2016-08-13
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Web;
using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class ProjectSecurityResolver : IProjectSecurityResolver
    {
        public ProjectSecurityResolver(
            SiteUserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            IProjectSettingsResolver projectResolver,
            IAuthorizationService authorizationService
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.projectResolver = projectResolver;
            this.authorizationService = authorizationService;
        }

        private SiteUserManager<SiteUser> userManager;
        private SignInManager<SiteUser> signInManager;
        private IAuthorizationService authorizationService;
        private IProjectSettingsResolver projectResolver;

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

            var authUser = await userManager.FindByNameAsync(userName);

            if (authUser != null)
            {
                isAuthenticated = await userManager.CheckPasswordAsync(authUser, providedPassword);
            }

            if (isAuthenticated)
            {
                var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(authUser);
                if (string.IsNullOrEmpty(projectId))
                {
                    projectId = claimsPrincipal.GetProjectId();
                }

                if (string.IsNullOrEmpty(projectId))
                {
                    var project = await projectResolver.GetCurrentProjectSettings(cancellationToken);
                    if (project != null) projectId = project.Id;
                }
                if (!string.IsNullOrEmpty(projectId))
                {
                    canEditPosts = await claimsPrincipal.CanEditBlog(projectId, authorizationService);
                    canEditPages = await claimsPrincipal.CanEditPages(projectId, authorizationService);     
                }

                //displayName = claimsPrincipal.GetDisplayName();
                displayName = claimsPrincipal.Identity.Name;
            }

            var blogSecurity = new ProjectSecurityResult(displayName, projectId, isAuthenticated, canEditPosts, canEditPages);

            return blogSecurity;

        }

    }
}
