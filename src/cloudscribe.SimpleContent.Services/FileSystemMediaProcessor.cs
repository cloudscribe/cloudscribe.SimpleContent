// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-12
// Last Modified:           2016-02-16
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class FileSystemMediaProcessor : IMediaProcessor
    {
        public FileSystemMediaProcessor(
            ILogger<FileSystemMediaProcessor> logger,
            IHostingEnvironment env)
        {
            hosting = env;
            log = logger;
        }

        private ILogger log;

        private IHostingEnvironment hosting;

        public Task ConvertBase64EmbeddedImagesToFilesWithUrls(
            string mediaVirtualPath,
            Post post)
        {
            if (string.IsNullOrEmpty(post.Content)) { return Task.FromResult(0); }
            
            // process base64 encoded images into the file system and replace with relative urls
            foreach (Match match in Regex.Matches(post.Content, "(src|href)=\"(data:([^\"]+))\"(>.*?</a>)?"))
            {
                string extension = string.Empty;
                string filename = Guid.NewGuid().ToString();

                // Image
                if (match.Groups[1].Value == "src")
                {
                    extension = Regex.Match(match.Value, "data:([^/]+)/([a-z]+);base64").Groups[2].Value;
                }
                // Other file type
                else
                {
                    // Entire filename
                    extension = Regex.Match(match.Value, "data:([^/]+)/([a-z0-9+-.]+);base64.*\">(.*)</a>").Groups[3].Value;
                }

                if (string.IsNullOrWhiteSpace(extension))
                    extension = ".bin";
                else
                    extension = "." + extension.Trim('.');

                try
                {
                    byte[] bytes = ConvertToBytes(match.Groups[2].Value);
                    var newUrl = mediaVirtualPath + filename + extension;
                    var fsPath = hosting.WebRootPath + newUrl.Replace('/', Path.DirectorySeparatorChar);

                    File.WriteAllBytes(fsPath, bytes);

                    string value = string.Format("src=\"{0}\" alt=\"\" ", newUrl);

                    if (match.Groups[1].Value == "href")
                        value = string.Format("href=\"{0}\"", newUrl);

                    Match m = Regex.Match(match.Value, "(src|href)=\"(data:([^\"]+))\"");
                    post.Content = post.Content.Replace(m.Value, value);
                }
                catch (Exception ex)
                {
                    log.LogError("something went wrong while trying to process media from base64 to the file system", ex);
                }


            }


            return Task.FromResult(0);
        }

        public Task SaveMedia(string mediaVirtualPath, string fileName, byte[] bytes)
        {
            var newUrl = mediaVirtualPath + fileName;
            var fsPath = hosting.WebRootPath + newUrl.Replace('/', Path.DirectorySeparatorChar);

            File.WriteAllBytes(fsPath, bytes);
            return Task.FromResult(0);

        }


        private byte[] ConvertToBytes(string base64)
        {
            int index = base64.IndexOf("base64,", StringComparison.Ordinal) + 7;
            return Convert.FromBase64String(base64.Substring(index));
        }
    }
}
