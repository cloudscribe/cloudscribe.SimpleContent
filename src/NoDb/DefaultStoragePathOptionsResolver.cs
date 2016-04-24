// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-23
// 

using Microsoft.Extensions.PlatformAbstractions;
using System.Threading.Tasks;

namespace NoDb
{
    public class DefaultStoragePathOptionsResolver : IStoragePathOptionsResolver
    {
        public DefaultStoragePathOptionsResolver(
            IApplicationEnvironment appEnv)
        {
            env = appEnv;
        }

        private IApplicationEnvironment env;

        public Task<StoragePathOptions> Resolve(string projectId)
        {
            //if (!IsValidProjectId(projectId))
            //{
            //    throw new ArgumentException("invalid blog id");
            //}

            var result = new StoragePathOptions();
            result.AppRootFolderPath = env.ApplicationBasePath;
            //result.ProjectIdFolderName = projectId;

            return Task.FromResult(result);
        }
    }
}
