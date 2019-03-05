// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2017-01-08
// Last Modified:           2019-03-04

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class MultiTenantPageRoutes : IPageRoutes
    {
        public MultiTenantPageRoutes(
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            CultureHelper cultureHelper
            )
        {
            _currentSite = currentSite;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _cultureHelper = cultureHelper;
        }

        private SiteContext _currentSite;
        private MultiTenantOptions _multiTenantOptions;
        private readonly CultureHelper _cultureHelper;

        public string PageRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPageIndexRouteName;
                        }

                        return ProjectConstants.FolderPageIndexRouteName;
                    }
                }

                if(_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePageIndexRouteName;
                }

                return ProjectConstants.PageIndexRouteName;
            }
        }

        public string PageEditRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPageEditRouteName;
                        }

                        return ProjectConstants.FolderPageEditRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePageEditRouteName;
                }

                return ProjectConstants.PageEditRouteName;
            }
        }

        public string PageEditWithTemplateRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPageEditWithTemplateRouteName;
                        }

                        return ProjectConstants.FolderPageEditWithTemplateRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePageEditWithTemplateRouteName;
                }

                return ProjectConstants.PageEditWithTemplateRouteName;
            }
        }

        public string NewPageRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderNewPageRouteName;
                        }

                        return ProjectConstants.FolderNewPageRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CultureNewPageRouteName;
                }

                return ProjectConstants.NewPageRouteName;
            }
        }

        public string PageDeleteRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPageDeleteRouteName;
                        }

                        return ProjectConstants.FolderPageDeleteRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePageDeleteRouteName;
                }

                return ProjectConstants.PageDeleteRouteName;
            }
        }

        public string PageDevelopRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPageDevelopRouteName;
                        }

                        return ProjectConstants.FolderPageDevelopRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePageDevelopRouteName;
                }

                return ProjectConstants.PageDevelopRouteName;
            }
        }

        public string PageTreeRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPageTreeRouteName;
                        }

                        return ProjectConstants.FolderPageTreeRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePageTreeRouteName;
                }

                return ProjectConstants.PageTreeRouteName;
            }
        }

        public string PageHistoryRouteName
        {
            get
            {
                if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPageHistoryRouteName;
                        }

                        return ProjectConstants.FolderPageHistoryRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePageHistoryRouteName;
                }

                return ProjectConstants.PageHistoryRouteName;
            }
        }

    }
}
