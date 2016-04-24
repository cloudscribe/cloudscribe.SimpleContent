// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-24
// 


using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    public class DefaultStoragePathResolver<TObject> : IStoragePathResolver<TObject> where TObject : class 
    {
        public DefaultStoragePathResolver(
            IStoragePathOptionsResolver storageOptionsResolver)
        {
            optionsResolver = storageOptionsResolver;
        }

        private IStoragePathOptionsResolver optionsResolver;

        /// <summary>
        /// resolves the storage path for a given TObject and projectid. if no key is provided it just returns the folder path
        /// where that type is stored
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <param name="ensureFoldersExist"></param>
        /// <returns></returns>
        public async Task<string> ResolvePath(
            string projectId, 
            string key = "", 
            bool ensureFoldersExist = false,
            string fileExtension = ".json",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provied");
            var type = typeof(TObject).Name.ToLowerInvariant();
            var pathOptions = await optionsResolver.Resolve(projectId).ConfigureAwait(false);

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
                + projectId
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
                + projectId
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

            return typeFolderPath + key + fileExtension;
        }


        /// <summary>
        /// the purpose of this overload is so implementors of the interface can do more complex storage patterns
        /// ie I will try to implement for the blog to store posts in year/month folders according to pubdate
        /// so my custom resolver for Posts will be able to interogate the object that needs to be saved and determine where to put it
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="ensureFoldersExist"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> ResolvePath(
            string projectId,
            string key,
            TObject obj,
            bool ensureFoldersExist = false,
            string fileExtension = ".json",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            // in the default implementation we are not doing anything based on the object properties
            // but some custom logic could be plugged in here by implementing the interface yourself

            return await ResolvePath(projectId, key, ensureFoldersExist, fileExtension, cancellationToken).ConfigureAwait(false);

        }
    }
}
