// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2016-11-25
// Last Modified:           2016-11-25
// 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using cloudscribe.SimpleContent.Models.EventHandlers;
using System;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Services
{
    public class PageEvents
    {
        public PageEvents(
            IEnumerable<IHandlePagePreUpdate> preUpdateHandlers,
            IEnumerable<IHandlePagePreDelete> preDeleteHandlers,
            IEnumerable<IHandlePageCreated> createdHandlers,
            IEnumerable<IHandlePageUpdated> updateHandlers,
            ILogger<PageEvents> logger
            )
        {
            this.preUpdateHandlers = preUpdateHandlers;
            this.preDeleteHandlers = preDeleteHandlers;
            this.createdHandlers = createdHandlers;
            this.updateHandlers = updateHandlers;
            log = logger;
        }

        private IEnumerable<IHandlePagePreUpdate> preUpdateHandlers;
        private IEnumerable<IHandlePagePreDelete> preDeleteHandlers;
        private IEnumerable<IHandlePageCreated> createdHandlers;
        private IEnumerable<IHandlePageUpdated> updateHandlers;
        private ILogger log;

        public async Task HandlePreUpdate(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach(var handler in preUpdateHandlers)
            {
                try
                {
                    await handler.Handle(projectId, pageId, cancellationToken).ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }
                
            }
        }

        public async Task HandlePreDelete(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in preDeleteHandlers)
            {
                try
                {
                    await handler.Handle(projectId, pageId, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }
                
            }
        }

        public async Task HandleCreated(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in createdHandlers)
            {
                try
                {
                    await handler.Handle(projectId, page, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }
                
            }
        }

        public async Task HandleUpdated(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            foreach (var handler in updateHandlers)
            {
                try
                {
                    await handler.Handle(projectId, page, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError(ex.Message + "-" + ex.StackTrace);
                }
                
            }
        }

    }
}
