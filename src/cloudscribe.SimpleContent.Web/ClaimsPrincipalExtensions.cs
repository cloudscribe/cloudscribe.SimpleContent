// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-02-09
// Last Modified:			2016-08-15
// 

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Web
{
    public static class ClaimsPrincipalExtensions
    {

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst("Email");
            return claim != null ? claim.Value : null;
        }

        public static string GetUserDisplayName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst("DisplayName");
            return claim != null ? claim.Value : null;
        }

        public static string GetProjectId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ProjectConstants.ContentEditorClaimType);


            return claim != null ? claim.Value : null;
        }

        public static bool CanEditProject(this ClaimsPrincipal principal, string projectId)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ProjectConstants.ContentEditorClaimType);
            if(claim == null) { return false; }
            if(claim.Value == projectId) { return true; }
            return false;
        }

        public static async Task<bool> CanEditPages(
            this ClaimsPrincipal principal, 
            string projectId,
            IAuthorizationService authorizationService
            )
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (principal.CanEditProject(projectId)) return true;

            var claim = principal.FindFirst(ProjectConstants.PageEditorClaimType);
            if (claim != null && claim.Value == projectId) { return true; }
           
            if(authorizationService != null)
            {
                try
                {
                    var result = await authorizationService.AuthorizeAsync(principal, ProjectConstants.PageEditPolicy).ConfigureAwait(false);
                    return result.Succeeded;
                }
                catch(InvalidOperationException) { }
                
            }
            return false;
        }

        public static async Task<bool> CanEditBlog(
            this ClaimsPrincipal principal, 
            string projectId,
            IAuthorizationService authorizationService
            )
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (principal.CanEditProject(projectId)) return true;

            var claim = principal.FindFirst(ProjectConstants.BlogEditorClaimType);
            if (claim != null && claim.Value == projectId) { return true; }

            if (authorizationService != null)
            {
                try
                {
                    var result = await authorizationService.AuthorizeAsync(principal, ProjectConstants.BlogEditPolicy).ConfigureAwait(false);
                    return result.Succeeded;
                }
                catch (InvalidOperationException) { }

            }

            return false;
        }

        public static bool IsInRoles(this ClaimsPrincipal principal, string allowedRolesCsv)
        {
            // Administraotors can not be blocked access to anything
            if (principal.IsInRole("Administrators")) return true;

            if (string.IsNullOrEmpty(allowedRolesCsv)) { return true; } // empty indicates no role filtering
            string[] roles;
            // in some cases we are using semicolon separated not comma
            if (allowedRolesCsv.Contains(";"))
            {
                roles = allowedRolesCsv.Split(';');
            }
            else
            {
                roles = allowedRolesCsv.Split(',');
            }
            if (roles.Length == 0) { return true; }

            //if (!principal.IsSignedIn()) { return false; }

            foreach (string role in roles)
            {
                if (role.Length == 0) continue;
                if (role == "All Users") { return true; }
                if (principal.IsInRole(role)) { return true; }
            }


            return false;

        }


    }
}
