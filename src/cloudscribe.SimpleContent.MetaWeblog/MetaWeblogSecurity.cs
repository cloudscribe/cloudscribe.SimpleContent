// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-02-15
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.MetaWeblog;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.MetaWeblog
{
    public class MetaWeblogSecurity : IMetaWeblogSecurity
    {
        public MetaWeblogSecurity(IProjectSecurityResolver blogSecurity)
        {
            this.blogSecurity = blogSecurity;
        }

        private IProjectSecurityResolver blogSecurity;


        public async Task<MetaWeblogSecurityResult> ValiatePermissions(MetaWeblogRequest request, CancellationToken cancellationToken)
        {
            var blogResult = await blogSecurity.ValidatePermissions(
                request.BlogId,
                request.UserName,
                request.Password,
                cancellationToken
                ).ConfigureAwait(false);

            return new MetaWeblogSecurityResult(
                blogResult.DisplayName,
                blogResult.ProjectId,
                blogResult.IsAllowed, 
                blogResult.IsProjectOwner);
            
        }
    }
}
