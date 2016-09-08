// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-09-08
// 

using cloudscribe.SimpleContent.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class DefaultProjectSettingsResolver : IProjectSettingsResolver
    {
        public DefaultProjectSettingsResolver(
            IProjectQueries blogSettingsRepository
            )
        {
            blogRepo = blogSettingsRepository;
        }

        private IProjectQueries blogRepo;

        public async Task<IProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            return await blogRepo.GetProjectSettings("default", cancellationToken).ConfigureAwait(false);
 
        }
    }
}
