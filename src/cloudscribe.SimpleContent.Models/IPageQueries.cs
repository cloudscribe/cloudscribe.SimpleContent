// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-08-02
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
            CancellationToken cancellationToken
            );

        Task<Page> GetPage(
            string blogId,
            string postId,
            CancellationToken cancellationToken
            );

        Task<Page> GetPageBySlug(
            string blogId,
            string slug,
            CancellationToken cancellationToken
            );

        Task<List<Page>> GetAllPages(
            string blogId,
            CancellationToken cancellationToken);

        Task<List<Page>> GetRootPages(
            string blogId,
            CancellationToken cancellationToken
            );

        Task<List<Page>> GetChildPages(
            string blogId,
            string pageId,
            CancellationToken cancellationToken
            );
    }
}
