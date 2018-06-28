// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-11-25
// Last Modified:           2018-06-28
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.EventHandlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PostEvents
    {
        public PostEvents(
            IEnumerable<IHandlePostPreUpdate> preUpdateHandlers,
            IEnumerable<IHandlePostPreDelete> preDeleteHandlers,
            IEnumerable<IHandlePostCreated> createdHandlers,
            IEnumerable<IHandlePostUpdated> updateHandlers,
            IEnumerable<IHandlePostPublished> publishHandlers,
            IEnumerable<IHandlePostUnPublished> unPublishHandlers,
            ILogger<PostEvents> logger
            )
        {
            _preUpdateHandlers = preUpdateHandlers;
            _preDeleteHandlers = preDeleteHandlers;
            _createdHandlers = createdHandlers;
            _updateHandlers = updateHandlers;
            _publishHandlers = publishHandlers;
            _unPublishHandlers = unPublishHandlers;
            _log = logger;
        }

        private readonly IEnumerable<IHandlePostPreUpdate> _preUpdateHandlers;
        private readonly IEnumerable<IHandlePostPreDelete> _preDeleteHandlers;
        private readonly IEnumerable<IHandlePostCreated> _createdHandlers;
        private readonly IEnumerable<IHandlePostUpdated> _updateHandlers;
        private readonly IEnumerable<IHandlePostPublished> _publishHandlers;
        private readonly IEnumerable<IHandlePostUnPublished> _unPublishHandlers;
        private readonly ILogger _log;

        public async Task HandlePreUpdate(
            string projectId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _preUpdateHandlers)
            {
                try
                {
                    await handler.Handle(projectId, postId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} : {ex.StackTrace}");
                }

            }
        }

        public async Task HandlePreDelete(
            string projectId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _preDeleteHandlers)
            {
                try
                {
                    await handler.Handle(projectId, postId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} : {ex.StackTrace}");
                }

            }
        }

        public async Task HandleCreated(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _createdHandlers)
            {
                try
                {
                    await handler.Handle(projectId, post, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} : {ex.StackTrace}");
                }

            }
        }

        public async Task HandleUpdated(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _updateHandlers)
            {
                try
                {
                    await handler.Handle(projectId, post, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} : {ex.StackTrace}");
                }

            }
        }

        public async Task HandlePublished(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _publishHandlers)
            {
                try
                {
                    await handler.Handle(projectId, post, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} : {ex.StackTrace}");
                }

            }
        }

        public async Task HandleUnPublished(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in _unPublishHandlers)
            {
                try
                {
                    await handler.Handle(projectId, post, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _log.LogError($"{ex.Message} : {ex.StackTrace}");
                }

            }
        }


    }
}
