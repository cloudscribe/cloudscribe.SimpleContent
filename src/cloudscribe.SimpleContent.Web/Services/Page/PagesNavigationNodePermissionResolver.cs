// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-12
// Last Modified:			2019-07-31
// 

using cloudscribe.SimpleContent.Web;
using cloudscribe.Web.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
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
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }

        private IHttpContextAccessor _httpContextAccessor;
        private IAuthorizationService _authorizationService;

        public virtual async Task<bool> ShouldAllowView(TreeNode<NavigationNode> menuNode)
        {
            // for unpublished pages CustomData will be populated with the projectId
            // in that case we need to filter it from navigation unless the user has edit permissions
            if (string.IsNullOrEmpty(menuNode.Value.CustomData)) { return true; }
            
            // if the user is not authenticated no need to check further
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            bool canEdit = false;

            var projectId = menuNode.Value.CustomData;
            var user = _httpContextAccessor.HttpContext.User;
            try
            {
                // http://stackoverflow.com/questions/22628087/calling-async-method-synchronously
                canEdit = await user.CanEditPages(projectId, _authorizationService);
            }
            catch(InvalidOperationException)
            { }
           
            return canEdit;
        }
    }
}
