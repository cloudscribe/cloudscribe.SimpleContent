// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-08-09
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    /// <summary>
    /// this one uses config based list of projects rather than actually using NoDb
    /// this was implemented first and is suitable for use with SimpleAuth which is also config based
    /// it is read only, settings must be updated manually in the config files
    /// </summary>
    public class ConfigProjectQueries : IProjectQueries
    {
        public ConfigProjectQueries(
            IOptions<List<ProjectSettings>> projectListAccessor
            )
        {
            allProjects = projectListAccessor.Value;
        }

        private List<ProjectSettings> allProjects;

        public Task<ProjectSettings> GetProjectSettings(
            string projectId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = allProjects.Where(b => b.ProjectId == projectId).FirstOrDefault();
            return Task.FromResult(result);
        }

        public async Task<List<ProjectSettings>> GetProjectSettingsByUser(
            string userName,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new List<ProjectSettings>();
            var defaultProject = await GetProjectSettings("default", cancellationToken).ConfigureAwait(false);
            if (defaultProject != null)
            {
                result.Add(defaultProject);
            }

            return result;
        }


    }
}
