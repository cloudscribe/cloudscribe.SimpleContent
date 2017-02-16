// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-02-15
// Last Modified:           2017-02-15
// 

using cloudscribe.FileManager.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Services
{
    public class FileManagerService
    {
        public FileManagerService(
            IMediaRootPathResolver mediaPathResolver,
            IImageResizer imageResizer,
            ILogger<FileManagerService> logger
            )
        {
            this.mediaPathResolver = mediaPathResolver;
            this.imageResizer = imageResizer;
            log = logger;
        }

        private IImageResizer imageResizer;
        private IMediaRootPathResolver mediaPathResolver;
        private MediaRootPathInfo rootPath;
        private ILogger log;

        private async Task EnsureProjectSettings()
        {
            if (rootPath != null) { return; }
            rootPath = await mediaPathResolver.Resolve().ConfigureAwait(false);
            if (rootPath != null) { return; }
        }

        private void EnsureSubFolders(string basePath, string[] segments)
        {
            var p = basePath;
            for (int i=0; i< segments.Length; i++)
            {
                p = Path.Combine(p, segments[i]);
                if(!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
            }
        }

        public async Task<ImageUploadResult> ProcessFile(IFormFile formFile, ImageProcessingOptions options, int eventCode)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            var origSizeVirtualPath = rootPath.RootVirtualPath + options.ImageOriginalSizeVirtualSubPath;
            var origSegments = options.ImageOriginalSizeVirtualSubPath.Split('/');
            EnsureSubFolders(rootPath.RootFileSystemPath, origSegments);
            var origSizeFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(origSegments));
            var newName = formFile.FileName.ToCleanFileName();
            var newUrl = origSizeVirtualPath + "/" + newName;
            var fsPath = Path.Combine(origSizeFsPath, newName);

            try
            {
                using (var stream = new FileStream(fsPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                // TODO: create websize
                // return websize

                return new ImageUploadResult
                {
                    OriginalSizeUrl = newUrl,
                    Name = newName,
                    Length = formFile.Length,
                    Type = formFile.ContentType

                };
            }
            catch (Exception ex)
            {
                log.LogError(eventCode, ex, ex.StackTrace);

                return new ImageUploadResult
                {
                    ErrorMessage = "There was an error logged during file processing"

                };
            }

            

           
        }

    }
}
