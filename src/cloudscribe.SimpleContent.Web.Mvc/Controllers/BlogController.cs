// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-02-05
// 


using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.SimpleContent.Web.ViewModels;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using cloudscribe.SimpleContent.Web.Config;
using cloudscribe.SimpleContent.Web.Services;

namespace cloudscribe.SimpleContent.Web.Mvc.Controllers
{
    public class BlogController : Controller
    {

        public BlogController(
            IProjectService projectService,
            IBlogService blogService,
            IBlogRoutes blogRoutes,
            IContentProcessor contentProcessor,
            IProjectEmailService emailService,
            IAuthorizationService authorizationService,
            IAuthorNameResolver authorNameResolver,
            ITimeZoneHelper timeZoneHelper,
            IStringLocalizer<SimpleContent> localizer,
            IOptions<SimpleContentConfig> configOptionsAccessor,
            ILogger<BlogController> logger
            
            )
        {
            this.projectService = projectService;
            this.blogService = blogService;
            this.contentProcessor = contentProcessor;
            this.blogRoutes = blogRoutes;
            this.authorNameResolver = authorNameResolver;
            this.emailService = emailService;
            this.authorizationService = authorizationService;
            this.timeZoneHelper = timeZoneHelper;
            
            sr = localizer;
            log = logger;
            config = configOptionsAccessor.Value;
        }

        private IProjectService projectService;
        private IBlogService blogService;
        private IBlogRoutes blogRoutes;
        private IAuthorNameResolver authorNameResolver;
        private IProjectEmailService emailService;
        private IContentProcessor contentProcessor;
        private ILogger log;
        private ITimeZoneHelper timeZoneHelper;
        private IAuthorizationService authorizationService;
        private IStringLocalizer<SimpleContent> sr;
        private SimpleContentConfig config;
        

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string category = "",
            int page = 1)
        {
            var projectSettings = await projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                HttpContext.Response.StatusCode = 404;
                return new EmptyResult();
            }

            var model = new BlogViewModel(contentProcessor);
            model.ProjectSettings = projectSettings;
            // check if the user has the BlogEditor claim or meets policy
            model.CanEdit = await User.CanEditBlog(projectSettings.Id, authorizationService);
            model.ProjectSettings = projectSettings;
            model.BlogRoutes = blogRoutes;
            model.CurrentCategory = category;
            if(!string.IsNullOrEmpty(model.CurrentCategory))
            {
                model.ListRouteName = blogRoutes.BlogCategoryRouteName;
            }
            else
            {
                model.ListRouteName = blogRoutes.BlogIndexRouteName;
            }

            ViewData["Title"] = model.ProjectSettings.Title;
            var result = await blogService.GetPosts(category, page, model.CanEdit);
            model.Posts = result.Data;
            model.Categories = await blogService.GetCategories(model.CanEdit);
            model.Archives = await blogService.GetArchives(model.CanEdit);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems; 
            model.TimeZoneHelper = timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;
            model.NewItemPath = Url.RouteUrl(blogRoutes.PostEditRouteName, new { slug = "" });

