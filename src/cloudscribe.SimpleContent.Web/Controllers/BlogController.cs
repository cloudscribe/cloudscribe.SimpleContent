﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2019-07-03
// 

using cloudscribe.DateTimeUtils;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.ViewModels;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Recaptcha;
using cloudscribe.Web.Navigation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
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
    public class BlogController : Controller
    {

        public BlogController(
            IMediator mediator,
            IProjectService projectService,
            IBlogService blogService,
            IBlogUrlResolver blogUrlResolver,
            IBlogRoutes blogRoutes,
            IContentProcessor contentProcessor,
            IProjectEmailService emailService,
            IAuthorizationService authorizationService,
            IAuthorNameResolver authorNameResolver,
            IContentTemplateService templateService,
            IContentHistoryCommands historyCommands,
            IContentHistoryQueries historyQueries,
            ITimeZoneHelper timeZoneHelper,
            ITimeZoneIdResolver timeZoneIdResolver,
            IRecaptchaServerSideValidator recaptchaServerSideValidator,
            IStringLocalizer<SimpleContent> localizer,
            IOptions<BlogEditOptions> configOptionsAccessor,
            ILogger<BlogController> logger
            
            )
        {
            Mediator = mediator;
            ProjectService = projectService;
            BlogService = blogService;
            BlogUrlResolver = blogUrlResolver;
            ContentProcessor = contentProcessor;
            TemplateService = templateService;
            BlogRoutes = blogRoutes;
            AuthorNameResolver = authorNameResolver;
            HistoryCommands = historyCommands;
            HistoryQueries = historyQueries;
            EmailService = emailService;
            AuthorizationService = authorizationService;
            TimeZoneHelper = timeZoneHelper;
            TimeZoneIdResolver = timeZoneIdResolver;
            StringLocalizer = localizer;
            Log = logger;
            EditOptions = configOptionsAccessor.Value;
            RecaptchaServerSideValidator = recaptchaServerSideValidator;
        }

        protected IMediator Mediator { get; private set; }
        protected IProjectService ProjectService { get; private set; }
        protected IBlogService BlogService { get; private set; }
        protected IBlogUrlResolver BlogUrlResolver { get; private set; }
        protected IBlogRoutes BlogRoutes { get; private set; }
        protected IContentTemplateService TemplateService { get; private set; }
        protected IAuthorNameResolver AuthorNameResolver { get; private set; }
        protected IProjectEmailService EmailService { get; private set; }
        protected IContentProcessor ContentProcessor { get; private set; }
        protected ILogger Log { get; private set; }
        protected ITimeZoneHelper TimeZoneHelper { get; private set; }
        protected ITimeZoneIdResolver TimeZoneIdResolver { get; private set; }
        protected IAuthorizationService AuthorizationService { get; private set; }
        protected IStringLocalizer<SimpleContent> StringLocalizer { get; private set; }
        protected BlogEditOptions EditOptions { get; private set; }
        protected IContentHistoryCommands HistoryCommands { get; private set; }
        protected IContentHistoryQueries HistoryQueries { get; private set; }

        protected IRecaptchaServerSideValidator RecaptchaServerSideValidator { get; private set; }

        [HttpHead]
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> Index(
            CancellationToken cancellationToken,
            string category = "",
            int page = 1)
        {
            await BlogService.PublishReadyDrafts(cancellationToken);

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                HttpContext.Response.StatusCode = 404;
                return new EmptyResult();
            }

            var model = new BlogViewModel(ContentProcessor)
            {
                ProjectSettings = project,
                // check if the user has the BlogEditor claim or meets policy
                CanEdit = await User.CanEditBlog(project.Id, AuthorizationService),
                BlogRoutes = BlogRoutes,
                CurrentCategory = category
            };
            
            if(!string.IsNullOrEmpty(model.CurrentCategory))
            {
                model.ListRouteName = BlogRoutes.BlogCategoryRouteName;
            }
            else
            {
                model.ListRouteName = BlogRoutes.BlogIndexRouteName;
            }

            ViewData["Title"] = model.ProjectSettings.Title;
            var result = await BlogService.GetPosts(category, page, model.CanEdit, cancellationToken);
            model.Posts = result.Data;
            model.Categories = await BlogService.GetCategories(model.CanEdit, cancellationToken);
            model.Archives = await BlogService.GetArchives(model.CanEdit, cancellationToken);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems; 
            model.TimeZoneHelper = TimeZoneHelper;
            model.TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId(cancellationToken);
            model.NewItemPath = Url.RouteUrl(BlogRoutes.NewPostRouteName);

            return View("Index", model);
        }

       
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> MostRecent(CancellationToken cancellationToken)
        {
            var project = await ProjectService.GetCurrentProjectSettings();
            if (project == null)
            {
                return RedirectToAction("Index");
            }

            var result = await BlogService.GetRecentPosts(1, cancellationToken);
            if ((result != null) && (result.Count > 0))
            {
                var post = result[0];
                var url = await BlogUrlResolver.ResolvePostUrl(post, project);
                return Redirect(url);
            }

            return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
        }

        [HttpHead]
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> Archive(
            CancellationToken cancellationToken,
            int year,
            int month = 0,
            int day = 0,
            int page = 1)
        {
            var model = new BlogViewModel(ContentProcessor)
            {
                ProjectSettings = await ProjectService.GetCurrentProjectSettings(),
                BlogRoutes = BlogRoutes
            };
            model.CanEdit = await User.CanEditBlog(model.ProjectSettings.Id, AuthorizationService);
            model.NewItemPath = Url.RouteUrl(BlogRoutes.NewPostRouteName);

            ViewData["Title"] = model.ProjectSettings.Title;

            var result = await BlogService.GetPosts(
                model.ProjectSettings.Id,
                year,
                month,
                day,
                page,
                model.ProjectSettings.PostsPerPage,
                model.CanEdit,
                cancellationToken
                );

            model.Posts = result.Data;
            model.Categories = await BlogService.GetCategories(model.CanEdit, cancellationToken);
            model.Archives = await BlogService.GetArchives(model.CanEdit, cancellationToken);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems;
            
            model.TimeZoneHelper = TimeZoneHelper;
            model.TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId(cancellationToken);
            model.Year = year;
            model.Month = month;
            model.Day = day;

            var breadCrumbHelper = new TailCrumbUtility(HttpContext);
            var crumbText = year.ToString() + "-" + month.ToString("00");
            breadCrumbHelper.AddTailCrumb("archive", crumbText, "");

            return View("Archive", model);
        }

        [HttpHead]
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> Category(
            CancellationToken cancellationToken,
            string category = "",
            int page = 1)
        {
            var breadCrumbHelper = new TailCrumbUtility(HttpContext);
            breadCrumbHelper.AddTailCrumb("category", category, "");

            return await Index(cancellationToken, category, page);
        }

        [HttpHead]
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        [ActionName("PostNoDate")]
        public virtual async Task<IActionResult> Post(
             CancellationToken cancellationToken,
            string slug, 
            bool showDraft = false, 
            Guid? historyId = null)
        {
            return await Post(cancellationToken, 0, 0, 0, slug, showDraft, historyId);
        }

        [HttpHead]
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        [ActionName("PostWithDate")]
        public virtual async Task<IActionResult> Post(
            CancellationToken cancellationToken,
            int year , 
            int month, 
            int day, 
            string slug, 
            bool showDraft = false,
            Guid? historyId = null
            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            if(!project.IncludePubDateInPostUrls)
            {
                if(year > 0)
                {
                    return RedirectToRoute(BlogRoutes.PostWithoutDateRouteName, new { slug });
                }
            }

            var canEdit = await User.CanEditBlog(project.Id, AuthorizationService);

            PostResult result = null;
            if(!string.IsNullOrEmpty(slug))
            {
                result = await BlogService.GetPostBySlug(slug, cancellationToken);
            }
            ContentHistory history = null;
            var postWasDeleted = false;
            var hasDraft = false;
            var hasPublishedVersion = false;
            if (result != null && result.Post != null)
            {
                hasDraft = result.Post.HasDraftVersion();
                hasPublishedVersion = result.Post.HasPublishedVersion();
            }

            if (canEdit && historyId.HasValue)
            {
                history = await HistoryQueries.Fetch(project.Id, historyId.Value, cancellationToken);
                if (history != null)
                {
                    if (result == null || result.Post == null) //page must have been deleted, restore from hx
                    {
                        var post = new Post();
                        history.CopyTo(post);
                        if (history.IsDraftHx)
                        {
                            post.PromoteDraftTemporarilyForRender();
                        }
                        postWasDeleted = true;
                        if(result == null)
                        {
                            result = new PostResult();
                        }
                        result.Post = post;
                    }
                    else
                    {
                        var postCopy = new Post();
                        result.Post.CopyTo(postCopy);
                        if (history.IsDraftHx)
                        {
                            postCopy.Content = history.DraftContent;
                            postCopy.Author = history.DraftAuthor;
                        }
                        else
                        {
                            postCopy.Content = history.Content;
                            postCopy.Author = history.Author;
                        }

                        result.Post = postCopy;
                    }
                }
            }
            
            if ((result == null) || (result.Post == null))
            {
                Log.LogWarning("post not found for slug " + slug + ", so redirecting to index");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            if (!canEdit && result != null && result.Post != null)
            {
                if (!result.Post.HasPublishedVersion())
                {
                    Log.LogWarning($"page {result.Post.Title} is unpublished and user is not editor so returning 404");
                    return NotFound();
                }
            }

            var model = new BlogViewModel(ContentProcessor)
            {
                CanEdit = canEdit,
                ShowingDeleted = postWasDeleted,
                HasDraft = hasDraft,
                HasPublishedVersion = hasPublishedVersion
            };

            if(history != null)
            {
                model.HistoryId = history.Id;
                model.HistoryArchiveDate = history.ArchivedUtc;
            }
            
            if (project.IncludePubDateInPostUrls)
            {
                if(year == 0)
                {
                    DateTime? pubDate = null;
                    if(result.Post.PubDate.HasValue)
                    {
                        pubDate = result.Post.PubDate;
                    }
                    else
                    {
                        pubDate = result.Post.DraftPubDate;
                    }

                    if(!pubDate.HasValue)
                    {
                        pubDate = DateTime.UtcNow;
                    }
                    var routeVals = new RouteValueDictionary
                    {
                        { "year", pubDate.Value.Year },
                        { "month", pubDate.Value.Month.ToString("00") },
                        { "day", pubDate.Value.Day.ToString("00") },
                        { "slug", result.Post.Slug }
                    };

                    if (showDraft)
                    {
                        routeVals.Add("showDraft", true);
                    }
                    if(historyId.HasValue)
                    {
                        routeVals.Add("historyId", historyId.Value);
                    }
                    
                    return RedirectToRoute(BlogRoutes.PostWithDateRouteName, routeVals);
                }
            }

            ViewData["Title"] = result.Post.Title;
            
            if(history == null)
            {
                if (canEdit && model.HasDraft && (showDraft || !model.HasPublishedVersion))
                {
                    // we can't update the actual post here since it is cached
                    var postCopy = new Post();
                    result.Post.CopyTo(postCopy);
                    postCopy.PromoteDraftTemporarilyForRender();
                    result.Post = postCopy;
                    model.ShowingDraft = true;
                }
            }
           
            
            model.CurrentPost = result.Post;
            if(result.PreviousPost != null)
            {
                model.PreviousPostUrl = await BlogUrlResolver.ResolvePostUrl(result.PreviousPost, project);
            }
            if (result.NextPost != null)
            {
                model.NextPostUrl = await BlogUrlResolver.ResolvePostUrl(result.NextPost, project);
            }
            
            var currentUrl = await BlogUrlResolver.ResolvePostUrl(result.Post, project);
            var breadCrumbHelper = new TailCrumbUtility(HttpContext);
            breadCrumbHelper.AddTailCrumb(result.Post.Id, result.Post.Title, currentUrl);

            model.NewItemPath = Url.RouteUrl(BlogRoutes.NewPostRouteName);
            model.EditPath = Url.RouteUrl(BlogRoutes.PostEditRouteName, new { slug = result.Post.Slug });

            model.ProjectSettings = project;
            model.BlogRoutes = BlogRoutes;
            model.Categories = await BlogService.GetCategories(model.CanEdit, cancellationToken);
            model.Archives = await BlogService.GetArchives(model.CanEdit, cancellationToken);
            model.ShowComments = result.Post.ShowComments; //mode.Length == 0; // do we need this for a global disable
            model.CommentsAreOpen = await BlogService.CommentsAreOpen(result.Post, canEdit);
            model.TimeZoneHelper = TimeZoneHelper;
            model.TimeZoneId = await TimeZoneIdResolver.GetUserTimeZoneId(cancellationToken);

            if (!string.IsNullOrWhiteSpace(model.CurrentPost.TemplateKey))
            {
                model.Template = await TemplateService.GetTemplate(project.Id, model.CurrentPost.TemplateKey);
            }

            return View("Post", model);
            
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> NewPost(
            CancellationToken cancellationToken,
            string query = null,
            int pageNumber = 1,
            int pageSize = 10
            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditBlog(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var templateCount = await TemplateService.GetCountOfTemplates(project.Id, ProjectConstants.BlogFeatureName);
            if (templateCount == 0)
            {
                return RedirectToRoute(BlogRoutes.PostEditRouteName);
            }

            var templates = await TemplateService.GetTemplates(
                project.Id,
                ProjectConstants.BlogFeatureName,
                query,
                pageNumber,
                pageSize,
                cancellationToken);
            
            var model = new NewContentViewModel()
            {
                Templates = templates,
                Query = query,
                PageNumber = pageNumber,
                PageSize = pageSize,
                CountOfTemplates = templateCount,
                SearchRouteName = BlogRoutes.NewPostRouteName,
                PostActionName = "InitTemplatedPost"
            };
            
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> InitTemplatedPost(NewContentViewModel model)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditBlog(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            if (!ModelState.IsValid)
            {
                model.Templates = await TemplateService.GetTemplates(
                    project.Id,
                    ProjectConstants.BlogFeatureName,
                    model.Query,
                    model.PageNumber,
                    model.PageSize

                    );
                model.SearchRouteName = BlogRoutes.NewPostRouteName;
                model.PostActionName = "InitTemplatedPost";

                return View("NewPost", model);
            }

            var template = await TemplateService.GetTemplate(project.Id, model.SelectedTemplate);

            if (template == null)
            {
                Log.LogWarning($"redirecting to index because content template {model.SelectedTemplate} was not found");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var request = new InitTemplatedPostRequest(
                project.Id,
                User.Identity.Name,
                await AuthorNameResolver.GetAuthorName(User),
                model,
                template);

            var response = await Mediator.Send(request);
            if (response.Succeeded)
            {
                Log.LogDebug($"succeeded in initializing a page with template {model.SelectedTemplate}");
                return RedirectToRoute(BlogRoutes.PostEditWithTemplateRouteName, new { slug = response.Value.Slug });
            }
            else
            {
                if (response.ErrorMessages != null && response.ErrorMessages.Count > 0)
                {
                    foreach (var err in response.ErrorMessages)
                    {
                        this.AlertDanger(err, true);
                    }
                }
            }

            return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
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
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditBlog(project.Id, AuthorizationService);
            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var postResult = await BlogService.GetPostBySlug(slug, cancellationToken);
            ContentHistory history = null;
            var didReplaceDraft = false;
            var didRestoreDeleted = false;
            Post post = null;

            if (historyId.HasValue)
            {
                history = await HistoryQueries.Fetch(project.Id, historyId.Value).ConfigureAwait(false);
                if (history != null)
                {
                    if (postResult == null || postResult.Post == null) // page was deleted, restore it from history
                    {
                        post = new Post();
                        history.CopyTo(post);
                        if (history.IsDraftHx)
                        {
                            post.PromoteDraftTemporarilyForRender();
                        }
                        didRestoreDeleted = true;
                    }
                    else
                    {
                        didReplaceDraft = post.HasDraftVersion();
                        var postCopy = new Post();
                        post.CopyTo(postCopy);
                        if (history.IsDraftHx)
                        {
                            postCopy.DraftAuthor = history.DraftAuthor;
                            postCopy.DraftContent = history.DraftContent;
                            postCopy.DraftSerializedModel = history.DraftSerializedModel;
                        }
                        else
                        {
                            postCopy.DraftAuthor = history.Author;
                            postCopy.DraftContent = history.Content;
                            postCopy.DraftSerializedModel = history.SerializedModel;
                        }
                        if(postResult == null) { postResult = new PostResult(); }
                        postResult.Post = postCopy;
                    }
                }
            }

            if (postResult == null)
            {
                Log.LogError($"redirecting to index because page was not found for slug {slug}");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }
            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], postResult.Post.Title);

            var template = await TemplateService.GetTemplate(project.Id, postResult.Post.TemplateKey);
            if (template == null)
            {
                Log.LogError($"redirecting to index because content template {postResult.Post.TemplateKey} was not found");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var model = new PostEditWithTemplateViewModel()
            {
                ProjectId = project.Id,
                Author = postResult.Post.Author,
                Id = postResult.Post.Id,
                Categories = string.Join(",", postResult.Post.Categories),
                ContentType = postResult.Post.ContentType,
                CorrelationKey = postResult.Post.CorrelationKey,
                CurrentPostUrl = await BlogUrlResolver.ResolvePostUrl(postResult.Post, project).ConfigureAwait(false),
                DeletePostRouteName = BlogRoutes.PostDeleteRouteName,
                ImageUrl = postResult.Post.ImageUrl,
                ThumbnailUrl = postResult.Post.ThumbnailUrl,
                IsFeatured = postResult.Post.IsFeatured,
                IsPublished = postResult.Post.IsPublished,
                ShowComments = postResult.Post.ShowComments,
                MetaDescription = postResult.Post.MetaDescription,
                Slug = postResult.Post.Slug,
                Title = postResult.Post.Title,
                TeaserOverride = postResult.Post.TeaserOverride,
                SuppressTeaser = postResult.Post.SuppressTeaser,
                Template = template,
                TemplateModel = TemplateService.DesrializeTemplateModel(postResult.Post, template),
                DidReplaceDraft = didReplaceDraft,
                DidRestoreDeleted = didRestoreDeleted,
                HasDraft = postResult.Post.HasDraftVersion()
            };
            
            if (history != null)
            {
                model.HistoryArchiveDate = history.ArchivedUtc;
                model.HistoryId = history.Id;
            }

            var tzId = await TimeZoneIdResolver.GetUserTimeZoneId(cancellationToken);

            if (postResult.Post.PubDate.HasValue)
            {
                model.PubDate = TimeZoneHelper.ConvertToLocalTime(postResult.Post.PubDate.Value, tzId);
            }

            if (postResult.Post.DraftPubDate.HasValue)
            {
                model.DraftPubDate = TimeZoneHelper.ConvertToLocalTime(postResult.Post.DraftPubDate.Value, tzId);
            }

            if (!string.IsNullOrWhiteSpace(postResult.Post.DraftAuthor))
            {
                model.Author = postResult.Post.DraftAuthor;
            }

            if (model.TemplateModel == null)
            {
                Log.LogError($"redirecting to index model desrialization failed for page {postResult.Post.Title}");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> EditWithTemplate(PostEditWithTemplateViewModel model)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditBlog(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var post = await BlogService.GetPost(model.Id);

            if (post == null && model.HistoryId.HasValue) // restore a deleted page from history
            {
                var history = await HistoryQueries.Fetch(project.Id, model.HistoryId.Value).ConfigureAwait(false);
                if (history != null)
                {
                    post = new Post();
                    history.CopyTo(post);
                    await BlogService.Create(post); // re-create post here because handler expects existing page and only updates
                }
            }

            if (post == null)
            {
                Log.LogError($"redirecting to index because post was not found for id {model.Id}");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }
            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], post.Title);

            var template = await TemplateService.GetTemplate(project.Id, post.TemplateKey);
            if (template == null)
            {
                Log.LogError($"redirecting to index because content template {post.TemplateKey} was not found");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var request = new UpdateTemplatedPostRequest(
                project.Id,
                User.Identity.Name,
                model,
                template,
                post,
                Request.Form,
                ModelState
                );

            var response = await Mediator.Send(request);
            if (response.Succeeded)
            {
                if (model.SaveMode == SaveMode.DeleteCurrentDraft)
                {
                    this.AlertSuccess(StringLocalizer["The current draft of this post has been deleted."], true);
                }
                else
                {
                    this.AlertSuccess(StringLocalizer["The post was updated successfully."], true);
                }

                if (model.SaveMode == SaveMode.SaveDraftAndPreview)
                {
                    return RedirectToRoute(BlogRoutes.PostEditWithTemplateRouteName, new { slug = response.Value.Slug, preview = "true" });
                }
                else
                {
                    return RedirectToRoute(BlogRoutes.PostWithoutDateRouteName, new { slug = response.Value.Slug });
                }
            }
            else
            {
                model.Template = template;
                
                return View(model);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Edit(
            CancellationToken cancellationToken,
            string slug = "", 
            string type="",
            Guid? historyId = null
            )
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);
            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            if (slug == "none") { slug = string.Empty; }

            var model = new PostEditViewModel
            {
                ProjectId = project.Id,
                TeasersEnabled = project.TeaserMode != TeaserMode.Off
            };

            PostResult postResult = null;
            if (!string.IsNullOrEmpty(slug))
            {
                postResult = await BlogService.GetPostBySlug(slug, cancellationToken);
            }

            var routeVals = new RouteValueDictionary
            {
                { "slug", slug }
            };
            if (historyId.HasValue)
            {
                routeVals.Add("historyId", historyId.Value);
            }

            if (postResult != null && postResult.Post != null && !string.IsNullOrWhiteSpace(postResult.Post.TemplateKey))
            {
                return RedirectToRoute(BlogRoutes.PostEditWithTemplateRouteName, routeVals);
            }

            ContentHistory history = null;
            var hasDraft = false;
            var hasPublishedVersion = false;
            if(postResult != null && postResult.Post != null)
            {
                hasDraft = postResult.Post.HasDraftVersion();
                hasPublishedVersion = postResult.Post.HasPublishedVersion();
            }

            if (historyId.HasValue)
            {
                history = await HistoryQueries.Fetch(project.Id, historyId.Value, cancellationToken);
                if (history != null)
                {
                    if(postResult == null || postResult.Post == null)
                    {
                        var post = new Post();
                        history.CopyTo(post);
                        if(postResult == null)
                        {
                            postResult = new PostResult();
                        }
                        postResult.Post = post;
                        model.DidRestoreDeleted = true;
                    }

                    if (history.IsDraftHx)
                    {
                        model.Author = history.DraftAuthor;
                        model.Content = history.DraftContent;
                    }
                    else
                    {
                        model.Author = history.Author;
                        model.Content = history.Content;
                    }

                    model.HistoryArchiveDate = history.ArchivedUtc;
                    model.HistoryId = history.Id;
                    model.DidReplaceDraft = hasDraft;
                }
            }

            if (postResult == null || postResult.Post == null)
            {
                ViewData["Title"] = StringLocalizer["New Post"];
                model.Author = await AuthorNameResolver.GetAuthorName(User);
                model.CurrentPostUrl = Url.RouteUrl(BlogRoutes.BlogIndexRouteName);
                model.ContentType = project.DefaultContentType;
                if(EditOptions.AllowMarkdown && !string.IsNullOrWhiteSpace(type) && type == "markdown")
                {
                    model.ContentType = "markdown";  
                }
                if(!string.IsNullOrWhiteSpace(type) && type == "html")
                {
                    model.ContentType = "html";
                }
                model.ShowComments = true;
            }
            else
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], postResult.Post.Title);
                
                if(history == null)
                {
                    if (string.IsNullOrWhiteSpace(postResult.Post.DraftContent))
                    {
                        model.Author = postResult.Post.Author;
                        model.Content = postResult.Post.Content;
                    }
                    else
                    {
                        model.Author = postResult.Post.DraftAuthor;
                        model.Content = postResult.Post.DraftContent;
                    }
                }
                
                model.Id = postResult.Post.Id;
                model.CorrelationKey = postResult.Post.CorrelationKey;
                model.IsPublished = postResult.Post.IsPublished;
                model.ShowComments = postResult.Post.ShowComments;
                model.MetaDescription = postResult.Post.MetaDescription;
                model.Slug = postResult.Post.Slug;
                model.Title = postResult.Post.Title;
                model.CurrentPostUrl = await BlogUrlResolver.ResolvePostUrl(postResult.Post, project).ConfigureAwait(false);
                model.DeletePostRouteName = BlogRoutes.PostDeleteRouteName;
                model.Categories = string.Join(",", postResult.Post.Categories);
                model.ImageUrl = postResult.Post.ImageUrl;
                model.ThumbnailUrl = postResult.Post.ThumbnailUrl;
                model.IsFeatured = postResult.Post.IsFeatured;
                model.ContentType = postResult.Post.ContentType;
                model.TeaserOverride = postResult.Post.TeaserOverride;
                model.SuppressTeaser = postResult.Post.SuppressTeaser;
                model.HasDraft = postResult.Post.HasDraftVersion();

                var tzId = await TimeZoneIdResolver.GetUserTimeZoneId(cancellationToken);

                if (postResult.Post.PubDate.HasValue)
                {
                    model.PubDate = TimeZoneHelper.ConvertToLocalTime(postResult.Post.PubDate.Value, tzId);
                }

                if (postResult.Post.DraftPubDate.HasValue)
                {
                    model.DraftPubDate = TimeZoneHelper.ConvertToLocalTime(postResult.Post.DraftPubDate.Value, tzId);
                }
            }
                
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(PostEditViewModel model)
        {
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            IPost post = null;
            if(!string.IsNullOrWhiteSpace(model.Id))
            {
                post = await BlogService.GetPost(model.Id);
            }

            if (post == null && model.HistoryId.HasValue) // restore a deleted post from history
            {
                var history = await HistoryQueries.Fetch(project.Id, model.HistoryId.Value).ConfigureAwait(false);
                if (history != null)
                {
                    post = new Post();
                    history.CopyTo(post);
                    await BlogService.Create(post); // re-create post here so it gets the previous id and keeps previous history
                }
            }

            var isNew = (post == null);

            var request = new CreateOrUpdatePostRequest(
                project.Id,
                User.Identity.Name,
                model,
                post,
                ModelState
                );

            var response = await Mediator.Send(request);
            if(!response.Succeeded)
            {
                // // not effective here...:
                //if (model.SaveMode == SaveMode.DeleteCurrentDraft)
                //    this.AlertSuccess(StringLocalizer["The current draft of this post has been deleted."], true);
                //else
                //    this.AlertSuccess(StringLocalizer["The post was updated successfully."], true);

                if (string.IsNullOrEmpty(model.Id))
                {
                    ViewData["Title"] = StringLocalizer["New Post"];
                }
                else
                {
                    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], model.Title);
                }
                model.ProjectId = project.Id;
                model.TeasersEnabled = project.TeaserMode != TeaserMode.Off;

                return View(model);
            }

            if (project.IncludePubDateInPostUrls)
            {
                DateTime? pubDate = null;
                if (response.Value.PubDate.HasValue)
                {
                    pubDate = response.Value.PubDate;
                }
                else
                {
                    pubDate = response.Value.DraftPubDate;
                }

                if (!pubDate.HasValue)
                {
                    pubDate = DateTime.UtcNow;
                }

                if (model.SaveMode == SaveMode.SaveDraftAndPreview)
                {
                    return RedirectToRoute(BlogRoutes.PostEditRouteName, new { slug = response.Value.Slug, preview = "true" });
                }
                else
                {
                    return RedirectToRoute(BlogRoutes.PostWithDateRouteName,
                    new
                    {
                        year = pubDate.Value.Year,
                        month = pubDate.Value.Month.ToString("00"),
                        day = pubDate.Value.Day.ToString("00"),
                        slug = response.Value.Slug
                    });
                }
            }
            else
            {
                if (model.SaveMode == SaveMode.SaveDraftAndPreview)
                {
                    return RedirectToRoute(BlogRoutes.PostEditRouteName, new { slug = response.Value.Slug, preview = "true" });
                }
                else
                {
                    return RedirectToRoute(BlogRoutes.PostWithoutDateRouteName,
                    new { slug = response.Value.Slug });
                }
            }
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Log.LogInformation("postid not provided, redirecting");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("project settings not found, redirecting");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            bool canEdit = await User.CanEditBlog(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("user is not allowed to edit, redicrecting");

                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }
            
            var post = await BlogService.GetPost(id);

            if (post == null)
            {
                Log.LogInformation("post not found, redirecting");

                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }
            
            var history = post.CreateHistory(User.Identity.Name, true);
            await HistoryCommands.Create(project.Id, history);

            await BlogService.Delete(post.Id);

            Log.LogWarning("user " + User.Identity.Name + " deleted post " + post.Slug);

            return RedirectToRoute(BlogRoutes.BlogIndexRouteName);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> UnPublish(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Log.LogInformation("postid not provided, redirecting");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogInformation("project settings not found, redirecting");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            bool canEdit = await User.CanEditBlog(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogInformation("user is not allowed to edit, redicrecting");

                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var post = await BlogService.GetPost(id);

            if (post == null)
            {
                Log.LogInformation("post not found, redirecting");

                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var history = post.CreateHistory(User.Identity.Name, true);
            await HistoryCommands.Create(project.Id, history);
            if(post.HasPublishedVersion())
            {
                await BlogService.FireUnPublishEvent(post);
                post.DraftAuthor = post.Author;
                post.DraftContent = post.Content;
                post.DraftSerializedModel = post.SerializedModel;
                post.Content = null;
                post.SerializedModel = null;
            }
            
            post.DraftPubDate = null;
            post.PubDate = null;
            post.IsPublished = false;

            await BlogService.Update(post);

            Log.LogWarning("user " + User.Identity.Name + " unpublished post " + post.Title);

            return RedirectToRoute(BlogRoutes.PostWithoutDateRouteName, new { slug = post.Slug });

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
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, AuthorizationService);

            if (!canEdit)
            {
                Log.LogWarning("user is not allowed to edit, redirecting/rejecting");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var post = await BlogService.GetPost(id);

            if (post == null)
            {
                Log.LogInformation($"page not found for {id}, redirecting/rejecting");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
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

            return RedirectToRoute(BlogRoutes.PostHistoryRouteName, new { slug = post.Slug });
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

            var result = await BlogService.GetPostBySlug(slug);

            if ((result == null) || (result.Post == null))
            {
                Log.LogWarning("post not found for slug " + slug + ", so redirecting to index");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var model = new ContentHistoryViewModel()
            {
                History = await HistoryQueries.GetByContent(
                    project.Id,
                    result.Post.Id,
                    pageNumber,
                    pageSize,
                    cancellationToken),
                ContentId = result.Post.Id,
                ContentSource = ContentSource.Blog,
                ContentTitle = result.Post.Title,
                ContentSlug = result.Post.Slug,
                CanEditPosts = await User.CanEditBlog(project.Id, AuthorizationService)
            };

            return View(model);
        }
        
        [HttpPost]
        [Authorize(Policy = "BlogViewPolicy")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AjaxPostComment(CommentViewModel model)
        {
            // disable status code page for ajax requests
            var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
            if (statusCodePagesFeature != null)
            {
                statusCodePagesFeature.Enabled = false;
            }

            // this should validate the [EmailAddress] on the model
            // failure here should indicate invalid email since it is the only attribute in use
            if (!ModelState.IsValid)
            {  
                Response.StatusCode = 403;
                //await Response.WriteAsync("Please enter a valid e-mail address");
                return Content(StringLocalizer["Please enter a valid e-mail address"]);
            }
            
            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogDebug("returning 500 blog not found");
                return StatusCode(500);
            }

            if (string.IsNullOrEmpty(model.PostId))
            {
                Log.LogDebug("returning 500 because no postid was posted");
                return StatusCode(500);
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                Log.LogDebug("returning 403 because no name was posted");
                Response.StatusCode = 403;
                //await Response.WriteAsync("Please enter a valid name");
                return Content("Please enter a valid name");
            }

            if (string.IsNullOrEmpty(model.Content))
            {
                Log.LogDebug("returning 403 because no content was posted");
                Response.StatusCode = 403;
                //await Response.WriteAsync("Please enter a valid content");
                return Content("Please enter a valid content");
            }

            var blogPost = await BlogService.GetPost(model.PostId);

            if (blogPost == null)
            {
                Log.LogDebug("returning 500 blog post not found");
                return StatusCode(500);
            }

            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                if(!string.IsNullOrEmpty(project.RecaptchaPublicKey))
                {
                    var captchaResponse = await RecaptchaServerSideValidator.ValidateRecaptcha(Request, project.RecaptchaPrivateKey);
                    if (!captchaResponse.Success)
                    {
                        Log.LogDebug("returning 403 captcha validation failed");
                        Response.StatusCode = 403;
                        //await Response.WriteAsync("captcha validation failed");
                        return Content("captcha validation failed");
                    }
                }
            }

            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var canEdit = await User.CanEditBlog(project.Id, AuthorizationService);
            
            var isApproved = canEdit;
            if (!isApproved) isApproved = !project.ModerateComments;

            var comment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                Author = model.Name,
                Email = model.Email,
                Website = GetUrl(model.WebSite),
                Ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                UserAgent = userAgent,
                IsAdmin = User.CanEditProject(project.Id),
                Content = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(
                    model.Content.Trim()).Replace("\n", "<br />"),

                IsApproved = isApproved,
                PubDate = DateTime.UtcNow
            };
            
            blogPost.Comments.Add(comment);
            await BlogService.Update(blogPost);

            // TODO: clear cache

            //no need to send notification when project owner posts a comment, ie in response
            var shouldSendEmail = !canEdit;
       
            if(shouldSendEmail)
            {
                var postUrl = await BlogUrlResolver.ResolvePostUrl(blogPost, project);
                var baseUrl = string.Concat(HttpContext.Request.Scheme,
                        "://",
                        HttpContext.Request.Host.ToUriComponent());

                postUrl = baseUrl + postUrl;

                EmailService.SendCommentNotificationEmailAsync(
                    project,
                    blogPost,
                    comment,
                    postUrl,
                    postUrl,
                    postUrl
                    ).Forget(); //async but don't want to wait
            }

            var viewModel = new BlogViewModel(ContentProcessor)
            {
                ProjectSettings = project,
                BlogRoutes = BlogRoutes,
                CurrentPost = blogPost,
                TmpComment = comment,
                TimeZoneHelper = TimeZoneHelper,
                TimeZoneId = project.TimeZoneId,
                CanEdit = canEdit
            };
            

            return PartialView("CommentPartial", viewModel);
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AjaxApproveComment(string postId, string commentId)
        {
            // disable status code page for ajax requests
            var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
            if (statusCodePagesFeature != null)
            {
                statusCodePagesFeature.Enabled = false;
            }

            if (string.IsNullOrEmpty(postId))
            {
                Log.LogDebug("returning 404 because no postid was posted");
               
                return StatusCode(404);
            }

            if (string.IsNullOrEmpty(commentId))
            {
                Log.LogDebug("returning 404 because no commentid was posted");
                //Response.StatusCode = 404;
                // await Response.WriteAsync("Comm");
                return StatusCode(404);
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogDebug("returning 500 blog not found");
                //Response.StatusCode = 500;
                return StatusCode(500);
            }

            bool canEdit = await User.CanEditBlog(project.Id, AuthorizationService);
            
            if (!canEdit)
            {
                Log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            var blogPost = await BlogService.GetPost(postId);

            if (blogPost == null)
            {
                Log.LogDebug("returning 404 blog post not found");
                return StatusCode(404);
            }

            var comment = blogPost.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                Log.LogDebug("returning 404 comment not found");
                return StatusCode(404);
            }

            comment.IsApproved = true;
            await BlogService.Update(blogPost);

            return StatusCode(200);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AjaxDeleteComment(string postId, string commentId)
        {
            // disable status code page for ajax requests
            var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
            if (statusCodePagesFeature != null)
            {
                statusCodePagesFeature.Enabled = false;
            }

            if (string.IsNullOrEmpty(postId))
            {
                Log.LogDebug("returning 404 because no postid was posted");
                return StatusCode(404);
            }

            if (string.IsNullOrEmpty(commentId))
            {
                Log.LogDebug("returning 404 because no commentid was posted");
                return StatusCode(404);
            }

            var project = await ProjectService.GetCurrentProjectSettings();

            if (project == null)
            {
                Log.LogDebug("returning 404 blog not found");
                return StatusCode(404);
            }

            bool canEdit = await User.CanEditBlog(project.Id, AuthorizationService);
            
            if (!canEdit)
            {
                Log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            var blogPost = await BlogService.GetPost(postId);

            if (blogPost == null)
            {
                Log.LogDebug("returning 404 blog post not found");
                return StatusCode(404);
            }

            var comment = blogPost.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                Log.LogDebug("returning 404 comment not found");
                return StatusCode(404);
            }

            //comment.IsApproved = true;
            //blogPost.Comments.Remove(comment);
            var copyOfComments = blogPost.Comments.ToList();
            for (var i =0; i < copyOfComments.Count; i++)
            {
                if(copyOfComments[i].Id == commentId)
                {
                    copyOfComments.RemoveAt(i);
                }
            }
            blogPost.Comments = copyOfComments;
            await BlogService.Update(blogPost);

            return StatusCode(200);
        }

        [HttpGet]
        [Authorize]
        public virtual async Task<IActionResult> CanEdit(CancellationToken cancellationToken)
        {
            var project = await ProjectService.GetCurrentProjectSettings();
            if (project == null)
            {
                Log.LogError("project settings not found returning 404");
                return NotFound();
            }

            var canEdit = await User.CanEditBlog(project.Id, AuthorizationService);

            return Ok(canEdit);
        }

        protected string GetUrl(string website)
        {
            if(string.IsNullOrEmpty(website)) { return string.Empty; }

            if (!website.Contains("://"))
                website = "http://" + website;

            if (Uri.TryCreate(website, UriKind.Absolute, out Uri url))
                return url.ToString();

            return string.Empty;
        }
  

    }
}
