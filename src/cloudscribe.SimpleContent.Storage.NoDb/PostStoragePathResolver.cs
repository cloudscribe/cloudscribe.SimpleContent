// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-04-25
// 

using cloudscribe.SimpleContent.Models;
using NoDb;
using System;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PostStoragePathResolver : IStoragePathResolver<Post>
    {
        public PostStoragePathResolver(
            IStoragePathOptionsResolver storageOptionsResolver)
        {
            optionsResolver = storageOptionsResolver;
        }

        private IStoragePathOptionsResolver optionsResolver;

        /// <summary>
        /// if key is not provided this method will return the post folder path
        /// if key is provided it will try to find the file which should be nested in year/month
        /// folder based on pubdate, if the file exists it will return the path to the file
        /// if not it will return the post folder plus key plus file extension
        /// however that is not the correct place to store a new post. The other ResolvePath method 
        /// which takes an instance of Post should be used for determining where to save a post
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <param name="fileExtension"></param>
        /// <param name="ensureFoldersExist"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> ResolvePath(
            string projectId,
            string key = "",
            string fileExtension = ".json",
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

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

            var type = typeof(Post).Name.ToLowerInvariant();

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

            var fileName = key + fileExtension;
            var filePath =  typeFolderPath + key + fileExtension;
            if (File.Exists(filePath)) return filePath;

            // if the file is not found in the type folder
            // we need to check for deeper folders year/month
            // if the file is found there then return the path
            foreach (string file in Directory.EnumerateFiles(
                typeFolderPath,
                fileName,
                SearchOption.AllDirectories) // this is needed for blog posts which are nested in year/month folders
                )
            {
                return file; 
            }

            // otherwise return the best path calculation based on info provided
            // ie to save a file the other ResolvePath method should be used and the
            // Post should be passed in to determine the year/month path
            return filePath;

        }

        /// <summary>
        /// this method should be used to calculate the path to create or update a post
        /// it will use the year/month of the post.PubDate in determining the path
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <param name="post"></param>
        /// <param name="fileExtension"></param>
        /// <param name="ensureFoldersExist"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> ResolvePath(
            string projectId,
            string key,
            Post post,
            string fileExtension = ".json",
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

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

            var type = typeof(Post).Name.ToLowerInvariant();

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

            var yearFolderName = post.PubDate.Year.ToString(CultureInfo.InvariantCulture);
            var monthFolderName = post.PubDate.Month.ToString("00", CultureInfo.InvariantCulture);

            var yearPath = typeFolderPath
                    + yearFolderName
                    + pathOptions.FolderSeparator;

            if (ensureFoldersExist && !Directory.Exists(yearPath))
            {
                Directory.CreateDirectory(yearPath);
            }

            var monthPath = yearPath
                    + monthFolderName
                    + pathOptions.FolderSeparator;

            if (ensureFoldersExist && !Directory.Exists(monthPath))
            {
                Directory.CreateDirectory(monthPath);
            }

            // we don't care if this file eists
            // this method is for calculating where to save a post
            return monthPath + key + fileExtension;
        }

    }
}
