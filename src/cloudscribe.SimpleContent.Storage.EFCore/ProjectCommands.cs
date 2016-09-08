// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-09-08
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class ProjectCommands : IProjectCommands
    {
        public ProjectCommands(SimpleContentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private SimpleContentDbContext dbContext;

        public async Task Create(
            string projectId,
            IProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (project == null) throw new ArgumentException("project must not be null");
            if (string.IsNullOrEmpty(projectId)) throw new ArgumentException("projectId must be provided");

            var p = ProjectSettings.FromIProjectSettings(project);

            if (string.IsNullOrEmpty(p.Id)) { p.Id = projectId; }
            
            dbContext.Projects.Add(p);

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task Update(
            string projectId,
            IProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (project == null) throw new ArgumentException("project must not be null");
            if (string.IsNullOrEmpty(project.Id)) throw new ArgumentException("can only update an existing project with a populated Id");

            //if (string.IsNullOrEmpty(projectId)) throw new ArgumentException("projectId must be provided");
            var p = ProjectSettings.FromIProjectSettings(project);

            bool tracking = dbContext.ChangeTracker.Entries<ProjectSettings>().Any(x => x.Entity.Id == p.Id);
            if (!tracking)
            {
                dbContext.Projects.Update(p);
            }

            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task Delete(
            string projectId,
            string projectKey,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var itemToRemove = await dbContext.Projects.SingleOrDefaultAsync(
               x => x.Id == projectKey 
               , cancellationToken)
               .ConfigureAwait(false);

            if (itemToRemove == null) throw new InvalidOperationException("Post not found");

            dbContext.Projects.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

    }
}
