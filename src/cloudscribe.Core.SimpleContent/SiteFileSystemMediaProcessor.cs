// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-05-28
// Last Modified:           2018-07-09
// 


using cloudscribe.Core.Models;
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
            SiteContext currentSite,
            IMediaPathResolver mediaPathResolver,
            ILogger<FileSystemMediaProcessor> logger,
            IWebHostEnvironment env
            ):base(logger, env)
        {
            _currentSite = currentSite;
            _mediaPathResolver = mediaPathResolver;
        }

        private readonly SiteContext _currentSite;
        private IMediaPathResolver _mediaPathResolver;
        private MediaRootPathInfo rootPath;

        private async Task EnsurePathInfo()
        {
            if (rootPath != null) { return; }
            rootPath = await _mediaPathResolver.Resolve().ConfigureAwait(false);
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

        public override async Task<string> ResolveMediaUrl(string mediaVirtualPath, string fileName)
        {
            var result = await base.ResolveMediaUrl(mediaVirtualPath, fileName);
            if(!string.IsNullOrWhiteSpace(_currentSite.SiteFolderName))
            {
                return  "/" + _currentSite.SiteFolderName + result;
            }

            return result;
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
