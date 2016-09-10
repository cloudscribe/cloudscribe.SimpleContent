// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-09-08
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageQueries
    {
        Task<bool> SlugIsAvailable(
            string blogId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<IPage> GetPage(
            string blogId,
            string postId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<IPage> GetPageBySlug(
            string blogId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IPage>> GetAllPages(
            string blogId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IPage>> GetRootPages(
            string blogId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IPage>> GetChildPages(
            string blogId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
