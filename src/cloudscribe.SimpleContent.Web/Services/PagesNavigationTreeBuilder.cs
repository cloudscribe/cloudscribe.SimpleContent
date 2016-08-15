// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2016-08-15
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
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
            IPageRouteHelper pageRouteHelper,
            IActionContextAccessor actionContextAccesor
            )
        {
            this.projectService = projectService;
            this.pageService = pageService;
            this.prefixProvider = prefixProvider;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccesor = actionContextAccesor;
            this.pageRouteHelper = pageRouteHelper;
        }

        private IProjectService projectService;
        private IPageService pageService;
        private INodeUrlPrefixProvider prefixProvider;
        private IUrlHelperFactory urlHelperFactory;
        private IPageRouteHelper pageRouteHelper;
        private IActionContextAccessor actionContextAccesor;
        private TreeNode<NavigationNode> rootNode = null;

        public string Name
        {
            get { return "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder"; }
        }

        public async Task<TreeNode<NavigationNode>> BuildTree(NavigationTreeBuilderService service)
        {

            if (rootNode == null)
            {
                rootNode = await BuildTreeInternal(service);
            }

            return rootNode;
        }

        private async Task<TreeNode<NavigationNode>> BuildTreeInternal(NavigationTreeBuilderService service)
        {
            NavigationNode rootNav;

            var project = await projectService.GetCurrentProjectSettings();

            Page homePage = null;

            if(
                project != null 
                && project.UseDefaultPageAsRootNode
                && !string.IsNullOrEmpty(project.DefaultPageSlug)
                )
            {
                //make the home page the "root" which contains all the other pages
                homePage = await pageService.GetPageBySlug(project.ProjectId, project.DefaultPageSlug);

            }

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var folderPrefix = prefixProvider.GetPrefix();
            if (homePage != null)
            {
                rootNav = new NavigationNode();
                rootNav.IsRootNode = true;
                rootNav.Key = homePage.Id;
                rootNav.Text = homePage.Title;
                rootNav.Url = pageRouteHelper.ResolveHomeUrl(urlHelper, folderPrefix); // urlHelper.Content("~/" + folderPrefix);
            }
            else
            {
                rootNav = new NavigationNode();
                rootNav.IsRootNode = true;
                rootNav.Key = "pagesRoot";
                rootNav.Title = "Home";
                rootNav.Text = "Home";
                rootNav.Url = pageRouteHelper.ResolveHomeUrl(urlHelper, folderPrefix);  // rootNav.Url = urlHelper.Content("~/" + folderPrefix);
                //rootNav.ChildContainerOnly = true;
            }

            

            var treeRoot = new TreeNode<NavigationNode>(rootNav);

            var rootList = await pageService.GetRootPages().ConfigureAwait(false);

            if(rootList.Count() <= 1)
            {   // if there are no pages we won't hit the loop below so go ahead and add the blog page
                if (project.AddBlogToPagesTree)
                {
                    var node = new NavigationNode();
                    node.Key = project.BlogPageText;
                    node.ParentKey = "RootNode";
                    node.Text = project.BlogPageText;
                    if(project.BlogMenuLinksToNewestPost)
                    {
                        node.Url = urlHelper.Action("MostRecent", "Blog");
                    }
                    else
                    {
                        node.Url = urlHelper.Action("Index", "Blog");
                    }
                    
                    node.ComponentVisibility = project.BlogPageNavComponentVisibility;
                    var blogNode = treeRoot.AddChild(node);

                }
            }
            

            //rootList.Insert()
            var rootPosition = 1;
            foreach (var page in rootList)
            {
                var node = new NavigationNode();
                if (project.AddBlogToPagesTree && rootPosition == project.BlogPagePosition)
                {
                    node.Key = project.BlogPageText;
                    node.ParentKey = "RootNode";
                    node.Text = project.BlogPageText;
                    if (project.BlogMenuLinksToNewestPost)
                    {
                        node.Url = urlHelper.Action("MostRecent", "Blog");
                    }
                    else
                    {
                        node.Url = urlHelper.Action("Index", "Blog");
                    }
                    node.ComponentVisibility = project.BlogPageNavComponentVisibility;
                    var blogNode = treeRoot.AddChild(node);

                    node = new NavigationNode(); // new it up again for use below
                }

                if (homePage != null && homePage.Id == page.Id) { rootPosition += 1; continue; }
                
                node.Key = page.Id;
                node.ParentKey = page.ParentId;
                node.Text = page.Title;
                node.ViewRoles = page.ViewRoles;
                if(string.IsNullOrEmpty(folderPrefix))
                {
                    node.Url = urlHelper.RouteUrl(pageRouteHelper.PageIndexRouteName, new { slug = page.Slug });
                }
                else
                {
                    node.Url = urlHelper.RouteUrl(pageRouteHelper.FolderPageIndexRouteName, new { slug = page.Slug });
                }
                
                // for unpublished pages PagesNavigationNodePermissionResolver
                // will look for projectid in CustomData and if it exists
                // filter node from view unless user has edit permissions
                if (!page.IsPublished) { node.CustomData = project.ProjectId; }

                var treeNode = treeRoot.AddChild(node);
                await AddChildNodes(treeNode, project, folderPrefix).ConfigureAwait(false);
                rootPosition += 1;
            }

            return treeRoot;
        }

        private async Task AddChildNodes(
            TreeNode<NavigationNode> treeNode,
            ProjectSettings project,
            string folderPrefix
            )
        {
            var childList = await pageService.GetChildPages(treeNode.Value.Key).ConfigureAwait(false);
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            foreach (var page in childList)
            {
                var node = new NavigationNode();
                node.Key = page.Id;
                node.ParentKey = page.ParentId;
                node.Text = page.Title;
                node.ViewRoles = page.ViewRoles;

                if (string.IsNullOrEmpty(folderPrefix))
                {
                    node.Url = urlHelper.RouteUrl(pageRouteHelper.PageIndexRouteName, new { slug = page.Slug });
                }
                else
                {
                    node.Url = urlHelper.RouteUrl(pageRouteHelper.FolderPageIndexRouteName, new { slug = page.Slug });
                }

                // for unpublished pages PagesNavigationNodePermissionResolver
                // will look for projectid in CustomData and if it exists
                // filter node from view unless user has edit permissions
                if (!page.IsPublished) { node.CustomData = project.ProjectId; }

                var childNode = treeNode.AddChild(node);
                await AddChildNodes(childNode, project, folderPrefix).ConfigureAwait(false); //recurse
            }
        }

    }
}
