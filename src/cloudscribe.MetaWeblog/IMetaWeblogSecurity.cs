// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-02-15
// 

using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{
    public interface IMetaWeblogSecurity
    {
        Task<MetaWeblogSecurityResult> ValiatePermissions(MetaWeblogRequest request, CancellationToken cancellationToken);
        
    }
}
