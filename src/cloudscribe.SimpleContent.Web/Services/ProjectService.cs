// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2019-02-17
// 

using cloudscribe.SimpleContent.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class ProjectService : IProjectService
    {

        public ProjectService(
            IProjectSettingsResolver settingsResolver,
            IProjectQueries projectQueries,
            IProjectCommands projectCommands
            )
        {
            _projectQueries = projectQueries;
            _projectCommands = projectCommands;
            _settingsResolver = settingsResolver;
           
           
        }
        
        private readonly IProjectQueries _projectQueries;
        private readonly IProjectCommands _projectCommands;
        private readonly IProjectSettingsResolver _settingsResolver;
        private IProjectSettings _currentSettings = null;
        
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
