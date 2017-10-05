// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-05
//	Last Modified:              2017-10-05
// 

using cloudscribe.SimpleContent.Web.Mvc.Controllers;
using cloudscribe.Web.Common.Setup;
using System;
using System.Reflection;

namespace cloudscribe.SimpleContent.Web.Mvc
{
    public class ControllerVersionInfo : IVersionProvider
    {

        public string Name { get { return "cloudscribe.SimpleContent.Web.Mvc"; } }

        public Guid ApplicationId { get { return new Guid("2e4edb3b-f3c4-4909-b864-38fe8fefc50d"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(BlogController).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
