// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-07
// Last Modified:			2019-03-04
//

using cloudscribe.Core.Models;
using cloudscribe.Core.SimpleContent.Integration.ViewModels;
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
        private readonly IProjectService _projectService;
        private readonly ISiteQueries _siteQueries;
        private readonly IPageQueries _pageQueries;
        private readonly IPostQueries _postQueries;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer sr;

        public ContentCloningController(
            IProjectService projectService,
            ISiteQueries siteQueries,
            IPageQueries pageQueries,
            IPostQueries postQueries,
            IConfiguration configuration,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer
            )
        {
            _projectService = projectService;
            _siteQueries = siteQueries;
            _pageQueries = pageQueries;
            _postQueries = postQueries;
            _configuration = configuration;
            sr = localizer;
        }

        [Authorize(Policy = "AdminPolicy")]
        // GET: /ContentSettings
        [HttpGet]
        public async Task<IActionResult> Index(string cloneFromSiteId = null, string cloneToSiteId = null)
        {
            ViewData["Title"] = sr["Content Cloning"];

            //var projects = await projectService.GetCurrentProjects();
            // var projectSettings = await projectService.GetCurrentProjectSettings();
            // var sourceProjectSettings = await projectService.GetProjectSettings(projectSettings.Id.ToString());

            var model = await AddSitesToModel(
                new ContentCloningViewModel()
                {
                    CloneFromSiteId = cloneFromSiteId,
                    CloneToSiteId = cloneToSiteId
                }
            );

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

            model = await AddSitesToModel(model); //add the list of sites to the model

            if(model.CloneAllowed)
            {
                // var pages = await _pageQueries.GetAllPages(model.CloneToSiteId);
                // var posts = await _postQueries.GetPosts(model.CloneToSiteId, true);
                if (model.CloneToPageCount > 0 || model.CloneToPostCount > 0)
                {
                    model.CloneAllowed = false;
                    this.AlertDanger(
                        string.Format(sr["The destination site you have chosen already contains {0} pages and {1} blog posts!"], model.CloneToPageCount, model.CloneToPostCount),
                        true);
                }
            }

            //var projectSettings = await _projectService.GetCurrentProjectSettings();




            return View(model);
        }

        private async Task<ContentCloningViewModel> AddSitesToModel(ContentCloningViewModel model)
        {
            bool useFolderNames = _configuration.GetSection("MultiTenantOptions").GetValue<string>("Mode") == "FolderName";

            List<ISiteInfo> sites = await _siteQueries.GetList();
            foreach(var site in sites)
            {
                string url = string.Empty;
                if(useFolderNames) url = "/" + site.SiteFolderName;
                else url = site.PreferredHostName;

                bool addFrom = true;
                bool addTo = true;
                if(!string.IsNullOrWhiteSpace(model.CloneToSiteId) &&
                    model.CloneToSiteId == site.Id.ToString())
                {
                    addFrom= false;
                    model.CloneToSiteName = site.SiteName + " (" + url + ")";
                    model.CloneToPageCount = await _pageQueries.GetCount(model.CloneToSiteId, true);
                    model.CloneToPostCount = await _postQueries.GetCount(model.CloneToSiteId, null, true);
                }
                if (!string.IsNullOrWhiteSpace(model.CloneFromSiteId) &&
                    model.CloneFromSiteId == site.Id.ToString())
                {
                    addTo = false;
                    model.CloneFromSiteName = site.SiteName + " (" + url + ")";
                    model.CloneFromPageCount = await _pageQueries.GetCount(model.CloneFromSiteId, true);
                    model.CloneFromPostCount = await _postQueries.GetCount(model.CloneFromSiteId, null, true);
                }

                if(addFrom)
                {
                    model.CloneFromSites.Add(new ContentCloningViewModel.SiteDetails
                    {
                        SiteId = site.Id.ToString(),
                        SiteName = site.SiteName + " (" + url + ")"
                    });
                }

                if (addTo)
                {
                    model.CloneToSites.Add(new ContentCloningViewModel.SiteDetails
                    {
                        SiteId = site.Id.ToString(),
                        SiteName = site.SiteName + " (" + url + ")"
                    });
                }

            }

            if(!string.IsNullOrWhiteSpace(model.CloneFromSiteId) &&
                !string.IsNullOrWhiteSpace(model.CloneToSiteId))
            {
                model.CloneAllowed = true;
            }

            return model;
        }
    }

}
