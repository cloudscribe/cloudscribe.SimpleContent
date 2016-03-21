// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2016-03-21
// 

using cloudscribe.SimpleContent.Common;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Services;

namespace cloudscribe.SimpleContent.Web.Controllers
{
    public class PageController : Controller
    {
        public PageController(
            IProjectService projectService,
            IPageService blogService,
            IUrlHelper urlHelper,
            ILogger<PageController> logger)
        {
            this.projectService = projectService;
            this.pageService = blogService;
            this.urlHelper = urlHelper;
            log = logger;
        }

        private IProjectService projectService;
        private IPageService pageService;
        private IUrlHelper urlHelper;
        private ILogger log;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string slug = "", 
            string mode = "")
        {
            var projectSettings = await projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                HttpContext.Response.StatusCode = 404;
                return new EmptyResult();
            }

            if(slug == "none") { slug = string.Empty; }
            

            var canEdit = User.CanEditProject(projectSettings.ProjectId);
            var isNew = canEdit && (mode == "new");
            var isEditing = canEdit && (mode == "edit");
            if(!isNew && string.IsNullOrEmpty(slug)) { slug = projectSettings.DefaultPageSlug; }

            Page page = null;
            if(!string.IsNullOrEmpty(slug))
            {
                page = await pageService.GetPageBySlug(projectSettings.ProjectId, slug);
                
            }
            

            var model = new PageViewModel();

            if (page == null)
            {
                if (isNew)
                {
                    page = new Page();
                    page.ProjectId = projectSettings.ProjectId;
                }
                else
                {
                    // a site starts out with no pages 
                    if (canEdit)
                    {
                        page = new Page();
                        page.ProjectId = projectSettings.ProjectId;
                        mode = "new";
                    }
                    else
                    {
                        
                        Response.StatusCode = 404;
                        return new EmptyResult();
                    }

                    
                }

            }
            else
            {
                ViewData["Title"] = page.Title;
            }

            model.Mode = mode;
            model.CurrentPage = page;
            model.ProjectSettings = projectSettings;
            model.CanEdit = canEdit;
            model.ShowComments = mode.Length == 0; // do we need this for a global disable
            //model.CommentsAreOpen = await blogService.CommentsAreOpen(post, canEdit);
            model.CommentsAreOpen = false;
            //TODO: fix https://github.com/joeaudette/cloudscribe.SimpleContent/issues/1
            try
            {
                model.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(model.ProjectSettings.TimeZoneId);
            }
            catch (Exception)
            {
                //temporary workaround for mac/linux
                model.TimeZone = TimeZoneInfo.Utc;
            }

            if (canEdit)
            {
                if(model.CurrentPage != null)
                {
                    model.EditorSettings.CancelEditPath = Url.Action("Index", "Page", new { slug = model.CurrentPage.Slug });
                    model.EditorSettings.CurrentSlug = model.CurrentPage.Slug;
                    model.EditorSettings.IsPublished = model.CurrentPage.IsPublished;
                    model.EditorSettings.EditPath = Url.Action("Index", "Page", new { slug = model.CurrentPage.Slug, mode="edit"});
                    model.EditorSettings.SortOrder = model.CurrentPage.PageOrder;
                }
                else
                {
                    model.EditorSettings.CancelEditPath = Url.Content("~/");
                    model.EditorSettings.EditPath = Url.Action("Index", "Page", new { slug="",  mode = "new" });
                }

                model.EditorSettings.EditMode = mode;
                model.EditorSettings.NewItemButtonText = "New Page";
                model.EditorSettings.IndexUrl = Url.Content("~/");
                model.EditorSettings.CategoryPath = Url.Action("Category", "Page"); // TODO: should we support categories on pages? this action doesn't exist right now
                model.EditorSettings.DeletePath = Url.Action("AjaxDelete", "Page");
                model.EditorSettings.SavePath = Url.Action("AjaxPost", "Page");
                model.EditorSettings.NewItemPath = Url.Action("Index", "Page", new { slug = "", mode = "new" });
                model.EditorSettings.ContentType = "Page";
                model.EditorSettings.SupportsCategories = false;

            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task AjaxPost(PageEditViewModel model)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                log.LogInformation("returning 500 because no title was posted");
                Response.StatusCode = 500;
                return;
            }

            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogInformation("returning 500 blog not found");
                Response.StatusCode = 500;
                return;
            }

            if (!User.CanEditProject(project.ProjectId))
            {
                log.LogInformation("returning 403 user is not allowed to edit");
                Response.StatusCode = 403;
                return;
            }

            //string[] categories = new string[0];
            //if (!string.IsNullOrEmpty(model.Categories))
            //{
            //    categories = model.Categories.Split(new char[] { ',' },
            //    StringSplitOptions.RemoveEmptyEntries);
            //}


            Page page = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                page = await pageService.GetPage(project.ProjectId, model.Id);
            }

            var isNew = false;
            if (page != null)
            {
                page.Title = model.Title;
                page.MetaDescription = model.MetaDescription;
                page.Content = model.Content;
                //post.Categories = categories.ToList();
            }
            else
            {
                isNew = true;
                var slug = ContentUtils.CreateSlug(model.Title);
                var available = await pageService.SlugIsAvailable(project.ProjectId, slug);
                if (!available)
                {
                    log.LogInformation("returning 409 because slug already in use");
                    Response.StatusCode = 409;
                    return;
                }

                page = new Page()
                {
                    ProjectId = project.ProjectId,
                    Author = User.GetDisplayName(),
                    Title = model.Title,
                    MetaDescription = model.MetaDescription,
                    Content = model.Content,
                    Slug = slug,
                    ParentId = "0"
                    
                    //,Categories = categories.ToList()
                };
            }

            page.PageOrder = model.PageOrder;
            page.IsPublished = model.IsPublished;
            if (!string.IsNullOrEmpty(model.PubDate))
            {
                var localTime = DateTime.Parse(model.PubDate);
                try
                {
                    //TODO: fix https://github.com/joeaudette/cloudscribe.SimpleContent/issues/1
                    var tz = TimeZoneInfo.FindSystemTimeZoneById(project.TimeZoneId);
                    page.PubDate = TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Utc);
                }
                catch(Exception)
                {
                    page.PubDate = localTime;
                }
                

            }

            await pageService.Save(project.ProjectId, page, isNew, model.IsPublished);
            if(isNew)
            {
                // TODO: clear the page tree cache
            }

            var url = urlHelper.Action("Index", "Page", new { slug = page.Slug });
            await Response.WriteAsync(url);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task AjaxDelete(string id)
        {
            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogInformation("returning 500 blog not found");
                Response.StatusCode = 500;
                return; // new EmptyResult();
            }

            if (!User.CanEditProject(project.ProjectId))
            {
                log.LogInformation("returning 403 user is not allowed to edit");
                Response.StatusCode = 403;
                return; //new EmptyResult();
            }

            if (string.IsNullOrEmpty(id))
            {
                log.LogInformation("returning 404 postid not provided");
                Response.StatusCode = 404;
                return; //new EmptyResult();
            }

            var page = await pageService.GetPage(project.ProjectId, id);

            if (page == null)
            {
                log.LogInformation("returning 404 not found");
                Response.StatusCode = 404;
                return; //new EmptyResult();
            }

            var result = await pageService.DeletePage(project.ProjectId, page.Id);

            // TODO: clear the page tree cache

            Response.StatusCode = 200;
            return; //new EmptyResult();

        }


    }
}
