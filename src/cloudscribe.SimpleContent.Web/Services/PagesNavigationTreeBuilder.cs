// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-27
// Last Modified:           2016-08-04
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
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
            IActionContextAccessor actionContextAccesor
            )
        {
            this.projectService = projectService;
            this.pageService = pageService;
            this.prefixProvider = prefixProvider;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccesor = actionContextAccesor;
        }

        private IProjectService projectService;
        private IPageService pageService;
        private INodeUrlPrefixProvider prefixProvider;
        private IUrlHelperFactory urlHelperFactory;
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
                //rootNav.Title = homePage.Title;
                rootNav.Text = homePage.Title;

                //rootNav.Url = urlHelper.Action("Index", "Page", new { slug = homePage.Slug });
                rootNav.Url = urlHelper.Content("~/" + folderPrefix);
            }
            else
            {
                rootNav = new NavigationNode();
                rootNav.IsRootNode = true;
                rootNav.Key = "pagesRoot";
                rootNav.Title = "Home";
                rootNav.Text = "Home";
                rootNav.Url = urlHelper.Content("~/" + folderPrefix);
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
                    node.Url = urlHelper.Action("Index", "Blog");
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
                    node.Url = urlHelper.Action("Index", "Blog");
                    node.ComponentVisibility = project.BlogPageNavComponentVisibility;
                    var blogNode = treeRoot.AddChild(node);

                    node = new NavigationNode(); // new it up again for use below
                }

                if (homePage != null && homePage.Id == page.Id) { rootPosition += 1; continue; }
                
                node.Key = page.Id;
                node.ParentKey = page.ParentId;
                node.Text = page.Title;
                if(string.IsNullOrEmpty(folderPrefix))
                {
                    node.Url = urlHelper.RouteUrl(ProjectConstants.PageIndexRouteName, new { slug = page.Slug });
                }
                else
                {
                    //node.Url = urlHelper.Action("Index", "Page", new { slug = page.Slug });
                    node.Url = urlHelper.RouteUrl(ProjectConstants.FolderPageIndexRouteName, new { slug = page.Slug });
                }
                

                if (!page.IsPublished) { node.ViewRoles = project.AllowedEditRoles; }

                var treeNode = treeRoot.AddChild(node);
                await AddChildNodes(treeNode, folderPrefix).ConfigureAwait(false);
                rootPosition += 1;
            }

            return treeRoot;
        }

        private async Task AddChildNodes(
            TreeNode<NavigationNode> treeNode,
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
                //node.Url = urlHelper.Action("Index", "Pages", new { slug = page.Slug });
                if (string.IsNullOrEmpty(folderPrefix))
                {
                    node.Url = urlHelper.RouteUrl(ProjectConstants.PageIndexRouteName, new { slug = page.Slug });
                }
                else
                {
                    //node.Url = urlHelper.Action("Index", "Page", new { slug = page.Slug });
                    node.Url = urlHelper.RouteUrl(ProjectConstants.FolderPageIndexRouteName, new { slug = page.Slug });
                }

                var childNode = treeNode.AddChild(node);
                await AddChildNodes(childNode, folderPrefix).ConfigureAwait(false); //recurse
            }
        }

    }
}
