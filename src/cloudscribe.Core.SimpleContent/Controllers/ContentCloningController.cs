// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-07
// Last Modified:			2019-03-04
//

using cloudscribe.Core.Models;
using cloudscribe.Core.SimpleContent.Integration.ViewModels;
using cloudscribe.Core.Web.Components;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration.Mvc.Controllers
{

    public class ContentCloningController : Controller
    {
        private readonly IProjectService    _projectService;
        private readonly SiteManager        _siteManager;
        private readonly ISiteQueries       _siteQueries;
        private readonly IProjectCommands   _projectCommands;
        private readonly IPageQueries       _pageQueries;
        private readonly IPageCommands      _pageCommands;
        private readonly IPostQueries       _postQueries;
        private readonly IPostCommands      _postCommands;
        private readonly IConfiguration     _configuration;
        private readonly IStringLocalizer   sr;

        public ContentCloningController(
            IProjectService projectService,
            SiteManager siteManager,
            ISiteQueries siteQueries,
            IProjectCommands projectCommands,
            IPageQueries pageQueries,
            IPageCommands pageCommands,
            IPostQueries postQueries,
            IPostCommands postCommands,
            IConfiguration configuration,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer
            )
        {
            _projectService = projectService;
            _siteManager = siteManager;
            _siteQueries = siteQueries;
            _projectCommands = projectCommands;
            _pageQueries = pageQueries;
            _pageCommands = pageCommands;
            _postQueries = postQueries;
            _postCommands = postCommands;
            _configuration = configuration;
            sr = localizer;
        }

        [Authorize(Policy = "AdminPolicy")]
        // GET: /ContentCloning/index[?siteId=]
        [HttpGet]
        public async Task<IActionResult> Index(string siteId = null)
        {
            ViewData["Title"] = sr["Content Cloning"];

            var model = new ContentCloningViewModel() { SiteId = siteId };

            model = await PopulateAndValidateModel(model); //add the list of sites to the model and do validation

            return View(model);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> Index(ContentCloningViewModel model)
        {
            ViewData["Title"] = sr["Content Cloning"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model = await PopulateAndValidateModel(model); //add the list of sites to the model and do validation

            if (!model.CloneAllowed)
            {
                return View(model);
            }

            if(string.IsNullOrWhiteSpace(model.Command) || model.Command != "clone")
            {
                return View(model);
            }

            // at this point we're ready to clone the ProjectSettings, Pages and Posts

            //Project aka Site Content Settings
            if(model.CloneContentSettings)
            {
                try
                {
                    string projectId = await _projectCommands.CloneToNewProject(
                        model.CloneFromSiteId,
                        model.CloneToSiteId,
                        model.CloneToSiteName
                    );
                    this.AlertSuccess(sr["Content Settings cloning was successful!"], true);
                }
                catch(Exception ex)
                {
                    this.AlertDanger(sr["Failed to clone the Content Settings!"], true);
                    this.AlertDanger(ex.Message, true);
                    return View(model);
                }
            }

            //Clone the Pages
            int pageCount = 0;
            if(model.ClonePages)
            {
                try
                {
                    List<IPage> pages = await _pageQueries.GetAllPages(model.CloneFromSiteId);
                    foreach(var page in pages)
                    {
                        string pageId = await _pageCommands.CloneToNewProject(
                            model.CloneFromSiteId,
                            model.CloneToSiteId,
                            page.Id
                        );
                        if(!string.IsNullOrWhiteSpace(pageId)) pageCount++;
                    }
                }
                catch (Exception ex)
                {
                    this.AlertDanger(string.Format(sr["An error occurred while cloning content pages. Only {0}/{1} were copied."], pageCount, model.CloneFromPageCount), true);
                    this.AlertDanger(ex.Message, true);
                }
                if(pageCount == model.CloneFromPageCount)
                {
                    this.AlertSuccess(sr["Content pages cloning was successful!"], true);
                }
            }

            //Clone the Posts
            int postCount = 0;
            if (model.CloneBlogPosts)
            {
                try
                {
                    List<IPost> posts = await _postQueries.GetPosts(model.CloneFromSiteId, true);
                    foreach (var post in posts)
                    {
                        string postId = await _postCommands.CloneToNewProject(
                            model.CloneFromSiteId,
                            model.CloneToSiteId,
                            post.Id
                        );
                        if (!string.IsNullOrWhiteSpace(postId)) postCount++;
                    }
                }
                catch (Exception ex)
                {
                    this.AlertDanger(string.Format(sr["An error occurred while cloning blog posts. Only {0}/{1} were copied."],postCount, model.CloneFromPostCount), true);
                    this.AlertDanger(ex.Message, true);
                }
                if(postCount == model.CloneFromPostCount)
                {
                    this.AlertSuccess(sr["Blog post cloning was successful!"], true);
                }
            }

            if(model.RewriteContentUrls)
            {
                this.AlertInformation(sr["Rewriting content urls is not yet implemented."], true);
            }

            return View(model);
        }

        private async Task<ContentCloningViewModel> PopulateAndValidateModel(ContentCloningViewModel model)
        {

            bool isServerAdminSite = _siteManager.CurrentSite.IsServerAdminSite;
            string currentSiteId = _siteManager.CurrentSite.Id.ToString();

            if(isServerAdminSite)
            {
                //if we are on a specific site settings page then we can preselect the site to clone to
                if(!string.IsNullOrWhiteSpace(model.SiteId))
                {
                    model.CloneToSiteId = model.SiteId;
                    model.AllowCloneToSiteSelection = false;
                }
            }
            else
            {   //can't allow destination site selection if we are not on the server admin site
                model.SiteId = currentSiteId;
                model.CloneToSiteId = currentSiteId;
                model.AllowCloneToSiteSelection = false;
            }

            bool useFolderNames = _configuration.GetSection("MultiTenantOptions").GetValue<string>("Mode") == "FolderName";

            List<ISiteInfo> sites = await _siteQueries.GetList();

            //build the sites list for To and From, excluding any site selected in the other list
            foreach(var site in sites)
            {
                string url = string.Empty;
                if(useFolderNames) url = "/" + site.SiteFolderName;
                else url = site.PreferredHostName;

                bool addTo = true;
                bool addFrom = true;

                if(!string.IsNullOrWhiteSpace(model.CloneToSiteId) &&
                    model.CloneToSiteId == site.Id.ToString())
                {
                    addFrom= false;
                    model.CloneToSiteName = site.SiteName;
                    model.CloneToPageCount = await _pageQueries.GetCount(model.CloneToSiteId, true);
                    model.CloneToPostCount = await _postQueries.GetCount(model.CloneToSiteId, null, true);
                }

                if (!string.IsNullOrWhiteSpace(model.CloneFromSiteId) &&
                    model.CloneFromSiteId == site.Id.ToString())
                {
                    addTo = false;
                    model.CloneFromSiteName = site.SiteName;
                    model.CloneFromPageCount = await _pageQueries.GetCount(model.CloneFromSiteId, true);
                    model.CloneFromPostCount = await _postQueries.GetCount(model.CloneFromSiteId, null, true);
                }

                if (addTo)
                {
                    model.CloneToSites.Add(new ContentCloningViewModel.SiteDetails
                    {
                        SiteId = site.Id.ToString(),
                        SiteIdentifier = site.SiteName + " (" + url + ") [ " + site.Id + " ]"
                    });
                }

                if(addFrom)
                {
                    model.CloneFromSites.Add(new ContentCloningViewModel.SiteDetails
                    {
                        SiteId = site.Id.ToString(),
                        SiteIdentifier = site.SiteName + " (" + url + ") [ " + site.Id + " ]"
                    });
                }
            }

            model.CloneAllowed = true;

            if(string.IsNullOrWhiteSpace(model.CloneToSiteId))
            {
                model.CloneAllowed = false;
                this.AlertDanger(sr["Please select a destination site!"], true);
            }
            else
            {
                if (model.ClonePages && model.CloneToPageCount > 0)
                {
                    model.CloneAllowed = false;
                    if(model.AllowCloneToSiteSelection)
                    {
                        this.AlertWarning(
                            string.Format(sr["The destination site you have chosen already contains {0} content pages!"], model.CloneToPageCount),
                            true);
                    }
                    else
                    {
                        this.AlertWarning(
                            string.Format(sr["This site already contains {0} content pages!"], model.CloneToPageCount),
                            true);
                    }
                }

                if (model.CloneBlogPosts && model.CloneToPostCount > 0)
                {
                    model.CloneAllowed = false;
                    if (model.AllowCloneToSiteSelection)
                    {
                        this.AlertWarning(
                            string.Format(sr["The destination site you have chosen already contains {0} blog posts!"], model.CloneToPostCount),
                            true);
                    }
                    else
                    {
                        this.AlertWarning(
                            string.Format(sr["This site already contains {0} blog posts!"], model.CloneToPostCount),
                            true);
                    }
                }
            }

            if(model.CloneAllowed)
            {
                if(string.IsNullOrWhiteSpace(model.CloneFromSiteId))
                {
                    model.CloneAllowed = false;
                    this.AlertDanger(sr["Please select a source site!"], true);
                }
                else
                {
                    if (model.CloneFromPageCount == 0 && model.CloneFromPostCount == 0)
                    {
                        model.CloneAllowed = false;
                        this.AlertWarning(
                            sr["The source site you have chosen does not contain any content pages or blog posts!"],
                            true);
                    }
                    else
                    {
                        this.AlertInformation(
                            string.Format(sr["The source site you have chosen contains {0} content pages and {1} blog posts."], model.CloneFromPageCount, model.CloneFromPostCount),
                            true);
                    }
                }
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(model.CloneToSiteId))
                    this.AlertDanger(sr["Content Cloning is not possible for this site."], true);
            }

            return model;
        }
    }

}
