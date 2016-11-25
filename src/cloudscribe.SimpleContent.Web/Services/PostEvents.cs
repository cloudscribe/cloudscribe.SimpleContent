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
            this.preUpdateHandlers = preUpdateHandlers;
            this.preDeleteHandlers = preDeleteHandlers;
            this.createdHandlers = createdHandlers;
            this.updateHandlers = updateHandlers;
            log = logger;
        }

        private IEnumerable<IHandlePostPreUpdate> preUpdateHandlers;
        private IEnumerable<IHandlePostPreDelete> preDeleteHandlers;
        private IEnumerable<IHandlePostCreated> createdHandlers;
        private IEnumerable<IHandlePostUpdated> updateHandlers;
        private ILogger log;

        public async Task HandlePreUpdate(
            string projectId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in preUpdateHandlers)
            {
                try
                {
                    await handler.Handle(projectId, postId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }

            }
        }

        public async Task HandlePreDelete(
            string projectId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in preDeleteHandlers)
            {
                try
                {
                    await handler.Handle(projectId, postId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }

            }
        }

        public async Task HandleCreated(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in createdHandlers)
            {
                try
                {
                    await handler.Handle(projectId, post, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }

            }
        }

        public async Task HandleUpdated(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in updateHandlers)
            {
                try
                {
                    await handler.Handle(projectId, post, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }

            }
        }


    }
}
