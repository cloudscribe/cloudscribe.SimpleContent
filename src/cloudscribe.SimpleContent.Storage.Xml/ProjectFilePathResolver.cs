// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-03-20
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Xml
{
    public class ProjectFilePathResolver
    {
        public ProjectFilePathResolver(
            IXmlFileSystemOptionsResolver optionsResolver)
        {
            this.optionsResolver = optionsResolver;
        }

        private IXmlFileSystemOptionsResolver optionsResolver;
        private XmlFileSystemOptions fsOptions = null;

        public async Task EnsureInitialized(string projectId)
        {
            if (fsOptions == null)
            {
                fsOptions = await optionsResolver.Resolve(projectId).ConfigureAwait(false);
            }
        }

        public string GetPostRootFolderPath()
        {
            if (fsOptions == null) { throw new InvalidOperationException("call Initialize first"); }

            return fsOptions.AppRootFolderPath
                + fsOptions.BaseFolderVPath.Replace("/", fsOptions.FolderSeparator)
                + fsOptions.FolderSeparator
                + fsOptions.ProjectsFolderName 
                + fsOptions.FolderSeparator 
                + fsOptions.ProjectIdFolderName
                + fsOptions.FolderSeparator
                + fsOptions.PostsFolderName 
                + fsOptions.FolderSeparator;
        }

        public string GetPostFolderPath(DateTime pubDate)
        {
            if (fsOptions == null) { throw new InvalidOperationException("call Initialize first"); }

            //return GetPostFolderPath()
            //    + fsOptions.FolderSeparator
            //    + pubDate.Year.ToString(CultureInfo.InvariantCulture)
            //    + fsOptions.FolderSeparator;

            return GetPostFolderPath(pubDate, false);
        }

        public string GetPostFolderPath(DateTime pubDate, bool ensureDirectories)
        {
            if (fsOptions == null) { throw new InvalidOperationException("call Initialize first"); }

            var postFolderPath = GetPostRootFolderPath();
            var yearFolderName = pubDate.Year.ToString(CultureInfo.InvariantCulture);
            var monthFolderName = pubDate.Month.ToString("00", CultureInfo.InvariantCulture);

            if (ensureDirectories)
            {
                var yearPath = postFolderPath
                    + yearFolderName
                    + fsOptions.FolderSeparator;

                if (!Directory.Exists(yearPath))
                {
                    Directory.CreateDirectory(yearPath);
                }

                var monthPath = yearPath
                    + monthFolderName
                    + fsOptions.FolderSeparator;

                if (!Directory.Exists(monthPath))
                {
                    Directory.CreateDirectory(monthPath);
                }

            }

            return postFolderPath
                + yearFolderName
                + fsOptions.FolderSeparator
                + monthFolderName
                + fsOptions.FolderSeparator
                ;

        }

        
    }
}
