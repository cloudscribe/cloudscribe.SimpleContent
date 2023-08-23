// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-08-04
// Last Modified:           2018-10-09
//

using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectCommandsSingleton : IProjectCommands
    {

    }

    public interface IProjectCommands
    {
        Task Create(
            string projectId,
            IProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Update(
            string projectId,
            IProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Delete(
            string projectId,
            string projectKey,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<string> CloneToNewProject(
            string sourceProjectId,
            string targetProjectId,
            string newSiteName = null,
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }
}
