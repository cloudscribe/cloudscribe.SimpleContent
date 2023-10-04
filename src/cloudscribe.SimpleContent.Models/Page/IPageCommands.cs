// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2018-10-09
//

using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageCommandsSingleton : IPageCommands
    {

    }

    public interface IPageCommands
    {
        Task Create(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Update(
            string projectId,
            IPage page,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Delete(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        /// <summary>
        /// Clone a page from one project (tenant) to another
        /// </summary>
        /// <param name="sourceProjectId"></param>
        /// <param name="targetProjectId"></param>
        /// <param name="pageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The Id of the new page</returns>

        Task<string> CloneToNewProject(
            string sourceProjectId,
            string targetProjectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
