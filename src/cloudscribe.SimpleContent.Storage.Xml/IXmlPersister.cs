// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-03-14
// 

using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cloudscribe.SimpleContent.Storage.Xml
{
    /// <summary>
    /// the idea of this interface is so we could implement a way to persist files to a git repo
    /// which would be cool if doing continuos integration in azure where commits to the repo are automatically deployed to azure
    /// we would always read the blog xml files from the local file system and the default IXmlPersister would
    /// save files directly to the local file system
    /// In Azure web apps saving files to the file system would be problematic because new web nodes that get spinned up
    /// for scaling would only have the latest files from the git repo and would not know about other files on other nodes
    /// So we could use the GitHub api or VS Taam Services API to commit new files to the repo automatically so
    /// they would automatically show up in every web node.
    /// to avoid any delay we would also add the post directly to the cached post list
    /// There still could be an issue for images that are sent to the repo taking time to appear but I think there are ways to solve that as well.
    /// 
    /// </summary>
    public interface IXmlPersister
    {
        Task DeletePostFile(string blogId, string postId, DateTime pubDate);
        //Task SaveFileToDisk(string blogId, byte[] bytes, string fileName);
        Task SavePostFile(string blogId, string postId, DateTime pubDate, XDocument xml);
    }
}