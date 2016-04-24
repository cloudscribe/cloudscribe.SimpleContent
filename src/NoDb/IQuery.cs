// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-24
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    public interface IQuery<TObject> : IDisposable where TObject : class
    {
        Task<TObject> FetchAsync(
            string projectId,
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<int> GetCountAsync(
            string projectId,
            Type type,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<TObject>> GetAllAsync(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        //Task<IEnumerable<TObject>> GetPage(
        //    string projectId,
        //    string type,
        //    int pageNumber,
        //    int pageSize,
        //    CancellationToken cancellationToken = default(CancellationToken)
        //    );

        //Task<IEnumerable<TObject>> GetPage(
        //    string projectId,
        //    string type,
        //    DateTime modifiedBeginDate,
        //    DateTime modifiedEndDate,
        //    int pageNumber,
        //    int pageSize,
        //    CancellationToken cancellationToken = default(CancellationToken)
        //    );
    }
}
