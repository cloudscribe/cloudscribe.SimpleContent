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
    /// <summary>
    /// TObject must be serializable to json
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface ICommand<TObject> : IDisposable where TObject : class
    {
        Task<bool> CreateAsync(
            string projectId,
            string key,
            TObject obj, 
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<bool> UpdateAsync(
            string projectId,
            string key,
            TObject obj, 
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<bool> DeleteAsync(
            string projectId, 
            string key, 
            Type type, 
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }

    
}
