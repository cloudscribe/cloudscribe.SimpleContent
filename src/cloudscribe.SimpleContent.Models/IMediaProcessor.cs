// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-12
// Last Modified:           2016-03-29
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    /// <summary>
    /// the media processor is responsible for finding base64 encoded media embedded
    /// in the content, extracting the media and persisting it to storage
    /// and returning the url of the file, replacing the embeded base64 content with 
    /// src set to the new media url
    /// 
    /// </summary>
    public interface IMediaProcessor
    {
        Task ConvertBase64EmbeddedImagesToFilesWithUrls(string mediaVirtualPath, Post post);
        Task ConvertBase64EmbeddedImagesToFilesWithUrls(string mediaVirtualPath, Page page);

        Task SaveMedia(string mediaVirtualPath, string fileName, byte[] bytes);
    }
}
