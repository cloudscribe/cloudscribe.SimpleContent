// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-03-20
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public class ProjectFilePathResolver
    {
        public ProjectFilePathResolver(
            IJsonFileSystemOptionsResolver optionsResolver)
        {
            this.optionsResolver = optionsResolver;
        }

        private IJsonFileSystemOptionsResolver optionsResolver;
        private JsonFileSystemOptions fsOptions = null;

        public async Task EnsureInitialized(string projectId)
        {
            if (fsOptions == null)
            {
                fsOptions = await optionsResolver.Resolve(projectId).ConfigureAwait(false);
            }
        }

        public async Task EnsureFoldersExist(string projectId)
        {
            if (fsOptions == null)
            {
                fsOptions = await optionsResolver.Resolve(projectId).ConfigureAwait(false);
            }

            var firstFolderPath = fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator);

            if(!Directory.Exists(firstFolderPath))
            {
                Directory.CreateDirectory(firstFolderPath);
            }

            var tenantsFolderPath = fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator)
                + fsOptions.FolderSeparator
                + fsOptions.ProjectsFolderName
                + fsOptions.FolderSeparator
                ;

            if (!Directory.Exists(tenantsFolderPath))
            {
                Directory.CreateDirectory(tenantsFolderPath);
            }

            var projectIdFolderPath = fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator)
                + fsOptions.FolderSeparator
                + fsOptions.ProjectsFolderName
                + fsOptions.FolderSeparator
                + fsOptions.ProjectIdFolderName
                + fsOptions.FolderSeparator
                ;

            if (!Directory.Exists(projectIdFolderPath))
            {
                Directory.CreateDirectory(projectIdFolderPath);
            }

            var postsFolderPath = fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator)
                + fsOptions.FolderSeparator
                + fsOptions.ProjectsFolderName
                + fsOptions.FolderSeparator
                + fsOptions.ProjectIdFolderName
                + fsOptions.FolderSeparator
                + fsOptions.PostsFolderName
                + fsOptions.FolderSeparator
                ;

            if (!Directory.Exists(postsFolderPath))
            {
                Directory.CreateDirectory(postsFolderPath);
            }

            var pagesFolderPath = fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator)
                + fsOptions.FolderSeparator
                + fsOptions.ProjectsFolderName
                + fsOptions.FolderSeparator
                + fsOptions.ProjectIdFolderName
                + fsOptions.FolderSeparator
                + fsOptions.PagesFolderName
                + fsOptions.FolderSeparator
                ;

            if (!Directory.Exists(pagesFolderPath))
            {
                Directory.CreateDirectory(pagesFolderPath);
            }

        }

        public string GetPostFolderPath()
        {
            if (fsOptions == null) { throw new InvalidOperationException("call Initialize first"); }

            return fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator)
                + fsOptions.FolderSeparator
                + fsOptions.ProjectsFolderName 
                + fsOptions.FolderSeparator 
                + fsOptions.ProjectIdFolderName
                + fsOptions.FolderSeparator
                + fsOptions.PostsFolderName 
                + fsOptions.FolderSeparator;
        }

        public string GetPagesFolderPath()
        {
            if (fsOptions == null) { throw new InvalidOperationException("call Initialize first"); }

            return fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator)
                + fsOptions.FolderSeparator
                + fsOptions.ProjectsFolderName
                + fsOptions.FolderSeparator
                + fsOptions.ProjectIdFolderName
                + fsOptions.FolderSeparator
                + fsOptions.PagesFolderName
                + fsOptions.FolderSeparator;
        }

        
    }
}
