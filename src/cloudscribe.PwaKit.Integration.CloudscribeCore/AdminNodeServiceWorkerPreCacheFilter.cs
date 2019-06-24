using cloudscribe.PwaKit.Integration.Navigation;
using cloudscribe.Web.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Integration.CloudscribeCore
{
    /// <summary>
    /// to filter out cloudscribe admin menu items from the serviceworker pre-cache
    /// </summary>
    public class AdminNodeServiceWorkerPreCacheFilter : INavigationNodeServiceWorkerFilter
    {
        public AdminNodeServiceWorkerPreCacheFilter()
        {

            _excludedControllers = new List<string>()
            {
                "SiteAdmin",
                "RoleAdmin",
                "UserAdmin",
                "CoreData",
                "SystemLog",
                "Manage",
                "Account",
                "FileManager",
                "ContentSettings",
                "ContentHistory"

            };

        }

        private List<string> _excludedControllers;

        public Task<bool> ShouldRenderNode(NavigationNode node)
        {
            var include = true;

            if(!string.IsNullOrEmpty(node.Controller) && _excludedControllers.Contains(node.Controller))
            {
                include = false;
            }
            else if (!string.IsNullOrEmpty(node.Controller) && !string.IsNullOrEmpty(node.Action))
            {
                if(node.Controller == "Page" && node.Action == "Tree")
                {
                    include = false;
                }
            }
            

            return Task.FromResult(include);


        }

    }
}
