// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-02-09
// Last Modified:			2016-02-15
// 

using System;
using System.Security.Claims;


namespace cloudscribe.SimpleContent.Common
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

        public static string GetDisplayName(this ClaimsPrincipal principal)
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
            var claim = principal.FindFirst("ProjectId");
            return claim != null ? claim.Value : null;
        }

        public static bool CanEditProject(this ClaimsPrincipal principal, string projectId)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst("ProjectId");
            if(claim == null) { return false; }
            if(claim.Value == projectId) { return true; }
            return false;
        }



    }
}
