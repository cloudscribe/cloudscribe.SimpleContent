// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2017-06-05
// 

using cloudscribe.SimpleContent.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class DefaultProjectSettingsResolver : IProjectSettingsResolver
    {
        public DefaultProjectSettingsResolver(
            IProjectQueries projectQueries
            )
        {
            _queries = projectQueries;
        }

        private IProjectQueries _queries;

        public async Task<IProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            return await _queries.GetProjectSettings("default", cancellationToken).ConfigureAwait(false);
 
        }
    }
}
