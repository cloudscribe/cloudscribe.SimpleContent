// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-27
// Last Modified:           2019-04-07
// 

using cloudscribe.DateTimeUtils;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using cloudscribe.Web.Navigation.Caching;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class CreateOrUpdatePageHandler : IRequestHandler<CreateOrUpdatePageRequest, CommandResult<IPage>>
    {
        public CreateOrUpdatePageHandler(
            IProjectService projectService,
            IPageService pageService,
            ITreeCache treeCache,
            ITimeZoneHelper timeZoneHelper,
            ITimeZoneIdResolver timeZoneIdResolver,
            IContentHistoryCommands historyCommands,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer,
            ILogger<CreateOrUpdatePageHandler> logger
            )
        {
            _projectService = projectService;
            _navigationCache = treeCache;
            _pageService = pageService;
            _historyCommands = historyCommands;
            _timeZoneHelper = timeZoneHelper;
            _timeZoneIdResolver = timeZoneIdResolver;
            _localizer = localizer;
            _log = logger;
        }

        private readonly IProjectService _projectService;
        private readonly IPageService _pageService;
        private readonly IContentHistoryCommands _historyCommands;
        private readonly IStringLocalizer _localizer;
        private readonly ITimeZoneHelper _timeZoneHelper;
        private readonly ITimeZoneIdResolver _timeZoneIdResolver;
        private readonly ITreeCache _navigationCache;
        private readonly ILogger _log;

        public async Task<CommandResult<IPage>> Handle(CreateOrUpdatePageRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var errors = new List<string>();
            
            try
            {
                bool isNew = false;
                var project = await _projectService.GetCurrentProjectSettings();
                var page = request.Page;
                ContentHistory history = null;
                if(page == null)
                {
                    isNew = true;
                    page = new Page()
                    {
                        ProjectId = request.ProjectId,
                        ParentId = "0",
                        CreatedByUser = request.UserName,
                        ContentType = request.ViewModel.ContentType
                    };
                }
                else
                {
                    history = page.CreateHistory(request.UserName);
                }

                
                if (!string.IsNullOrEmpty(request.ViewModel.Slug))
                {
                    // remove any bad characters
                    var slug = ContentUtils.CreateSlug(request.ViewModel.Slug);
                    if (slug != page.Slug)
                    {
                        var slugIsAvailable = await _pageService.SlugIsAvailable(slug);
                        while (!slugIsAvailable)
                        {
                            slug += "-";
                            slugIsAvailable = await _pageService.SlugIsAvailable(slug);
                        }
                        if(slugIsAvailable)
                        {
                            page.Slug = slug;
                        }
                    }
                }

                if (request.ModelState.IsValid)
                {
                    page.Title = request.ViewModel.Title;
                    page.CorrelationKey = request.ViewModel.CorrelationKey;
                    page.LastModified = DateTime.UtcNow;
                    page.LastModifiedByUser = request.UserName;
                    page.MenuFilters = request.ViewModel.MenuFilters;
                    page.MetaDescription = request.ViewModel.MetaDescription;
                    page.PageOrder = request.ViewModel.PageOrder;

                    page.ShowHeading = request.ViewModel.ShowHeading;
                    page.ShowMenu = request.ViewModel.ShowMenu;
                    page.MenuOnly = request.ViewModel.MenuOnly;
                    page.DisableEditor = request.ViewModel.DisableEditor;
                    page.ShowComments = request.ViewModel.ShowComments;
                    page.MenuFilters = request.ViewModel.MenuFilters;
                    page.ExternalUrl = request.ViewModel.ExternalUrl;
                    page.ViewRoles = request.ViewModel.ViewRoles;

                    page.ShowCreatedBy = request.ViewModel.ShowCreatedBy;
                    page.ShowCreatedDate = request.ViewModel.ShowCreatedDate;
                    page.ShowLastModifiedBy = request.ViewModel.ShowLastModifiedBy;
                    page.ShowLastModifiedDate = request.ViewModel.ShowLastModifiedDate;
                    
                    if (!string.IsNullOrEmpty(request.ViewModel.ParentSlug))
                    {
                        var parentPage = await _pageService.GetPageBySlug(request.ViewModel.ParentSlug);
                        if (parentPage != null)
                        {
                            if (parentPage.Id != page.ParentId)
                            {
                                page.ParentId = parentPage.Id;
                                page.ParentSlug = parentPage.Slug;
                            }
                        }
                    }
                    else
                    {
                        // empty means root level
                        page.ParentSlug = string.Empty;
                        page.ParentId = "0";
                    }

                    if (page.ParentSlug == project.DefaultPageSlug)
                    {
                        _log.LogWarning($"{request.UserName} tried to explicitely set the default page slug as the parent slug which is not allowed since all root pages are already children of the default page");
                        page.ParentSlug = string.Empty;
                        page.ParentId = "0";
                    }

                    var tzId = await _timeZoneIdResolver.GetUserTimeZoneId(CancellationToken.None);
                    var shouldFirePublishEvent = false;
                    
                    var saveMode = request.ViewModel.SaveMode;
                    if (saveMode == SaveMode.PublishLater)
                    {
                        if(request.ViewModel.NewPubDate.HasValue)
                        {
                            var newPubDate = _timeZoneHelper.ConvertToUtc(request.ViewModel.NewPubDate.Value, tzId);
                            if(newPubDate < DateTime.UtcNow)
                            {
                                saveMode = SaveMode.PublishNow;
                                page.PubDate = newPubDate;
                            } 
                        }
                    }

                    switch (saveMode)
                    {
                        //case SaveMode.UnPublish:
                            
                        //    page.DraftContent = request.ViewModel.Content;
                        //    page.DraftAuthor = request.ViewModel.Author;
                        //    page.DraftPubDate = null;
                        //    page.IsPublished = false;
                        //    page.PubDate = null;

                        //    shouldFireUnPublishEvent = true;

                        //    break;

                        case SaveMode.SaveDraft:
                            
                            page.DraftContent = request.ViewModel.Content;
                            page.DraftAuthor = request.ViewModel.Author;
                            // should we clear the draft pub date if save draft clicked?
                            //page.DraftPubDate = null;
                            if(!page.PubDate.HasValue) { page.IsPublished = false; }

                            break;

                        case SaveMode.PublishLater:

                            page.DraftContent = request.ViewModel.Content;
                            page.DraftAuthor = request.ViewModel.Author;
                            if (request.ViewModel.NewPubDate.HasValue)
                            {
                                
                                page.DraftPubDate = _timeZoneHelper.ConvertToUtc(request.ViewModel.NewPubDate.Value, tzId); 
                            }
                            if (!page.PubDate.HasValue) { page.IsPublished = false; }

                            break;

                        case SaveMode.PublishNow:

                            page.Content = request.ViewModel.Content;
                            page.Author = request.ViewModel.Author;
                            if(!page.PubDate.HasValue)
                            {
                                page.PubDate = DateTime.UtcNow;
                            }
                            page.IsPublished = true;
                            shouldFirePublishEvent = true;

                            page.DraftAuthor = null;
                            page.DraftContent = null;
                            page.DraftPubDate = null;

                            break;

                        case SaveMode.DeleteCurrentDraft:

                            shouldFirePublishEvent = false;

                            page.DraftAuthor = null;
                            page.DraftContent = null;
                            page.DraftPubDate = null;
                            page.DraftSerializedModel = null;

                            break;
                    }

                    if(history != null)
                    {
                        await _historyCommands.Create(request.ProjectId, history).ConfigureAwait(false);
                    }

                    if(isNew)
                    {
                        await _pageService.Create(page);
                    }
                    else
                    {
                        await _pageService.Update(page);
                    }

                    if(shouldFirePublishEvent)
                    {
                        await _pageService.FirePublishEvent(page).ConfigureAwait(false);
                        await _historyCommands.DeleteDraftHistory(project.Id, page.Id).ConfigureAwait(false);
                    }

                    //if (shouldFireUnPublishEvent)
                    //{
                    //    await _pageService.FireUnPublishEvent(page).ConfigureAwait(false);
                    //}

                    await _navigationCache.ClearTreeCache();

                }

                return new CommandResult<IPage>(page, request.ModelState.IsValid, errors);

            }
            catch (Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");

                errors.Add(_localizer["Updating a page failed. An error has been logged."]);

                return new CommandResult<IPage>(null, false, errors);
            }

        }

    }
}
