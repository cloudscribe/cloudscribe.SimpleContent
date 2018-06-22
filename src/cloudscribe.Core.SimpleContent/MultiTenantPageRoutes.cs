// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-01-08
// Last Modified:           2018-06-22

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class MultiTenantPageRoutes : IPageRoutes
    {
        public MultiTenantPageRoutes(
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor)
        {
            this.currentSite = currentSite;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        private SiteContext currentSite;
        private MultiTenantOptions multiTenantOptions;

        public string PageRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPageIndexRouteName;
                    }
                }

                return ProjectConstants.PageIndexRouteName;
            }
        }

        public string PageEditRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPageEditRouteName;
                    }
                }

                return ProjectConstants.PageEditRouteName;
            }
        }

        public string PageEditWithTemplateRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPageEditWithTemplateRouteName;
                    }
                }

                return ProjectConstants.PageEditWithTemplateRouteName;
            }
        }

        public string NewPageRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderNewPageRouteName;
                    }
                }

                return ProjectConstants.NewPageRouteName;
            }
        }

        public string PageDeleteRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPageDeleteRouteName;
                    }
                }

                return ProjectConstants.PageDeleteRouteName;
            }
        }

        public string PageDevelopRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPageDevelopRouteName;
                    }
                }

                return ProjectConstants.PageDevelopRouteName;
            }
        }

        public string PageTreeRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        return ProjectConstants.FolderPageTreeRouteName;
                    }
                }

                return ProjectConstants.PageTreeRouteName;
            }
        }



    }
}
