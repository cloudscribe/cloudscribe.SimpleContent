using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.EventHandlers
{
    public class HandleRoleUpdated : IHandleRoleUpdated
    {
        private readonly IPageService                 _pageService;
        private readonly NavigationTreeBuilderService _nav;
        private readonly IHttpContextAccessor         _httpContextAccessor;
        private readonly ILogger<HandleRoleUpdated>   _logger;

        public HandleRoleUpdated(IPageService pageService, 
            NavigationTreeBuilderService nav, 
            IHttpContextAccessor httpContextAccessor,
            ILogger<HandleRoleUpdated> logger)
        {
            _pageService         = pageService;
            _nav                 = nav;
            _httpContextAccessor = httpContextAccessor;
            _logger              = logger;
        }


        async Task IHandleRoleUpdated.Handle(ISiteRole role, string oldRoleName)
        {
            var pages = await _pageService.GetAllPages(role.SiteId.ToString());
            int pagesUpdated = 0;

            foreach (var page in pages.Where(p => !string.IsNullOrWhiteSpace(p.ViewRoles)))
            {
                // assuming no case sensitivity issues here
                if (page.ViewRoles.Contains(oldRoleName))
                {
                    var rolesStrings = page.ViewRoles.Split(',');
                    var newRolesString = new StringBuilder();
                    foreach (var roleName in rolesStrings)
                    {
                        if (roleName.Trim().Equals(oldRoleName.Trim()))
                            newRolesString.Append(role.RoleName);
                        else
                            newRolesString.Append(roleName.Trim());

                        newRolesString.Append(",");
                    }

                    page.ViewRoles = newRolesString.ToString().TrimEnd(',');

                    await _pageService.Update(page);
                    pagesUpdated++;
                }
            }

            ///////////////////////////////////
            // This is an attempt to fiddle the navigation cache and the
            // roles of the current user so that they remain in step with one another 
            // after a user renames a role that they themselves are in.
            // Otherwise pages can mysteriously vanish from the nav when they should not do,
            // following a role rename - jk
            if (pagesUpdated > 0)
            {
                try
                {
                    var user = _httpContextAccessor?.HttpContext?.User;

                    if (user != null && user.Identity != null && user.Identity.IsAuthenticated && user.IsInRole(oldRoleName))
                    {
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Role, role.RoleName)
                            };

                        var appIdentity = new ClaimsIdentity(claims);
                        user.AddIdentity(appIdentity);
                    }

                    await _nav.GetTree();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error re-setting user claims and cache after role update");
                }
            }
            ////////////////////////////////////////
        }
    }
}
