// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-06-28
// 


using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Config;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.ViewModels;
using cloudscribe.Web.Common;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Recaptcha;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
            IRecaptchaServerSideValidator recaptchaServerSideValidator,
            IStringLocalizer<SimpleContent> localizer,
            IOptions<SimpleContentConfig> configOptionsAccessor,
            ILogger<BlogController> logger
            
            )
        {
            ProjectService = projectService;
            BlogService = blogService;
            ContentProcessor = contentProcessor;
            BlogRoutes = blogRoutes;
            AuthorNameResolver = authorNameResolver;
            EmailService = emailService;
            AuthorizationService = authorizationService;
            TimeZoneHelper = timeZoneHelper;
            StringLocalizer = localizer;
            Log = logger;
            ContentOptions = configOptionsAccessor.Value;
            RecaptchaServerSideValidator = recaptchaServerSideValidator;
        }

        protected IProjectService ProjectService { get; private set; }
        protected IBlogService BlogService { get; private set; }
        protected IBlogRoutes BlogRoutes { get; private set; }
        protected IAuthorNameResolver AuthorNameResolver { get; private set; }
        protected IProjectEmailService EmailService { get; private set; }
        protected IContentProcessor ContentProcessor { get; private set; }
        protected ILogger Log { get; private set; }
        protected ITimeZoneHelper TimeZoneHelper { get; private set; }
        protected IAuthorizationService AuthorizationService { get; private set; }
        protected IStringLocalizer<SimpleContent> StringLocalizer { get; private set; }
        protected SimpleContentConfig ContentOptions { get; private set; }

        protected IRecaptchaServerSideValidator RecaptchaServerSideValidator { get; private set; }


        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> Index(
            string category = "",
            int page = 1)
        {
            var projectSettings = await ProjectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                HttpContext.Response.StatusCode = 404;
                return new EmptyResult();
            }

            var model = new BlogViewModel(ContentProcessor)
            {
                ProjectSettings = projectSettings,
                // check if the user has the BlogEditor claim or meets policy
                CanEdit = await User.CanEditBlog(projectSettings.Id, AuthorizationService),
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
            var result = await BlogService.GetPosts(category, page, model.CanEdit);
            model.Posts = result.Data;
            model.Categories = await BlogService.GetCategories(model.CanEdit);
            model.Archives = await BlogService.GetArchives(model.CanEdit);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems; 
            model.TimeZoneHelper = TimeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;
            model.NewItemPath = Url.RouteUrl(BlogRoutes.PostEditRouteName, new { slug = "" });

            return View("Index", model);
        }

       
        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> MostRecent()
        {
            var projectSettings = await ProjectService.GetCurrentProjectSettings();


            if (projectSettings == null)
            {
                return RedirectToAction("Index");
            }

            var result = await BlogService.GetRecentPosts(1);
            if ((result != null) && (result.Count > 0))
            {
                var post = result[0];
                var url = await BlogService.ResolvePostUrl(post);
                return Redirect(url);

            }

            return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
        }



        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> Archive(
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
            model.NewItemPath = Url.RouteUrl(BlogRoutes.PostEditRouteName, new { slug = "" });

            ViewData["Title"] = model.ProjectSettings.Title;

            var result = await BlogService.GetPosts(
                model.ProjectSettings.Id,
                year,
                month,
                day,
                page,
                model.ProjectSettings.PostsPerPage,
                model.CanEdit
                );

            model.Posts = result.Data;
            model.Categories = await BlogService.GetCategories(model.CanEdit);
            model.Archives = await BlogService.GetArchives(model.CanEdit);
            model.Paging.ItemsPerPage = model.ProjectSettings.PostsPerPage;
            model.Paging.CurrentPage = page;
            model.Paging.TotalItems = result.TotalItems;
            
            model.TimeZoneHelper = TimeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;
            model.Year = year;
            model.Month = month;
            model.Day = day;

            return View("Archive", model);
        }

        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        public virtual async Task<IActionResult> Category(
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
        public virtual async Task<IActionResult> Post(string slug)
        {
            return await Post(0, 0, 0, slug);
        }

        [HttpGet]
        [Authorize(Policy = "BlogViewPolicy")]
        [ActionName("PostWithDate")]
        public virtual async Task<IActionResult> Post(int year , int month, int day, string slug)
        {
            var projectSettings = await ProjectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            if(!projectSettings.IncludePubDateInPostUrls)
            {
                if(year > 0)
                {
                    //TODO: an option for permanent redirect
                    return RedirectToRoute(BlogRoutes.PostWithoutDateRouteName, new { slug });
                }
            }

            var canEdit = await User.CanEditBlog(projectSettings.Id, AuthorizationService);
            
            //var isNew = false;
            PostResult result = null;
            if(!string.IsNullOrEmpty(slug))
            {
                result = await BlogService.GetPostBySlug(slug);
            }

            var model = new BlogViewModel(ContentProcessor)
            {
                CanEdit = canEdit
            };

            if ((result == null)||(result.Post == null))
            {
                Log.LogWarning("post not found for slug " + slug + ", so redirecting to index");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }
            else
            {
               if(projectSettings.IncludePubDateInPostUrls)
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
                        
                        return RedirectToRoute(BlogRoutes.PostWithDateRouteName, 
                            new {
                                year = pubDate.Value.Year,
                                month = pubDate.Value.Month.ToString("00"),
                                day = pubDate.Value.Day.ToString("00"),
                                slug = result.Post.Slug
                            });
                    }
                }

                ViewData["Title"] = result.Post.Title;
            }

            
            model.CurrentPost = result.Post;
            if(result.PreviousPost != null)
            {
                model.PreviousPostUrl = await BlogService.ResolvePostUrl(result.PreviousPost);
            }
            if (result.NextPost != null)
            {
                model.NextPostUrl = await BlogService.ResolvePostUrl(result.NextPost);
            }

            

            var currentUrl = await BlogService.ResolvePostUrl(result.Post);
            var breadCrumbHelper = new TailCrumbUtility(HttpContext);
            breadCrumbHelper.AddTailCrumb(result.Post.Id, result.Post.Title, currentUrl);

            model.NewItemPath = Url.RouteUrl(BlogRoutes.PostEditRouteName, new { slug = "" });
            model.EditPath = Url.RouteUrl(BlogRoutes.PostEditRouteName, new { slug = result.Post.Slug });

            model.ProjectSettings = projectSettings;
            model.BlogRoutes = BlogRoutes;
            model.Categories = await BlogService.GetCategories(model.CanEdit);
            model.Archives = await BlogService.GetArchives(model.CanEdit);
            model.ShowComments = true; //mode.Length == 0; // do we need this for a global disable
            model.CommentsAreOpen = await BlogService.CommentsAreOpen(result.Post, canEdit);
            //model.ApprovedCommentCount = post.Comments.Where(c => c.IsApproved == true).Count();
            model.TimeZoneHelper = TimeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;

            if (!canEdit)
            {           
                if((!model.CurrentPost.IsPublished) || model.CurrentPost.PubDate > DateTime.UtcNow)
                {
                    return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
                }
            }

            return View("Post", model);
            

        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Edit(string slug = "", string type="")
        {
            var projectSettings = await ProjectService.GetCurrentProjectSettings();

            if (projectSettings == null)
            {
                Log.LogInformation("redirecting to index because project settings not found");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var canEdit = await User.CanEditPages(projectSettings.Id, AuthorizationService);
            if (!canEdit)
            {
                Log.LogInformation("redirecting to index because user cannot edit");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
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
                postResult = await BlogService.GetPostBySlug(slug);
            }
            if (postResult== null || postResult.Post == null)
            {
                ViewData["Title"] = StringLocalizer["New Post"];
                model.Author = await AuthorNameResolver.GetAuthorName(User);
                model.IsPublished = true;
                //model.PubDate = TimeZoneHelper.ConvertToLocalTime(DateTime.UtcNow, projectSettings.TimeZoneId).ToString();
                model.CurrentPostUrl = Url.RouteUrl(BlogRoutes.BlogIndexRouteName);
                model.ContentType = projectSettings.DefaultContentType;
                if(ContentOptions.AllowMarkdown && !string.IsNullOrWhiteSpace(type) && type == "markdown")
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
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, StringLocalizer["Edit - {0}"], postResult.Post.Title);

                if(string.IsNullOrWhiteSpace(postResult.Post.DraftContent))
                {
                    model.Author = postResult.Post.Author;
                    model.Content = postResult.Post.Content;
                }
                else
                {
                    model.Author = postResult.Post.DraftAuthor;
                    model.Content = postResult.Post.DraftContent;
                }
                
                model.Id = postResult.Post.Id;
                model.CorrelationKey = postResult.Post.CorrelationKey;
                model.IsPublished = postResult.Post.IsPublished;
                model.MetaDescription = postResult.Post.MetaDescription;
                if(postResult.Post.PubDate.HasValue)
                {
                    model.PubDate = TimeZoneHelper.ConvertToLocalTime(postResult.Post.PubDate.Value, projectSettings.TimeZoneId);
                }
                
                model.Slug = postResult.Post.Slug;
                model.Title = postResult.Post.Title;
                model.CurrentPostUrl = await BlogService.ResolvePostUrl(postResult.Post).ConfigureAwait(false);
                model.DeletePostRouteName = BlogRoutes.PostDeleteRouteName;
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

            if (!ModelState.IsValid)
            {
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
            
            var categories = new List<string>();

            if (!string.IsNullOrEmpty(model.Categories))
            {
                if(ContentOptions.ForceLowerCaseCategories)
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
                post = await BlogService.GetPost(model.Id);
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
                post.LastModifiedByUser = User.Identity.Name;
                if(model.Slug != post.Slug)
                {
                    // remove any bad chars
                    model.Slug = ContentUtils.CreateSlug(model.Slug);
                    slugAvailable = await BlogService.SlugIsAvailable(project.Id, model.Slug);
                    if(slugAvailable)
                    {
                        post.Slug = model.Slug;
                    }
                    else
                    {
                        //log.LogWarning($"slug {model.Slug} was requested but not changed because it is already in use");
                        this.AlertDanger(StringLocalizer["The post slug was not changed because the requested slug is already in use."], true);
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
                    slugAvailable = await BlogService.SlugIsAvailable(project.Id, slug);
                    if(!slugAvailable)
                    {
                        slug = ContentUtils.CreateSlug(model.Title);
                    }
                }
                else
                {
                    slug = ContentUtils.CreateSlug(model.Title);
                }
                
                slugAvailable = await BlogService.SlugIsAvailable(project.Id, slug);
                if (!slugAvailable)
                {
                    //log.LogInformation("returning 409 because slug already in use");
                    ModelState.AddModelError("postediterror", StringLocalizer["slug is already in use."]);

                    return View(model);
                }

                post = new Post()
                {
                    BlogId = project.Id,
                    Author = await AuthorNameResolver.GetAuthorName(User),
                    Title = model.Title,
                    MetaDescription = model.MetaDescription,
                    Content = model.Content,
                    Slug = slug
                    ,Categories = categories.ToList(),
                    CreatedByUser = User.Identity.Name
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

            if (model.PubDate.HasValue)
            {
                var localTime = model.PubDate.Value;
                post.PubDate = TimeZoneHelper.ConvertToUtc(localTime, project.TimeZoneId);

            }

            if (isNew)
            {
                await BlogService.Create(post);
            }
            else
            {
                await BlogService.Update(post);
            }

            if (project.IncludePubDateInPostUrls)
            {
                DateTime? pubDate = null;
                if (post.PubDate.HasValue)
                {
                    pubDate = post.PubDate;
                }
                else
                {
                    pubDate = post.DraftPubDate;
                }

                if (!pubDate.HasValue)
                {
                    pubDate = DateTime.UtcNow;
                }

                return RedirectToRoute(BlogRoutes.PostWithDateRouteName,
                    new
                    {
                        year = pubDate.Value.Year,
                        month = pubDate.Value.Month.ToString("00"),
                        day = pubDate.Value.Day.ToString("00"),
                        slug = post.Slug
                    }); 
            }
            else
            {
                return RedirectToRoute(BlogRoutes.PostWithoutDateRouteName,
                    new { slug = post.Slug });  
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Delete(string id)
        {
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

            if (string.IsNullOrEmpty(id))
            {
                Log.LogInformation("postid not provided, redirecting");
                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }

            var post = await BlogService.GetPost(id);

            if (post == null)
            {
                Log.LogInformation("post not found, redirecting");

                return RedirectToRoute(BlogRoutes.BlogIndexRouteName);
            }
            Log.LogWarning("user " + User.Identity.Name + " deleted post " + post.Slug);

            await BlogService.Delete(post.Id);
            
            return RedirectToRoute(BlogRoutes.BlogIndexRouteName);

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
                var postUrl = await BlogService.ResolvePostUrl(blogPost);
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
