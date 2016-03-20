// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-15
// Last Modified:           2016-02-15
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Json
{
    
    public class ProjectSettingsRepository : IProjectSettingsRepository
    {
        public ProjectSettingsRepository(
            IOptions<List<ProjectSettings>> blogListAccessor)
        {
            allBlogs = blogListAccessor.Value;
        }

        private List<ProjectSettings> allBlogs;

        public Task<ProjectSettings> GetProjectSettings(
            string blogId,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = allBlogs.Where(b => b.ProjectId == blogId).FirstOrDefault();
            return Task.FromResult(result);
        }

        public async Task<List<ProjectSettings>> GetProjectSettingsByUser(
            string userName,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new List<ProjectSettings>();
            var defaultBlog = await GetProjectSettings("default", cancellationToken).ConfigureAwait(false);
            result.Add(defaultBlog);

            //var result = allBlogs.Where(b => b.BlogId == blogId).FirstOrDefault();
            return result;
        }


        //public Task<BlogSettings> GetCurrentBlogSettings(CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();
        //    return GetBlogSetings("default", cancellationToken);
        //}
    }
}
