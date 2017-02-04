// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-09-08
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PageCommands : IPageCommands
    {
        public PageCommands(
            IBasicCommands<Page> pageCommands,
            IBasicQueries<Page> pageQueries
            //,ILogger<PageCommands> logger
            )
        {
            commands = pageCommands;
            query = pageQueries;
            //log = logger;
        }

        private IBasicCommands<Page> commands;
        private IBasicQueries<Page> query;
        //private ILogger log;

        public async Task Create(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrEmpty(page.Id)) { page.Id = Guid.NewGuid().ToString(); }

            var p = Page.FromIPage(page);
            p.LastModified = DateTime.UtcNow;
            
            p.PubDate = DateTime.UtcNow;
  
            await commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);
           
        }

        public async Task Update(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            //if (string.IsNullOrEmpty(page.Id)) { page.Id = Guid.NewGuid().ToString(); }
            var p = Page.FromIPage(page);
            p.LastModified = DateTime.UtcNow;
            
            await commands.UpdateAsync(projectId, p.Id, p).ConfigureAwait(false);
            
        }

        public async Task Delete(
            string projectId, 
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {

            var page = await query.FetchAsync(projectId, pageId, CancellationToken.None);
            if (page != null)
            {
                var pages = await GetAllPages(projectId, CancellationToken.None).ConfigureAwait(false);
                await commands.DeleteAsync(projectId, pageId).ConfigureAwait(false);
                pages.Remove(page);

            }


        }

        private async Task<List<Page>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken)
        {

            var l = await query.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var list = l.ToList();
            
            return list;
        }

    }
}
