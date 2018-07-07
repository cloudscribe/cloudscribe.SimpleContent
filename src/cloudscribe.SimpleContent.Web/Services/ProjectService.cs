// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2018-07-07
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
            IProjectSecurityResolver security,
            IProjectQueries projectQueries,
            IProjectCommands projectCommands,
            IMemoryCache cache,
            IPageNavigationCacheKeys cacheKeys
            )
        {
            _security = security;
            _projectQueries = projectQueries;
            _projectCommands = projectCommands;
            _settingsResolver = settingsResolver;
            _cacheKeys = cacheKeys;
            _cache = cache;
           // _context = contextAccessor?.HttpContext;
        }

        //private readonly HttpContext _context;
        //private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;
        private IProjectSecurityResolver _security;
        private IProjectQueries _projectQueries;
        private IProjectCommands _projectCommands;
        private IProjectSettingsResolver _settingsResolver;
        private IProjectSettings _currentSettings = null;
        private IPageNavigationCacheKeys _cacheKeys;
        private IMemoryCache _cache;

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
                //if (context.User.Identity.IsAuthenticated)
                //{
                //    var userBlog = context.User.GetBlogId();
                //    if (!string.IsNullOrEmpty(userBlog))
                //    {
                //        if (currentSettings.ProjectId == userBlog) { userIsBlogOwner = true; }

                //    }
                //}

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

        //public async Task<List<IProjectSettings>> GetUserProjects(string userName, string password)
        //{
        //    var permission = await _security.ValidatePermissions(
        //        string.Empty,
        //        userName,
        //        password,
        //        CancellationToken
        //        ).ConfigureAwait(false);

        //    var result = new List<IProjectSettings>(); //empty

        //    if (!permission.CanEditPosts)
        //    {
        //        return result; //empty
        //    }

        //    var project = await _projectQueries.GetProjectSettings(permission.ProjectId, CancellationToken);
        //    if(project != null)
        //    {
        //        result.Add(project);
        //        return result;
        //    }
            
        //    //await EnsureBlogSettings().ConfigureAwait(false);
        //    //return settings;
        //    return await _projectQueries.GetProjectSettingsByUser(userName, CancellationToken).ConfigureAwait(false);
        //}
    }
}