            return View("Index", model);
        }

       
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MostRecent()
        {
            var projectSettings = await projectService.GetCurrentProjectSettings();


            if (projectSettings == null)
            {
                return RedirectToAction("Index");
            }

            var result = await blogService.GetRecentPosts(1);
            if ((result != null) && (result.Count > 0))
            {
                var post = result[0];
                var url = await blogService.ResolvePostUrl(post);
                return Redirect(url);

            }

            return RedirectToRoute(blogRoutes.BlogIndexRouteName);
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Archive(
            int year,
            int month = 0,
            int day = 0,
            int page = 1)
        {

            var model = new BlogViewModel(contentProcessor);
            model.ProjectSettings = await projectService.GetCurrentProjectSettings();
            model.BlogRoutes = blogRoutes;
            model.CanEdit = await User.CanEditBlog(model.ProjectSettings.Id, authorizationService);
            model.NewItemPath = Url.RouteUrl(blogRoutes.PostEditRouteName, new { slug = "" });

            ViewData["Title"] = model.ProjectSettings.Title;

            var result = await blogService.GetPosts(
                model.ProjectSettings.Id,
                year,
                month,
                day,
                page,
                model.ProjectSettings.PostsPerPage,
                model.CanEdit
                );

            model.Posts = result.Data;
            model.Categories = await blogService.GetCategories(model.CanEdit);
            model.Archives = await blogService.GetArchives(model.CanEdit);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems;
            
            model.TimeZoneHelper = timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;
            model.Year = year;
            model.Month = month;
            model.Day = day;

            return View("Archive", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Category(
            string category = "",
            int page = 1)
        {
            return await Index(category, page);
        }

        //[HttpGet]
        //public async Task<IActionResult> New()
        //{
        //    return await Post(0, 0, 0, "", "new");
        //}

        [HttpGet]
        [AllowAnonymous]
        [ActionName("PostNoDate")]
        public async Task<IActionResult> Post(string slug)
        {
            return await Post(0, 0, 0, slug);
        }

        [HttpGet]
        [AllowAnonymous]
        [ActionName("PostWithDate")]
        public async Task<IActionResult> Post(int year , int month, int day, string slug)
        {
            var projectSettings = await projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            if(!projectSettings.IncludePubDateInPostUrls)
            {
                if(year > 0)
                {
                    //TODO: an option for permanent redirect
                    return RedirectToRoute(blogRoutes.PostWithoutDateRouteName, new { slug = slug });
                }
            }

            var canEdit = await User.CanEditBlog(projectSettings.Id, authorizationService);
            
            //var isNew = false;
            PostResult result = null;
            if(!string.IsNullOrEmpty(slug))
            {
                result = await blogService.GetPostBySlug(slug);
            }
            
            var model = new BlogViewModel(contentProcessor);
            model.CanEdit = canEdit;

            if ((result == null)||(result.Post == null))
            {
                log.LogWarning("post not found for slug " + slug + ", so redirecting to index");
                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }
            else
            {
               if(projectSettings.IncludePubDateInPostUrls)
                {
                    if(year == 0)
                    {
                        //TODO: option whether to use permanent redirect
                        return RedirectToRoute(blogRoutes.PostWithDateRouteName, 
                            new {
                                year = result.Post.PubDate.Year,
                                month = result.Post.PubDate.Month.ToString("00"),
                                day = result.Post.PubDate.Day.ToString("00"),
                                slug = result.Post.Slug
                            });
                    }
                }

                ViewData["Title"] = result.Post.Title;
            }

            
            model.CurrentPost = result.Post;
            if(result.PreviousPost != null)
            {
                model.PreviousPostUrl = await blogService.ResolvePostUrl(result.PreviousPost);
            }
            if (result.NextPost != null)
            {
                model.NextPostUrl = await blogService.ResolvePostUrl(result.NextPost);
            }

            

            var currentUrl = await blogService.ResolvePostUrl(result.Post);
            var breadCrumbHelper = new TailCrumbUtility(HttpContext);
            breadCrumbHelper.AddTailCrumb(result.Post.Id, result.Post.Title, currentUrl);

            model.NewItemPath = Url.RouteUrl(blogRoutes.PostEditRouteName, new { slug = "" });
            model.EditPath = Url.RouteUrl(blogRoutes.PostEditRouteName, new { slug = result.Post.Slug });

            model.ProjectSettings = projectSettings;
            model.BlogRoutes = blogRoutes;
            model.Categories = await blogService.GetCategories(model.CanEdit);
            model.Archives = await blogService.GetArchives(model.CanEdit);
            model.ShowComments = true; //mode.Length == 0; // do we need this for a global disable
            model.CommentsAreOpen = await blogService.CommentsAreOpen(result.Post, canEdit);
            //model.ApprovedCommentCount = post.Comments.Where(c => c.IsApproved == true).Count();
            model.TimeZoneHelper = timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;

            if (!canEdit)
            {           
                if((!model.CurrentPost.IsPublished) || model.CurrentPost.PubDate > DateTime.UtcNow)
                {
                    return RedirectToRoute(blogRoutes.BlogIndexRouteName);
                }
            }

            return View("Post", model);
            

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(string slug = "", string type="")
        {
            var projectSettings = await projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(projectSettings.Id, authorizationService);
            if (!canEdit)
            {
                log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            if (slug == "none") { slug = string.Empty; }

            var model = new PostEditViewModel();
            model.ProjectId = projectSettings.Id;
            model.TeasersEnabled = projectSettings.AutoTeaserMode == AutoTeaserMode.On;
            
            PostResult postResult = null;
            if (!string.IsNullOrEmpty(slug))
            {
                postResult = await blogService.GetPostBySlug(slug);
            }
            if (postResult== null || postResult.Post == null)
            {
                ViewData["Title"] = sr["New Post"];
                model.Author = await authorNameResolver.GetAuthorName(User);
                model.IsPublished = true;
                model.PubDate = timeZoneHelper.ConvertToLocalTime(DateTime.UtcNow, projectSettings.TimeZoneId).ToString();
                model.CurrentPostUrl = Url.RouteUrl(blogRoutes.BlogIndexRouteName);
                model.ContentType = projectSettings.DefaultContentType;
                if(config.AllowMarkdown && !string.IsNullOrWhiteSpace(type) && type == "markdown")
                {
                    model.ContentType = "markdown";  
                }
                if(!string.IsNullOrWhiteSpace(type) && type == "html")
                {
                    model.ContentType = "html";
                }
            }
            else
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["Edit - {0}"], postResult.Post.Title);
                model.Author = postResult.Post.Author;
                model.Content = postResult.Post.Content;
                model.Id = postResult.Post.Id;
                model.CorrelationKey = postResult.Post.CorrelationKey;
                model.IsPublished = postResult.Post.IsPublished;
                model.MetaDescription = postResult.Post.MetaDescription;
                model.PubDate = timeZoneHelper.ConvertToLocalTime(postResult.Post.PubDate, projectSettings.TimeZoneId).ToString();
                model.Slug = postResult.Post.Slug;
                model.Title = postResult.Post.Title;
                model.CurrentPostUrl = await blogService.ResolvePostUrl(postResult.Post).ConfigureAwait(false);
                model.DeletePostRouteName = blogRoutes.PostDeleteRouteName;
                model.Categories = string.Join(",", postResult.Post.Categories);
                model.ImageUrl = postResult.Post.ImageUrl;
                model.ThumbnailUrl = postResult.Post.ThumbnailUrl;
                model.IsFeatured = postResult.Post.IsFeatured;
                model.ContentType = postResult.Post.ContentType;
                model.TeaserOverride = postResult.Post.TeaserOverride;
                model.SuppressAutoTeaser = postResult.Post.SuppressAutoTeaser;
            }


            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditViewModel model)
        {
            var project = await projectService.GetCurrentProjectSettings();

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    ViewData["Title"] = sr["New Post"];
                }
                else
                {
                    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["Edit - {0}"], model.Title);
                }
                model.ProjectId = project.Id;
                model.TeasersEnabled = project.AutoTeaserMode == AutoTeaserMode.On;

                return View(model);
            }

           
            

            if (project == null)
            {
                log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, authorizationService);

            if (!canEdit)
            {
                log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            var categories = new List<string>();

            if (!string.IsNullOrEmpty(model.Categories))
            {
                if(config.ForceLowerCaseCategories)
                {
                    categories = model.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim().ToLower())
                    .Where(x =>
                    !string.IsNullOrWhiteSpace(x)
                    && x != ","
                    )
                    .Distinct()
                    .ToList();
                }
                else
                {
                    categories = model.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())
                    .Where(x =>
                    !string.IsNullOrWhiteSpace(x)
                    && x != ","
                    )
                    .Distinct()
                    .ToList();
                }
                
            }


            IPost post = null;
            if (!string.IsNullOrEmpty(model.Id))
            {
                post = await blogService.GetPost(model.Id);
            }

            
            var isNew = false;
            bool slugAvailable;
            string slug;
            if (post != null)
            {
                post.Title = model.Title;
                post.MetaDescription = model.MetaDescription;
                post.Content = model.Content;
                post.Categories = categories;
                if(model.Slug != post.Slug)
                {
                    // remove any bad chars
                    model.Slug = ContentUtils.CreateSlug(model.Slug);
                    slugAvailable = await blogService.SlugIsAvailable(project.Id, model.Slug);
                    if(slugAvailable)
                    {
                        post.Slug = model.Slug;
                    }
                    else
                    {
                        //log.LogWarning($"slug {model.Slug} was requested but not changed because it is already in use");
                        this.AlertDanger(sr["The post slug was not changed because the requested slug is already in use."], true);
                    }
                }
            }
            else
            {
                isNew = true;
                if(!string.IsNullOrEmpty(model.Slug))
                {
                    // remove any bad chars
                    model.Slug = ContentUtils.CreateSlug(model.Slug);
                    slug = model.Slug;
                    slugAvailable = await blogService.SlugIsAvailable(project.Id, slug);
                    if(!slugAvailable)
                    {
                        slug = ContentUtils.CreateSlug(model.Title);
                    }
                }
                else
                {
                    slug = ContentUtils.CreateSlug(model.Title);
                }
                
                slugAvailable = await blogService.SlugIsAvailable(project.Id, slug);
                if (!slugAvailable)
                {
                    //log.LogInformation("returning 409 because slug already in use");
                    ModelState.AddModelError("postediterror", sr["slug is already in use."]);

                    return View(model);
                }

                post = new Post()
                {
                    BlogId = project.Id,
                    Author = await authorNameResolver.GetAuthorName(User),
                    Title = model.Title,
                    MetaDescription = model.MetaDescription,
                    Content = model.Content,
                    Slug = slug
                    ,Categories = categories.ToList()
                };
            }
            if(!string.IsNullOrEmpty(model.Author))
            {
                post.Author = model.Author;
            }
            
            post.IsPublished = model.IsPublished;
            post.CorrelationKey = model.CorrelationKey;
            post.ImageUrl = model.ImageUrl;
            post.ThumbnailUrl = model.ThumbnailUrl;
            post.IsFeatured = model.IsFeatured;
            post.ContentType = model.ContentType;

            post.TeaserOverride = model.TeaserOverride;
            post.SuppressAutoTeaser = model.SuppressAutoTeaser;

            if (!string.IsNullOrEmpty(model.PubDate))
            {
                var localTime = DateTime.Parse(model.PubDate);
                post.PubDate = timeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);

            }

            if (isNew)
            {
                await blogService.Create(post);
            }
            else
            {
                await blogService.Update(post);
            }

            if (project.IncludePubDateInPostUrls)
            {
                return RedirectToRoute(blogRoutes.PostWithDateRouteName,
                    new
                    {
                        year = post.PubDate.Year,
                        month = post.PubDate.Month.ToString("00"),
                        day = post.PubDate.Day.ToString("00"),
                        slug = post.Slug
                    }); 
            }
            else
            {
                return RedirectToRoute(blogRoutes.PostWithoutDateRouteName,
                    new { slug = post.Slug });  
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogInformation("project settings not found, redirecting");
                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            bool canEdit = await User.CanEditBlog(project.Id, authorizationService);

            if (!canEdit)
            {
                log.LogInformation("user is not allowed to edit, redicrecting");

                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            if (string.IsNullOrEmpty(id))
            {
                log.LogInformation("postid not provided, redirecting");
                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }

            var post = await blogService.GetPost(id);

            if (post == null)
            {
                log.LogInformation("post not found, redirecting");

                return RedirectToRoute(blogRoutes.BlogIndexRouteName);
            }
            log.LogWarning("user " + User.Identity.Name + " deleted post " + post.Slug);

            await blogService.Delete(post.Id);
            
            return RedirectToRoute(blogRoutes.BlogIndexRouteName);

        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AjaxPost(PostViewModel model)
        //{
        //    // disable status code page for ajax requests
        //    var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
        //    if (statusCodePagesFeature != null)
        //    {
        //        statusCodePagesFeature.Enabled = false;
        //    }

        //    if (string.IsNullOrEmpty(model.Title))
        //    {
        //        log.LogInformation("returning 500 because no title was posted");
        //        return StatusCode(500);
        //    }
            
        //    var project = await projectService.GetCurrentProjectSettings();

        //    if (project == null)
        //    {
        //        log.LogInformation("returning 500 blog not found");
        //        return StatusCode(500);
        //    }

        //    bool canEdit = await User.CanEditBlog(project.Id, authorizationService);
            

        //    if (!canEdit)
        //    {
        //        log.LogInformation("returning 403 user is not allowed to edit");
        //        return StatusCode(403);
        //    }

        //    var categories = new List<string>();

        //    if (!string.IsNullOrEmpty(model.Categories))
        //    {
        //        categories = model.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim().ToLower())
        //            .Where(x => 
        //            !string.IsNullOrWhiteSpace(x)
        //            && x != ","
        //            )
        //            .Distinct()
        //            .ToList();
        //    }

        //    IPost post = null;
        //    if (!string.IsNullOrEmpty(model.Id))
        //    {
        //        post = await blogService.GetPost(model.Id);
        //    }

        //    var isNew = false;
        //    if (post != null)
        //    {
        //        post.Title = model.Title;
        //        post.MetaDescription = model.MetaDescription;
        //        post.Content = model.Content;
        //        post.Categories = categories;
        //    }
        //    else
        //    {
        //        isNew = true;
        //        var slug = blogService.CreateSlug(model.Title);
        //        var available = await blogService.SlugIsAvailable(slug);
        //        if (!available)
        //        {
        //            log.LogInformation("returning 409 because slug already in use");
                    
        //            return StatusCode(409);
        //        }

        //        post = new Post()
        //        {
        //            BlogId = project.Id,
        //            Author = User.GetUserDisplayName(),
        //            Title = model.Title,
        //            MetaDescription = model.MetaDescription,
        //            Content = model.Content,
        //            Slug = slug,
        //            Categories = categories.ToList()
        //        };
        //    }

        //    post.IsPublished = model.IsPublished;
        //    if(!string.IsNullOrEmpty(model.PubDate))
        //    {
        //        var localTime = DateTime.Parse(model.PubDate);
        //        var pubDate = timeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);

        //        if (!isNew)
        //        {
        //            if (pubDate != post.PubDate)
        //            {
                        
        //                await blogService.HandlePubDateAboutToChange(post, pubDate).ConfigureAwait(false);
        //            }
        //        }
        //        post.PubDate = pubDate;
               
        //    }

        //    try
        //    {
        //        if (isNew)
        //        {
        //            await blogService.Create(post);
        //        }
        //        else
        //        {
        //            await blogService.Update(post);
        //        }

                
        //        string url;
        //        if (project.IncludePubDateInPostUrls)
        //        {
        //            url = Url.Link(blogRoutes.PostWithDateRouteName,
        //                new
        //                {
        //                    year = post.PubDate.Year,
        //                    month = post.PubDate.Month.ToString("00"),
        //                    day = post.PubDate.Date.Day.ToString("00"),
        //                    slug = post.Slug
        //                });
        //        }
        //        else
        //        {
        //            url = Url.Link(blogRoutes.PostWithoutDateRouteName, new { slug = post.Slug });
        //        }

        //        //Response.StatusCode = 200;
        //        //await Response.WriteAsync(url);
        //        return Content(url);
                
        //    }
        //    catch(Exception ex)
        //    {
        //        log.LogError("ajax post failed with exception " + ex.Message + " " + ex.StackTrace, ex);
                
        //        return StatusCode(500);
        //    }
            
            
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

        //    bool canEdit = await User.CanEditBlog(project.Id, authorizationService);
            
        //    if (!canEdit)
        //    {
        //        log.LogInformation("returning 403 user is not allowed to edit");
                
        //        return StatusCode(403);
        //    }

        //    if (string.IsNullOrEmpty(id))
        //    {
        //        log.LogInformation("returning 404 postid not provided");
        //        Response.StatusCode = 404;
        //        return StatusCode(404);
        //    }

        //    var post = await blogService.GetPost(id);

        //    if (post == null)
        //    {
        //        log.LogInformation("returning 404 not found");
                
        //        return StatusCode(404);
        //    }
        //    log.LogWarning("user " + User.Identity.Name + " deleted post " + post.Slug);

        //    await blogService.Delete(post.Id);

        //    // TODO: clear cache

        //    //Response.StatusCode = 200;
        //    //return; //new EmptyResult();
        //    return StatusCode(200);

        //}

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxPostComment(CommentViewModel model)
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
                return Content(sr["Please enter a valid e-mail address"]);
            }
            
            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogDebug("returning 500 blog not found");
                return StatusCode(500);
            }

            if (string.IsNullOrEmpty(model.PostId))
            {
                log.LogDebug("returning 500 because no postid was posted");
                return StatusCode(500);
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                log.LogDebug("returning 403 because no name was posted");
                Response.StatusCode = 403;
                //await Response.WriteAsync("Please enter a valid name");
                return Content("Please enter a valid name");
            }

            if (string.IsNullOrEmpty(model.Content))
            {
                log.LogDebug("returning 403 because no content was posted");
                Response.StatusCode = 403;
                //await Response.WriteAsync("Please enter a valid content");
                return Content("Please enter a valid content");
            }

            var blogPost = await blogService.GetPost(model.PostId);

            if (blogPost == null)
            {
                log.LogDebug("returning 500 blog post not found");
                return StatusCode(500);
            }

            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                if(!string.IsNullOrEmpty(project.RecaptchaPublicKey))
                {
                    var captchaResponse = await this.ValidateRecaptcha(Request, project.RecaptchaPrivateKey);
                    if (!captchaResponse.Success)
                    {
                        log.LogDebug("returning 403 captcha validation failed");
                        Response.StatusCode = 403;
                        //await Response.WriteAsync("captcha validation failed");
                        return Content("captcha validation failed");
                    }
                }
            }

            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var canEdit = await User.CanEditBlog(project.Id, authorizationService);
            
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
            await blogService.Update(blogPost);

            // TODO: clear cache

            //no need to send notification when project owner posts a comment, ie in response
            var shouldSendEmail = !canEdit;
       
            if(shouldSendEmail)
            {
                var postUrl = await blogService.ResolvePostUrl(blogPost);
                var baseUrl = string.Concat(HttpContext.Request.Scheme,
                        "://",
                        HttpContext.Request.Host.ToUriComponent());

                postUrl = baseUrl + postUrl;

                emailService.SendCommentNotificationEmailAsync(
                    project,
                    blogPost,
                    comment,
                    postUrl,
                    postUrl,
                    postUrl
                    ).Forget(); //async but don't want to wait
            }
            
            var viewModel = new BlogViewModel(contentProcessor);
            viewModel.ProjectSettings = project;
            viewModel.BlogRoutes = blogRoutes;
            viewModel.CurrentPost = blogPost;
            viewModel.TmpComment = comment;
            viewModel.TimeZoneHelper = timeZoneHelper;
            viewModel.TimeZoneId = viewModel.ProjectSettings.TimeZoneId;

            
            viewModel.CanEdit = canEdit;

            return PartialView("CommentPartial", viewModel);
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxApproveComment(string postId, string commentId)
        {
            // disable status code page for ajax requests
            var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
            if (statusCodePagesFeature != null)
            {
                statusCodePagesFeature.Enabled = false;
            }

            if (string.IsNullOrEmpty(postId))
            {
                log.LogDebug("returning 404 because no postid was posted");
               
                return StatusCode(404);
            }

            if (string.IsNullOrEmpty(commentId))
            {
                log.LogDebug("returning 404 because no commentid was posted");
                //Response.StatusCode = 404;
                // await Response.WriteAsync("Comm");
                return StatusCode(404);
            }

            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogDebug("returning 500 blog not found");
                //Response.StatusCode = 500;
                return StatusCode(500);
            }

            bool canEdit = await User.CanEditBlog(project.Id, authorizationService);
            
            if (!canEdit)
            {
                log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            var blogPost = await blogService.GetPost(postId);

            if (blogPost == null)
            {
                log.LogDebug("returning 404 blog post not found");
                return StatusCode(404);
            }

            var comment = blogPost.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                log.LogDebug("returning 404 comment not found");
                return StatusCode(404);
            }

            comment.IsApproved = true;
            await blogService.Update(blogPost);

            return StatusCode(200);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxDeleteComment(string postId, string commentId)
        {
            // disable status code page for ajax requests
            var statusCodePagesFeature = HttpContext.Features.Get<IStatusCodePagesFeature>();
            if (statusCodePagesFeature != null)
            {
                statusCodePagesFeature.Enabled = false;
            }

            if (string.IsNullOrEmpty(postId))
            {
                log.LogDebug("returning 404 because no postid was posted");
                return StatusCode(404);
            }

            if (string.IsNullOrEmpty(commentId))
            {
                log.LogDebug("returning 404 because no commentid was posted");
                return StatusCode(404);
            }

            var project = await projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                log.LogDebug("returning 404 blog not found");
                return StatusCode(404);
            }

            bool canEdit = await User.CanEditBlog(project.Id, authorizationService);
            
            if (!canEdit)
            {
                log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            var blogPost = await blogService.GetPost(postId);

            if (blogPost == null)
            {
                log.LogDebug("returning 404 blog post not found");
                return StatusCode(404);
            }

            var comment = blogPost.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                log.LogDebug("returning 404 comment not found");
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
            await blogService.Update(blogPost);

            return StatusCode(200);
        }

        private string GetUrl(string website)
        {
            if(string.IsNullOrEmpty(website)) { return string.Empty; }

            if (!website.Contains("://"))
                website = "http://" + website;

            Uri url;
            if (Uri.TryCreate(website, UriKind.Absolute, out url))
                return url.ToString();

            return string.Empty;
        }
  

    }
}
