// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-08
// Last Modified:           2016-02-09
// 

using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.OptionsModel;
using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Xml
{
    public class DefaultXmlFileSystemOptionsResolver : IXmlFileSystemOptionsResolver
    {
        public DefaultXmlFileSystemOptionsResolver(
            IApplicationEnvironment appEnv,
            IOptions<List<ProjectSettings>> blogListAccessor)
        {
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (blogListAccessor == null) { throw new ArgumentNullException(nameof(blogListAccessor)); }

            env = appEnv;
            allBlogs = blogListAccessor.Value;
        }

        private IApplicationEnvironment env;
        private List<ProjectSettings> allBlogs;

        public Task<XmlFileSystemOptions> Resolve(string blogId)
        {
            if(!IsValidBlogId(blogId))
            {
                throw new ArgumentException("invalid blog id");
            }

            var result = new XmlFileSystemOptions();

            result.AppRootFolderPath = env.ApplicationBasePath;
            result.BaseFolderVPath = result.BaseFolderVPath.Replace('/', Path.DirectorySeparatorChar);
            result.FolderSeparator = Path.DirectorySeparatorChar.ToString();
            result.ProjectIdFolderName = blogId;


            return Task.FromResult(result);
        }

        private bool IsValidBlogId(string blogId)
        {
            foreach(ProjectSettings s in allBlogs)
            {
                if(s.ProjectId == blogId) { return true; }
            }

            return false;
        }
    }
}
