// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-12
// Last Modified:           2016-10-10
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
        Task<string> ResolveMediaUrl(string mediaVirtualPath, string fileName);

        Task<string> ConvertBase64EmbeddedImagesToFilesWithUrls(string mediaVirtualPath, string content);
        
        Task SaveMedia(string mediaVirtualPath, string fileName, byte[] bytes);
    }
}
