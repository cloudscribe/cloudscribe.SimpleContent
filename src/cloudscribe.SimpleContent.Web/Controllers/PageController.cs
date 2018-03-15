// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2018-03-15
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.ViewModels;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
        public PageController(
            IProjectService projectService,
            IPageService blogService,
            IContentProcessor contentProcessor,
            IPageRoutes pageRoutes,
            IAuthorizationService authorizationService,
            ITimeZoneHelper timeZoneHelper,
            IAuthorNameResolver authorNameResolver,
            IStringLocalizer<SimpleContent> localizer,
            IOptions<PageEditOptions> pageEditOptionsAccessor,
            ILogger<PageController> logger)
        {
            _projectService = projectService;
            _pageService = blogService;
            _contentProcessor = contentProcessor;
            _authorizationService = authorizationService;
            _authorNameResolver = authorNameResolver;
            _timeZoneHelper = timeZoneHelper;
            _pageRoutes = pageRoutes;
            _editOptions = pageEditOptionsAccessor.Value;
            _sr = localizer;
            _log = logger;
        }

        private IProjectService _projectService;
        private IPageService _pageService;
        private IContentProcessor _contentProcessor;
        private IAuthorizationService _authorizationService;
        private IAuthorNameResolver _authorNameResolver;
        private ITimeZoneHelper _timeZoneHelper;
        private ILogger _log;
        private IPageRoutes _pageRoutes;
        private IStringLocalizer<SimpleContent> _sr;
        private PageEditOptions _editOptions;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string slug = "")
        {
            var projectSettings = await _projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                _log.LogError("project settings not found returning 404");
                return NotFound();
            }

            var canEdit = await User.CanEditPages(projectSettings.Id, _authorizationService);

            if(string.IsNullOrEmpty(slug) || slug == "none") { slug = projectSettings.DefaultPageSlug; }

            IPage page = await _pageService.GetPageBySlug(slug);

            var model = new PageViewModel(_contentProcessor)
            {
                CurrentPage = page,
                ProjectSettings = projectSettings,
                CanEdit = canEdit,
                CommentsAreOpen = false,
                TimeZoneHelper = _timeZoneHelper,
                TimeZoneId = projectSettings.TimeZoneId,
                PageTreePath = Url.Action("Tree")
            };
  
            if (canEdit)
            {
                if (model.CurrentPage != null)
                {
                    model.EditPath = Url.RouteUrl(_pageRoutes.PageEditRouteName, new { slug = model.CurrentPage.Slug });

                    if (model.CurrentPage.Slug == projectSettings.DefaultPageSlug)
                    {   
                       // not setting the parent slug if the current page is home page
                       // otherwise it would be awkward to create more root level pages
                        model.NewItemPath = Url.RouteUrl(_pageRoutes.PageEditRouteName, new { slug = "" });
                    }
                    else
                    {
                        // for non home pages if the use clicks the new link
                        // make it use the current page slug as the parent slug for the new item
                        model.NewItemPath = Url.RouteUrl(_pageRoutes.PageEditRouteName, new { slug = "", parentSlug = model.CurrentPage.Slug });

                    }

                }
                else
                {
                    model.NewItemPath = Url.RouteUrl(_pageRoutes.PageEditRouteName, new { slug = "" });
                   
                }

            }

            if (page == null)
            { 
                var rootList = await _pageService.GetRootPages().ConfigureAwait(false);
                // a site starts out with no pages 
                if (canEdit && rootList.Count == 0)
                {
                    page = new Page
                    {
                        ProjectId = projectSettings.Id
                    };
                    if (HttpContext.Request.Path == "/")
                    {
                        ViewData["Title"] = _sr["Home"];
                        page.Title = "No pages found, please click the pencil to create the home page";
                    }
                    else
                    {
                        ViewData["Title"] = _sr["No Pages Found"];
                        page.Title = "No pages found, please click the pencil to create the first page";
                        
                    }
                    
                    model.CurrentPage = page;
                    model.EditPath = Url.RouteUrl(_pageRoutes.PageEditRouteName, new { slug = "home" });
                    
                }
                else
                {
                    
                    if(rootList.Count > 0)
                    {
                        if(slug == projectSettings.DefaultPageSlug)
                        {
                            // slug was empty and no matching page found for default slug
                            // but since there exist root level pages we should
                            // show an index menu. 
                            // esp useful if not using pages as the default route
                            // /p or /docs
                            ViewData["Title"] = _sr["Content Index"];
                            //model.EditorSettings.EditMode = "none";
                            return View("IndexMenu", model);
                        }

                        
                        return NotFound();
                    }
                    else
                    {
                        Response.StatusCode = 404;
                        return View("NoPages", 404);
                    }    
                    
                }
            }
            else // page is not null
            {
                // if the page is protected by view roles return 404 if user is not in an allowed role
                if ((!string.IsNullOrEmpty(page.ViewRoles)))
                {
                    if (!User.IsInRoles(page.ViewRoles))
                    {
                        _log.LogWarning($"page {page.Title} is protected by roles that user is not in so returning 404");
                        return NotFound();
                    }
                }

                if (!canEdit)
                { 
                    if(!page.IsPublished || page.PubDate > DateTime.UtcNow)
                    {
                        _log.LogWarning($"page {page.Title} is unpublished and user is not editor so returning 404");
                        return NotFound();
                    }
                }
                
                
                if(!string.IsNullOrEmpty(page.ExternalUrl))
                {
                    if(canEdit)
                    {
                        _log.LogWarning($"page {page.Title} has override url {page.ExternalUrl}, redirecting to edit since user can edit");
                        return RedirectToRoute(_pageRoutes.PageEditRouteName, new { slug= page.Slug });

                    }
                    else
                    {
                        _log.LogWarning($"page {page.Title} has override url {page.ExternalUrl}, not intended to be viewed so returning 404");
                        return NotFound();
                    }
                }

                ViewData["Title"] = page.Title;
                
            }

            if (page != null && page.MenuOnly)
            {
                return View("ChildMenu", model);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(
            string slug = "",
            string parentSlug = "",
            string type =""
            )
        {
            var projectSettings = await _projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                _log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(projectSettings.Id, _authorizationService);
            if(!canEdit)
            {
                _log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            if (slug == "none") { slug = string.Empty; }

            var model = new PageEditViewModel
            {
                ProjectId = projectSettings.Id,
                DisqusShortname = projectSettings.DisqusShortName
            };

            IPage page = null;
            if (!string.IsNullOrEmpty(slug))
            {
                page = await _pageService.GetPageBySlug(slug);
            }
            if(page == null)
            {
                ViewData["Title"] = _sr["New Page"];
                model.Slug = slug;
                model.ParentSlug = parentSlug;
                model.PageOrder = await _pageService.GetNextChildPageOrder(parentSlug);
                model.ContentType = projectSettings.DefaultContentType;
                if (_editOptions.AllowMarkdown && !string.IsNullOrWhiteSpace(type) && type == "markdown")
                {
                    model.ContentType = "markdown";
                }
                if (!string.IsNullOrWhiteSpace(type) && type == "html")
                {
                    model.ContentType = "html";
                }

                var rootList = await _pageService.GetRootPages().ConfigureAwait(false);
                if(rootList.Count == 0)
                {
                    var rootPagePath = Url.RouteUrl(_pageRoutes.PageRouteName);
                    // expected if home page doesn't exist yet
                    if(slug == "home" && rootPagePath == "/home")
                    {
                        model.Title = _sr["Home"];
                    }

                }
                model.Author = await _authorNameResolver.GetAuthorName(User);
                model.PubDate = _timeZoneHelper.ConvertToLocalTime(DateTime.UtcNow, projectSettings.TimeZoneId).ToString();

            }
            else // page not null
            {
                // if the page is protected by view roles return 404 if user is not in an allowed role
                if ((!string.IsNullOrEmpty(page.ViewRoles)))
                {
                    if (!User.IsInRoles(page.ViewRoles))
                    {
                        _log.LogWarning($"page {page.Title} is protected by roles that user is not in so returning 404");
                        return NotFound();
                    }
                }

                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["Edit - {0}"], page.Title);
                model.Author = page.Author;
                model.Content = page.Content;
                model.Id = page.Id;
                model.CorrelationKey = page.CorrelationKey;
                model.IsPublished = page.IsPublished;
                model.ShowMenu = page.ShowMenu;
                model.MenuOnly = page.MenuOnly;
                model.MetaDescription = page.MetaDescription;
                model.PageOrder = page.PageOrder;
                model.ParentId = page.ParentId;
                model.ParentSlug = page.ParentSlug;
                model.PubDate = _timeZoneHelper.ConvertToLocalTime(page.PubDate, projectSettings.TimeZoneId).ToString();
                model.ShowHeading = page.ShowHeading;
                model.Slug = page.Slug;
                model.ExternalUrl = page.ExternalUrl;
                model.Title = page.Title;
                model.MenuFilters = page.MenuFilters;
                model.ViewRoles = page.ViewRoles;
                model.ShowComments = page.ShowComments;
                model.DisableEditor = page.DisableEditor;
                model.ContentType = page.ContentType;

            }

            
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if(string.IsNullOrEmpty(model.Id))
                {
                    ViewData["Title"] = _sr["New Page"];
                }
                else
                {
                    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["Edit - {0}"], model.Title);
                }
                return View(model);
            }
            
            var project = await _projectService.GetCurrentProjectSettings();
            
            if (project == null)
            {
                _log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                page = await _pageService.GetPage(model.Id);
            }

            var needToClearCache = false;
            var isNew = false;
            string slug = string.Empty; ;
            bool slugIsAvailable = false;
            if (page != null)
            {
                if ((!string.IsNullOrEmpty(page.ViewRoles)))
                {
                    if (!User.IsInRoles(page.ViewRoles))
                    {
                        _log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                        return RedirectToRoute(_pageRoutes.PageRouteName);
                    }
                }

                if (page.Title != model.Title)
                {
                    needToClearCache = true;
                }
                page.Title = model.Title;
                page.MetaDescription = model.MetaDescription;
                page.Content = model.Content;
                if (page.IsPublished != model.IsPublished) needToClearCache = true;
                if (page.PageOrder != model.PageOrder) needToClearCache = true;
                if(!string.IsNullOrEmpty(model.Slug))
                {
                    // remove any bad characters
                    model.Slug = ContentUtils.CreateSlug(model.Slug);
                    if(model.Slug != page.Slug)
                    {
                        slugIsAvailable = await _pageService.SlugIsAvailable(model.Slug);
                        if(slugIsAvailable)
                        {
                            page.Slug = model.Slug;
                            needToClearCache = true;
                        }
                        else
                        {
                            this.AlertDanger(_sr["The page slug was not changed because the requested slug is already in use."], true);

                        }
                    }

                }

            }
            else
            {
                isNew = true;
                needToClearCache = true;
                if(!string.IsNullOrEmpty(model.Slug))
                {
                    // remove any bad chars
                    model.Slug = ContentUtils.CreateSlug(model.Slug);
                    slugIsAvailable = await _pageService.SlugIsAvailable(model.Slug);
                    if(slugIsAvailable)
                    {
                        slug = model.Slug;
                    }
                }

                if(string.IsNullOrEmpty(slug))
                {
                    slug = ContentUtils.CreateSlug(model.Title);
                }

                slugIsAvailable = await _pageService.SlugIsAvailable(slug);
                if (!slugIsAvailable)
                {
                    model.DisqusShortname = project.DisqusShortName;
                    //log.LogInformation("returning 409 because slug already in use");
                    ModelState.AddModelError("pageediterror", _sr["slug is already in use."]);

                    return View(model);
                }

                page = new Page()
                {
                    ProjectId = project.Id,
                    Author = await _authorNameResolver.GetAuthorName(User),
                    Title = model.Title,
                    MetaDescription = model.MetaDescription,
                    Content = model.Content,
                    Slug = slug,
                    ParentId = "0"

                    //,Categories = categories.ToList()
                };
            }


            if (!string.IsNullOrEmpty(model.ParentSlug))
            {
                var parentPage = await _pageService.GetPageBySlug(model.ParentSlug);
                if (parentPage != null)
                {
                    if (parentPage.Id != page.ParentId)
                    {
                        page.ParentId = parentPage.Id;
                        page.ParentSlug = parentPage.Slug;
                        needToClearCache = true;
                    }

                }
            }
            else
            {
                // empty means root level
                page.ParentSlug = string.Empty;
                page.ParentId = "0";
            }
            if (page.ViewRoles != model.ViewRoles)
            {
                needToClearCache = true;
            }
            page.ViewRoles = model.ViewRoles;
            page.CorrelationKey = model.CorrelationKey;

            page.PageOrder = model.PageOrder;
            page.IsPublished = model.IsPublished;
            page.ShowHeading = model.ShowHeading;
            page.ShowMenu = model.ShowMenu;
            page.MenuOnly = model.MenuOnly;
            page.DisableEditor = model.DisableEditor;
            page.ShowComments = model.ShowComments;
            if (page.MenuFilters != model.MenuFilters)
            {
                needToClearCache = true;
            }
            page.MenuFilters = model.MenuFilters;
            if (page.ExternalUrl != model.ExternalUrl)
            {
                needToClearCache = true;
            }
            page.ExternalUrl = model.ExternalUrl;
            page.ContentType = model.ContentType;

            if(!string.IsNullOrEmpty(model.Author))
            {
                page.Author = model.Author;
            }

            if (!string.IsNullOrEmpty(model.PubDate))
            {
                var localTime = DateTime.Parse(model.PubDate);
                page.PubDate = _timeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);

            }
            if(page.ProjectId != project.Id)
            {
                page.ProjectId = project.Id;
            }

            if (isNew)
            {
                await _pageService.Create(page, model.IsPublished);
                this.AlertSuccess(_sr["The page was created successfully."], true);
            }
            else
            {
                await _pageService.Update(page, model.IsPublished);
                this.AlertSuccess(_sr["The page was updated successfully."], true);
            }


            if (needToClearCache)
            {
                _pageService.ClearNavigationCache();
            }

            

            if (page.Slug == project.DefaultPageSlug)
            {
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug="" });
            }

            if(!string.IsNullOrEmpty(page.ExternalUrl))
            {
                this.AlertWarning(_sr["Note that since this page has an override url, the menu item will link to the url so the page is used only as a means to add a link in the menu, the content is not used."], true);
                return RedirectToRoute(_pageRoutes.PageEditRouteName, new { slug = page.Slug });
            }

            //var url = Url.RouteUrl(pageRoutes.PageRouteName, new { slug = page.Slug });
            return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = page.Slug });
            //return Content(url);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Development(string slug)
        {
            var projectSettings = await _projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                _log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(projectSettings.Id, _authorizationService);
            if (!canEdit)
            {
                _log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            var canDev = _editOptions.AlwaysShowDeveloperLink ? true : User.IsInRole(_editOptions.DeveloperAllowedRole);

            if (!canDev)
            {
                _log.LogInformation("redirecting to index because user is not allowed by edit config for developer tools");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(slug))
            {
                page = await _pageService.GetPageBySlug(slug);
            }
            if (page == null)
            {
                _log.LogInformation("page not found, redirecting");
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    _log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(_pageRoutes.PageRouteName);
                }
            }

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["Developer Tools - {0}"], page.Title);

            var model = new PageDevelopmentViewModel
            {
                Slug = page.Slug
            };
            model.AddResourceViewModel.Slug = page.Slug;
            model.Css = page.Resources.Where(x => x.Type == "css").OrderBy(x => x.Sort).ThenBy(x => x.Url).ToList<IPageResource>();
            model.Js = page.Resources.Where(x => x.Type == "js").OrderBy(x => x.Sort).ThenBy(x => x.Url).ToList<IPageResource>();
            

            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResource(AddPageResourceViewModel model)
        {
            if(!ModelState.IsValid)
            {
                this.AlertDanger(_sr["Invalid request"], true);
                return RedirectToRoute(_pageRoutes.PageDevelopRouteName, new { slug = model.Slug });
            }

            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);
            if (!canEdit)
            {
                _log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }
            var canDev = _editOptions.AlwaysShowDeveloperLink ? true : User.IsInRole(_editOptions.DeveloperAllowedRole);
            if (!canDev)
            {
                _log.LogInformation("redirecting to index because user is not allowed by edit config for developer tools");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(model.Slug))
            {
                page = await _pageService.GetPageBySlug(model.Slug);
            }

            if (page == null)
            {
                _log.LogInformation("page not found, redirecting");
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    _log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(_pageRoutes.PageRouteName);
                }
            }

            var resource = new PageResource
            {
                ContentId = page.Id,
                Type = model.Type,
                Environment = model.Environment,
                Sort = model.Sort,
                Url = model.Url
            };
            page.Resources.Add(resource);
            


            await _pageService.Update(page, page.IsPublished);

            return RedirectToRoute(_pageRoutes.PageDevelopRouteName, new { slug = page.Slug });

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveResource(string slug, string id)
        {
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);
            if (!canEdit)
            {
                _log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }
            var canDev = _editOptions.AlwaysShowDeveloperLink ? true : User.IsInRole(_editOptions.DeveloperAllowedRole);
            if (!canDev)
            {
                _log.LogInformation("redirecting to index because user is not allowed by edit config for developer tools");
                return RedirectToRoute(_pageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(slug))
            {
                page = await _pageService.GetPageBySlug(slug);
            }

            if (page == null)
            {
                _log.LogInformation("page not found, redirecting");
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    _log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(_pageRoutes.PageRouteName);
                }
            }

            var found = false;
            var copyOfResources = page.Resources.ToList();
            for (var i = 0; i < copyOfResources.Count; i++)
            {
                if(copyOfResources[i].Id == id)
                {
                    found = true;
                    copyOfResources.RemoveAt(i);
                }
            }
            if(found)
            {
                // needed so the ef entity shadow copy of resources gets synced
                page.Resources = copyOfResources;
                await _pageService.Update(page, page.IsPublished);
            }
            else
            {
                _log.LogWarning($"page resource not found for {slug} and {id}");
            }
            

            return RedirectToRoute(_pageRoutes.PageDevelopRouteName, new { slug = page.Slug });

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                
                _log.LogInformation("project not found, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug="" });
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("user is not allowed to edit, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            if (string.IsNullOrEmpty(id))
            {
                _log.LogInformation("postid not provided, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });

            }

            var page = await _pageService.GetPage(id);

            if (page == null)
            {
                _log.LogInformation($"page not found for {id}, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    _log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(_pageRoutes.PageRouteName);
                }
            }

            if (page.Slug == project.DefaultPageSlug) // don't allow delete the home/default page from the ui
            {
                _log.LogWarning($"Rejecting/redirecting, user {User.Identity.Name} tried to delete the default page {page.Slug}");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            

            await _pageService.DeletePage(page.Id);
            _pageService.ClearNavigationCache();

            _log.LogWarning($"user {User.Identity.Name} deleted page {page.Slug}");

            if (Request.IsAjaxRequest())
            {
                return Json(new PageActionResult(true,"success"));
            }

            return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });

        }

        [HttpGet]
        public IActionResult SiteMap()
        {
            return View("SiteMapIndex");
        }

        [HttpGet]
  
        public async Task<IActionResult> Tree()
        {
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("project not found, redirecting");
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("user is not allowed to edit, redirecting");
                return RedirectToRoute(_pageRoutes.PageRouteName, new { slug = "" });
            }

            ViewData["Title"] = _sr["Page Management"];
            var model = new PageTreeViewModel
            {
                TreeServiceUrl = Url.Action("TreeJson"),
                EditUrl = Url.RouteUrl(_pageRoutes.PageEditRouteName),
                ViewUrl = Url.RouteUrl(_pageRoutes.PageRouteName),
                MoveUrl = Url.Action("Move"),
                DeleteUrl = Url.Action("Delete"),
                SortUrl = Url.Action("SortChildPagesAlpha")
            };

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TreeJson(string node = "root")
        {
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("project not found");
                return BadRequest();
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("user is not allowed to edit");
                return BadRequest();
            }

            var result = await _pageService.GetPageTreeJson(User, node);

            return new ContentResult
            {
                ContentType = "application/json",
                Content = result,
                StatusCode = 200
            };

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move(PageMoveModel model)
        {
            if (model == null)
            {
                _log.LogInformation("model was null");
                return BadRequest();
            }

            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("project not found");
                return BadRequest();
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("user is not allowed to edit");
                return BadRequest();
            }


            var result = await _pageService.Move(model);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SortChildPagesAlpha(string pageId)
        {
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("project not found");
                return BadRequest();
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("user is not allowed to edit");
                return BadRequest();
            }


            var result = await _pageService.SortChildPagesAlpha(pageId);

            return Json(result);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AjaxPost(PageEditViewModel model)
        //{
        //    // disable status code page for ajax requests
        //    var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
        //    if (statusCodePagesFeature != null)
        //    {
        //        statusCodePagesFeature.Enabled = false;
        //    }

        //    if (string.IsNullOrEmpty(model.Title))
        //    {
        //        // if a page has been configured to not show the title
        //        // this may be null on edit, if it is a new page then it should be required
        //        // because it is used for generating the slug
        //        //if (string.IsNullOrEmpty(model.Slug))
        //        //{
        //        log.LogInformation("returning 500 because no title was posted");
        //        return StatusCode(500);
        //        //}

        //    }

        //    var project = await projectService.GetCurrentProjectSettings();

        //    if (project == null)
        //    {
        //        log.LogInformation("returning 500 blog not found");
        //        return StatusCode(500);
        //    }

        //    var canEdit = await User.CanEditPages(project.Id, authorizationService);

        //    if (!canEdit)
        //    {
        //        log.LogInformation("returning 403 user is not allowed to edit");
        //        return StatusCode(403);
        //    }

        //    //string[] categories = new string[0];
        //    //if (!string.IsNullOrEmpty(model.Categories))
        //    //{
        //    //    categories = model.Categories.Split(new char[] { ',' },
        //    //    StringSplitOptions.RemoveEmptyEntries);
        //    //}


        //    IPage page = null;
        //    if (!string.IsNullOrEmpty(model.Id))
        //    {
        //        page = await pageService.GetPage(model.Id);
        //    }

        //    var needToClearCache = false;
        //    var isNew = false;
        //    if (page != null)
        //    {
        //        if(page.Title != model.Title)
        //        {
        //            needToClearCache = true;
        //        }
        //        page.Title = model.Title;
        //        page.MetaDescription = model.MetaDescription;
        //        page.Content = model.Content;
        //        if (page.PageOrder != model.PageOrder) needToClearCache = true;

        //    }
        //    else
        //    {
        //        isNew = true;
        //        needToClearCache = true;
        //        var slug = ContentUtils.CreateSlug(model.Title);
        //        var available = await pageService.SlugIsAvailable(project.Id, slug);
        //        if (!available)
        //        {
        //            log.LogInformation("returning 409 because slug already in use");
        //            return StatusCode(409);
        //        }

        //        page = new Page()
        //        {
        //            ProjectId = project.Id,
        //            Author = User.GetUserDisplayName(),
        //            Title = model.Title,
        //            MetaDescription = model.MetaDescription,
        //            Content = model.Content,
        //            Slug = slug,
        //            ParentId = "0"

        //            //,Categories = categories.ToList()
        //        };
        //    }

        //    if(!string.IsNullOrEmpty(model.ParentSlug))
        //    {
        //        var parentPage = await pageService.GetPageBySlug(project.Id, model.ParentSlug);
        //        if (parentPage != null)
        //        {
        //            if(parentPage.Id != page.ParentId)
        //            {
        //                page.ParentId = parentPage.Id;
        //                page.ParentSlug = parentPage.Slug;
        //                needToClearCache = true;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        // empty means root level
        //        page.ParentSlug = string.Empty;
        //        page.ParentId = "0";
        //    }
        //    if(page.ViewRoles != model.ViewRoles)
        //    {
        //        needToClearCache = true;
        //    }
        //    page.ViewRoles = model.ViewRoles;

        //    page.PageOrder = model.PageOrder;
        //    page.IsPublished = model.IsPublished;
        //    page.ShowHeading = model.ShowHeading;
        //    page.MenuOnly = model.MenuOnly;
        //    if (!string.IsNullOrEmpty(model.PubDate))
        //    {
        //        var localTime = DateTime.Parse(model.PubDate);
        //        page.PubDate = timeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);

        //    }

        //    if(isNew)
        //    {
        //        await pageService.Create(page, model.IsPublished);
        //    }
        //    else
        //    {
        //        await pageService.Update(page, model.IsPublished);
        //    }


        //    if(needToClearCache)
        //    {
        //        pageService.ClearNavigationCache();
        //    }

        //    var url = Url.RouteUrl(pageRoutes.PageRouteName, new { slug = page.Slug });
        //    return Content(url);

        //}



        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AjaxDelete(string id)
        //{
        //    // disable status code page for ajax requests
        //    var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
        //    if (statusCodePagesFeature != null)
        //    {
        //        statusCodePagesFeature.Enabled = false;
        //    }

        //    var project = await projectService.GetCurrentProjectSettings();

        //    if (project == null)
        //    {
        //        log.LogInformation("returning 500 blog not found");
        //        return StatusCode(500);
        //    }

        //    var canEdit = await User.CanEditPages(project.Id, authorizationService);

        //    if (!canEdit)
        //    {
        //        log.LogInformation("returning 403 user is not allowed to edit");
        //        return StatusCode(403);
        //    }

        //    if (string.IsNullOrEmpty(id))
        //    {
        //        log.LogInformation("returning 404 postid not provided");
        //        return StatusCode(404);
        //    }

        //    var page = await pageService.GetPage(id);

        //    if (page == null)
        //    {
        //        log.LogInformation("returning 404 not found");
        //        return StatusCode(404);
        //    }

        //    log.LogWarning("user " + User.Identity.Name + " deleted page " + page.Slug);

        //    await pageService.DeletePage(project.Id, page.Id);

        //    return StatusCode(200);

        //}


    }
}
