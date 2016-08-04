// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-08-04
// Last Modified:           2016-08-04
// 

using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectCommands
    {
        Task Create(
            string projectId,
            ProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Update(
            string projectId,
            ProjectSettings project,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Delete(
            string projectId,
            string projectKey,
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }
}
