// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2018-07-07
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
    public class ProjectQueries : IProjectQueries
    {
        public ProjectQueries(ISimpleContentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ISimpleContentDbContext _dbContext;

        public async Task<IProjectSettings> GetProjectSettings(
            string projectId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _dbContext.Projects
                .Where(p => p.Id == projectId)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        
    }
}
