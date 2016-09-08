// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-09-08
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class PageQueries : IPageQueries
    {
        public PageQueries(SimpleContentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private SimpleContentDbContext dbContext;

        public async Task<List<IPage>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.Pages
                        select x;

            var items = await query
                .AsNoTracking()
                .ToListAsync<IPage>(cancellationToken)
                .ConfigureAwait(false);

            return items;

        }

        public async Task<IPage> GetPage(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == pageId, cancellationToken)
                .ConfigureAwait(false)
                ;

        }

        public async Task<List<IPage>> GetRootPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Pages
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

        public async Task<List<IPage>> GetChildPages(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Pages
                .AsNoTracking()
                .Where(p => 
                p.ParentId == pageId && p.ProjectId == projectId
                )
                .OrderBy(p => p.PageOrder)
                .ToListAsync<IPage>(cancellationToken)
                .ConfigureAwait(false)
                ;

        }

        public async Task<IPage> GetPageBySlug(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Pages
                .AsNoTracking()
                .Where(p => 
                p.Slug == slug && p.ProjectId == projectId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false)
                ;
        }

        public async Task<bool> SlugIsAvailable(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var isInUse = await dbContext.Pages.AnyAsync(
                p => p.Slug == slug && p.ProjectId == projectId,
                cancellationToken
                ).ConfigureAwait(false);

            return !isInUse;

        }

        // not implemented, do we need categories for pages?
        public Task<int> GetCount(
            string projectId,
            string category,
            bool userIsBlogOwner,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(0);
        }

        public Task<Dictionary<string, int>> GetCategories(
            string projectId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken
            )
        {
            var result = new Dictionary<string, int>();


            return Task.FromResult(result);
        }


        #region IDisposable Support

        private void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
