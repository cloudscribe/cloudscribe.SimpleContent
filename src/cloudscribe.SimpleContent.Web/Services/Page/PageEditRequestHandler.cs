// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-07-24
// Last Modified:           2018-07-24
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageEditRequestHandler : IRequestHandler<PageEditRquest, PageEditContext>
    {
        public PageEditRequestHandler(
            IProjectService projectService,
            IPageService pageService,
            IAuthorizationService authorizationService,
            IAutoPublishDraftPage autoPublishDraftPage,
            IContentHistoryQueries historyQueries
            )
        {
            _projectService = projectService;
            _pageService = pageService;
            _authorizationService = authorizationService;
            _autoPublishDraftPage = autoPublishDraftPage;
            _historyQueries = historyQueries;
        }

        private readonly IProjectService _projectService;
        private readonly IPageService _pageService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAutoPublishDraftPage _autoPublishDraftPage;
        private readonly IContentHistoryQueries _historyQueries;

        public async Task<PageEditContext> Handle(PageEditRquest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            IPage page = null;
            ContentHistory history = null;
            var canEdit = false;
            var didReplaceDraft = false;
            var didRestoreDeleted = false;
            var rootPageCount = -1;
            IProjectSettings project = await _projectService.GetCurrentProjectSettings();
            if(project != null)
            {
                canEdit = await request.User.CanEditPages(project.Id, _authorizationService);
                var slug = request.Slug;
                if (slug == "none") { slug = string.Empty; }

                if (!string.IsNullOrEmpty(slug))
                {
                    page = await _pageService.GetPageBySlug(slug, cancellationToken);
                }
                else if(!string.IsNullOrWhiteSpace(request.PageId))
                {
                    page = await _pageService.GetPage(request.PageId, cancellationToken);
                }

                if (request.HistoryId.HasValue)
                {
                    history = await _historyQueries.Fetch(project.Id, request.HistoryId.Value).ConfigureAwait(false);
                    if (history != null)
                    {
                        //if (!string.IsNullOrWhiteSpace(history.TemplateKey))
                        //{
                        //    return RedirectToRoute(PageRoutes.PageEditWithTemplateRouteName, routeVals);
                        //}

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

                        //model.HistoryArchiveDate = history.ArchivedUtc;
                        //model.HistoryId = history.Id;
                        //model.DidReplaceDraft = didReplaceDraft;
                        //model.DidRestoreDeleted = didRestoreDeleted;
                    }
                }

                if(page == null)
                {
                    var rootList = await _pageService.GetRootPages(cancellationToken).ConfigureAwait(false);
                    rootPageCount = rootList.Count;
                }



            }


            return new PageEditContext(
                project,
                page,
                history,
                canEdit,
                didReplaceDraft,
                didRestoreDeleted,
                rootPageCount
                );
        }

    }
}
