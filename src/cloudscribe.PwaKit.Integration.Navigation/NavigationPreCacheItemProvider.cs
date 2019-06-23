using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Integration.Navigation
{
    public class NavigationPreCacheItemProvider : IPreCacheItemProvider
    {
        public NavigationPreCacheItemProvider(
            NavigationTreeBuilderService siteMapTreeBuilder,
            IEnumerable<INavigationNodePermissionResolver> permissionResolvers,
            IEnumerable<INavigationNodeServiceWorkerFilter> navigationNodeServiceWorkerFilters,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            IHttpContextAccessor contextAccessor
            )
        {
            _siteMapTreeBuilder = siteMapTreeBuilder;
            _permissionResolvers = permissionResolvers;
            _navigationNodeServiceWorkerFilters = navigationNodeServiceWorkerFilters;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _contextAccessor = contextAccessor;
        }



        private readonly NavigationTreeBuilderService _siteMapTreeBuilder;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccesor;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEnumerable<INavigationNodePermissionResolver> _permissionResolvers;
        private readonly IEnumerable<INavigationNodeServiceWorkerFilter> _navigationNodeServiceWorkerFilters;

        private List<string> addedUrls = new List<string>();
        

        public async Task<List<PreCacheItem>> GetItems()
        {
            var result = new List<PreCacheItem>();
            var rootNode = await _siteMapTreeBuilder.GetTree();
            
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            foreach (var navNode in rootNode.Flatten())
            {
                if (!ShouldRenderNode(navNode)) continue;

                var url = ResolveUrl(navNode, urlHelper);

                if (string.IsNullOrWhiteSpace(url)) continue;

                if (addedUrls.Contains(url)) continue;

                

                result.Add(new PreCacheItem()
                {
                    Url = url,
                    LastModifiedUtc = navNode.LastModifiedUtc
                });

            }


            return result;
        }


        private bool ShouldRenderNode(NavigationNode node)
        {
            if (node.Controller == "Account") return false;

            TreeNode<NavigationNode> treeNode = new TreeNode<NavigationNode>(node);
            foreach (var permission in _permissionResolvers)
            {
                bool ok = permission.ShouldAllowView(treeNode);
                if (!ok) return false;
            }

            return true;
        }

        private string ResolveUrl(NavigationNode node, IUrlHelper urlHelper)
        {
            if (node.HideFromAnonymous) return string.Empty;

            // if url is already fully resolved just return it
            if (node.Url.StartsWith("http")) return node.Url;

            string urlToUse = string.Empty;
            if ((node.Action.Length > 0) && (node.Controller.Length > 0))
            {
                var a = node.Area == null ? "" : node.Area;
                urlToUse = urlHelper.Action(node.Action, node.Controller, new { area = a });
            }
            else if (node.NamedRoute.Length > 0)
            {
                urlToUse = urlHelper.RouteUrl(node.NamedRoute);
            }

            if (string.IsNullOrEmpty(urlToUse)) urlToUse = node.Url;

            if (urlToUse.StartsWith("http")) return urlToUse;

            return urlToUse;
        }


    }
}
