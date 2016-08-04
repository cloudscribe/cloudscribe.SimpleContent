// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-08-02
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Options;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    
    public class ProjectQueries : IProjectQueries
    {

        public ProjectQueries(
            IBasicQueries<ProjectSettings> queries,
            IOptions<List<ProjectSettings>> projectListAccessor
            )
        {
            configProjects = projectListAccessor.Value;
            this.queries = queries;
        }

        private IBasicQueries<ProjectSettings> queries;

        private List<ProjectSettings> configProjects;

        public async Task<ProjectSettings> GetProjectSettings(
            string projectId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = configProjects.Where(b => b.ProjectId == projectId).FirstOrDefault();
            if(result != null) { return result; }
            result = await queries.FetchAsync(projectId, projectId, cancellationToken);
            return result;
        }

        public async Task<List<ProjectSettings>> GetProjectSettingsByUser(
            string userName,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new List<ProjectSettings>();
            var defaultProject = await GetProjectSettings("default", cancellationToken).ConfigureAwait(false);
            if(defaultProject != null)
            {
                result.Add(defaultProject);
            }
            
            return result;
        }


        
    }
}
