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
            IMediaPathResolver mediaPathResolver,
            IImageResizer imageResizer,
            ILogger<FileManagerService> logger
            )
        {
            this.mediaPathResolver = mediaPathResolver;
            this.imageResizer = imageResizer;
            log = logger;
        }

        private IImageResizer imageResizer;
        private IMediaPathResolver mediaPathResolver;
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

            var origSizeVirtualPath = rootPath.RootVirtualPath + options.ImageDefaultVirtualSubPath;
            var origSegments = options.ImageDefaultVirtualSubPath.Split('/');
            EnsureSubFolders(rootPath.RootFileSystemPath, origSegments);
            var origSizeFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(origSegments));
            var newName = formFile.FileName.ToCleanFileName();
            var newUrl = origSizeVirtualPath + "/" + newName;
            var fsPath = Path.Combine(origSizeFsPath, newName);

            var ext = Path.GetExtension(newName);
            var webSizeName = Path.GetFileNameWithoutExtension(newName) + "-ws" + ext;
            var webFsPath = Path.Combine(origSizeFsPath, webSizeName);
            var webUrl = origSizeVirtualPath + "/" + webSizeName;

            try
            {
                using (var stream = new FileStream(fsPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                
                if(options.AutoResize)
                {
                    var mimeType = GetMimeType(ext);
                    imageResizer.ResizeImage(
                        fsPath,
                        origSizeFsPath,
                        webSizeName,
                        mimeType,
                        options.WebSizeImageMaxWidth,
                        options.WebSizeImageMaxHeight
                        );
                }
                
                return new ImageUploadResult
                {
                    OriginalSizeUrl = newUrl,
                    WebSizeUrl = webUrl,
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

        public string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return string.Empty; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);

            switch (fileType)
            {
                case "doc":
                case "docx":
                    return "application/msword";

                case "xls":
                case "xlsx":
                    return "application/vnd.ms-excel";

                case "exe":
                    return "application/octet-stream";

                case "ppt":
                case "pptx":
                    return "application/vnd.ms-powerpoint";

                case "jpg":
                case "jpeg":
                    return "image/jpeg";

                case "gif":
                    return "image/gif";

                case "png":
                    return "image/png";

                case "bmp":
                    return "image/bmp";

                case "wmv":
                    return "video/x-ms-wmv";

                case "mpg":
                case "mpeg":
                    return "video/mpeg";

                case "mp4":
                    return "video/mp4";

                case "mp3":
                    return "audio/mp3";

                case "m4a":
                    return "audio/m4a";

                case "m4v":
                    return "video/m4v";

                case "oog":
                case "ogv":
                    return "video/ogg";
                case "oga":
                case "spx":
                    return "audio/ogg";

                case "webm":
                    return "video/webm";

                case "mov":
                    return "video/quicktime";

                case "flv":
                    return "video/x-flv";

                case "ico":
                    return "image/x-icon";

                case "htm":
                case "html":
                    return "text/html";

                case "css":
                    return "text/css";

                case "eps":
                    return "application/postscript";

            }

            return "application/" + fileType;


        }

        public bool IsNonAttacmentFileType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return false; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);
            if (fileType == "pdf") { return true; }
            //if (fileType == "wmv") { return true; } //necessary?


            return false;
        }

    }
}
