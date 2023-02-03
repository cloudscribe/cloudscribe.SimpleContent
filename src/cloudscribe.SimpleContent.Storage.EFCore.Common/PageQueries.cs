// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2018-10-09
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class PageQueries : IPageQueries, IPageQueriesSingleton
    {
        public PageQueries(ISimpleContentDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly ISimpleContentDbContextFactory _contextFactory;

        public async Task<List<IPage>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Pages
                            where x.ProjectId == projectId
                            select x;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<IPage>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }
            
        }

        public async Task<List<IPage>> GetPagesReadyForPublish(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentTime = DateTime.UtcNow;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.Pages
                            where x.ProjectId == projectId
                            && x.DraftPubDate != null
                            && x.DraftPubDate < currentTime
                            select x;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<IPage>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }
            
        }

        public async Task<IPage> GetPage(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Pages
                .Include(p => p.PageResources)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == pageId, cancellationToken)
                .ConfigureAwait(false)
                ;
            }
            
        }

        public async Task<List<IPage>> GetRootPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Pages
                .Include(p => p.PageResources)
                .AsNoTracking()
                .Where(p =>
                p.ProjectId == projectId
                && (p.ParentId == "0" || p.ParentId == null || p.ParentId == "")
                )
                .OrderBy(p => p.PageOrder)
                .ToListAsync<IPage>(cancellationToken)

                .ConfigureAwait(false)
                ;
            }
            
        }

        public async Task<List<IPage>> GetChildPages(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Pages
                .Include(p => p.PageResources)
                .AsNoTracking()
                .Where(p =>
                p.ParentId == pageId && p.ProjectId == projectId
                )
                .OrderBy(p => p.PageOrder)
                .ToListAsync<IPage>(cancellationToken)
                .ConfigureAwait(false)
                ;
            }
            
        }

        public async Task<IPage> GetPageBySlug(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Pages
                .Include(p => p.PageResources)
                .AsNoTracking()
                .Where(p =>
                p.Slug == slug && p.ProjectId == projectId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false)
                ;
            } 
        }

        public async Task<IPage> GetPageByCorrelationKey(
            string projectId,
            string correlationKey,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                return await dbContext.Pages
               .Include(p => p.PageResources)
               .AsNoTracking()
               .Where(p =>
               p.CorrelationKey == correlationKey && p.ProjectId == projectId)
               .FirstOrDefaultAsync(cancellationToken)
               .ConfigureAwait(false)
               ;
            }
        }

        public async Task<bool> SlugIsAvailable(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var dbContext = _contextFactory.CreateContext())
            {
                var isInUse = await dbContext.Pages.AnyAsync(
                p => p.Slug == slug && p.ProjectId == projectId,
                cancellationToken
                ).ConfigureAwait(false);

                return !isInUse;
            }
            
        }

        public async Task<int> GetChildPageCount(
            string projectId,
            string pageId,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentTime = DateTime.UtcNow;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var count = await dbContext.Pages
                .CountAsync(x =>
                x.ProjectId == projectId
                && x.ParentId == pageId
                && (includeUnpublished || (x.IsPublished && x.PubDate <= currentTime))

                );

                return count;
            }
        }

        // not implemented, do we need categories for pages?
        //public Task<int> GetCount(
        //    string projectId,
        //    string category,
        //    bool userIsBlogOwner,
        //    CancellationToken cancellationToken = default(CancellationToken)
        //    )
        //{
        //    return Task.FromResult(0);
        //}

        //public Task<Dictionary<string, int>> GetCategories(
        //    string projectId,
        //    bool userIsBlogOwner,
        //    CancellationToken cancellationToken
        //    )
        //{
        //    var result = new Dictionary<string, int>();


        //    return Task.FromResult(result);
        //}

    }
}
