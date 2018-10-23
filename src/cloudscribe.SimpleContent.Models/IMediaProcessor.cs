// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-12
// Last Modified:           2017-05-28
// 

using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    // this is only uised in MetaweblogService
    public interface IMediaProcessor
    {
        Task<string> ResolveMediaUrl(string mediaVirtualPath, string fileName);

        //Task<string> ConvertBase64EmbeddedImagesToFilesWithUrls(string mediaVirtualPath, string content);
        
        Task SaveMedia(string mediaVirtualPath, string fileName, byte[] bytes);
    }
}
