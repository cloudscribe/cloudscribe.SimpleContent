

using cloudscribe.Web.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.SimpleContent.Web.Services
{
    /// <summary>
    /// individual blog posts are not menu items so no currentNode is found to highlight in the menu
    /// by plugging in this custom implementation of IFindCurrentNode we can find the blog index or mostrecentpost node
    /// and highlight it
    /// </summary>
    public class NavigationBlogNodeFinder : IFindCurrentNode
    {
        public TreeNode<NavigationNode> FindNode(
            TreeNode<NavigationNode> rootNode,
            IUrlHelper urlHelper,
            string currentUrl,
            string urlPrefix = "")
        {
            if (string.IsNullOrEmpty(currentUrl)) return null;
            if (rootNode == null) return null;

            var blogUrl = (string.IsNullOrEmpty(urlPrefix)) ? "/blog" : "/" + urlPrefix + "/blog";

            Func<TreeNode<NavigationNode>, bool> match = delegate (TreeNode<NavigationNode> n)
            {
                if (n == null) { return false; }

                if (currentUrl.StartsWith(blogUrl))
                {
                    if (n.Value.Controller == "Blog") return true;
                }

                return false;
            };


            return rootNode.Find(match);
        }
    }
}
