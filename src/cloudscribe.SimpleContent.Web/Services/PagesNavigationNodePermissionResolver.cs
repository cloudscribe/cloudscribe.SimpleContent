// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-12
// Last Modified:			2016-08-12
// 

using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Helpers;
using cloudscribe.SimpleContent.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PagesNavigationNodePermissionResolver : INavigationNodePermissionResolver
    {
        public PagesNavigationNodePermissionResolver(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authorizationService = authorizationService;
        }

        private IHttpContextAccessor httpContextAccessor;
        private IAuthorizationService authorizationService;

        public virtual bool ShouldAllowView(TreeNode<NavigationNode> menuNode)
        {
            // for unpublished pages CustomData will be populated with the projectId
            // in that case we need to filter it from navigation unless the user has edit permissions
            if (string.IsNullOrEmpty(menuNode.Value.CustomData)) { return true; }
            
            // if the user is not authenticated no need to check further
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            var projectId = menuNode.Value.CustomData;
            var user = httpContextAccessor.HttpContext.User;
            if (user.CanEditPages(projectId))
            {
                return true;
            }
            // http://stackoverflow.com/questions/22628087/calling-async-method-synchronously
            bool canEdit = authorizationService.AuthorizeAsync(user, "PageEditPolicy").Result;
           
            return canEdit;
        }
    }
}
