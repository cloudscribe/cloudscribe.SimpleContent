// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-02-15
// Last Modified:           2017-02-17
// 

using cloudscribe.FileManager.Web.Models;
using cloudscribe.FileManager.Web.Models.TreeView;
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
            IOptions<FileManagerIcons> iconsAccessor,
            ILogger<FileManagerService> logger
            )
        {
            this.mediaPathResolver = mediaPathResolver;
            this.imageResizer = imageResizer;
            icons = iconsAccessor.Value;
            log = logger;
        }

        private IImageResizer imageResizer;
        private IMediaPathResolver mediaPathResolver;
        private MediaRootPathInfo rootPath;
        private FileManagerIcons icons;
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
            for (int i = 0; i < segments.Length; i++)
            {
                p = Path.Combine(p, segments[i]);
                if (!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
            }
        }

        private int GetMaxWidth(int? maxWidth, ImageProcessingOptions options)
        {
            if(maxWidth.HasValue)
            {
                if(maxWidth.Value >= options.ResizeMinAllowedWidth && maxWidth.Value <= options.ResizeMaxAllowedWidth)
                {
                    return maxWidth.Value;
                }
            }

            return options.WebSizeImageMaxWidth;
        }

        private int GetMaxHeight(int? maxHeight, ImageProcessingOptions options)
        {
            if (maxHeight.HasValue)
            {
                if (maxHeight.Value >= options.ResizeMinAllowedHeight && maxHeight.Value <= options.ResizeMaxAllowedHeight)
                {
                    return maxHeight.Value;
                }
            }

            return options.WebSizeImageMaxWidth;
        }

        public async Task<string> GetRootVirtualPath()
        {
            await EnsureProjectSettings().ConfigureAwait(false);
            return rootPath.RootVirtualPath;
        }

        //public async Task<List<Node>> CreateFolder(string virtualStartPath)
        //{

        //}

        public async Task<UploadResult> ProcessFile(
            IFormFile formFile,
            ImageProcessingOptions options,
            bool? resizeImages,
            int? maxWidth,
            int? maxHeight,
            string requestedVirtualPath = "")
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            string currentFsPath = rootPath.RootFileSystemPath;
            string currentVirtualPath = rootPath.RootVirtualPath;
            string[] virtualSegments = options.ImageDefaultVirtualSubPath.Split('/');

            if ((!string.IsNullOrEmpty(requestedVirtualPath)) && (requestedVirtualPath.StartsWith(rootPath.RootVirtualPath)))
            {

                var virtualSubPath = requestedVirtualPath.Substring(rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                if(segments.Length > 0)
                {
                    var requestedFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments));
                    if (!Directory.Exists(requestedFsPath))
                    {
                        log.LogError("directory not found for currentPath " + requestedFsPath);
                    }
                    else
                    {
                        currentVirtualPath = requestedVirtualPath;
                        virtualSegments = segments;
                        currentFsPath = Path.Combine(currentFsPath, Path.Combine(virtualSegments));
                    }
                }    
                
            }
            else
            {

                // only ensure the folders if no currentDir provided,
                // if it is provided it must be an existing path
                // options.ImageDefaultVirtualSubPath might not exist on first upload so need to ensure it
                currentVirtualPath = currentVirtualPath + options.ImageDefaultVirtualSubPath;
                currentFsPath = Path.Combine(currentFsPath, Path.Combine(virtualSegments));
                EnsureSubFolders(rootPath.RootFileSystemPath, virtualSegments);
            }

            var newName = formFile.FileName.ToCleanFileName();
            var newUrl = currentVirtualPath + "/" + newName;
            var fsPath = Path.Combine(currentFsPath, newName);

            var ext = Path.GetExtension(newName);
            var webSizeName = Path.GetFileNameWithoutExtension(newName) + "-ws" + ext;
            var webFsPath = Path.Combine(currentFsPath, webSizeName);
            string webUrl = string.Empty;

            try
            {
                using (var stream = new FileStream(fsPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                if ((options.AutoResize)&& IsWebImageFile(ext))
                {
                    var mimeType = GetMimeType(ext);
                    webUrl = currentVirtualPath + "/" + webSizeName;
                    int resizeWidth = GetMaxWidth(maxWidth, options);
                    int resizeHeight = GetMaxWidth(maxHeight, options);

                    imageResizer.ResizeImage(
                        fsPath,
                        currentFsPath,
                        webSizeName,
                        mimeType,
                        resizeWidth,
                        resizeHeight
                        );
                }

                return new UploadResult
                {
                    OriginalUrl = newUrl,
                    ResizedUrl = webUrl,
                    Name = newName,
                    Length = formFile.Length,
                    Type = formFile.ContentType

                };
            }
            catch (Exception ex)
            {
                log.LogError(MediaLoggingEvents.FILE_PROCCESSING, ex, ex.StackTrace);

                return new UploadResult
                {
                    ErrorMessage = "There was an error logged during file processing"

                };
            }
        }


        public async Task<List<Node>> GetFileTree(string virtualStartPath)
        {
            await EnsureProjectSettings().ConfigureAwait(false);

            var list = new List<Node>();

            if(!Directory.Exists(rootPath.RootFileSystemPath)) 
            {
                log.LogError("directory not found for RootFileSystemPath " + rootPath.RootFileSystemPath);
                return list;
            }

            DirectoryInfo currentDirectory;
            IEnumerable<DirectoryInfo> folders;
            //bool isRoot = false;
            string currentFsPath = rootPath.RootFileSystemPath;
            string currentVirtualPath = rootPath.RootVirtualPath;

            if (!string.IsNullOrEmpty(virtualStartPath))
            {
                if(!virtualStartPath.StartsWith(rootPath.RootVirtualPath))
                {
                    log.LogError("virtualStartPath did not start with RootFileSystemPath " + virtualStartPath);
                    return list;
                }
                var virtualSubPath = virtualStartPath.Substring(rootPath.RootVirtualPath.Length);
                var segments = virtualSubPath.Split('/');
                currentFsPath = Path.Combine(rootPath.RootFileSystemPath, Path.Combine(segments)); 
                if(!Directory.Exists(currentFsPath))
                {
                    log.LogError("directory not found for currentPath " + currentFsPath);
                    return list;
                }
                currentDirectory = new DirectoryInfo(currentFsPath);
                currentVirtualPath = virtualStartPath;
            }
            else
            {
                //isRoot = true;
                currentDirectory = new DirectoryInfo(rootPath.RootFileSystemPath);
            }
            
            folders = from folder in currentDirectory.GetDirectories("*", SearchOption.TopDirectoryOnly)
                      select folder;


            foreach (var folder in folders)
            {
                var node = new Node();
                node.Text = folder.Name;
                node.Type = "d";
                node.VirtualPath = currentVirtualPath + "/" + folder.Name;
                node.Id = node.VirtualPath; // TODO: maybe just use id
                node.Created = folder.CreationTimeUtc;
                node.Modified = folder.LastWriteTimeUtc;
                node.Icon = icons.Folder;
                node.SelectedIcon = node.Icon;
                node.LazyLoad = true;
                list.Add(node);
            }
            var rootFiles = Directory.GetFiles(currentFsPath);
            foreach (var filePath in rootFiles)
            {
                var file = new FileInfo(filePath);
                var node = new Node();
                node.Text = file.Name;
                node.Type = "f";
                node.VirtualPath = currentVirtualPath + "/" + file.Name;
                node.Id = node.VirtualPath;  // TODO: maybe just use id
                node.Size = file.Length;
                // TODO: timezome adjustment
                node.Created = file.CreationTimeUtc;
                node.Modified = file.LastWriteTimeUtc;
                node.CanPreview = IsWebImageFile(file.Extension);
                node.Icon = GetIconCssClass(file.Extension);
                node.SelectedIcon = node.Icon;
                //file.
                list.Add(node);
            }

            return list;

        }



        public static bool IsWebImageFile(string fileExtension)
        {
            if (string.Equals(fileExtension, ".gif", StringComparison.OrdinalIgnoreCase)) { return true; }
            if (string.Equals(fileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)) { return true; }
            if (string.Equals(fileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)) { return true; }
            if (string.Equals(fileExtension, ".png", StringComparison.OrdinalIgnoreCase)) { return true; }

            return false;
        }

        public string GetIconCssClass(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return string.Empty; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);

            switch (fileType)
            {
                case "doc":
                case "docx":
                    return icons.FileWord;

                case "xls":
                case "xlsx":
                    return icons.FileExcel;

                
                case "ppt":
                case "pptx":
                    return icons.FilePowerpoint;

                case "jpg":
                case "jpeg":
                case "gif":
                case "png":
                case "ico":
                    return icons.FileImage;

                case "wmv":
                case "mpg":
                case "mpeg":
                case "mp4":
                case "m4v":
                case "oog":
                case "ogv":
                case "webm":
                case "mov":
                case "flv":
                    return icons.FileVideo;


                case "mp3":
                case "m4a":
                case "oga":
                case "spx":
                    return icons.FileAudio;

               
                case "htm":
                case "html":
                case "css":
                case "js":
                    return icons.FileCode;

                case "zip":
                case "tar":
                    return icons.FileArchive;

                case "pdf":
                    return icons.FilePdf;


                default:
                    return icons.File;

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
