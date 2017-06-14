//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2016-02-09
//// Last Modified:			2016-08-12
//// 

//using System;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;

//namespace cloudscribe.Core.SimpleContent.Integration
//{
//    public static class ClaimsPrincipalExtensions
//    {

//        public static string GetEmail(this ClaimsPrincipal principal)
//        {
//            if (principal == null)
//            {
//                throw new ArgumentNullException(nameof(principal));
//            }
//            var claim = principal.FindFirst("Email");
//            return claim != null ? claim.Value : null;
//        }

//        public static string GetUserDisplayName(this ClaimsPrincipal principal)
//        {
//            if (principal == null)
//            {
//                throw new ArgumentNullException(nameof(principal));
//            }
//            var claim = principal.FindFirst("DisplayName");
//            return claim != null ? claim.Value : null;
//        }

//        public static string GetProjectId(this ClaimsPrincipal principal)
//        {
//            if (principal == null)
//            {
//                throw new ArgumentNullException(nameof(principal));
//            }
//            var claim = principal.FindFirst("ContentEditor");
//            return claim != null ? claim.Value : null;
//        }

//        public static bool CanEditProject(this ClaimsPrincipal principal, string projectId)
//        {
//            if (principal == null)
//            {
//                throw new ArgumentNullException(nameof(principal));
//            }
//            var claim = principal.FindFirst("ContentEditor");
//            if (claim == null) { return false; }
//            if (claim.Value == projectId) { return true; }
//            return false;
//        }

//        public static bool CanEditPages(
//            this ClaimsPrincipal principal,
//            string projectId,
//            IAuthorizationService authorizationService = null
//            )
//        {
//            if (principal == null)
//            {
//                throw new ArgumentNullException(nameof(principal));
//            }
//            if (principal.CanEditProject(projectId)) return true;

//            var claim = principal.FindFirst("PageEditor");
//            if (claim == null) { return false; }
//            if (claim.Value == projectId) { return true; }
//            return false;
//        }

//        public static bool CanEditBlog(this ClaimsPrincipal principal, string projectId)
//        {
//            if (principal == null)
//            {
//                throw new ArgumentNullException(nameof(principal));
//            }
//            if (principal.CanEditProject(projectId)) return true;

//            var claim = principal.FindFirst("BlogEditor");
//            if (claim == null) { return false; }
//            if (claim.Value == projectId) { return true; }
//            return false;
//        }



//    }
//}
