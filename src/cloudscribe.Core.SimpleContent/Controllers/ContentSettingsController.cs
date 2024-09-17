﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
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
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration.Mvc.Controllers
{

    public class ContentSettingsController : Controller
    {
        public ContentSettingsController(
            IProjectService projectService,
            IAuthorizationService authorizationService,
            IUserQueries userQueries,
            ITeaserService teaserService,
            NavigationTreeBuilderService navigationTreeBuilderService,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer
            )
        {
            this.projectService = projectService;
            this.authorizationService = authorizationService;
            this.userQueries = userQueries;
            sr = localizer;
            if(teaserService is TeaserServiceDisabled)
            {
                _teasersDisabled = true;
            }
            _navigationTreeBuilderService = navigationTreeBuilderService;
        }

        private IProjectService projectService;
        private IAuthorizationService authorizationService;
        private IUserQueries userQueries;
        private bool _teasersDisabled = false;
        private IStringLocalizer sr;
        private readonly NavigationTreeBuilderService _navigationTreeBuilderService;

        [Authorize(Policy = "AdminPolicy")]
        // GET: /ContentSettings
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = sr["Content Settings"];

            var projectSettings = await projectService.GetCurrentProjectSettings();

            var model = new ContentSettingsViewModel();
            model.ChannelCategoriesCsv = projectSettings.ChannelCategoriesCsv;
            //model.ChannelRating = projectSettings.ChannelRating;
            //model.ChannelTimeToLive = projectSettings.ChannelTimeToLive;
            model.CommentNotificationEmail = projectSettings.CommentNotificationEmail;
            model.DaysToComment = projectSettings.DaysToComment;
            model.Description = projectSettings.Description;
            model.IncludePubDateInPostUrls = projectSettings.IncludePubDateInPostUrls;
            model.LanguageCode = projectSettings.LanguageCode;
            model.ManagingEditorEmail = projectSettings.ManagingEditorEmail;
            model.ModerateComments = projectSettings.ModerateComments;
            model.PostsPerPage = projectSettings.PostsPerPage;
            model.PubDateFormat = projectSettings.PubDateFormat;
            //model.RemoteFeedProcessorUseAgentFragment = projectSettings.RemoteFeedProcessorUseAgentFragment;
            model.RemoteFeedUrl = projectSettings.RemoteFeedUrl;
            model.ShowTitle = projectSettings.ShowTitle;
            model.Title = projectSettings.Title; //aka Blog Page Title
            //model.UseMetaDescriptionInFeed = projectSettings.UseMetaDescriptionInFeed;
            model.WebmasterEmail = projectSettings.WebmasterEmail;
            model.Publisher = projectSettings.Publisher;
            model.PublisherLogoUrl = projectSettings.PublisherLogoUrl;
            model.PublisherLogoHeight = projectSettings.PublisherLogoHeight;
            model.PublisherLogoWidth = projectSettings.PublisherLogoWidth;
            model.PublisherEntityType = projectSettings.PublisherEntityType;
            model.DisqusShortName = projectSettings.DisqusShortName;
            model.PostsPerPage = projectSettings.PostsPerPage;

            model.BlogMenuLinksToNewestPost = projectSettings.BlogMenuLinksToNewestPost;
            model.DefaultPageSlug = projectSettings.DefaultPageSlug;
            model.ShowRecentPostsOnDefaultPage = projectSettings.ShowRecentPostsOnDefaultPage;
            model.ShowFeaturedPostsOnDefaultPage = projectSettings.ShowFeaturedPostsOnDefaultPage;

            model.AddBlogToPagesTree = projectSettings.AddBlogToPagesTree;
            model.BlogPagePosition = projectSettings.BlogPagePosition;
            model.BlogPageText = projectSettings.BlogPageText;
            model.BlogPageNavComponentVisibility = projectSettings.BlogPageNavComponentVisibility;
            model.LocalMediaVirtualPath = projectSettings.LocalMediaVirtualPath;
            model.CdnUrl = projectSettings.CdnUrl;

            model.FacebookAppId = projectSettings.FacebookAppId;
            model.SiteName = projectSettings.SiteName;
            model.TwitterCreator = projectSettings.TwitterCreator;
            model.TwitterPublisher = projectSettings.TwitterPublisher;
            model.DefaultContentType = projectSettings.DefaultContentType;

            model.TeasersDisabled = _teasersDisabled;
            model.TeaserMode = projectSettings.TeaserMode;
            model.TeaserTruncationMode = projectSettings.TeaserTruncationMode;
            model.TeaserTruncationLength = projectSettings.TeaserTruncationLength;

            model.DefaultFeedItems = projectSettings.DefaultFeedItems;
            model.MaxFeedItems = projectSettings.MaxFeedItems;
            model.AboutContent = projectSettings.AboutContent;
            model.AboutHeading = projectSettings.AboutHeading;
            model.ShowAboutBox = projectSettings.ShowAboutBox;
            model.ShowRelatedPosts = projectSettings.ShowRelatedPosts;

            model.ShowCreatedBy = projectSettings.ShowCreatedBy;
            model.ShowCreatedDate = projectSettings.ShowCreatedDate;
            model.ShowLastModifiedBy = projectSettings.ShowLastModifiedBy;
            model.ShowLastModifiedDate = projectSettings.ShowLastModifiedDate;

            bool canManageUsers = false;
            try
            {
                var result = await authorizationService.AuthorizeAsync(User, "UserManagementPolicy");
                canManageUsers = result.Succeeded;
            }
            catch (InvalidOperationException) { } // thrown if policy doesn't exist


            if(canManageUsers)
            {
                var projectId = projectSettings.Id;
                if(projectId.Length > 36)
                {
                    projectId = projectId.Substring(0, 36);
                }

                var editors = new List<ISiteUser>();

                var contentEditors = await userQueries.GetUsersForClaim(
                    new Guid(projectId),
                    ProjectConstants.ContentEditorClaimType,
                    projectSettings.Id
                    );

                if(contentEditors != null)
                {
                    editors.AddRange(contentEditors);
                }

                var blogEditors = await userQueries.GetUsersForClaim(
                    new Guid(projectId),
                    ProjectConstants.BlogEditorClaimType,
                    projectSettings.Id
                    );

                if(blogEditors != null)
                {
                    editors.AddRange(blogEditors);
                }

                var pageEditors = await userQueries.GetUsersForClaim(
                    new Guid(projectId),
                    ProjectConstants.PageEditorClaimType,
                    projectSettings.Id
                    );


                if (pageEditors != null)
                {
                    editors.AddRange(pageEditors);
                }

                foreach(var e in editors)
                {
                    if(!model.Editors.Any(x => x.Id == e.Id))
                    {
                        model.Editors.Add(e);
                    }
                }

                //model.Editors.AddRange(editors.Distinct());


            }


            return View(model);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> Index(ContentSettingsViewModel model)
        {
            ViewData["Title"] = sr["Content Settings"];

            if (!ModelState.IsValid)
            {
                model.TeasersDisabled = _teasersDisabled;
                return View(model);
            }

            var projectSettings = await projectService.GetCurrentProjectSettings();

            projectSettings.ChannelCategoriesCsv = model.ChannelCategoriesCsv;
            //projectSettings.ChannelRating = model.ChannelRating;
            //projectSettings.ChannelTimeToLive = model.ChannelTimeToLive;
            projectSettings.CommentNotificationEmail = model.CommentNotificationEmail;
            projectSettings.DaysToComment = model.DaysToComment;
            projectSettings.Description = model.Description;
            projectSettings.IncludePubDateInPostUrls = model.IncludePubDateInPostUrls;
            projectSettings.LanguageCode = model.LanguageCode;
            projectSettings.ManagingEditorEmail = model.ManagingEditorEmail;
            projectSettings.ModerateComments = model.ModerateComments;
            projectSettings.PostsPerPage = model.PostsPerPage;
            projectSettings.PubDateFormat = model.PubDateFormat;
            //projectSettings.RemoteFeedProcessorUseAgentFragment = model.RemoteFeedProcessorUseAgentFragment;
            projectSettings.RemoteFeedUrl = model.RemoteFeedUrl;
            projectSettings.ShowTitle = model.ShowTitle;
            projectSettings.Title = model.Title;
            //projectSettings.UseMetaDescriptionInFeed = model.UseMetaDescriptionInFeed;
            projectSettings.WebmasterEmail = model.WebmasterEmail;
            projectSettings.Publisher = model.Publisher;
            projectSettings.PublisherLogoUrl = model.PublisherLogoUrl;
            projectSettings.PublisherLogoWidth = model.PublisherLogoWidth;
            projectSettings.PublisherLogoHeight = model.PublisherLogoHeight;
            projectSettings.PublisherEntityType = model.PublisherEntityType;
            projectSettings.DisqusShortName = model.DisqusShortName;
            projectSettings.ShowRecentPostsOnDefaultPage = model.ShowRecentPostsOnDefaultPage;
            projectSettings.ShowFeaturedPostsOnDefaultPage = model.ShowFeaturedPostsOnDefaultPage;

            bool needToClearMenuCache = false;

            if (model.BlogMenuLinksToNewestPost != projectSettings.BlogMenuLinksToNewestPost) needToClearMenuCache = true;
            if (model.DefaultPageSlug != projectSettings.DefaultPageSlug) needToClearMenuCache = true;
            if (model.AddBlogToPagesTree != projectSettings.AddBlogToPagesTree) needToClearMenuCache = true;
            if (model.BlogPagePosition != projectSettings.BlogPagePosition) needToClearMenuCache = true;
            if (model.BlogPageText != projectSettings.BlogPageText) needToClearMenuCache = true;
            if (model.BlogPageNavComponentVisibility != projectSettings.BlogPageNavComponentVisibility) needToClearMenuCache = true;

            projectSettings.BlogMenuLinksToNewestPost = model.BlogMenuLinksToNewestPost;
            projectSettings.DefaultPageSlug = model.DefaultPageSlug;
            projectSettings.BlogPagePosition = model.BlogPagePosition;
            projectSettings.AddBlogToPagesTree = model.AddBlogToPagesTree;
            projectSettings.BlogPageText = model.BlogPageText;
            projectSettings.BlogPageNavComponentVisibility = model.BlogPageNavComponentVisibility;
            projectSettings.LocalMediaVirtualPath = model.LocalMediaVirtualPath;
            projectSettings.CdnUrl = model.CdnUrl;

            projectSettings.FacebookAppId = model.FacebookAppId;
            projectSettings.SiteName = model.SiteName;
            projectSettings.TwitterPublisher = model.TwitterPublisher;
            projectSettings.TwitterCreator = model.TwitterCreator;
            projectSettings.DefaultContentType = model.DefaultContentType;

            projectSettings.TeaserMode = model.TeaserMode;
            projectSettings.TeaserTruncationLength = model.TeaserTruncationLength;
            projectSettings.TeaserTruncationMode = model.TeaserTruncationMode;

            projectSettings.DefaultFeedItems = model.DefaultFeedItems;
            projectSettings.MaxFeedItems = model.MaxFeedItems;
            projectSettings.AboutContent = model.AboutContent;
            projectSettings.AboutHeading = model.AboutHeading;
            projectSettings.ShowAboutBox = model.ShowAboutBox;
            projectSettings.ShowRelatedPosts = model.ShowRelatedPosts;

            projectSettings.ShowCreatedBy = model.ShowCreatedBy;
            projectSettings.ShowCreatedDate = model.ShowCreatedDate;
            projectSettings.ShowLastModifiedBy = model.ShowLastModifiedBy;
            projectSettings.ShowLastModifiedDate = model.ShowLastModifiedDate;

            await projectService.Update(projectSettings);
            if(needToClearMenuCache)
            {
                //projectService.ClearNavigationCache();
                await _navigationTreeBuilderService.ClearTreeCache();

            }

            this.AlertSuccess(sr["Content Settings were successfully updated."], true);

            return RedirectToAction("Index");


        }

    }
}
