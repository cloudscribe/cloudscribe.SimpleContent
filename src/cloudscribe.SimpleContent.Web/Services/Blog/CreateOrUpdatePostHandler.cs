// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-28
// Last Modified:           2018-07-27
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using cloudscribe.Web.Common;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class CreateOrUpdatePostHandler : IRequestHandler<CreateOrUpdatePostRequest, CommandResult<IPost>>
    {
        public CreateOrUpdatePostHandler(
            IProjectService projectService,
            IBlogService blogService,
            IContentHistoryCommands historyCommands,
            ITimeZoneHelper timeZoneHelper,
            ITimeZoneIdResolver timeZoneIdResolver,
            IOptions<BlogEditOptions> configOptionsAccessor,
            IStringLocalizer<SimpleContent> localizer,
            ILogger<CreateOrUpdatePostHandler> logger
            )
        {
            _projectService = projectService;
            _blogService = blogService;
            _historyCommands = historyCommands;
            _timeZoneHelper = timeZoneHelper;
            _timeZoneIdResolver = timeZoneIdResolver;
            _editOptions = configOptionsAccessor.Value;
            _localizer = localizer;
            _log = logger;
        }

        private readonly IProjectService _projectService;
        private readonly IBlogService _blogService;
        private readonly IContentHistoryCommands _historyCommands;
        private ITimeZoneHelper _timeZoneHelper;
        private readonly ITimeZoneIdResolver _timeZoneIdResolver;
        private readonly BlogEditOptions _editOptions;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger _log;

        public async Task<CommandResult<IPost>> Handle(CreateOrUpdatePostRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var errors = new List<string>();

            try
            {
                bool isNew = false;
                var project = await _projectService.GetProjectSettings(request.ProjectId);
                var post = request.Post;
                ContentHistory history = null;
                if (post == null)
                {
                    isNew = true;
                    post = new Post()
                    {
                        BlogId = request.ProjectId,
                        Title = request.ViewModel.Title,
                        MetaDescription = request.ViewModel.MetaDescription,                  
                        Slug = ContentUtils.CreateSlug(request.ViewModel.Title),
                        CreatedByUser = request.UserName
                    };
                }
                else
                {
                    history = post.CreateHistory(request.UserName);
                }

                if (!string.IsNullOrEmpty(request.ViewModel.Slug))
                {
                    // remove any bad characters
                    request.ViewModel.Slug = ContentUtils.CreateSlug(request.ViewModel.Slug);
                    if (request.ViewModel.Slug != post.Slug)
                    {
                        var slugIsAvailable = await _blogService.SlugIsAvailable(request.ViewModel.Slug);
                        if (!slugIsAvailable)
                        {
                            request.ModelState.AddModelError("Slug", _localizer["The page slug was not changed because the requested slug is already in use."]);
                        }
                    }
                }

                if (request.ModelState.IsValid)
                {
                    post.Title = request.ViewModel.Title;
                    post.CorrelationKey = request.ViewModel.CorrelationKey;
                    post.ImageUrl = request.ViewModel.ImageUrl;
                    post.ThumbnailUrl = request.ViewModel.ThumbnailUrl;
                    post.IsFeatured = request.ViewModel.IsFeatured;
                    post.ContentType = request.ViewModel.ContentType;
                    post.TeaserOverride = request.ViewModel.TeaserOverride;
                    post.SuppressTeaser = request.ViewModel.SuppressTeaser;
                    post.LastModified = DateTime.UtcNow;
                    post.LastModifiedByUser = request.UserName;

                    if (!string.IsNullOrEmpty(request.ViewModel.Slug))
                    {
                        post.Slug = request.ViewModel.Slug;
                    }


                    var categories = new List<string>();

                    if (!string.IsNullOrEmpty(request.ViewModel.Categories))
                    {
                        if (_editOptions.ForceLowerCaseCategories)
                        {
                            categories = request.ViewModel.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim().ToLower())
                            .Where(x =>
                            !string.IsNullOrWhiteSpace(x)
                            && x != ","
                            )
                            .Distinct()
                            .ToList();
                        }
                        else
                        {
                            categories = request.ViewModel.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())
                            .Where(x =>
                            !string.IsNullOrWhiteSpace(x)
                            && x != ","
                            )
                            .Distinct()
                            .ToList();
                        }
                    }
                    post.Categories = categories;


                    var shouldFirePublishEvent = false;
                    switch (request.ViewModel.SaveMode)
                    {
                        case SaveMode.SaveDraft:

                            post.DraftContent = request.ViewModel.Content;
                            post.DraftAuthor = request.ViewModel.Author;
                            
                            break;

                        case SaveMode.PublishLater:

                            post.DraftContent = request.ViewModel.Content;
                            post.DraftAuthor = request.ViewModel.Author;
                            if (request.ViewModel.NewPubDate.HasValue)
                            {
                                var tzId = await _timeZoneIdResolver.GetUserTimeZoneId(CancellationToken.None);
                                post.DraftPubDate = _timeZoneHelper.ConvertToUtc(request.ViewModel.NewPubDate.Value, tzId);
                            }
                            if (!post.PubDate.HasValue) { post.IsPublished = false; }

                            break;

                        case SaveMode.PublishNow:

                            post.Content = request.ViewModel.Content;
                            post.Author = request.ViewModel.Author;
                            if(!post.PubDate.HasValue)
                            {
                                post.PubDate = DateTime.UtcNow;
                            }
                            else if (post.PubDate > DateTime.UtcNow)
                            {
                                post.PubDate = DateTime.UtcNow;
                            }
                            
                            post.IsPublished = true;
                            shouldFirePublishEvent = true;

                            post.DraftAuthor = null;
                            post.DraftContent = null;
                            post.DraftPubDate = null;

                            break;
                    }

                    if (history != null)
                    {
                        await _historyCommands.Create(request.ProjectId, history).ConfigureAwait(false);
                    }

                    if (isNew)
                    {
                        await _blogService.Create(post);
                    }
                    else
                    {
                        await _blogService.Update(post);
                    }

                    if(shouldFirePublishEvent)
                    {
                        await _blogService.FirePublishEvent(post);
                        await _historyCommands.DeleteDraftHistory(request.ProjectId, post.Id).ConfigureAwait(false);
                    }
                   
                }

                return new CommandResult<IPost>(post, request.ModelState.IsValid, errors);

            }
            catch(Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");

                errors.Add(_localizer["Updating a page failed. An error has been logged."]);

                return new CommandResult<IPost>(null, false, errors);
            }

        }


    }
}
