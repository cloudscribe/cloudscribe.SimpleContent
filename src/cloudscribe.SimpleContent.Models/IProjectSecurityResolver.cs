// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-02-16
// 

using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectSecurityResolver
    {
        Task<ProjectSecurityResult> ValidatePermissions(
            string blogId, 
            string userName,
            string providedPassword,
            CancellationToken cancellationToken);
    }

}
