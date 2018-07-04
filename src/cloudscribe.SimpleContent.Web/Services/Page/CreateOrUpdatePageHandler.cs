// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-27
// Last Modified:           2018-07-04
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using cloudscribe.SimpleContent.Services;
using cloudscribe.Web.Common;
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
            ITimeZoneHelper timeZoneHelper,
            PageEvents pageEvents,
            IContentHistoryCommands historyCommands,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer,
            ILogger<CreateOrUpdatePageHandler> logger
            )
        {
            _projectService = projectService;
            _pageService = pageService;
            _pageEvents = pageEvents;
            _historyCommands = historyCommands;
            _timeZoneHelper = timeZoneHelper;
            _localizer = localizer;
            _log = logger;
        }

        private readonly IProjectService _projectService;
        private readonly IPageService _pageService;
        private readonly PageEvents _pageEvents;
        private readonly IContentHistoryCommands _historyCommands;
        private readonly IStringLocalizer _localizer;
        private readonly ITimeZoneHelper _timeZoneHelper;
        private readonly ILogger _log;

        public async Task<CommandResult<IPage>> Handle(CreateOrUpdatePageRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var errors = new List<string>();
            
            try
            {
                bool isNew = false;
                var project = await _projectService.GetProjectSettings(request.ProjectId);
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
                        Slug = ContentUtils.CreateSlug(request.ViewModel.Title),
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
                    request.ViewModel.Slug = ContentUtils.CreateSlug(request.ViewModel.Slug);
                    if (request.ViewModel.Slug != page.Slug)
                    {
                        var slugIsAvailable = await _pageService.SlugIsAvailable(request.ViewModel.Slug);
                        if (!slugIsAvailable)
                        {
                            errors.Add(_localizer["The page slug was invalid because the requested slug is already in use."]);
                            request.ModelState.AddModelError("Slug", _localizer["The page slug was not changed because the requested slug is already in use."]);
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


                    if (!string.IsNullOrEmpty(request.ViewModel.Slug))
                    {
                        page.Slug = request.ViewModel.Slug;
                    }
                    

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


                    var shouldFirePublishEvent = false;
                    var shouldFireUnPublishEvent = false;
                    switch (request.ViewModel.SaveMode)
                    {
                        case SaveMode.UnPublish:
                            
                            page.DraftContent = request.ViewModel.Content;
                            page.DraftAuthor = request.ViewModel.Author;
                            page.DraftPubDate = null;
                            page.IsPublished = false;
                            page.PubDate = null;

                            shouldFireUnPublishEvent = true;

                            break;

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
                                page.DraftPubDate = _timeZoneHelper.ConvertToUtc(request.ViewModel.NewPubDate.Value, project.TimeZoneId); 
                            }
                            if (!page.PubDate.HasValue) { page.IsPublished = false; }

                            break;

                        case SaveMode.PublishNow:

                            page.Content = request.ViewModel.Content;
                            page.Author = request.ViewModel.Author;
                            page.PubDate = DateTime.UtcNow;
                            page.IsPublished = true;
                            shouldFirePublishEvent = true;

                            page.DraftAuthor = null;
                            page.DraftContent = null;
                            page.DraftPubDate = null;

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
                        await _pageEvents.HandlePublished(project.Id, page).ConfigureAwait(false);
                        await _historyCommands.DeleteDraftHistory(project.Id, page.Id).ConfigureAwait(false);
                    }

                    if (shouldFireUnPublishEvent)
                    {
                        await _pageEvents.HandleUnPublished(page.ProjectId, page).ConfigureAwait(false);
                    }
                    
                    _pageService.ClearNavigationCache();

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
