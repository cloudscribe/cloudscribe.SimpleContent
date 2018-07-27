// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-07-24
// Last Modified:           2018-07-27
// 

using cloudscribe.SimpleContent.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageViewContextRequestHandler : IRequestHandler<PageViewContextRequest, PageViewContext>
    {
        public PageViewContextRequestHandler(
            IProjectService projectService,
            IPageService pageService,
            IAuthorizationService authorizationService,
            IContentHistoryQueries historyQueries
            )
        {
            _projectService = projectService;
            _pageService = pageService;
            _authorizationService = authorizationService;
            _historyQueries = historyQueries;
        }

        private readonly IProjectService _projectService;
        private readonly IPageService _pageService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IContentHistoryQueries _historyQueries;

        public async Task<PageViewContext> Handle(PageViewContextRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            IPage page = null;
            ContentHistory history = null;
            var canEdit = false;
            var hasDraft = false;
            var hasPublishedVersion = false;
            var didReplaceDraft = false;
            var didRestoreDeleted = false;
            var showingDraft = false;
            var rootPageCount = -1;
            IProjectSettings project = await _projectService.GetCurrentProjectSettings();
            if(project != null)
            {
                canEdit = await request.User.CanEditPages(project.Id, _authorizationService);

                var slug = request.Slug;
                if (string.IsNullOrEmpty(slug) || slug == "none") { slug = project.DefaultPageSlug; }

                page = await _pageService.GetPageBySlug(slug, cancellationToken);
                
                if (page != null)
                {
                    hasDraft = page.HasDraftVersion();
                    hasPublishedVersion = page.HasPublishedVersion();
                }

                if (canEdit && request.HistoryId.HasValue)
                {
                    history = await _historyQueries.Fetch(project.Id, request.HistoryId.Value);
                    if (history != null)
                    {
                        if (page == null) //page must have been deleted, restore from hx
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
                            didReplaceDraft = hasDraft;
                        }
                    }
                }
                else if (canEdit && page != null && ((request.ShowDraft && page.HasDraftVersion()) || !page.HasPublishedVersion()))
                {
                    var pageCopy = new Page();
                    page.CopyTo(pageCopy);
                    pageCopy.PromoteDraftTemporarilyForRender();
                    page = pageCopy;
                    showingDraft = true;
                }

                if (page == null)
                {
                    var rootList = await _pageService.GetRootPages(cancellationToken).ConfigureAwait(false);
                    rootPageCount = rootList.Count;
                }

            }
            
            return new PageViewContext(
                project,
                page,
                history,
                canEdit,
                hasDraft,
                hasPublishedVersion,
                didReplaceDraft,
                didRestoreDeleted,
                showingDraft,
                rootPageCount
                );
        }

    }
}
