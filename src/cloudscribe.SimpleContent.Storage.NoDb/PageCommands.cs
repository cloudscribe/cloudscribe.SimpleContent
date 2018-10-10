// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
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
            //,ILogger<PageCommands> logger
            )
        {
            _commands = pageCommands;
            _query = pageQueries;
            _keyGenerator = keyGenerator;
            //_log = logger;
        }

        private IBasicCommands<Page> _commands;
        private IBasicQueries<Page> _query;
        private IKeyGenerator _keyGenerator;
        //private ILogger _log;


        public async Task Create(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var p = Page.FromIPage(page);
            p.Id = _keyGenerator.GenerateKey(p);
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

    }
}
