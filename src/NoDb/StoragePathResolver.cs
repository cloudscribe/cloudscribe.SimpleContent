// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-23
// 


using System;
using System.IO;

namespace NoDb
{
    public class StoragePathResolver : IStoragePathResolver
    {
        public StoragePathResolver()
        {

        }

        public string ResolvePath(StoragePathOptions pathOptions, string type, string key = "", bool ensureFoldersExist = false)
        {
            if (pathOptions == null) throw new ArgumentException("StoragePathOptions must be provided");
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("type must be provied");

            var firstFolderPath = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator);

            if (ensureFoldersExist && !Directory.Exists(firstFolderPath))
            {
                Directory.CreateDirectory(firstFolderPath);
            }

            var projectsFolderPath = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator)
                + pathOptions.FolderSeparator
                + pathOptions.ProjectsFolderName
                + pathOptions.FolderSeparator
                ;

            if (ensureFoldersExist && !Directory.Exists(projectsFolderPath))
            {
                Directory.CreateDirectory(projectsFolderPath);
            }

            var projectIdFolderPath = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator)
                + pathOptions.FolderSeparator
                + pathOptions.ProjectsFolderName
                + pathOptions.FolderSeparator
                + pathOptions.ProjectIdFolderName
                + pathOptions.FolderSeparator
                ;

            if (ensureFoldersExist && !Directory.Exists(projectIdFolderPath))
            {
                Directory.CreateDirectory(projectIdFolderPath);
            }

            var typeFolderPath = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator)
                + pathOptions.FolderSeparator
                + pathOptions.ProjectsFolderName
                + pathOptions.FolderSeparator
                + pathOptions.ProjectIdFolderName
                + pathOptions.FolderSeparator
                + type.ToLowerInvariant().Trim()
                + pathOptions.FolderSeparator
                ;

            if (ensureFoldersExist && !Directory.Exists(typeFolderPath))
            {
                Directory.CreateDirectory(typeFolderPath);
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return typeFolderPath;
            }

            return typeFolderPath + key + ".json";
        }
    }
}
