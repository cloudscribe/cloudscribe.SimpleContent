// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-09-23
//	Last Modified:              2017-10-05
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using cloudscribe.Versioning;
using System;
using System.Reflection;


namespace cloudscribe.SimpleContent.Web
{
    public class VersionInfo : IVersionProvider
    {
        public string Name { get { return "cloudscribe.SimpleContent.Web"; } }

        public Guid ApplicationId { get { return new Guid("f83067b4-919d-4910-acd1-4b3b1c210ecf"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(PageService).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }

    public class DataStorageVersionInfo : IVersionProvider
    {
        public DataStorageVersionInfo(
            IStorageInfo dbPlatform)
        {
            _dbPlatform = dbPlatform;
            name = _dbPlatform.GetType().Assembly.GetName().Name;

        }

        private IStorageInfo _dbPlatform;
        private string name = "DataStorageVersionInfo";

        public string Name
        {
            get { return name; }

        }

        public Guid ApplicationId { get { return new Guid("c0928150-0f7b-498a-b33b-1d113681d69a"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = _dbPlatform.GetType().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }

    }

    

}
