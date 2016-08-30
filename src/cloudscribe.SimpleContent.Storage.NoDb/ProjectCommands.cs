// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-08-04
// Last Modified:           2016-08-04
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class ProjectCommands : IProjectCommands
    {
        public ProjectCommands(
            IBasicCommands<ProjectSettings> pageCommands,
            IBasicQueries<ProjectSettings> pageQueries,
            ILogger<ProjectCommands> logger
            )
        {
            commands = pageCommands;
            query = pageQueries;
            log = logger;
        }

        private IBasicCommands<ProjectSettings> commands;
        private IBasicQueries<ProjectSettings> query;
        private ILogger log;

        public async Task Create(
            string projectId,
            ProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrEmpty(project.Id)) { project.Id = Guid.NewGuid().ToString(); }
            
            await commands.CreateAsync(projectId, project.Id, project).ConfigureAwait(false);
        }

        public async Task Update(
            string projectId,
            ProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await commands.UpdateAsync(projectId, project.Id, project).ConfigureAwait(false);
        }

        public async Task Delete(
            string projectId,
            string projectKey,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await commands.DeleteAsync(projectId, projectKey).ConfigureAwait(false);  
        }


    }
}
