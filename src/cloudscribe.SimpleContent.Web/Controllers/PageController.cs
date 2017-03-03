// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2017-01-08
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    public class PageController : Controller
    {
        public PageController(
            IProjectService projectService,
            IPageService blogService,
            IPageRoutes pageRoutes,
            IAuthorizationService authorizationService,
            ITimeZoneHelper timeZoneHelper,
            IStringLocalizer<SimpleContent> localizer,
            ILogger<PageController> logger)
        {
            this.projectService = projectService;
            this.pageService = blogService;
            this.authorizationService = authorizationService;
            this.timeZoneHelper = timeZoneHelper;
            this.pageRoutes = pageRoutes;
            sr = localizer;
            log = logger;
        }

        private IProjectService projectService;
        private IPageService pageService;
        private IAuthorizationService authorizationService;
        private ITimeZoneHelper timeZoneHelper;
        private ILogger log;
        private IPageRoutes pageRoutes;
        private IStringLocalizer<SimpleContent> sr;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string slug = "", 
            string parentSlug = "",
            string mode = ""
            )
        {
            var projectSettings = await projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                HttpContext.Response.StatusCode = 404;
                return NotFound();
            }

            if(slug == "none") { slug = string.Empty; }
            
            var canEdit = await User.CanEditPages(projectSettings.Id, authorizationService);
            var isNew = canEdit && (mode == "new");
            var isEditing = canEdit && (mode == "edit");
            if(!isNew && string.IsNullOrEmpty(slug)) { slug = projectSettings.DefaultPageSlug; }

            IPage page = null;
            if(!string.IsNullOrEmpty(slug))
            {
                page = await pageService.GetPageBySlug(projectSettings.Id, slug);
                
            }
            
            var model = new PageViewModel();
            model.Mode = mode;
            model.CurrentPage = page;
            model.ProjectSettings = projectSettings;
            model.CanEdit = canEdit;
            model.ShowComments = mode.Length == 0; // do we need this for a global disable
            model.CommentsAreOpen = false;
            model.TimeZoneHelper = timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;
            if (canEdit)
            {
                if (model.CurrentPage != null)
                {
                    model.EditPath = Url.RouteUrl(pageRoutes.PageEditRouteName, new { slug = model.CurrentPage.Slug });

                    if (model.CurrentPage.Slug == projectSettings.DefaultPageSlug)
                    {
                       // model.EditorSettings.NewItemPath = Url.Action("Index", "Page", new { slug = "" });
                        model.NewItemPath = Url.Action("Index", "Page", new { slug = "" });
                    }
                    else
                    {
                       
                        //model.EditorSettings.NewItemPath = Url.RouteUrl(pageRoutes.PageEditRouteName, new { slug = "", parentSlug = model.CurrentPage.Slug });
                        model.NewItemPath = Url.RouteUrl(pageRoutes.PageEditRouteName, new { slug = "", parentSlug = model.CurrentPage.Slug });

                    }

                    //model.EditorSettings.CancelEditPath = Url.RouteUrl(pageRoutes.PageRouteName, new { slug = model.CurrentPage.Slug });
                    //model.EditorSettings.CurrentSlug = model.CurrentPage.Slug;
                    //model.EditorSettings.IsPublished = model.CurrentPage.IsPublished; 
                    //model.EditorSettings.EditPath = Url.RouteUrl(pageRoutes.PageEditRouteName, new { slug = model.CurrentPage.Slug });
                    //model.EditorSettings.SortOrder = model.CurrentPage.PageOrder;
                    //model.EditorSettings.ParentSlug = model.CurrentPage.ParentSlug;
                    //model.EditorSettings.ViewRoles = model.CurrentPage.ViewRoles;
                    //model.EditorSettings.ShowHeading = model.CurrentPage.ShowHeading;
                    //model.EditorSettings.MenuOnly = model.CurrentPage.MenuOnly;
                   
                    
                }
                else
                {
                    model.NewItemPath = Url.RouteUrl(pageRoutes.PageEditRouteName, new { slug = "" });
                    //model.EditorSettings.CancelEditPath = Url.Content("~/");
                    //model.EditorSettings.EditPath = Url.Action("Index", "Page", new { slug = "", mode = "new" });
                    //model.EditorSettings.NewItemPath = Url.RouteUrl(pageRoutes.PageEditRouteName, new { slug = "" });

                }

                //model.EditorSettings.EditMode = mode;
                //model.EditorSettings.NewItemButtonText = "New Page";
                //model.EditorSettings.IndexUrl = Url.Content("~/");
                //model.EditorSettings.CategoryPath = Url.Action("Category", "Page"); // TODO: should we support categories on pages? this action doesn't exist right now
                //model.EditorSettings.DeletePath = Url.Action("AjaxDelete", "Page");
                //model.EditorSettings.SavePath = Url.Action("AjaxPost", "Page");
                
                //model.EditorSettings.ContentType = "Page";
                //model.EditorSettings.SupportsCategories = false;
                //model.EditorSettings.ProjectId = projectSettings.Id;

           
            }

            if (page == null)
            {
                if (isNew)
                {
                    page = new Page();
                    page.ProjectId = projectSettings.Id;
                    
                    //model.EditorSettings.ParentSlug = parentSlug;
                    
                    
                   
                    model.CurrentPage = page;
                    ViewData["Title"] = sr["New Page"];
                }
                else
                {
                    var rootList = await pageService.GetRootPages().ConfigureAwait(false);
                    // a site starts out with no pages 
                    if (canEdit && rootList.Count == 0)
                    {
                        page = new Page();
                        page.ProjectId = projectSettings.Id;
                        page.Title = "Home";
                        //mode = "new";
                        model.CurrentPage = page;
                        ViewData["Title"] = sr["New Page"];
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
                                ViewData["Title"] = sr["Content Index"];
                                //model.EditorSettings.EditMode = "none";
                                return View("IndexMenu", model);
                            }

                            Response.StatusCode = 404;
                            // return View("NotFound", 404);
                            return NotFound();
                        }
                        else
                        {
                            Response.StatusCode = 404;
                            return View("NoPages", 404);
                        }    
                    } 
                }
            }
            else
            {
                // if the page is protected by view roles return 404 if user is not in an allowed role
                if((!canEdit) && (!string.IsNullOrEmpty(page.ViewRoles)))
                {
                    if(!User.IsInRoles(page.ViewRoles))
                    {
                        Response.StatusCode = 404;
                        //return View("NotFound", 404);
                        return NotFound();
                    }
                }

                ViewData["Title"] = page.Title;
                
            }

           
            

            

            if (page != null && page.MenuOnly && !isEditing)
            {
                return View("ChildMenu", model);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(
            string slug = "",
            string parentSlug = ""
            )
        {
            var projectSettings = await projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(pageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(projectSettings.Id, authorizationService);
            if(!canEdit)
            {
                log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(pageRoutes.PageRouteName);
            }

            if (slug == "none") { slug = string.Empty; }

            var model = new PageEditViewModel();
            model.ProjectId = projectSettings.Id;
            model.FileBrowseUrl = Url.Action("CkFileDialog", "FileManager", new { type = "file" });
            model.ImageBrowseUrl = Url.Action("CkFileDialog", "FileManager", new { type = "image" });
            model.DropFileUrl = Url.Action("Upload", "FileManager");

            IPage page = null;
            if (!string.IsNullOrEmpty(slug))
            {
                page = await pageService.GetPageBySlug(projectSettings.Id, slug);
            }
            if(page == null)
            {
                ViewData["Title"] = sr["New Page"];
                model.PubDate = timeZoneHelper.ConvertToLocalTime(DateTime.UtcNow, projectSettings.TimeZoneId).ToString();
            }
            else
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["Edit - {0}"], page.Title);
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
                model.PubDate = timeZoneHelper.ConvertToLocalTime(page.PubDate, projectSettings.TimeZoneId).ToString();
                model.ShowHeading = page.ShowHeading;
                model.Slug = page.Slug;
                model.ExternalUrl = page.ExternalUrl;
                model.Title = page.Title;
                model.ViewRoles = page.ViewRoles;
                
                
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
                    ViewData["Title"] = sr["New Page"];
                }
                else
                {
                    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["Edit - {0}"], model.Title);
                }
                return View(model);
            }

            //if (string.IsNullOrEmpty(model.Title))
            //{
            //    // if a page has been configured to not show the title
            //    // this may be null on edit, if it is a new page then it should be required
            //    // because it is used for generating the slug
            //    //if (string.IsNullOrEmpty(model.Slug))
            //    //{
            //    log.LogInformation("returning 500 because no title was posted");
            //    return StatusCode(500);
            //    //}

            //}

            var project = await projectService.GetCurrentProjectSettings();
            
            if (project == null)
            {
                log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(pageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, authorizationService);

            if (!canEdit)
            {
                log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(pageRoutes.PageRouteName);
            }

            //string[] categories = new string[0];
            //if (!string.IsNullOrEmpty(model.Categories))
            //{
            //    categories = model.Categories.Split(new char[] { ',' },
            //    StringSplitOptions.RemoveEmptyEntries);
            //}


            IPage page = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                page = await pageService.GetPage(model.Id);
            }

            var needToClearCache = false;
            var isNew = false;
            if (page != null)
            {
                if (page.Title != model.Title)
                {
                    needToClearCache = true;
                }
                page.Title = model.Title;
                page.MetaDescription = model.MetaDescription;
                page.Content = model.Content;
                if (page.PageOrder != model.PageOrder) needToClearCache = true;

            }
            else
            {
                isNew = true;
                needToClearCache = true;
                var slug = ContentUtils.CreateSlug(model.Title);
                var available = await pageService.SlugIsAvailable(project.Id, slug);
                if (!available)
                {
                    //log.LogInformation("returning 409 because slug already in use");
                    ModelState.AddModelError("pageediterror", sr["slug is already in use."]);

                    return View(model);
                }

                page = new Page()
                {
                    ProjectId = project.Id,
                    Author = User.GetUserDisplayName(),
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
                var parentPage = await pageService.GetPageBySlug(project.Id, model.ParentSlug);
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
            if (!string.IsNullOrEmpty(model.PubDate))
            {
                var localTime = DateTime.Parse(model.PubDate);
                page.PubDate = timeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);

            }

            if (isNew)
            {
                await pageService.Create(page, model.IsPublished);
            }
            else
            {
                await pageService.Update(page, model.IsPublished);
            }


            if (needToClearCache)
            {
                pageService.ClearNavigationCache();
            }

            //var url = Url.RouteUrl(pageRoutes.PageRouteName, new { slug = page.Slug });
            return RedirectToRoute(pageRoutes.PageRouteName, new { slug = page.Slug });
            //return Content(url);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogInformation("project not found, redirecting");
                return RedirectToRoute(pageRoutes.PageRouteName, new { slug="" });
            }

            var canEdit = await User.CanEditPages(project.Id, authorizationService);

            if (!canEdit)
            {
                log.LogInformation("user is not allowed to edit, redirecting");
                return RedirectToRoute(pageRoutes.PageRouteName, new { slug = "" });
            }

            if (string.IsNullOrEmpty(id))
            {
                log.LogInformation("postid not provided, redirecting");
                return RedirectToRoute(pageRoutes.PageRouteName, new { slug = "" });

            }

            var page = await pageService.GetPage(id);

            if (page == null)
            {
                log.LogInformation("page not found, redirecting");
                return RedirectToRoute(pageRoutes.PageRouteName, new { slug = "" });
            }

            log.LogWarning("user " + User.Identity.Name + " deleted page " + page.Slug);

            await pageService.DeletePage(project.Id, page.Id);

            return RedirectToRoute(pageRoutes.PageRouteName, new { slug = "" });

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxPost(PageEditViewModel model)
        {
            // disable status code page for ajax requests
            var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
            if (statusCodePagesFeature != null)
            {
                statusCodePagesFeature.Enabled = false;
            }

            if (string.IsNullOrEmpty(model.Title))
            {
                // if a page has been configured to not show the title
                // this may be null on edit, if it is a new page then it should be required
                // because it is used for generating the slug
                //if (string.IsNullOrEmpty(model.Slug))
                //{
                log.LogInformation("returning 500 because no title was posted");
                return StatusCode(500);
                //}

            }

            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogInformation("returning 500 blog not found");
                return StatusCode(500);
            }

            var canEdit = await User.CanEditPages(project.Id, authorizationService);
            
            if (!canEdit)
            {
                log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            //string[] categories = new string[0];
            //if (!string.IsNullOrEmpty(model.Categories))
            //{
            //    categories = model.Categories.Split(new char[] { ',' },
            //    StringSplitOptions.RemoveEmptyEntries);
            //}


            IPage page = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                page = await pageService.GetPage(model.Id);
            }

            var needToClearCache = false;
            var isNew = false;
            if (page != null)
            {
                if(page.Title != model.Title)
                {
                    needToClearCache = true;
                }
                page.Title = model.Title;
                page.MetaDescription = model.MetaDescription;
                page.Content = model.Content;
                if (page.PageOrder != model.PageOrder) needToClearCache = true;
               
            }
            else
            {
                isNew = true;
                needToClearCache = true;
                var slug = ContentUtils.CreateSlug(model.Title);
                var available = await pageService.SlugIsAvailable(project.Id, slug);
                if (!available)
                {
                    log.LogInformation("returning 409 because slug already in use");
                    return StatusCode(409);
                }

                page = new Page()
                {
                    ProjectId = project.Id,
                    Author = User.GetUserDisplayName(),
                    Title = model.Title,
                    MetaDescription = model.MetaDescription,
                    Content = model.Content,
                    Slug = slug,
                    ParentId = "0"
                    
                    //,Categories = categories.ToList()
                };
            }

            if(!string.IsNullOrEmpty(model.ParentSlug))
            {
                var parentPage = await pageService.GetPageBySlug(project.Id, model.ParentSlug);
                if (parentPage != null)
                {
                    if(parentPage.Id != page.ParentId)
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
            if(page.ViewRoles != model.ViewRoles)
            {
                needToClearCache = true;
            }
            page.ViewRoles = model.ViewRoles;

            page.PageOrder = model.PageOrder;
            page.IsPublished = model.IsPublished;
            page.ShowHeading = model.ShowHeading;
            page.MenuOnly = model.MenuOnly;
            if (!string.IsNullOrEmpty(model.PubDate))
            {
                var localTime = DateTime.Parse(model.PubDate);
                page.PubDate = timeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);
                
            }

            if(isNew)
            {
                await pageService.Create(page, model.IsPublished);
            }
            else
            {
                await pageService.Update(page, model.IsPublished);
            }

            
            if(needToClearCache)
            {
                pageService.ClearNavigationCache();
            }

            var url = Url.RouteUrl(pageRoutes.PageRouteName, new { slug = page.Slug });
            return Content(url);

        }

        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxDelete(string id)
        {
            // disable status code page for ajax requests
            var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
            if (statusCodePagesFeature != null)
            {
                statusCodePagesFeature.Enabled = false;
            }

            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogInformation("returning 500 blog not found");
                return StatusCode(500);
            }

            var canEdit = await User.CanEditPages(project.Id, authorizationService);
            
            if (!canEdit)
            {
                log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            if (string.IsNullOrEmpty(id))
            {
                log.LogInformation("returning 404 postid not provided");
                return StatusCode(404);
            }

            var page = await pageService.GetPage(id);

            if (page == null)
            {
                log.LogInformation("returning 404 not found");
                return StatusCode(404);
            }

            log.LogWarning("user " + User.Identity.Name + " deleted page " + page.Slug);

            await pageService.DeletePage(project.Id, page.Id);

            return StatusCode(200);

        }


    }
}
