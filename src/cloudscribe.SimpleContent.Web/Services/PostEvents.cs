// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-11-25
// Last Modified:           2016-11-25
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
            ILogger<PostEvents> logger
            )
        {
            _preUpdateHandlers = preUpdateHandlers;
            _preDeleteHandlers = preDeleteHandlers;
            _createdHandlers = createdHandlers;
            _updateHandlers = updateHandlers;
            _log = logger;
        }

        private IEnumerable<IHandlePostPreUpdate> _preUpdateHandlers;
        private IEnumerable<IHandlePostPreDelete> _preDeleteHandlers;
        private IEnumerable<IHandlePostCreated> _createdHandlers;
        private IEnumerable<IHandlePostUpdated> _updateHandlers;
        private ILogger _log;

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


    }
}
