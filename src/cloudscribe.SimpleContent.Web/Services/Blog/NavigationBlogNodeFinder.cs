using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace cloudscribe.SimpleContent.Web.Services
{
    /// <summary>
    /// individual blog posts are not menu items so no currentNode is found to highlight in the menu
    /// by plugging in this custom implementation of IFindCurrentNode we can find the blog index or mostrecentpost node
    /// and highlight it
    /// </summary>
    public class NavigationBlogNodeFinder : IFindCurrentNode
    {
        public NavigationBlogNodeFinder(
            IBlogRoutes blogRoutes
            )
        {
            _blogRoutes = blogRoutes;
        }

        private IBlogRoutes _blogRoutes;

        public TreeNode<NavigationNode> FindNode(
            TreeNode<NavigationNode> rootNode,
            IUrlHelper urlHelper,
            string currentUrl,
            string urlPrefix = "")
        {
            if (string.IsNullOrEmpty(currentUrl)) return null;
            if (rootNode == null) return null;
            if (rootNode.Value.Controller == "Blog") return rootNode;
            if (rootNode.Value.NamedRoute == _blogRoutes.BlogIndexRouteName) return rootNode;
            if (rootNode.Value.NamedRoute == _blogRoutes.MostRecentPostRouteName) return rootNode;

            var blogUrl = urlHelper.RouteUrl(_blogRoutes.BlogIndexRouteName);
            
            if (rootNode.Value.Url == blogUrl) return rootNode;
            
            Func<TreeNode<NavigationNode>, bool> match = delegate (TreeNode<NavigationNode> n)
            {
                if (n == null) { return false; }
                if(blogUrl == null) { return false; }
                if (currentUrl.StartsWith(blogUrl))
                {
                    if (n.Value.Controller == "Blog") return true;
                    if (n.Value.NamedRoute == _blogRoutes.BlogIndexRouteName) return true;
                    if (n.Value.NamedRoute == _blogRoutes.MostRecentPostRouteName) return true;
                    if (n.Value.Url == blogUrl) return true;
                }

                return false;
            };


            return rootNode.Find(match);
        }
    }
}
