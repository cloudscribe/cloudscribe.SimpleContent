// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:					2018-07-02
// Last Modified:			2018-07-02
// 

using cloudscribe.SimpleContent.Models;
using NoDb;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class ContentHistoryCommands : IContentHistoryCommands
    {
        public ContentHistoryCommands(
            IBasicCommands<ContentHistory> commands,
            IBasicQueries<ContentHistory> queries
            )
        {
            _commands = commands;
            _queries = queries;
        }

        private readonly IBasicCommands<ContentHistory> _commands;
        private readonly IBasicQueries<ContentHistory> _queries;

        public async Task Create(
            string projectId,
            ContentHistory contentHistory,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if(string.IsNullOrWhiteSpace(contentHistory.ProjectId))
            {
                contentHistory.ProjectId = projectId;
            }
            await _commands.CreateAsync(
                projectId,
                contentHistory.Id.ToString(),
                contentHistory,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(
            string projectId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await _commands.DeleteAsync(projectId, id.ToString(), cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteByContent(
            string projectId,
            string contentId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var items = await _queries.GetAllAsync(projectId).ConfigureAwait(false);
            var query = items.ToList().AsQueryable();
            var filtered = query.Where(x => x.ContentId == contentId && x.ProjectId == projectId);

            foreach (var item in filtered)
            {
                await _commands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task DeleteDraftHistory(
            string projectId,
            string contentId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var items = await _queries.GetAllAsync(projectId).ConfigureAwait(false);
            var query = items.ToList().AsQueryable();
            var filtered = query.Where(x => x.ContentId == contentId && x.IsDraftHx == true && x.ProjectId == projectId);

            foreach (var item in filtered)
            {
                await _commands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task DeleteByProject(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var items = await _queries.GetAllAsync(projectId).ConfigureAwait(false);
            var query = items.ToList().AsQueryable();
            var filtered = query.Where(x => x.ProjectId == projectId);

            foreach (var item in filtered)
            {
                await _commands.DeleteAsync(
                    projectId,
                    item.Id.ToString(),
                    cancellationToken).ConfigureAwait(false);
            }
        }


    }
}
