// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-08-01
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PageQueries : IPageQueries
    {
        public PageQueries(
            IBasicCommands<Page> pageCommands,
            IBasicQueries<Page> pageQueries,
            ILogger<PageQueries> logger
            )
        {
            commands = pageCommands;
            query = pageQueries;
            log = logger;
        }

        private IBasicCommands<Page> commands;
        private IBasicQueries<Page> query;
        private ILogger log;

        

        public async Task<List<Page>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken)
        {
            
            var l = await query.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var list = l.ToList();

            //if (list.Count > 0)
            //{
            //    list.Sort((p1, p2) => p2.PageOrder.CompareTo(p1.PageOrder));
            //    //HttpRuntime.Cache.Insert("posts", list);
            //}

            //if (HttpRuntime.Cache["posts"] != null)
            //{
            //    return (List<Post>)HttpRuntime.Cache["posts"];
            //}
            //return new List<Post>();

            return list;
        }


        public async Task<Page> GetPage(
            string projectId,
            string pageId,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.FirstOrDefault(p => p.Id == pageId);

        }

        public async Task<List<Page>> GetRootPages(
            string projectId,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.Where(p => p.ParentId == "0")
                .OrderBy(p => p.PageOrder).ToList();

        }

        public async Task<List<Page>> GetChildPages(
            string projectId,
            string pageId,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.Where(p => p.ParentId == pageId)
                .OrderBy(p => p.PageOrder).ToList();

        }

        public async Task<Page> GetPageBySlug(
            string projectId,
            string slug,
            CancellationToken cancellationToken
            )
        {

            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.FirstOrDefault(p => p.Slug == slug);
        }

        public async Task<bool> SlugIsAvailable(
            string projectId,
            string slug,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);

            var isInUse = allPages.Any(
                p => string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));

            return !isInUse;


        }

        // not implemented, do we need categories for pages?
        public Task<int> GetCount(
            string projectId,
            string category,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task<Dictionary<string, int>> GetCategories(
            string projectId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken
            )
        {
            var result = new Dictionary<string, int>();


            return Task.FromResult(result);
        }

    }
}
