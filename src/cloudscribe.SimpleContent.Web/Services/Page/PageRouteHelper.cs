// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-08-05
// Last Modified:           2016-08-05

using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.SimpleContent.Web.Services
{
    public interface IPageRouteHelper
    {
        string PageIndexRouteName { get; }

        string FolderPageIndexRouteName { get; }

        string ResolveHomeUrl(IUrlHelper urlHelper, string folderPrefix);
    }

    /// <summary>
    /// this implementation assumes that the PageController is being used as the default
    /// controller for the site.
    /// If you don't want to do that you can plugin your own implementation
    /// to control how the route is resolved for a page when building the navigation tree.
    /// </summary>
    public class DefaultPageRouteHelper : IPageRouteHelper
    {
        public string PageIndexRouteName
        {
            get { return ProjectConstants.PageIndexRouteName; }
        }

        public string FolderPageIndexRouteName
        {
            get { return ProjectConstants.FolderPageIndexRouteName; }
        }

        public string ResolveHomeUrl(IUrlHelper urlHelper, string folderPrefix)
        {
            return urlHelper.Content("~/" + folderPrefix);
        }
    }
}
