// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-08-15
// 

using cloudscribe.SimpleContent.Web;
using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

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
            IHttpContextAccessor contextAccessor = null)
        {
            this.security = security;
            this.projectQueries = projectQueries;
            this.projectCommands = projectCommands;
            this.settingsResolver = settingsResolver;
            this.cache = cache;
            context = contextAccessor?.HttpContext;
        }

        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;
        private IProjectSecurityResolver security;
        private IProjectQueries projectQueries;
        private IProjectCommands projectCommands;
        private IProjectSettingsResolver settingsResolver;
        private ProjectSettings currentSettings = null;
        private IMemoryCache cache;

        public void ClearNavigationCache()
        {
            var cacheKey = "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder";
            cache.Remove(cacheKey);
            cacheKey = "cloudscribe.Web.Navigation.XmlNavigationTreeBuilder";
            cache.Remove(cacheKey);
            cacheKey = "JsonNavigationTreeBuilder";
            cache.Remove(cacheKey);
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

        public async Task Create(ProjectSettings project)
        {
            await projectCommands.Create(project.ProjectId, project, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task Update(ProjectSettings project)
        {
            await projectCommands.Update(project.ProjectId, project, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<ProjectSettings> GetCurrentProjectSettings()
        {
            await EnsureSettings().ConfigureAwait(false);
            return currentSettings;
        }

        public async Task<ProjectSettings> GetProjectSettings(string projectId)
        {
            
            return await projectQueries.GetProjectSettings(projectId, CancellationToken).ConfigureAwait(false);
        }

        public async Task<List<ProjectSettings>> GetUserProjects(string userName, string password)
        {
            var permission = await security.ValidatePermissions(
                string.Empty,
                userName,
                password,
                CancellationToken
                ).ConfigureAwait(false);

            var result = new List<ProjectSettings>(); //empty

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
