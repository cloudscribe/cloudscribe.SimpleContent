// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-12
// Last Modified:           2019-09-02
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class FileSystemMediaProcessor : IMediaProcessor
    {
        public FileSystemMediaProcessor(
            ILogger<FileSystemMediaProcessor> logger,
            IWebHostEnvironment env)
        {
            Hosting = env;
            Log = logger;
        }

        protected ILogger Log;

        protected IWebHostEnvironment Hosting;

        public virtual Task<string> ResolveMediaUrl(string mediaVirtualPath, string fileName)
        {
            return Task.FromResult(mediaVirtualPath + fileName);
        }
  
        

        public virtual Task SaveMedia(string mediaVirtualPath, string fileName, byte[] bytes)
        {
            EnsureFsPath(mediaVirtualPath);
            var newUrl = mediaVirtualPath + fileName;
            var fsPath = Hosting.WebRootPath + newUrl.Replace('/', Path.DirectorySeparatorChar);

            File.WriteAllBytes(fsPath, bytes);
            return Task.FromResult(0);

        }

        protected void EnsureFolderPaths(string existingPath, string[] folderNameSegments)
        {
            if (string.IsNullOrEmpty(existingPath)) return;
            if (folderNameSegments.Length == 0) return;
            if (!Directory.Exists(existingPath)) return;

            var partial = existingPath;
            for (var i = 0; i < folderNameSegments.Length; i++)
            {
                partial = Path.Combine(partial, folderNameSegments[i]);

                if (!Directory.Exists(partial))
                {
                    try
                    {
                        Directory.CreateDirectory(partial);

                    }
                    catch (Exception ex)
                    {
                        Log.LogError($"failed to create folder {partial}", ex);

                    }
                }
            }
        }

        private void EnsureFsPath(string mediaVirtualPath)
        {
            var fsPath = Hosting.WebRootPath + mediaVirtualPath.Replace('/', Path.DirectorySeparatorChar);
            if (Directory.Exists(fsPath)) return; //nothing to do

            var segments = mediaVirtualPath.Split('/');

            EnsureFolderPaths(Hosting.WebRootPath, segments);
            
        }


        //private byte[] ConvertToBytes(string base64)
        //{
        //    int index = base64.IndexOf("base64,", StringComparison.Ordinal) + 7;
        //    return Convert.FromBase64String(base64.Substring(index));
        //}

        //public virtual Task<string> ConvertBase64EmbeddedImagesToFilesWithUrls(
        //    string mediaVirtualPath,
        //    string content)
        //{
        //    if (string.IsNullOrEmpty(content)) { return Task.FromResult(""); }

        //    // process base64 encoded images into the file system and replace with relative urls
        //    foreach (Match match in Regex.Matches(content, "(src|href)=\"(data:([^\"]+))\"(>.*?</a>)?"))
        //    {
        //        string extension = string.Empty;
        //        string filename = Guid.NewGuid().ToString();

        //        // Image
        //        if (match.Groups[1].Value == "src")
        //        {
        //            extension = Regex.Match(match.Value, "data:([^/]+)/([a-z]+);base64").Groups[2].Value;
        //        }
        //        // Other file type
        //        else
        //        {
        //            // Entire filename
        //            extension = Regex.Match(match.Value, "data:([^/]+)/([a-z0-9+-.]+);base64.*\">(.*)</a>").Groups[3].Value;
        //        }

        //        if (string.IsNullOrWhiteSpace(extension))
        //            extension = ".bin";
        //        else
        //            extension = "." + extension.Trim('.');

        //        try
        //        {
        //            byte[] bytes = ConvertToBytes(match.Groups[2].Value);
        //            var newUrl = mediaVirtualPath + filename + extension;
        //            var fsPath = hosting.WebRootPath + newUrl.Replace('/', Path.DirectorySeparatorChar);

        //            File.WriteAllBytes(fsPath, bytes);

        //            string value = string.Format("src=\"{0}\" alt=\"\" ", newUrl);

        //            if (match.Groups[1].Value == "href")
        //                value = string.Format("href=\"{0}\"", newUrl);

        //            Match m = Regex.Match(match.Value, "(src|href)=\"(data:([^\"]+))\"");
        //            content = content.Replace(m.Value, value);
        //        }
        //        catch (Exception ex)
        //        {
        //            log.LogError("something went wrong while trying to process media from base64 to the file system", ex);
        //        }

        //    }

        //    return Task.FromResult(content);
        //}
    }
}
