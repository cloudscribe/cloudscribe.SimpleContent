// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2018-10-09
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class PageCommands : IPageCommands, IPageCommandsSingleton
    {
        public PageCommands(ISimpleContentDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly ISimpleContentDbContextFactory _contextFactory;

        public async Task Create(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (page == null) throw new ArgumentException("page must not be null");
            
            var p = PageEntity.FromIPage(page);

            if (string.IsNullOrEmpty(p.Id)) { p.Id = Guid.NewGuid().ToString(); }

            if (string.IsNullOrEmpty(p.ProjectId)) p.ProjectId = projectId;
            p.LastModified = DateTime.UtcNow;

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.Pages.Add(p);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task Update(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (page == null) throw new ArgumentException("page must not be null");
            if (string.IsNullOrEmpty(page.Id)) throw new ArgumentException("can only update an existing page with a populated Id");
            var p = PageEntity.FromIPage(page);
            
            p.LastModified = DateTime.UtcNow;

            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<PageEntity>().Any(x => x.Entity.Id == p.Id);
                if (!tracking)
                {
                    dbContext.PageResources.RemoveRange(dbContext.PageResources.Where(x => x.PageEntityId == p.Id));
                    dbContext.Pages.Update(p);

                }

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                if (page.Resources.Count > 0)
                {
                    p.Resources = page.Resources;
                    foreach (var r in p.PageResources)
                    {
                        r.PageEntityId = p.Id;

                    }

                    rowsAffected = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            
        }

        public async Task Delete(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.Pages.SingleOrDefaultAsync(
                x => x.Id == pageId && x.ProjectId == projectId
                , cancellationToken)
                .ConfigureAwait(false);

                if (itemToRemove == null) throw new InvalidOperationException("Page not found");

                dbContext.Pages.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }


    }
}
