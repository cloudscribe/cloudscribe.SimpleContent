// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-03-15
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
            _projectService = projectService;
            _blogService = blogService;
            _contentProcessor = contentProcessor;
            _blogRoutes = blogRoutes;
            _authorNameResolver = authorNameResolver;
            _emailService = emailService;
            _authorizationService = authorizationService;
            _timeZoneHelper = timeZoneHelper;
            _sr = localizer;
            _log = logger;
            _config = configOptionsAccessor.Value;
        }

        private IProjectService _projectService;
        private IBlogService _blogService;
        private IBlogRoutes _blogRoutes;
        private IAuthorNameResolver _authorNameResolver;
        private IProjectEmailService _emailService;
        private IContentProcessor _contentProcessor;
        private ILogger _log;
        private ITimeZoneHelper _timeZoneHelper;
        private IAuthorizationService _authorizationService;
        private IStringLocalizer<SimpleContent> _sr;
        private SimpleContentConfig _config;
        

        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public async Task<IActionResult> Index(
            string category = "",
            int page = 1)
        {
            var projectSettings = await _projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                HttpContext.Response.StatusCode = 404;
                return new EmptyResult();
            }

            var model = new BlogViewModel(_contentProcessor)
            {
                ProjectSettings = projectSettings,
                // check if the user has the BlogEditor claim or meets policy
                CanEdit = await User.CanEditBlog(projectSettings.Id, _authorizationService),
                BlogRoutes = _blogRoutes,
                CurrentCategory = category
            };
            
            if(!string.IsNullOrEmpty(model.CurrentCategory))
            {
                model.ListRouteName = _blogRoutes.BlogCategoryRouteName;
            }
            else
            {
                model.ListRouteName = _blogRoutes.BlogIndexRouteName;
            }

            ViewData["Title"] = model.ProjectSettings.Title;
            var result = await _blogService.GetPosts(category, page, model.CanEdit);
            model.Posts = result.Data;
            model.Categories = await _blogService.GetCategories(model.CanEdit);
            model.Archives = await _blogService.GetArchives(model.CanEdit);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems; 
            model.TimeZoneHelper = _timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;
            model.NewItemPath = Url.RouteUrl(_blogRoutes.PostEditRouteName, new { slug = "" });

            return View("Index", model);
        }

       
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public async Task<IActionResult> MostRecent()
        {
            var projectSettings = await _projectService.GetCurrentProjectSettings();


            if (projectSettings == null)
            {
                return RedirectToAction("Index");
            }

            var result = await _blogService.GetRecentPosts(1);
            if ((result != null) && (result.Count > 0))
            {
                var post = result[0];
                var url = await _blogService.ResolvePostUrl(post);
                return Redirect(url);

            }

            return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
        }



        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public async Task<IActionResult> Archive(
            int year,
            int month = 0,
            int day = 0,
            int page = 1)
        {

            var model = new BlogViewModel(_contentProcessor)
            {
                ProjectSettings = await _projectService.GetCurrentProjectSettings(),
                BlogRoutes = _blogRoutes
            };
            model.CanEdit = await User.CanEditBlog(model.ProjectSettings.Id, _authorizationService);
            model.NewItemPath = Url.RouteUrl(_blogRoutes.PostEditRouteName, new { slug = "" });

            ViewData["Title"] = model.ProjectSettings.Title;

            var result = await _blogService.GetPosts(
                model.ProjectSettings.Id,
                year,
                month,
                day,
                page,
                model.ProjectSettings.PostsPerPage,
                model.CanEdit
                );

            model.Posts = result.Data;
            model.Categories = await _blogService.GetCategories(model.CanEdit);
            model.Archives = await _blogService.GetArchives(model.CanEdit);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems;
            
            model.TimeZoneHelper = _timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;
            model.Year = year;
            model.Month = month;
            model.Day = day;

            return View("Archive", model);
        }

        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
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
        [Authorize(Policy = "BlogViewPolicy")]
        [ActionName("PostNoDate")]
        public async Task<IActionResult> Post(string slug)
        {
            return await Post(0, 0, 0, slug);
        }

        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        [ActionName("PostWithDate")]
        public async Task<IActionResult> Post(int year , int month, int day, string slug)
        {
            var projectSettings = await _projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            if(!projectSettings.IncludePubDateInPostUrls)
            {
                if(year > 0)
                {
                    //TODO: an option for permanent redirect
                    return RedirectToRoute(_blogRoutes.PostWithoutDateRouteName, new { slug });
                }
            }

            var canEdit = await User.CanEditBlog(projectSettings.Id, _authorizationService);
            
            //var isNew = false;
            PostResult result = null;
            if(!string.IsNullOrEmpty(slug))
            {
                result = await _blogService.GetPostBySlug(slug);
            }

            var model = new BlogViewModel(_contentProcessor)
            {
                CanEdit = canEdit
            };

            if ((result == null)||(result.Post == null))
            {
                _log.LogWarning("post not found for slug " + slug + ", so redirecting to index");
                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }
            else
            {
               if(projectSettings.IncludePubDateInPostUrls)
                {
                    if(year == 0)
                    {
                        //TODO: option whether to use permanent redirect
                        return RedirectToRoute(_blogRoutes.PostWithDateRouteName, 
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
                model.PreviousPostUrl = await _blogService.ResolvePostUrl(result.PreviousPost);
            }
            if (result.NextPost != null)
            {
                model.NextPostUrl = await _blogService.ResolvePostUrl(result.NextPost);
            }

            

            var currentUrl = await _blogService.ResolvePostUrl(result.Post);
            var breadCrumbHelper = new TailCrumbUtility(HttpContext);
            breadCrumbHelper.AddTailCrumb(result.Post.Id, result.Post.Title, currentUrl);

            model.NewItemPath = Url.RouteUrl(_blogRoutes.PostEditRouteName, new { slug = "" });
            model.EditPath = Url.RouteUrl(_blogRoutes.PostEditRouteName, new { slug = result.Post.Slug });

            model.ProjectSettings = projectSettings;
            model.BlogRoutes = _blogRoutes;
            model.Categories = await _blogService.GetCategories(model.CanEdit);
            model.Archives = await _blogService.GetArchives(model.CanEdit);
            model.ShowComments = true; //mode.Length == 0; // do we need this for a global disable
            model.CommentsAreOpen = await _blogService.CommentsAreOpen(result.Post, canEdit);
            //model.ApprovedCommentCount = post.Comments.Where(c => c.IsApproved == true).Count();
            model.TimeZoneHelper = _timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;

            if (!canEdit)
            {           
                if((!model.CurrentPost.IsPublished) || model.CurrentPost.PubDate > DateTime.UtcNow)
                {
                    return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
                }
            }

            return View("Post", model);
            

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(string slug = "", string type="")
        {
            var projectSettings = await _projectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                _log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(projectSettings.Id, _authorizationService);
            if (!canEdit)
            {
                _log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            if (slug == "none") { slug = string.Empty; }

            var model = new PostEditViewModel
            {
                ProjectId = projectSettings.Id,
                TeasersEnabled = projectSettings.TeaserMode != TeaserMode.Off
            };

            PostResult postResult = null;
            if (!string.IsNullOrEmpty(slug))
            {
                postResult = await _blogService.GetPostBySlug(slug);
            }
            if (postResult== null || postResult.Post == null)
            {
                ViewData["Title"] = _sr["New Post"];
                model.Author = await _authorNameResolver.GetAuthorName(User);
                model.IsPublished = true;
                model.PubDate = _timeZoneHelper.ConvertToLocalTime(DateTime.UtcNow, projectSettings.TimeZoneId).ToString();
                model.CurrentPostUrl = Url.RouteUrl(_blogRoutes.BlogIndexRouteName);
                model.ContentType = projectSettings.DefaultContentType;
                if(_config.AllowMarkdown && !string.IsNullOrWhiteSpace(type) && type == "markdown")
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
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["Edit - {0}"], postResult.Post.Title);
                model.Author = postResult.Post.Author;
                model.Content = postResult.Post.Content;
                model.Id = postResult.Post.Id;
                model.CorrelationKey = postResult.Post.CorrelationKey;
                model.IsPublished = postResult.Post.IsPublished;
                model.MetaDescription = postResult.Post.MetaDescription;
                model.PubDate = _timeZoneHelper.ConvertToLocalTime(postResult.Post.PubDate, projectSettings.TimeZoneId).ToString();
                model.Slug = postResult.Post.Slug;
                model.Title = postResult.Post.Title;
                model.CurrentPostUrl = await _blogService.ResolvePostUrl(postResult.Post).ConfigureAwait(false);
                model.DeletePostRouteName = _blogRoutes.PostDeleteRouteName;
                model.Categories = string.Join(",", postResult.Post.Categories);
                model.ImageUrl = postResult.Post.ImageUrl;
                model.ThumbnailUrl = postResult.Post.ThumbnailUrl;
                model.IsFeatured = postResult.Post.IsFeatured;
                model.ContentType = postResult.Post.ContentType;
                model.TeaserOverride = postResult.Post.TeaserOverride;
                model.SuppressTeaser = postResult.Post.SuppressTeaser;
            }


            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostEditViewModel model)
        {
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("redirecting to index because project settings not found");

                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("redirecting to index because user is not allowed to edit");
                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    ViewData["Title"] = _sr["New Post"];
                }
                else
                {
                    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, _sr["Edit - {0}"], model.Title);
                }
                model.ProjectId = project.Id;
                model.TeasersEnabled = project.TeaserMode != TeaserMode.Off;

                return View(model);
            }
            
            var categories = new List<string>();

            if (!string.IsNullOrEmpty(model.Categories))
            {
                if(_config.ForceLowerCaseCategories)
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
                post = await _blogService.GetPost(model.Id);
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
                    slugAvailable = await _blogService.SlugIsAvailable(project.Id, model.Slug);
                    if(slugAvailable)
                    {
                        post.Slug = model.Slug;
                    }
                    else
                    {
                        //log.LogWarning($"slug {model.Slug} was requested but not changed because it is already in use");
                        this.AlertDanger(_sr["The post slug was not changed because the requested slug is already in use."], true);
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
                    slugAvailable = await _blogService.SlugIsAvailable(project.Id, slug);
                    if(!slugAvailable)
                    {
                        slug = ContentUtils.CreateSlug(model.Title);
                    }
                }
                else
                {
                    slug = ContentUtils.CreateSlug(model.Title);
                }
                
                slugAvailable = await _blogService.SlugIsAvailable(project.Id, slug);
                if (!slugAvailable)
                {
                    //log.LogInformation("returning 409 because slug already in use");
                    ModelState.AddModelError("postediterror", _sr["slug is already in use."]);

                    return View(model);
                }

                post = new Post()
                {
                    BlogId = project.Id,
                    Author = await _authorNameResolver.GetAuthorName(User),
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
            post.SuppressTeaser = model.SuppressTeaser;

            if (!string.IsNullOrEmpty(model.PubDate))
            {
                var localTime = DateTime.Parse(model.PubDate);
                post.PubDate = _timeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);

            }

            if (isNew)
            {
                await _blogService.Create(post);
            }
            else
            {
                await _blogService.Update(post);
            }

            if (project.IncludePubDateInPostUrls)
            {
                return RedirectToRoute(_blogRoutes.PostWithDateRouteName,
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
                return RedirectToRoute(_blogRoutes.PostWithoutDateRouteName,
                    new { slug = post.Slug });  
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogInformation("project settings not found, redirecting");
                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            bool canEdit = await User.CanEditBlog(project.Id, _authorizationService);

            if (!canEdit)
            {
                _log.LogInformation("user is not allowed to edit, redicrecting");

                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            if (string.IsNullOrEmpty(id))
            {
                _log.LogInformation("postid not provided, redirecting");
                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }

            var post = await _blogService.GetPost(id);

            if (post == null)
            {
                _log.LogInformation("post not found, redirecting");

                return RedirectToRoute(_blogRoutes.BlogIndexRouteName);
            }
            _log.LogWarning("user " + User.Identity.Name + " deleted post " + post.Slug);

            await _blogService.Delete(post.Id);
            
            return RedirectToRoute(_blogRoutes.BlogIndexRouteName);

        }

        

        [HttpPost]
        [Authorize(Policy = "BlogViewPolicy")]
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
                return Content(_sr["Please enter a valid e-mail address"]);
            }
            
            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogDebug("returning 500 blog not found");
                return StatusCode(500);
            }

            if (string.IsNullOrEmpty(model.PostId))
            {
                _log.LogDebug("returning 500 because no postid was posted");
                return StatusCode(500);
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                _log.LogDebug("returning 403 because no name was posted");
                Response.StatusCode = 403;
                //await Response.WriteAsync("Please enter a valid name");
                return Content("Please enter a valid name");
            }

            if (string.IsNullOrEmpty(model.Content))
            {
                _log.LogDebug("returning 403 because no content was posted");
                Response.StatusCode = 403;
                //await Response.WriteAsync("Please enter a valid content");
                return Content("Please enter a valid content");
            }

            var blogPost = await _blogService.GetPost(model.PostId);

            if (blogPost == null)
            {
                _log.LogDebug("returning 500 blog post not found");
                return StatusCode(500);
            }

            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                if(!string.IsNullOrEmpty(project.RecaptchaPublicKey))
                {
                    var captchaResponse = await this.ValidateRecaptcha(Request, project.RecaptchaPrivateKey);
                    if (!captchaResponse.Success)
                    {
                        _log.LogDebug("returning 403 captcha validation failed");
                        Response.StatusCode = 403;
                        //await Response.WriteAsync("captcha validation failed");
                        return Content("captcha validation failed");
                    }
                }
            }

            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var canEdit = await User.CanEditBlog(project.Id, _authorizationService);
            
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
            await _blogService.Update(blogPost);

            // TODO: clear cache

            //no need to send notification when project owner posts a comment, ie in response
            var shouldSendEmail = !canEdit;
       
            if(shouldSendEmail)
            {
                var postUrl = await _blogService.ResolvePostUrl(blogPost);
                var baseUrl = string.Concat(HttpContext.Request.Scheme,
                        "://",
                        HttpContext.Request.Host.ToUriComponent());

                postUrl = baseUrl + postUrl;

                _emailService.SendCommentNotificationEmailAsync(
                    project,
                    blogPost,
                    comment,
                    postUrl,
                    postUrl,
                    postUrl
                    ).Forget(); //async but don't want to wait
            }

            var viewModel = new BlogViewModel(_contentProcessor)
            {
                ProjectSettings = project,
                BlogRoutes = _blogRoutes,
                CurrentPost = blogPost,
                TmpComment = comment,
                TimeZoneHelper = _timeZoneHelper,
                TimeZoneId = project.TimeZoneId,
                CanEdit = canEdit
            };
            

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
                _log.LogDebug("returning 404 because no postid was posted");
               
                return StatusCode(404);
            }

            if (string.IsNullOrEmpty(commentId))
            {
                _log.LogDebug("returning 404 because no commentid was posted");
                //Response.StatusCode = 404;
                // await Response.WriteAsync("Comm");
                return StatusCode(404);
            }

            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogDebug("returning 500 blog not found");
                //Response.StatusCode = 500;
                return StatusCode(500);
            }

            bool canEdit = await User.CanEditBlog(project.Id, _authorizationService);
            
            if (!canEdit)
            {
                _log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            var blogPost = await _blogService.GetPost(postId);

            if (blogPost == null)
            {
                _log.LogDebug("returning 404 blog post not found");
                return StatusCode(404);
            }

            var comment = blogPost.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                _log.LogDebug("returning 404 comment not found");
                return StatusCode(404);
            }

            comment.IsApproved = true;
            await _blogService.Update(blogPost);

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
                _log.LogDebug("returning 404 because no postid was posted");
                return StatusCode(404);
            }

            if (string.IsNullOrEmpty(commentId))
            {
                _log.LogDebug("returning 404 because no commentid was posted");
                return StatusCode(404);
            }

            var project = await _projectService.GetCurrentProjectSettings();

            if (project == null)
            {
                _log.LogDebug("returning 404 blog not found");
                return StatusCode(404);
            }

            bool canEdit = await User.CanEditBlog(project.Id, _authorizationService);
            
            if (!canEdit)
            {
                _log.LogInformation("returning 403 user is not allowed to edit");
                return StatusCode(403);
            }

            var blogPost = await _blogService.GetPost(postId);

            if (blogPost == null)
            {
                _log.LogDebug("returning 404 blog post not found");
                return StatusCode(404);
            }

            var comment = blogPost.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                _log.LogDebug("returning 404 comment not found");
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
            await _blogService.Update(blogPost);

            return StatusCode(200);
        }

        private string GetUrl(string website)
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
