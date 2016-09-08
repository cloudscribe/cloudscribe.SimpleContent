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
    public class ProjectQueries : IProjectQueries
    {
        public ProjectQueries(SimpleContentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private SimpleContentDbContext dbContext;

        public async Task<IProjectSettings> GetProjectSettings(
            string projectId,
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Projects
                .Where(p => p.Id == projectId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<List<IProjectSettings>> GetProjectSettingsByUser(
            string userName,
            CancellationToken cancellationToken
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var result = new List<IProjectSettings>();

            //TODO: not sure we can implement this need to see where it is used and try to rafactor to not need it
            // looks like this is called from ProjecTService but only as a secondary query
            // after getting the projectid from another approach so it may not be needed
            // as long as metaweblog api works then it is not needed since that is the only place it is called from

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
