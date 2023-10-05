using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Navigation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.EventHandlers
{
    public class HandleRoleDeleted : IHandleRoleDeleted
    {
        private readonly IPageService _pageService;
        private readonly NavigationTreeBuilderService _nav;

        public HandleRoleDeleted(IPageService pageService, 
            NavigationTreeBuilderService nav)
        {
            _pageService = pageService;
            _nav         = nav;
        }

        async Task IHandleRoleDeleted.Handle(ISiteRole role)
        {
            var pages = await _pageService.GetAllPages(role.SiteId.ToString());
            int pagesUpdated = 0;

            foreach (var page in pages.Where(p => !string.IsNullOrWhiteSpace(p.ViewRoles)))
            {
                // assuming no case sensitivity issues here
                if (page.ViewRoles.Contains(role.RoleName))
                {
                    var rolesStrings = page.ViewRoles.Split(',');
                    var newRolesString = new StringBuilder();
                    foreach (var roleName in rolesStrings)
                    {
                        if (!roleName.Trim().Equals(role.RoleName.Trim()))
                        { 
                            newRolesString.Append(roleName.Trim());
                            newRolesString.Append(",");
                        }
                    }

                    page.ViewRoles = newRolesString.ToString().TrimEnd(',');

                    await _pageService.Update(page);
                    pagesUpdated++;
                }
            }

            if (pagesUpdated > 0)
                await _nav.GetTree();
        }
    }
}
