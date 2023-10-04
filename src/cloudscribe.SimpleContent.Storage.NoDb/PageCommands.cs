﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2018-07-04
//

using cloudscribe.SimpleContent.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PageCommands : IPageCommands, IPageCommandsSingleton
    {
        public PageCommands(
            IBasicCommands<Page> pageCommands,
            IBasicQueries<Page> pageQueries,
            IKeyGenerator keyGenerator
            )
        {
            _commands = pageCommands;
            _query = pageQueries;
            _keyGenerator = keyGenerator;

        }

        private IBasicCommands<Page> _commands;
        private IBasicQueries<Page> _query;
        private IKeyGenerator _keyGenerator;

        public async Task Create(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var p = Page.FromIPage(page);
            // metaweblog sets the id, don't change it if it exists
            if (string.IsNullOrWhiteSpace(p.Id))
            {
                p.Id = _keyGenerator.GenerateKey(p);
            }

            p.LastModified = DateTime.UtcNow;

            await _commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);

        }

        public async Task Update(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var p = Page.FromIPage(page);
            p.LastModified = DateTime.UtcNow;
            p.Resources = page.Resources;

            await _commands.UpdateAsync(projectId, p.Id, p).ConfigureAwait(false);

        }

        public async Task Delete(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var page = await _query.FetchAsync(projectId, pageId, CancellationToken.None);
            if (page != null)
            {
                var pages = await GetAllPages(projectId, CancellationToken.None).ConfigureAwait(false);
                await _commands.DeleteAsync(projectId, pageId).ConfigureAwait(false);
                pages.Remove(page);
            }
        }

        private async Task<List<Page>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken)
        {
            var l = await _query.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var list = l.ToList();
            return list;
        }

        public async Task<string> CloneToNewProject(
            string sourceProjectId,
            string targetProjectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var page = await _query.FetchAsync(sourceProjectId, pageId, CancellationToken.None);
            if (page == null) throw new InvalidOperationException("page not found");

            var p = Page.FromIPage(page);

            p.LastModified = DateTime.UtcNow;
            p.ProjectId = targetProjectId;
            p.Id = _keyGenerator.GenerateKey(p);
            p.Resources = page.Resources;

            await _commands.CreateAsync(targetProjectId, p.Id, p).ConfigureAwait(false);

            return p.Id;
        }

    }
}
