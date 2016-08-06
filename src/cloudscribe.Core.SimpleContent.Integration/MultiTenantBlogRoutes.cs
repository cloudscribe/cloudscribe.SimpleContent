// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-08-06
// Last Modified:           2016-08-06

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class MultiTenantBlogRoutes : IBlogRoutes
    {
        public MultiTenantBlogRoutes(
            SiteSettings currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor
            )
        {
            this.currentSite = currentSite;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        private SiteSettings currentSite;
        private MultiTenantOptions multiTenantOptions;

        public string PostWithDateRouteName
        {
            get
            {
                if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if(!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPostWithDateRouteName;
                    }
                }

                return ProjectConstants.PostWithDateRouteName;
            }
        }

        public string PostWithoutDateRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPostWithoutDateRouteName;
                    }
                }

                return ProjectConstants.PostWithoutDateRouteName;
            }
        }

    }
}
