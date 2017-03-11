// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2017-03-11
// 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageQueries
    {
        Task<bool> SlugIsAvailable(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<IPage> GetPage(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<IPage> GetPageBySlug(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<IPage> GetPageByCorrelationKey(
            string projectId,
            string correlationKey,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IPage>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IPage>> GetRootPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IPage>> GetChildPages(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
