// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2018-07-14
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.Templating;
using cloudscribe.SimpleContent.Web.ViewModels;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
        public PageController(
            IMediator mediator,
            IProjectService projectService,
            IPageService blogService,
            IContentProcessor contentProcessor,
            IPageRoutes pageRoutes,
            IAuthorizationService authorizationService,
            ITimeZoneHelper timeZoneHelper,
            IAuthorNameResolver authorNameResolver,
            IContentTemplateService templateService,
            IAutoPublishDraftPage autoPublishDraftPage,
            IContentHistoryCommands historyCommands,
            IContentHistoryQueries historyQueries,
            IStringLocalizer<SimpleContent> localizer,
            IOptions<PageEditOptions> pageEditOptionsAccessor,
            ILogger<PageController> logger)
        {
            Mediator = mediator;
            ProjectService = projectService;
            PageService = blogService;
            TemplateService = templateService;
            ContentProcessor = contentProcessor;
            AutoPublishDraftPage = autoPublishDraftPage;
            AuthorizationService = authorizationService;
            AuthorNameResolver = authorNameResolver;
            HistoryCommands = historyCommands;
            HistoryQueries = historyQueries;
            TimeZoneHelper = timeZoneHelper;
            PageRoutes = pageRoutes;
            EditOptions = pageEditOptionsAccessor.Value;
            StringLocalizer = localizer;
            Log = logger;
        }

        protected IProjectService ProjectService { get; private set; }
        protected IPageService PageService { get; private set; }
        protected IContentTemplateService TemplateService { get; private set; }
        protected IAutoPublishDraftPage AutoPublishDraftPage { get; private set; }
        protected IContentProcessor ContentProcessor { get; private set; }
        protected IAuthorizationService AuthorizationService { get; private set; }
        protected IAuthorNameResolver AuthorNameResolver { get; private set; }
        protected ITimeZoneHelper TimeZoneHelper { get; private set; }
        protected ILogger Log { get; private set; }
        protected IPageRoutes PageRoutes { get; private set; }
        protected IStringLocalizer<SimpleContent> StringLocalizer { get; private set; }
        protected PageEditOptions EditOptions { get; private set; }
        protected IContentHistoryCommands HistoryCommands { get; private set; }
        protected IContentHistoryQueries HistoryQueries { get; private set; }

        protected IMediator Mediator { get; private set; }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Index(
            CancellationToken cancellationToken,
            string slug = "", 
            bool showDraft = false,
            Guid? historyId = null
            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogError("project settings not found returning 404");
                return NotFound();
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if(string.IsNullOrEmpty(slug) || slug == "none") { slug = project.DefaultPageSlug; }

            IPage page = await PageService.GetPageBySlug(slug, cancellationToken);

            await AutoPublishDraftPage.PublishIfNeeded(page);

            ContentHistory history = null;
            var pageWasDeleted = false;
            var showingDraft = false;
            var hasDraft = false;
            var hasPublishedVersion = false;
            if(page != null)
            {
                hasDraft = page.HasDraftVersion();
                hasPublishedVersion = page.HasPublishedVersion();
            }

            if(canEdit && historyId.HasValue)
            {
                history = await HistoryQueries.Fetch(project.Id, historyId.Value);
                if (history != null)
                {
                    if(page == null) //page must have been deleted, restore from hx
                    {
                        page = new Page();
                        history.CopyTo(page);
                        if(history.IsDraftHx)
                        {
                            page.PromoteDraftTemporarilyForRender();
                        }
                        pageWasDeleted = true;
                    }
                    else
                    {
                        var pageCopy = new Page();
                        page.CopyTo(pageCopy);
                        if (history.IsDraftHx)
                        {
                            pageCopy.Content = history.DraftContent;
                            pageCopy.Author = history.DraftAuthor;
                        }
                        else
                        {
                            pageCopy.Content = history.Content;
                            pageCopy.Author = history.Author;
                        }

                        page = pageCopy;
                    }         
                } 
            }
            else if(canEdit && page != null && (showDraft || !page.HasPublishedVersion()))
            {
                var pageCopy = new Page();
                page.CopyTo(pageCopy);
                pageCopy.PromoteDraftTemporarilyForRender();
                page = pageCopy;
                showingDraft = true;
                
            }
            else if(page == null)
            {
                var rootList = await PageService.GetRootPages(cancellationToken).ConfigureAwait(false);
                if (canEdit && rootList.Count == 0)
                {
                    page = new Page
                    {
                        ProjectId = project.Id
                    };
                    if (HttpContext.Request.Path == "/")
                    {
                        ViewData["Title"] = StringLocalizer["Home"];
                        page.Title = "No pages found, please click the pencil to create the home page";
                    }
                    else
                    {
                        ViewData["Title"] = StringLocalizer["No Pages Found"];
                        page.Title = "No pages found, please click the pencil to create the first page";

                    }
                }
                else
                {
                    if (rootList.Count > 0)
                    {
                        if (slug == project.DefaultPageSlug)
                        {
                            // slug was empty and no matching page found for default slug
                            // but since there exist root level pages we should
                            // show an index menu. 
                            // esp useful if not using pages as the default route
                            // /p or /docs
                            ViewData["Title"] = StringLocalizer["Content Index"];
                            var emptyModel = new PageViewModel(ContentProcessor)
                            {
                                ProjectSettings = project,
                                CanEdit = canEdit,
                                CommentsAreOpen = false,
                                TimeZoneHelper = TimeZoneHelper,
                                TimeZoneId = project.TimeZoneId,
                                PageTreePath = Url.Action("Tree"),
                                NewItemPath = Url.RouteUrl(PageRoutes.NewPageRouteName, new { slug = "" }),
                                EditPath = Url.RouteUrl(PageRoutes.PageEditRouteName, new { slug })
                            };

                            return View("IndexMenu", emptyModel);
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
            // page is not null at this point

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    Log.LogWarning($"page {page.Title} is protected by roles that user is not in so returning 404");
                    return NotFound();
                }
            }

            if (!string.IsNullOrEmpty(page.ExternalUrl))
            {
                if (canEdit)
                {
                    Log.LogWarning($"page {page.Title} has override url {page.ExternalUrl}, redirecting to edit since user can edit");
                    return RedirectToRoute(PageRoutes.PageEditRouteName, new { slug = page.Slug });
                }
                else
                {
                    Log.LogWarning($"page {page.Title} has override url {page.ExternalUrl}, not intended to be viewed so returning 404");
                    return NotFound();
                }
            }
            
            if (!canEdit )
            {
                if (!page.HasPublishedVersion())
                {
                    Log.LogWarning($"page {page.Title} is unpublished and user is not editor so returning 404");
                    return NotFound();
                }
            }
            
            var model = new PageViewModel(ContentProcessor)
            {
                CurrentPage = page,
                ProjectSettings = project,
                CanEdit = canEdit,
                CommentsAreOpen = false,
                TimeZoneHelper = TimeZoneHelper,
                TimeZoneId = project.TimeZoneId,
                PageTreePath = Url.Action("Tree"),
                HasPublishedVersion = hasPublishedVersion,
                HasDraft = hasDraft,
                ShowingDraft = showingDraft
            };

            if(history != null)
            {
                model.HistoryId = history.Id;
                model.HistoryArchiveDate = history.ArchivedUtc;
                model.ShowingDeleted = pageWasDeleted;
            }

            model.EditPath = Url.RouteUrl(PageRoutes.PageEditRouteName, new { slug = model.CurrentPage.Slug });

            if (model.CurrentPage.Slug == project.DefaultPageSlug)
            {
                // not setting the parent slug if the current page is home page
                // otherwise it would be awkward to create more root level pages
                model.NewItemPath = Url.RouteUrl(PageRoutes.NewPageRouteName, new { slug = "" });
            }
            else
            {
                // for non home pages if the user clicks the new link
                // make it use the current page slug as the parent slug for the new item
                model.NewItemPath = Url.RouteUrl(PageRoutes.NewPageRouteName, new { slug = "", parentSlug = model.CurrentPage.Slug });
            }
            if(!string.IsNullOrWhiteSpace(page.TemplateKey))
            {
                model.Template = await TemplateService.GetTemplate(project.Id, page.TemplateKey);
            }
            
            ViewData["Title"] = page.Title;
                
            if (page.MenuOnly)
            {
                return View("ChildMenu", model);
            }

            return View(model);
        }

        
        
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> NewPage(
            CancellationToken cancellationToken,
            string parentSlug = "",
            string query = null,
            int pageNumber = 1,
            int pageSize = 10
            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var templateCount = await TemplateService.GetCountOfTemplates(project.Id, ProjectConstants.PageFeatureName);
            if (templateCount == 0)
            {
                return RedirectToRoute(PageRoutes.PageEditRouteName, new { parentSlug });
            }

            var templates = await TemplateService.GetTemplates(
                project.Id, 
                ProjectConstants.PageFeatureName, 
                query,
                pageNumber,
                pageSize,
                cancellationToken);

            
            var model = new NewContentViewModel()
            {
                ParentSlug = parentSlug,
                Templates = templates,
                Query = query,
                PageNumber = pageNumber,
                PageSize = pageSize,
                CountOfTemplates = templateCount,
                SearchRouteName = PageRoutes.NewPageRouteName,
                PostActionName = "InitTemplatedPage"
            };

            if(!string.IsNullOrWhiteSpace(parentSlug))
            {
                model.PageOrder = await PageService.GetNextChildPageOrder(parentSlug);
            }
            
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> InitTemplatedPage(NewContentViewModel model)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            if (!ModelState.IsValid)
            {
                model.Templates = await TemplateService.GetTemplates(
                    project.Id, 
                    ProjectConstants.PageFeatureName,
                    model.Query,
                    model.PageNumber,
                    model.PageSize

                    );
                model.SearchRouteName = PageRoutes.NewPageRouteName;
                model.PostActionName = "InitTemplatedPage";

                return View("NewPage", model);
            }

            var template = await TemplateService.GetTemplate(project.Id, model.SelectedTemplate);

            if (template == null)
            {
                Log.LogWarning($"redirecting to index because content template {model.SelectedTemplate} was not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var request = new InitTemplatedPageRequest(
                project.Id,
                User.Identity.Name,
                await AuthorNameResolver.GetAuthorName(User),
                model, 
                template);

            var response = await Mediator.Send(request);
            if(response.Succeeded)
            {
                Log.LogDebug($"succeeded in initializing a page with template {model.SelectedTemplate}");
                return RedirectToRoute(PageRoutes.PageEditWithTemplateRouteName, new { slug = response.Value.Slug });
            }
            else
            {
                if(response.ErrorMessages != null && response.ErrorMessages.Count > 0)
                {
                    foreach(var err in response.ErrorMessages)
                    {
                        this.AlertDanger(err, true);
                    }
                }
            }
            
            return RedirectToRoute(PageRoutes.PageRouteName);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> EditWithTemplate(
            CancellationToken cancellationToken,
            string slug,
            Guid? historyId = null
            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);
            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var page = await PageService.GetPageBySlug(slug, cancellationToken);
            ContentHistory history = null;
            var didReplaceDraft = false;
            var didRestoreDeleted = false;

            if (historyId.HasValue)
            {
                history = await HistoryQueries.Fetch(project.Id, historyId.Value).ConfigureAwait(false);
                if(history != null)
                {
                    if(page == null) // page was deleted, restore it from history
                    {
                        page = new Page();
                        history.CopyTo(page);
                        if(history.IsDraftHx)
                        {
                            page.PromoteDraftTemporarilyForRender();
                        }
                        didRestoreDeleted = true;
                    }
                    else
                    {
                        didReplaceDraft = page.HasDraftVersion();
                        var pageCopy = new Page();
                        page.CopyTo(pageCopy);
                        if (history.IsDraftHx)
                        {
                            pageCopy.DraftAuthor = history.DraftAuthor;
                            pageCopy.DraftContent = history.DraftContent;
                            pageCopy.DraftSerializedModel = history.DraftSerializedModel;
                        }
                        else
                        {
                            pageCopy.DraftAuthor = history.Author;
                            pageCopy.DraftContent = history.Content;
                            pageCopy.DraftSerializedModel = history.SerializedModel;
                        }

                        page = pageCopy;
                    }
                }
            }
            
            if (page == null)
            {
                Log.LogError($"redirecting to index because page was not found for slug {slug}");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }
            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], page.Title);

            var template = await TemplateService.GetTemplate(project.Id, page.TemplateKey);
            if (template == null)
            {
                Log.LogError($"redirecting to index because content template {page.TemplateKey} was not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }
            
            var model = new PageEditWithTemplateViewModel()
            {
                ProjectId = project.Id,
                DisqusShortname = project.DisqusShortName,
                Author = page.Author,
                Id = page.Id,
                CorrelationKey = page.CorrelationKey,
                IsPublished = page.IsPublished,
                ShowMenu = page.ShowMenu,
                MetaDescription = page.MetaDescription,
                PageOrder = page.PageOrder,
                ParentId = page.ParentId,
                ParentSlug = page.ParentSlug,
                ShowHeading = page.ShowHeading,
                Slug = page.Slug,
                Title = page.Title,
                MenuFilters = page.MenuFilters,
                ViewRoles = page.ViewRoles,
                ShowComments = page.ShowComments,
                Template = template,
                TemplateModel = TemplateService.DesrializeTemplateModel(page, template),
                ProjectDefaultSlug = project.DefaultPageSlug,
                DidReplaceDraft = didReplaceDraft,
                DidRestoreDeleted = didRestoreDeleted
            };

            if(history != null)
            {
                model.HistoryArchiveDate = history.ArchivedUtc;
                model.HistoryId = history.Id;
            }
            
            if (page.PubDate.HasValue)
            {
                model.PubDate = TimeZoneHelper.ConvertToLocalTime(page.PubDate.Value, project.TimeZoneId);
            }

            if (page.DraftPubDate.HasValue)
            {
                model.DraftPubDate = TimeZoneHelper.ConvertToLocalTime(page.DraftPubDate.Value, project.TimeZoneId);
            }

            if (!string.IsNullOrWhiteSpace(page.DraftAuthor))
            {
                model.Author = page.DraftAuthor;
            }

            if (model.TemplateModel == null)
            {
                Log.LogError($"redirecting to index model desrialization failed for page {page.Title}");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }
            
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> EditWithTemplate(PageEditWithTemplateViewModel model)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var page = await PageService.GetPage(model.Id);
            
            if(page == null && model.HistoryId.HasValue) // restore a deleted page from history
            {
                var history = await HistoryQueries.Fetch(project.Id, model.HistoryId.Value).ConfigureAwait(false);
                if(history != null)
                {
                    page = new Page();
                    history.CopyTo(page);
                    await PageService.Create(page); // re-create page here because handler expects existing page and only updates
                }
            }
            
            if (page == null)
            {
                Log.LogError($"redirecting to index because page was not found for id {model.Id}");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }
            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], page.Title);

            var template = await TemplateService.GetTemplate(project.Id, page.TemplateKey);
            if (template == null)
            {
                Log.LogError($"redirecting to index because content template {page.TemplateKey} was not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }
            
            var request = new UpdateTemplatedPageRequest(
                project.Id,
                User.Identity.Name,
                model,
                template,
                page,
                Request.Form,
                ModelState
                );

            var response = await Mediator.Send(request);
            if (response.Succeeded)
            {
            
                this.AlertSuccess(StringLocalizer["The page was updated successfully."], true);
                if(response.Value.Slug == project.DefaultPageSlug)
                {
                    return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
                }
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = response.Value.Slug });
            }
            else
            {
                model.Template = template;
                model.ProjectDefaultSlug = project.DefaultPageSlug;

                return View(model);
            }
            
        }


        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Edit(
            CancellationToken cancellationToken,
            string slug = "",
            string parentSlug = "",
            string type ="",
            Guid? historyId = null
            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);
            if(!canEdit)
            {
                Log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            if (slug == "none") { slug = string.Empty; }

            var model = new PageEditViewModel
            {
                ProjectId = project.Id,
                DisqusShortname = project.DisqusShortName,
                ProjectDefaultSlug = project.DefaultPageSlug
            };

            IPage page = null;
            if (!string.IsNullOrEmpty(slug))
            {
                page = await PageService.GetPageBySlug(slug, cancellationToken);
            }

            var routeVals = new RouteValueDictionary
            {
                { "slug", slug }
            };
            if (historyId.HasValue)
            {
                routeVals.Add("historyId", historyId.Value);
            }

            if (page != null && !string.IsNullOrWhiteSpace(page.TemplateKey))
            {
                return RedirectToRoute(PageRoutes.PageEditWithTemplateRouteName, routeVals);
            }

            ContentHistory history = null;
            var didReplaceDraft = false;
            var didRestoreDeleted = false;

            if (historyId.HasValue)
            {
                history = await HistoryQueries.Fetch(project.Id, historyId.Value).ConfigureAwait(false);
                if (history != null)
                {
                    if(!string.IsNullOrWhiteSpace(history.TemplateKey))
                    {
                        return RedirectToRoute(PageRoutes.PageEditWithTemplateRouteName, routeVals);
                    }
                    
                    if (page == null) // page was deleted, restore it from history
                    {
                        page = new Page();
                        history.CopyTo(page);
                        if (history.IsDraftHx)
                        {
                            page.PromoteDraftTemporarilyForRender();
                        }
                        didRestoreDeleted = true;
                    }
                    else
                    {
                        didReplaceDraft = page.HasDraftVersion();
                        var pageCopy = new Page();
                        page.CopyTo(pageCopy);
                        if (history.IsDraftHx)
                        {
                            pageCopy.DraftAuthor = history.DraftAuthor;
                            pageCopy.DraftContent = history.DraftContent;
                        }
                        else
                        {
                            pageCopy.DraftAuthor = history.Author;
                            pageCopy.DraftContent = history.Content;
                        }
                        page = pageCopy; 
                    }

                    model.HistoryArchiveDate = history.ArchivedUtc;
                    model.HistoryId = history.Id;
                    model.DidReplaceDraft = didReplaceDraft;
                    model.DidRestoreDeleted = didRestoreDeleted;
                }
            }
            
            if (page == null) // new page
            {
                ViewData["Title"] = StringLocalizer["New Page"];
                model.ParentSlug = parentSlug;
                model.PageOrder = await PageService.GetNextChildPageOrder(parentSlug, cancellationToken);
                model.ContentType = project.DefaultContentType;
                
                if (EditOptions.AllowMarkdown && !string.IsNullOrWhiteSpace(type) && type == "markdown")
                {
                    model.ContentType = "markdown";
                }
                if (!string.IsNullOrWhiteSpace(type) && type == "html")
                {
                    model.ContentType = "html";
                }

                var rootList = await PageService.GetRootPages(cancellationToken).ConfigureAwait(false);
                if(rootList.Count == 0) // expected if home page doesn't exist yet
                {
                    var rootPagePath = Url.RouteUrl(PageRoutes.PageRouteName);
                    if(string.IsNullOrWhiteSpace(slug))
                    {
                        slug = project.DefaultPageSlug;
                        model.Title = StringLocalizer["Home"];
                    }
                    
                }
                model.Author = await AuthorNameResolver.GetAuthorName(User);
                model.Slug = slug;
            }
            else // page not null
            {
                // if the page is protected by view roles return 404 if user is not in an allowed role
                if ((!string.IsNullOrEmpty(page.ViewRoles)))
                {
                    if (!User.IsInRoles(page.ViewRoles))
                    {
                        Log.LogWarning($"page {page.Title} is protected by roles that user is not in so returning 404");
                        return NotFound();
                    }
                }

                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], page.Title);
                if(string.IsNullOrWhiteSpace(page.DraftContent))
                {
                    model.Author = page.Author;
                    model.Content = page.Content;
                }
                else
                {
                    model.Author = page.DraftAuthor;
                    model.Content = page.DraftContent; 
                }
                
                model.Id = page.Id;
                model.CorrelationKey = page.CorrelationKey;
                model.IsPublished = page.IsPublished;
                model.ShowMenu = page.ShowMenu;
                model.MenuOnly = page.MenuOnly;
                model.MetaDescription = page.MetaDescription;
                model.PageOrder = page.PageOrder;
                model.ParentId = page.ParentId;
                model.ParentSlug = page.ParentSlug;
                model.ShowHeading = page.ShowHeading;
                model.Slug = page.Slug;
                model.ExternalUrl = page.ExternalUrl;
                model.Title = page.Title;
                model.MenuFilters = page.MenuFilters;
                model.ViewRoles = page.ViewRoles;
                model.ShowComments = page.ShowComments;
                model.DisableEditor = page.DisableEditor;
                model.ContentType = page.ContentType;
                if (page.PubDate.HasValue)
                {
                    model.PubDate = TimeZoneHelper.ConvertToLocalTime(page.PubDate.Value, project.TimeZoneId);
                }

                if (page.DraftPubDate.HasValue)
                {
                    model.DraftPubDate = TimeZoneHelper.ConvertToLocalTime(page.DraftPubDate.Value, project.TimeZoneId);
                }
            }
            
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(PageEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    ViewData["Title"] = StringLocalizer["New Page"];
                }
                else
                {
                    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], model.Title);
                }
                return View(model);
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                page = await PageService.GetPage(model.Id);
            }

            if (page == null && model.HistoryId.HasValue) // restore a deleted page from history
            {
                var history = await HistoryQueries.Fetch(project.Id, model.HistoryId.Value).ConfigureAwait(false);
                if (history != null)
                {
                    page = new Page();
                    history.CopyTo(page);
                    await PageService.Create(page); // re-create page here because handler expects existing page and only updates
                }
            }

            var isNew = (page == null);

            var request = new CreateOrUpdatePageRequest(
                project.Id,
                User.Identity.Name,
                model,
                page,
                ModelState
                );

            var response = await Mediator.Send(request);
            if (response.Succeeded)
            {
                if(isNew)
                {
                    this.AlertSuccess(StringLocalizer["The page was created successfully."], true);
                }
                else
                {
                    this.AlertSuccess(StringLocalizer["The page was updated successfully."], true);
                }

                if (!string.IsNullOrEmpty(response.Value.ExternalUrl))
                {
                    this.AlertWarning(StringLocalizer["Note that since this page has an override url, the menu item will link to the url so the page is used only as a means to add a link in the menu, the content is not used."], true);
                    return RedirectToRoute(PageRoutes.PageEditRouteName, new { slug = response.Value.Slug });
                }

                if(response.Value.Slug == project.DefaultPageSlug)
                {
                    return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
                }
                
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = response.Value.Slug });
            }
            else
            {
                model.DisqusShortname = project.DisqusShortName;
                model.ProjectDefaultSlug = project.DefaultPageSlug;

                return View(model);
            }
            
        }
        
        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Development(
            CancellationToken cancellationToken,
            string slug)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);
            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canDev = EditOptions.AlwaysShowDeveloperLink ? true : User.IsInRole(EditOptions.DeveloperAllowedRole);

            if (!canDev)
            {
                Log.LogInformation("redirecting to index because user is not allowed by edit config for developer tools");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(slug))
            {
                page = await PageService.GetPageBySlug(slug, cancellationToken);
            }
            if (page == null)
            {
                Log.LogInformation("page not found, redirecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    Log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(PageRoutes.PageRouteName);
                }
            }

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Developer Tools - {0}"], page.Title);

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
        public virtual async Task<IActionResult> AddResource(AddPageResourceViewModel model)
        {
            if(!ModelState.IsValid)
            {
                this.AlertDanger(StringLocalizer["Invalid request"], true);
                return RedirectToRoute(PageRoutes.PageDevelopRouteName, new { slug = model.Slug });
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);
            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }
            var canDev = EditOptions.AlwaysShowDeveloperLink ? true : User.IsInRole(EditOptions.DeveloperAllowedRole);
            if (!canDev)
            {
                Log.LogInformation("redirecting to index because user is not allowed by edit config for developer tools");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(model.Slug))
            {
                page = await PageService.GetPageBySlug(model.Slug);
            }

            if (page == null)
            {
                Log.LogInformation("page not found, redirecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    Log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(PageRoutes.PageRouteName);
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
            
            await PageService.Update(page);

            return RedirectToRoute(PageRoutes.PageDevelopRouteName, new { slug = page.Slug });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> RemoveResource(string slug, string id)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);
            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }
            var canDev = EditOptions.AlwaysShowDeveloperLink ? true : User.IsInRole(EditOptions.DeveloperAllowedRole);
            if (!canDev)
            {
                Log.LogInformation("redirecting to index because user is not allowed by edit config for developer tools");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            IPage page = null;
            if (!string.IsNullOrEmpty(slug))
            {
                page = await PageService.GetPageBySlug(slug);
            }

            if (page == null)
            {
                Log.LogInformation("page not found, redirecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    Log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(PageRoutes.PageRouteName);
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
                await PageService.Update(page);
            }
            else
            {
                Log.LogWarning($"page resource not found for {slug} and {id}");
            }
            
            return RedirectToRoute(PageRoutes.PageDevelopRouteName, new { slug = page.Slug });

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Log.LogInformation("postid not provided, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogWarning("project not found, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug="" });
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogWarning("user is not allowed to edit, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }
            
            var page = await PageService.GetPage(id);

            if (page == null)
            {
                Log.LogInformation($"page not found for {id}, redirecting/rejecting");
                if (Request.IsAjaxRequest())
                {
                    return BadRequest();
                }
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    Log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(PageRoutes.PageRouteName);
                }
            }

            if(!EditOptions.AllowDeleteDefaultPage)
            {
                if (page.Slug == project.DefaultPageSlug) // don't allow delete the home/default page from the ui
                {
                    Log.LogError($"Rejecting/redirecting, user {User.Identity.Name} tried to delete the default page {page.Title}");
                    if (Request.IsAjaxRequest())
                    {
                        return BadRequest();
                    }
                    this.AlertWarning(StringLocalizer["Sorry, deleting the default page is not allowed by configuration."], true);
                    return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
                }
            }


            if (string.IsNullOrWhiteSpace(page.ExternalUrl) && !page.MenuOnly)
            {
                //don't create history for deleted page that was just a menu item with an external url
                var history = page.CreateHistory(User.Identity.Name, true);
                await HistoryCommands.Create(project.Id, history);
            }
            
            
            await PageService.DeletePage(page.Id);
            PageService.ClearNavigationCache();

            Log.LogWarning($"user {User.Identity.Name} deleted page {page.Title}");

            if (Request.IsAjaxRequest())
            {
                return Json(new PageActionResult(true,"success"));
            }

            return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> UnPublish(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Log.LogInformation("pageId not provided, redirecting/rejecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogWarning("project not found, redirecting/rejecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogWarning("user is not allowed to edit, redirecting/rejecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            var page = await PageService.GetPage(id);

            if (page == null)
            {
                Log.LogInformation($"page not found for {id}, redirecting/rejecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            if ((!string.IsNullOrEmpty(page.ViewRoles)))
            {
                if (!User.IsInRoles(page.ViewRoles))
                {
                    Log.LogWarning($"page {page.Title} is protected by roles that user is not in so redirecting");
                    return RedirectToRoute(PageRoutes.PageRouteName);
                }
            }
            
            if (string.IsNullOrWhiteSpace(page.ExternalUrl) && !page.MenuOnly)
            {
                //don't create history for deleted page that was just a menu item with an external url
                var history = page.CreateHistory(User.Identity.Name, true);
                await HistoryCommands.Create(project.Id, history);
            }

            if (!string.IsNullOrWhiteSpace(page.ExternalUrl))
            {
                //don't unpublish links just delete
                await PageService.DeletePage(page.Id);
            }
            else
            {
                if(page.HasPublishedVersion())
                {
                    await PageService.FireUnPublishEvent(page);

                    page.DraftAuthor = page.Author;
                    page.DraftContent = page.Content;
                    page.DraftSerializedModel = page.SerializedModel;
                    page.Content = null;
                    page.SerializedModel = null;  
                }
                
                page.DraftPubDate = null;
                page.PubDate = null;
                page.IsPublished = false;
                await PageService.Update(page);
                
            }
            
            PageService.ClearNavigationCache();

            Log.LogWarning($"user {User.Identity.Name} unpublished page {page.Title}");

            var slug = page.Slug;
            if(slug == project.DefaultPageSlug) { slug = string.Empty; }

            return RedirectToRoute(PageRoutes.PageRouteName, new { slug = slug });

        }

        [Authorize(Policy = "ViewContentHistoryPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHistoryOlderThan(string id, int days)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogWarning("project not found, redirecting/rejecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogWarning("user is not allowed to edit, redirecting/rejecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            var page = await PageService.GetPage(id);

            if (page == null)
            {
                Log.LogInformation($"page not found for {id}, redirecting/rejecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            if (days < 0) //delete all history
            {
                await HistoryCommands.DeleteByContent(project.Id, id).ConfigureAwait(false);
            }
            else
            {
                var cutoffUtc = DateTime.UtcNow.AddDays(-days);
                await HistoryCommands.DeleteByContent(project.Id, id, cutoffUtc).ConfigureAwait(false);
            }
            
            return RedirectToRoute(PageRoutes.PageHistoryRouteName, new { slug = page.Slug });
        }

        [HttpGet]
        public virtual IActionResult SiteMap()
        {
            return View("SiteMapIndex");
        }

        [HttpGet]
  
        public virtual async Task<IActionResult> Tree()
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("project not found, redirecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("user is not allowed to edit, redirecting");
                return RedirectToRoute(PageRoutes.PageRouteName, new { slug = "" });
            }

            ViewData["Title"] = StringLocalizer["Page Management"];
            var model = new PageTreeViewModel
            {
                TreeServiceUrl = Url.Action("TreeJson"),
                EditUrl = Url.RouteUrl(PageRoutes.PageEditRouteName),
                ViewUrl = Url.RouteUrl(PageRoutes.PageRouteName),
                MoveUrl = Url.Action("Move"),
                DeleteUrl = Url.Action("Delete"),
                SortUrl = Url.Action("SortChildPagesAlpha")
            };

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> TreeJson(CancellationToken cancellationToken, string node = "root")
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("project not found");
                return BadRequest();
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("user is not allowed to edit");
                return BadRequest();
            }

            string resolveUrl(IPage page)
            {
                if (page.Slug == project.DefaultPageSlug)
                {
                    return Url.RouteUrl(PageRoutes.PageRouteName);
                }

                return Url.RouteUrl(PageRoutes.PageRouteName, new { slug = page.Slug });
            };

            var result = await PageService.GetPageTreeJson(User, resolveUrl, node, cancellationToken);

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
        public virtual async Task<IActionResult> Move(PageMoveModel model)
        {
            if (model == null)
            {
                Log.LogInformation("model was null");
                return BadRequest();
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("project not found");
                return BadRequest();
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("user is not allowed to edit");
                return BadRequest();
            }


            var result = await PageService.Move(model);

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> SortChildPagesAlpha(string pageId)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("project not found");
                return BadRequest();
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("user is not allowed to edit");
                return BadRequest();
            }


            var result = await PageService.SortChildPagesAlpha(pageId);

            return Json(result);
        }

        [HttpGet]
        [Authorize(Policy = "ViewContentHistoryPolicy")]
        public virtual async Task<IActionResult> History(
            CancellationToken cancellationToken,
            string slug,
            int pageNumber = 1,
            int pageSize = 10
            )
        {

            var project = await ProjectService.GetCurrentProjectSettings();
            if (project == null)
            {
                Log.LogError("project settings not found returning 404");
                return NotFound();
            }

            var page = await PageService.GetPageBySlug(slug, cancellationToken);

            if (page == null)
            {
                Log.LogWarning("page not found for slug " + slug + ", so redirecting to index");
                return RedirectToRoute(PageRoutes.PageRouteName);
            }

            var model = new ContentHistoryViewModel()
            {
                History = await HistoryQueries.GetByContent(
                    project.Id,
                    page.Id,
                    pageNumber,
                    pageSize,
                    cancellationToken),

                ContentId = page.Id,
                ContentTitle = page.Title,
                ContentSlug = page.Slug,
                ContentSource = ContentSource.Page,
                CanEditPages = await User.CanEditPages(project.Id, AuthorizationService)
            };

            return View(model);

        }

    }
}
