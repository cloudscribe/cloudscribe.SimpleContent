// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2017-04-17
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
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
            IPageNavigationCacheKeys cacheKeys,
            IHttpContextAccessor contextAccessor = null)
        {
            this.security = security;
            this.projectQueries = projectQueries;
            this.projectCommands = projectCommands;
            this.settingsResolver = settingsResolver;
            this.cacheKeys = cacheKeys;
            this.cache = cache;
            context = contextAccessor?.HttpContext;
        }

        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;
        private IProjectSecurityResolver security;
        private IProjectQueries projectQueries;
        private IProjectCommands projectCommands;
        private IProjectSettingsResolver settingsResolver;
        private IProjectSettings currentSettings = null;
        private IPageNavigationCacheKeys cacheKeys;
        private IMemoryCache cache;

        public void ClearNavigationCache()
        {
            cache.Remove(cacheKeys.PageTreeCacheKey);
            cache.Remove(cacheKeys.XmlTreeCacheKey);
            cache.Remove(cacheKeys.JsonTreeCacheKey);
        }

        private async Task<bool> EnsureSettings()
        {
            if (currentSettings != null) { return true; }
            currentSettings = await settingsResolver.GetCurrentProjectSettings(CancellationToken);
            if (currentSettings != null)
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
            await projectCommands.Create(project.Id, project, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task Update(IProjectSettings project)
        {
            await projectCommands.Update(project.Id, project, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<IProjectSettings> GetCurrentProjectSettings()
        {
            await EnsureSettings().ConfigureAwait(false);
            return currentSettings;
        }

        public async Task<IProjectSettings> GetProjectSettings(string projectId)
        {
            
            return await projectQueries.GetProjectSettings(projectId, CancellationToken).ConfigureAwait(false);
        }

        public async Task<List<IProjectSettings>> GetUserProjects(string userName, string password)
        {
            var permission = await security.ValidatePermissions(
                string.Empty,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            var result = new List<IProjectSettings>(); //empty

            if (!permission.CanEditPosts)
            {
                return result; //empty
            }

            var project = await projectQueries.GetProjectSettings(permission.ProjectId, CancellationToken);
            if(project != null)
            {
                result.Add(project);
                return result;
            }
            
            //await EnsureBlogSettings().ConfigureAwait(false);
            //return settings;
            return await projectQueries.GetProjectSettingsByUser(userName, CancellationToken).ConfigureAwait(false);
        }
    }
}
