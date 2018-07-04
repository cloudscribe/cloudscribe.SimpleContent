// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-28
// Last Modified:           2018-07-04
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class AutoPublishDraftPost : IAutoPublishDraftPost
    {
        public AutoPublishDraftPost(
            IBlogService blogService,
            PostEvents postEvents,
            IContentHistoryCommands historyCommands,
            ILogger<AutoPublishDraftPost> logger
            )
        {
            _blogService = blogService;
            _postEvents = postEvents;
            _historyCommands = historyCommands;
            _log = logger;
        }

        private readonly IBlogService _blogService;
        private readonly PostEvents _postEvents;
        private readonly IContentHistoryCommands _historyCommands;
        private readonly ILogger _log;

        public async Task PublishIfNeeded(IPost post)
        {
            if (post == null) { return; }

            if (!string.IsNullOrWhiteSpace(
                post.DraftContent)
                && post.DraftPubDate.HasValue
                && post.DraftPubDate.Value < DateTime.UtcNow)
            {

                post.Content = post.DraftContent;
                post.Author = post.DraftAuthor;
                post.PubDate = post.DraftPubDate.Value;
                post.SerializedModel = post.DraftSerializedModel;
                post.IsPublished = true;

                post.DraftAuthor = null;
                post.DraftContent = null;
                post.DraftSerializedModel = null;
                post.DraftPubDate = null;

                await _blogService.Update(post);

                await _postEvents.HandlePublished(post.BlogId, post).ConfigureAwait(false);
                await _historyCommands.DeleteDraftHistory(post.BlogId, post.Id).ConfigureAwait(false);

                _log.LogDebug($"auto published draft for post {post.Title}");

            }

        }
    }
}
