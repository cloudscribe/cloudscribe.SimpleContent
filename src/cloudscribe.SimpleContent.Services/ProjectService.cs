// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-02-25
// 

using cloudscribe.SimpleContent.Common;
using cloudscribe.SimpleContent.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class ProjectService : IProjectService
    {

        public ProjectService(
            IProjectSettingsResolver settingsResolver,
            IProjectSettingsRepository settingsRepo,
            IHttpContextAccessor contextAccessor = null)
        {
            this.settingsRepo = settingsRepo;
            this.settingsResolver = settingsResolver;
            context = contextAccessor?.HttpContext;
        }

        private readonly HttpContext context;
        private CancellationToken CancellationToken => context?.RequestAborted ?? CancellationToken.None;
        private IProjectSettingsRepository settingsRepo;
        private IProjectSettingsResolver settingsResolver;
        private ProjectSettings currentSettings = null;

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

        public async Task<ProjectSettings> GetCurrentProjectSettings()
        {
            await EnsureSettings().ConfigureAwait(false);
            return currentSettings;
        }

        public async Task<ProjectSettings> GetProjectSettings(string projectId)
        {
            
            return await settingsRepo.GetProjectSettings(projectId, CancellationToken).ConfigureAwait(false);
        }

        public async Task<List<ProjectSettings>> GetUserProjects(string userName)
        {
            //await EnsureBlogSettings().ConfigureAwait(false);
            //return settings;
            return await settingsRepo.GetProjectSettingsByUser(userName, CancellationToken).ConfigureAwait(false);
        }
    }
}
