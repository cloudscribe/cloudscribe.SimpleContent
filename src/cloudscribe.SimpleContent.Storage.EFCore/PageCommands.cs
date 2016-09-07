// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-09-06
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class PageCommands : IPageCommands
    {
        public PageCommands(SimpleContentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private SimpleContentDbContext dbContext;

        public async Task Create(
            string projectId,
            Page page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (page == null) throw new ArgumentException("page must not be null");
            //if (string.IsNullOrEmpty(projectId)) throw new ArgumentException("projectId must be provided");

            if (string.IsNullOrEmpty(page.Id)) { page.Id = Guid.NewGuid().ToString(); }

            if (string.IsNullOrEmpty(page.ProjectId)) page.ProjectId = projectId;
            page.LastModified = DateTime.UtcNow;
            page.PubDate = DateTime.UtcNow;
            
            dbContext.Pages.Add(page);

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task Update(
            string projectId,
            Page page,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (page == null) throw new ArgumentException("page must not be null");
            if (string.IsNullOrEmpty(page.Id)) throw new ArgumentException("can only update an existing page with a populated Id");

                //if (string.IsNullOrEmpty(projectId)) throw new ArgumentException("projectId must be provided");

            page.LastModified = DateTime.UtcNow;
            bool tracking = dbContext.ChangeTracker.Entries<Page>().Any(x => x.Entity.Id == page.Id);
            if (!tracking)
            {
                dbContext.Pages.Update(page);
            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task Delete(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
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
