// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-08-04
// Last Modified:           2016-09-08
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
            IProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrEmpty(project.Id)) { project.Id = Guid.NewGuid().ToString(); }

            var p = ProjectSettings.FromIProjectSettings(project);

            await commands.CreateAsync(projectId, p.Id, p).ConfigureAwait(false);
        }

        public async Task Update(
            string projectId,
            IProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var p = ProjectSettings.FromIProjectSettings(project);
            await commands.UpdateAsync(projectId, p.Id, p).ConfigureAwait(false);
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
