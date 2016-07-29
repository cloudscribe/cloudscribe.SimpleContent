// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-07-11
// Last Modified:           2016-07-11
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.Identity;
using cloudscribe.SimpleContent.Models;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class ProjectSecurityResolver : IProjectSecurityResolver
    {
        public ProjectSecurityResolver(
            SiteUserManager<SiteUser> userManager,
            SiteSignInManager<SiteUser> signInManager,
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
        private SiteSignInManager<SiteUser> signInManager;
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
            var canEdit = false;

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
                    if (project != null) projectId = project.ProjectId;
                }

                canEdit = claimsPrincipal.CanEditProject(projectId);
                if(!canEdit) canEdit = await authorizationService.AuthorizeAsync(claimsPrincipal, "BlogEditPolicy");

                displayName = claimsPrincipal.GetDisplayName();
            }

            var blogSecurity = new ProjectSecurityResult(displayName, projectId, isAuthenticated, canEdit);

            return blogSecurity;

        }

    }
}
