// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-27
// Last Modified:           2018-07-01
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using cloudscribe.SimpleContent.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Versioning
{
    public class AutoPublishDraftPage : IAutoPublishDraftPage
    {
        public AutoPublishDraftPage(
            IPageService pageService,
            PageEvents pageEvents,
            ILogger<AutoPublishDraftPage> logger
            )
        {
            _pageService = pageService;
            _pageEvents = pageEvents;
            _log = logger;
        }

        private readonly IPageService _pageService;
        private readonly PageEvents _pageEvents;
        private readonly ILogger _log;

        public async Task PublishIfNeeded(IPage page)
        {
            if(page == null) { return; }

            if(!string.IsNullOrWhiteSpace(
                page.DraftContent) 
                && page.DraftPubDate.HasValue 
                && page.DraftPubDate.Value < DateTime.UtcNow)
            {

                page.Content = page.DraftContent;
                page.Author = page.DraftAuthor;
                page.PubDate = page.DraftPubDate.Value;
                page.SerializedModel = page.DraftSerializedModel;
                page.IsPublished = true;

                page.DraftAuthor = null;
                page.DraftContent = null;
                page.DraftSerializedModel = null;
                page.DraftPubDate = null;

                await _pageService.Update(page);

                await _pageEvents.HandlePublished(page.ProjectId, page).ConfigureAwait(false);

                _log.LogDebug($"auto published draft for page {page.Title}");



            }

        }
    }
}
