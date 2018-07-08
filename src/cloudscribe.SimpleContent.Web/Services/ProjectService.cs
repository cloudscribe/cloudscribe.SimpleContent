// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-07-08
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class ProjectService : IProjectService
    {

        public ProjectService(
            IProjectSettingsResolver settingsResolver,
            IProjectQueries projectQueries,
            IProjectCommands projectCommands,
            IMemoryCache cache,
            IPageNavigationCacheKeys cacheKeys
            )
        {
            _projectQueries = projectQueries;
            _projectCommands = projectCommands;
            _settingsResolver = settingsResolver;
            _cacheKeys = cacheKeys;
            _cache = cache;
           
        }
        
        private readonly IProjectQueries _projectQueries;
        private readonly IProjectCommands _projectCommands;
        private readonly IProjectSettingsResolver _settingsResolver;
        private readonly IPageNavigationCacheKeys _cacheKeys;
        private readonly IMemoryCache _cache;
        private IProjectSettings _currentSettings = null;

        public void ClearNavigationCache()
        {
            _cache.Remove(_cacheKeys.PageTreeCacheKey);
            _cache.Remove(_cacheKeys.XmlTreeCacheKey);
            _cache.Remove(_cacheKeys.JsonTreeCacheKey);
        }

        private async Task<bool> EnsureSettings()
        {
            if (_currentSettings != null) { return true; }
            _currentSettings = await _settingsResolver.GetCurrentProjectSettings(CancellationToken.None);
            if (_currentSettings != null)
            {
                return true;
            }
            return false;
        }

        public async Task Create(IProjectSettings project)
        {
            await _projectCommands.Create(project.Id, project, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task Update(IProjectSettings project)
        {
            await _projectCommands.Update(project.Id, project, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<IProjectSettings> GetCurrentProjectSettings()
        {
            await EnsureSettings().ConfigureAwait(false);
            return _currentSettings;
        }

        public async Task<IProjectSettings> GetProjectSettings(string projectId)
        {
            
            return await _projectQueries.GetProjectSettings(projectId, CancellationToken.None).ConfigureAwait(false);
        }

    }
}
