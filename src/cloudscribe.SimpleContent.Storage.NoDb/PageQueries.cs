// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2017-04-23
// 


using cloudscribe.SimpleContent.Models;
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
           // IBasicCommands<Page> pageCommands,
            IBasicQueries<Page> pageQueries
            //,ILogger<PageQueries> logger
            )
        {
            //commands = pageCommands;
            query = pageQueries;
            //log = logger;
        }

        //private IBasicCommands<Page> commands;
        private IBasicQueries<Page> query;
        //private ILogger log;

        

        public async Task<List<IPage>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            
            var l = await query.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var list = l.ToList();
            var result = new List<IPage>();
            result.AddRange(list);
            
            return result;
        }


        public async Task<IPage> GetPage(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.FirstOrDefault(p => p.Id == pageId);

        }

        public async Task<List<IPage>> GetRootPages(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.Where(p => p.ParentId == "0")
                .OrderBy(p => p.PageOrder).ToList();

        }

        public async Task<List<IPage>> GetChildPages(
            string projectId,
            string pageId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.Where(p => p.ParentId == pageId)
                .OrderBy(p => p.PageOrder).ToList();

        }

        public async Task<IPage> GetPageBySlug(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {

            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.FirstOrDefault(p => p.Slug == slug);
        }

        public async Task<IPage> GetPageByCorrelationKey(
            string projectId,
            string correlationKey,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {

            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.FirstOrDefault(p => p.CorrelationKey == correlationKey);
        }

        public async Task<bool> SlugIsAvailable(
            string projectId,
            string slug,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);

            var isInUse = allPages.Any(
                p => string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));

            return !isInUse;


        }

        public async Task<int> GetChildPageCount(
            string projectId,
            string pageId,
            bool includeUnpublished,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var list = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);

            list = list.Where(p =>
            p.ParentId == pageId
                     && (includeUnpublished || (p.IsPublished && p.PubDate <= DateTime.UtcNow))
                      ).ToList<IPage>();
            
            return list.Count();
        }

        // not implemented, do we need categories for pages?
        //public Task<int> GetCount(
        //    string projectId,
        //    string category,
        //    bool userIsBlogOwner,
        //    CancellationToken cancellationToken = default(CancellationToken)
        //    )
        //{
        //    return Task.FromResult(0);
        //}

        //public Task<Dictionary<string, int>> GetCategories(
        //    string projectId,
        //    bool userIsBlogOwner,
        //    CancellationToken cancellationToken
        //    )
        //{
        //    var result = new Dictionary<string, int>();


        //    return Task.FromResult(result);
        //}

    }
}
