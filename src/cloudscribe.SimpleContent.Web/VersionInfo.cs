// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-09-23
//	Last Modified:              2017-09-23
// 

using cloudscribe.SimpleContent.Services;
using cloudscribe.Web.Common.Setup;
using System;
using System.Reflection;


namespace cloudscribe.SimpleContent.Web
{
    public class VersionInfo : IVersionProvider
    {
        public string Name { get { return "cloudscribe.SimpleContent"; } }

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
}
