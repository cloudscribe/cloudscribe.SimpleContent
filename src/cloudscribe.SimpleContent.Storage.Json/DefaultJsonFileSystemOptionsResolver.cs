// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-17
// Last Modified:           2016-02-17
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public class DefaultJsonFileSystemOptionsResolver : IJsonFileSystemOptionsResolver
    {
        public DefaultJsonFileSystemOptionsResolver(
            IApplicationEnvironment appEnv,
            IOptions<List<ProjectSettings>> projectListAccessor)
        {
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (projectListAccessor == null) { throw new ArgumentNullException(nameof(projectListAccessor)); }

            env = appEnv;
            allProjects = projectListAccessor.Value;
        }

        private IApplicationEnvironment env;
        private List<ProjectSettings> allProjects;

        public Task<JsonFileSystemOptions> Resolve(string projectId)
        {
            if (!IsValidProjectId(projectId))
            {
                throw new ArgumentException("invalid blog id");
            }

            var result = new JsonFileSystemOptions();
            result.AppRootFolderPath = env.ApplicationBasePath;
            result.ProjectIdFolderName = projectId;
            
            return Task.FromResult(result);
        }

        private bool IsValidProjectId(string projectId)
        {
            foreach (ProjectSettings s in allProjects)
            {
                if (s.ProjectId == projectId) { return true; }
            }

            return false;
        }

    }
}
