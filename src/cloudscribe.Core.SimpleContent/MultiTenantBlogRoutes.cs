// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-08-06
// Last Modified:           2019-03-04

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class MultiTenantBlogRoutes : IBlogRoutes
    {
        public MultiTenantBlogRoutes(
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
             CultureHelper cultureHelper
            )
        {
            this.currentSite = currentSite;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            _cultureHelper = cultureHelper;
        }

        private SiteContext currentSite;
        private MultiTenantOptions multiTenantOptions;
        private readonly CultureHelper _cultureHelper;

        public string PostWithDateRouteName
        {
            get
            {
                if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if(!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPostWithDateRouteName;
                        }

                        return ProjectConstants.FolderPostWithDateRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePostWithDateRouteName;
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
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPostWithoutDateRouteName;
                        }

                        return ProjectConstants.FolderPostWithoutDateRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePostWithoutDateRouteName;
                }

                return ProjectConstants.PostWithoutDateRouteName;
            }
        }

        public string MostRecentPostRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderMostRecentPostRouteName;
                        }

                        return ProjectConstants.FolderMostRecentPostRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CultureMostRecentPostRouteName;
                }

                return ProjectConstants.MostRecentPostRouteName;
            }
        }

        public string BlogCategoryRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderBlogCategoryRouteName;
                        }

                        return ProjectConstants.FolderBlogCategoryRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CultureBlogCategoryRouteName;
                }

                return ProjectConstants.BlogCategoryRouteName;
            }
        }

        public string BlogArchiveRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderBlogArchiveRouteName;
                        }

                        return ProjectConstants.FolderBlogArchiveRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CultureBlogArchiveRouteName;
                }

                return ProjectConstants.BlogArchiveRouteName;
            }
        }

        public string NewPostRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderNewPostRouteName;
                        }

                        return ProjectConstants.FolderNewPostRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CultureNewPostRouteName;
                }

                return ProjectConstants.NewPostRouteName;
            }
        }

        public string BlogIndexRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderBlogIndexRouteName;
                        }

                        return ProjectConstants.FolderBlogIndexRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CultureBlogIndexRouteName;
                }

                return ProjectConstants.BlogIndexRouteName;
            }
        }

        public string PostEditRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPostEditRouteName;
                        }

                        return ProjectConstants.FolderPostEditRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePostEditRouteName;
                }

                return ProjectConstants.PostEditRouteName;
            }
        }

        public string PostEditWithTemplateRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPostEditWithTemplateRouteName;
                        }

                        return ProjectConstants.FolderPostEditWithTemplateRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePostEditWithTemplateRouteName;
                }

                return ProjectConstants.PostEditWithTemplateRouteName;
            }
        }

        public string PostDeleteRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPostDeleteRouteName;
                        }

                        return ProjectConstants.FolderPostDeleteRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePostDeleteRouteName;
                }

                return ProjectConstants.PostDeleteRouteName;
            }
        }

        public string PostHistoryRouteName
        {
            get
            {
                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if (!string.IsNullOrEmpty(currentSite.SiteFolderName))
                    {
                        if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                        {
                            return ProjectConstants.CultureFolderPostHistoryRouteName;
                        }

                        return ProjectConstants.FolderPostHistoryRouteName;
                    }
                }

                if (_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
                {
                    return ProjectConstants.CulturePostHistoryRouteName;
                }

                return ProjectConstants.PostHistoryRouteName;
            }
        }

    }
}
