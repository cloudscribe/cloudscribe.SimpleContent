// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-05-28
// Last Modified:           2017-05-28
// 


using cloudscribe.FileManager.Web.Models;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;


namespace cloudscribe.Core.SimpleContent.Integration
{
    public class SiteFileSystemMediaProcessor : FileSystemMediaProcessor, IMediaProcessor
    {
        public SiteFileSystemMediaProcessor(
            IMediaPathResolver mediaPathResolver,
            ILogger<FileSystemMediaProcessor> logger,
            IHostingEnvironment env
            ):base(logger, env)
        {
            this.mediaPathResolver = mediaPathResolver;
        }

        private IMediaPathResolver mediaPathResolver;
        private MediaRootPathInfo rootPath;

        private async Task EnsurePathInfo()
        {
            if (rootPath != null) { return; }
            rootPath = await mediaPathResolver.Resolve().ConfigureAwait(false);
            if (rootPath != null) { return; }
        }

        private async Task EnsureFsPath(string mediaVirtualPath)
        {
            await EnsurePathInfo();
            var fsPath = rootPath.RootFileSystemPath + mediaVirtualPath.Replace('/', Path.DirectorySeparatorChar);
            if (Directory.Exists(fsPath)) return; //nothing to do

            var segments = mediaVirtualPath.Split('/');

            EnsureFolderPaths(rootPath.RootFileSystemPath, segments);

        }

        public override Task<string> ResolveMediaUrl(string mediaVirtualPath, string fileName)
        {
            return base.ResolveMediaUrl(mediaVirtualPath, fileName);
        }
        
        public override async Task SaveMedia(string mediaVirtualPath, string fileName, byte[] bytes)
        {
            await EnsureFsPath(mediaVirtualPath);
            var newUrl = mediaVirtualPath + fileName;
            var fsPath = rootPath.RootFileSystemPath + newUrl.Replace('/', Path.DirectorySeparatorChar);

            File.WriteAllBytes(fsPath, bytes);
            
        }
    }
}
