using System;
using System.Security.Claims;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public static class ClaimsPrincipalExtensions
    {
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
            if (claim == null) { return false; }
            if (claim.Value == projectId) { return true; }
            return false;
        }
    }
}
