// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-03-14
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cloudscribe.SimpleContent.Storage.Xml
{
    /// <summary>
    /// persists files to the local file system with path locations
    /// </summary>
    public class XmlFileSystemPersister : IXmlPersister
    {
        public XmlFileSystemPersister(
            ProjectFilePathResolver pathResolver)
        {
            this.pathResolver = pathResolver;
        }

        private ProjectFilePathResolver pathResolver;
        
        public async Task SavePostFile(string blogId, string postId, DateTime pubDate, XDocument xml)
        {
            await pathResolver.EnsureInitialized(blogId).ConfigureAwait(false);

            var folderPath = pathResolver.GetPostFolderPath(pubDate, true);

            //if (!Directory.Exists(folderPath))
            //    Directory.CreateDirectory(folderPath);

            var filePath = folderPath + postId + ".xml";

            if(File.Exists(filePath)) { File.Delete(filePath); }

            using (StreamWriter s = File.CreateText(filePath))
            {
                xml.Save(s, SaveOptions.None);
            }
            
        }

        public async Task DeletePostFile(string projectId, string postId, DateTime pubDate)
        {
            await pathResolver.EnsureInitialized(projectId).ConfigureAwait(false);
            var folderPath = pathResolver.GetPostFolderPath(pubDate);
            var filePath = folderPath + postId + ".xml";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                //coming from other blogs like MiniBlog it should be possible to just dump all the posts directly in the posts folder
                // ie miniblog did not store them in year month folders
                // upon any edit the post will be saved in the year month folder
                // but we need to delete it from the root folder if it exists 
                var postRootFolder = pathResolver.GetPostRootFolderPath();
                filePath = postRootFolder + postId + ".xml";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            
        }

        //public async Task SaveFileToDisk(string blogId, byte[] bytes, string fileName)
        //{
        //    await pathResolver.EnsureInitialized(blogId).ConfigureAwait(false);
        //    var folderPath = pathResolver.GetPostFilesFolderPath();
        //    var filePath = folderPath + fileName;
        //    File.WriteAllBytes(filePath, bytes);

        //}

    }
}
