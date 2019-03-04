// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2019-03-04
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class PagesNavigationTreeBuilder : INavigationTreeBuilder
    {
        public PagesNavigationTreeBuilder(
            IProjectService projectService,
            IPageService pageService,
            INodeUrlPrefixProvider prefixProvider,
            IUrlHelperFactory urlHelperFactory,
            IPageRoutes pageRoutes,
            IBlogRoutes blogRoutes,
            IActionContextAccessor actionContextAccesor
            )
        {
            _projectService = projectService;
            _pageService = pageService;
            _prefixProvider = prefixProvider;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _pageRoutes = pageRoutes;
            _blogRoutes = blogRoutes;
        }

        private IProjectService _projectService;
        private IPageService _pageService;
        private INodeUrlPrefixProvider _prefixProvider;
        private IUrlHelperFactory _urlHelperFactory;
        private IPageRoutes _pageRoutes;
        private IBlogRoutes _blogRoutes;
        private IActionContextAccessor _actionContextAccesor;
        private TreeNode<NavigationNode> _rootNode = null;

        public string Name
        {
            get { return "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder"; }
        }

        public async Task<TreeNode<NavigationNode>> BuildTree(NavigationTreeBuilderService service)
        {

            if (_rootNode == null)
            {
                _rootNode = await BuildTreeInternal(service);
            }

            return _rootNode;
        }

        private async Task<TreeNode<NavigationNode>> BuildTreeInternal(NavigationTreeBuilderService service)
        {
            NavigationNode rootNav;

            var project = await _projectService.GetCurrentProjectSettings();

            IPage homePage = null;

            if(
                project != null 
                && project.UseDefaultPageAsRootNode
                && !string.IsNullOrEmpty(project.DefaultPageSlug)
                )
            {
                //make the home page the "root" which contains all the other pages
                homePage = await _pageService.GetPageBySlug(project.DefaultPageSlug);

            }

            var rootList = await _pageService.GetRootPages().ConfigureAwait(false);
            var rootListCount = rootList.Count();

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            //var folderPrefix = _prefixProvider.GetPrefix();
            if ((homePage != null) && project.UseDefaultPageAsRootNode)
            {
                rootNav = new NavigationNode
                {
                    Key = homePage.Id,
                    Text = homePage.Title,
                    LastModifiedUtc = homePage.LastModified,
                    Url = urlHelper.RouteUrl(_pageRoutes.PageRouteName, new { slug = "" }) //_pageRouteHelper.ResolveHomeUrl(urlHelper, folderPrefix) // urlHelper.Content("~/" + folderPrefix);
                };
            }
            else
            {
                rootNav = new NavigationNode
                {
                    Key = "pagesRoot",
                    Title = "Home",
                    Text = "Home",
                    Url = urlHelper.RouteUrl(_pageRoutes.PageRouteName, new { slug = "" })
                };
                
            }
            
            var treeRoot = new TreeNode<NavigationNode>(rootNav);

            
            var blogPosition = project.BlogPagePosition;
            if (project.AddBlogToPagesTree)
            {
                if (blogPosition > rootListCount) blogPosition = rootListCount;
            }

            var didAddBlog = false;
            if (rootListCount <= 1)
            {   // if there are no pages we won't hit the loop below so go ahead and add the blog page
                if (project.AddBlogToPagesTree)
                {
                    var node = new NavigationNode
                    {
                        Key = project.BlogPageText,
                        Text = project.BlogPageText
                    };
                    if (project.BlogMenuLinksToNewestPost)
                    {
                        node.NamedRoute = _blogRoutes.MostRecentPostRouteName;
                        node.Url = urlHelper.RouteUrl(_blogRoutes.MostRecentPostRouteName);
                    }
                    else
                    {
                        node.NamedRoute = _blogRoutes.BlogIndexRouteName;
                        node.Url = urlHelper.RouteUrl(_blogRoutes.BlogIndexRouteName);
                        node.ExcludeFromSearchSiteMap = true;
                    }
                    
                    node.ComponentVisibility = project.BlogPageNavComponentVisibility;
                    var blogNode = treeRoot.AddChild(node);
                    didAddBlog = true;
                }
            }

            var rootPosition = 1;
            foreach (var page in rootList)
            {
                var node = new NavigationNode();
                if (!didAddBlog && (project.AddBlogToPagesTree && rootPosition == blogPosition))
                {
                    node.Key = project.BlogPageText;
                    //node.ParentKey = "RootNode";
                    node.Text = project.BlogPageText;
                    if (project.BlogMenuLinksToNewestPost)
                    {
                        node.NamedRoute = _blogRoutes.MostRecentPostRouteName;
                        node.Url = urlHelper.RouteUrl(_blogRoutes.MostRecentPostRouteName);
                    }
                    else
                    {
                        node.NamedRoute = _blogRoutes.BlogIndexRouteName;
                        node.Url = urlHelper.RouteUrl(_blogRoutes.BlogIndexRouteName);
                        node.ExcludeFromSearchSiteMap = true;
                    }
                    node.ComponentVisibility = project.BlogPageNavComponentVisibility;
                    var blogNode = treeRoot.AddChild(node);

                    node = new NavigationNode(); // new it up again for use below
                }

                if (project.UseDefaultPageAsRootNode && (homePage != null && homePage.Id == page.Id))
                {
                    rootPosition += 1;
                    await AddChildNodes(treeRoot, project, urlHelper).ConfigureAwait(false);
                    continue;
                }
                
                node.Key = page.Id;
                //node.ParentKey = page.ParentId;
                node.Text = page.Title;
                node.ViewRoles = page.ViewRoles;
                node.ComponentVisibility = page.MenuFilters;
                node.LastModifiedUtc = page.LastModified;
                SetUrl(node, page, urlHelper);
                
                // for unpublished pages PagesNavigationNodePermissionResolver
                // will look for projectid in CustomData and if it exists
                // filter node from view unless user has edit permissions
                if (!page.HasPublishedVersion())
                {
                    node.CustomData = project.Id;
                }

                var treeNode = treeRoot.AddChild(node);
                await AddChildNodes(treeNode, project, urlHelper).ConfigureAwait(false);
                rootPosition += 1;
            }

            return treeRoot;
        }

        

        private async Task AddChildNodes(
            TreeNode<NavigationNode> treeNode,
            IProjectSettings project,
            IUrlHelper urlHelper
            )
        {
            var childList = await _pageService.GetChildPages(treeNode.Value.Key).ConfigureAwait(false);
            //var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            foreach (var page in childList)
            {
                var node = new NavigationNode
                {
                    Key = page.Id,
                    Text = page.Title,
                    ViewRoles = page.ViewRoles,
                    ComponentVisibility = page.MenuFilters,
                    LastModifiedUtc = page.LastModified
                };
                SetUrl(node, page, urlHelper);

                // for unpublished pages PagesNavigationNodePermissionResolver
                // will look for projectid in CustomData and if it exists
                // filter node from view unless user has edit permissions
                if (!page.HasPublishedVersion())
                {
                    node.CustomData = project.Id;
                }

                var childNode = treeNode.AddChild(node);
                await AddChildNodes(childNode, project, urlHelper).ConfigureAwait(false); //recurse
            }
        }

        private void SetUrl(NavigationNode node, IPage page, IUrlHelper urlHelper)
        {
            if (!string.IsNullOrEmpty(page.ExternalUrl))
            {
                if (page.ExternalUrl.StartsWith("http"))
                {
                    node.ExcludeFromSearchSiteMap = true;
                    node.Target = "_blank"; // the default nav cshtml templates don't use this but could be customized to use it
                }
                else if(!page.ExternalUrl.StartsWith("/"))
                {
                    page.ExternalUrl = "/" + page.ExternalUrl;
                }
                node.Url = urlHelper.Content(page.ExternalUrl);
            }
            else
            {
                node.Url = urlHelper.RouteUrl(_pageRoutes.PageRouteName, new { slug = page.Slug });
            }
        }

    }
}
