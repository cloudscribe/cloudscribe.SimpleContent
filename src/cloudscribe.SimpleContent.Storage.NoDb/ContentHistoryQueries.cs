// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:					2018-07-02
// Last Modified:			2018-07-02
// 

using cloudscribe.Pagination.Models;
using cloudscribe.SimpleContent.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class ContentHistoryQueries : IContentHistoryQueries
    {
        public ContentHistoryQueries(
            IBasicQueries<ContentHistory> queries
            )
        {
            _queries = queries;
        }

        private readonly IBasicQueries<ContentHistory> _queries;

        public async Task<ContentHistory> Fetch(
            string projectId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _queries.FetchAsync(
                projectId,
                id.ToString(),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<ContentHistory>> GetByContent(
            string projectId,
            string contentId,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var all = await _queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var list = all.ToList().AsQueryable();

            var query = list
                .Where(x =>
                      x.ProjectId == projectId
                      && x.ContentId == contentId
                      );

            var count = query.Count();

            query = query
                .OrderByDescending(x => x.ArchivedUtc)
                    .Select(p => p)
                    .Skip(offset)
                    .Take(pageSize);


            var result = new PagedResult<ContentHistory>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = query.ToList(),
                TotalItems = count
            };

            return result;

        }


        public async Task<PagedResult<ContentHistory>> GetList(
            string projectId,
            string contentSource = null,
            string editorQuery = null,
            int pageNumber = 1,
            int pageSize = 20,
            int sortMode = 0, 
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var all = await _queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var list = all.ToList().AsQueryable();
            
            var query = list
                .Where(x => 
                      x.ProjectId == projectId 
                      && (contentSource == null || x.ContentSource == contentSource)
                      && (editorQuery == null || x.CreatedByUser == editorQuery || x.LastModifiedByUser == editorQuery)
                      );

            var count = query.Count();

            query = query
                .OrderByDescending(x => x.ArchivedUtc)
                .ThenBy(x => x.ContentSource)
                .ThenBy(x => x.Title)
                    .Select(p => p)
                    .Skip(offset)
                    .Take(pageSize);


            var result = new PagedResult<ContentHistory>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = query.ToList(),
                TotalItems = count
            };

            return result;
        }



    }
}
