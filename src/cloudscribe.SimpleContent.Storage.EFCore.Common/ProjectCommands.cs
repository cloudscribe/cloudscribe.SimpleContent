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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class ProjectCommands : IProjectCommands, IProjectCommandsSingleton
    {
        public ProjectCommands(ISimpleContentDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly ISimpleContentDbContextFactory _contextFactory;

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

            using (var db = _contextFactory.CreateContext())
            {
                db.Projects.Add(p);

                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

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

            using (var db = _contextFactory.CreateContext())
            {
                bool tracking = db.ChangeTracker.Entries<ProjectSettings>().Any(x => x.Entity.Id == p.Id);
                if (!tracking)
                {
                    db.Projects.Update(p);
                }

                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task Delete(
            string projectId,
            string projectKey,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var db = _contextFactory.CreateContext())
            {
                var itemToRemove = await db.Projects.SingleOrDefaultAsync(
               x => x.Id == projectKey
               , cancellationToken)
               .ConfigureAwait(false);

                if (itemToRemove == null) throw new InvalidOperationException("Post not found");

                db.Projects.Remove(itemToRemove);
                int rowsAffected = await db.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task<string> CloneToNewProject(
            string sourceProjectId,
            string targetProjectId,
            string newSiteName = null,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var db = _contextFactory.CreateContext())
            {

                var destinationProject = await db.Projects.SingleOrDefaultAsync(x =>
                    x.Id == targetProjectId,
                    cancellationToken).ConfigureAwait(false);

                var sourceProject = await db.Projects.SingleOrDefaultAsync(x =>
                    x.Id == sourceProjectId,
                    cancellationToken ).ConfigureAwait(false);

                if (sourceProject == null) throw new InvalidOperationException("Project not found");

                var p = ProjectSettings.FromIProjectSettings(sourceProject);
                p.Id = targetProjectId;

                if(destinationProject == null)
                {
                    db.Projects.Add(p);
                }
                else
                {
                    bool tracking = db.ChangeTracker.Entries<ProjectSettings>().Any(x => x.Entity.Id == p.Id);
                    if (!tracking)
                    {
                        db.Projects.Update(p);
                    };
                }
                int rowsAffected = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return p.Id;
            }
        }

    }
}
