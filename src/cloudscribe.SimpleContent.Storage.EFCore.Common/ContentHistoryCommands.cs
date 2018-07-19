// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:					2018-07-04
// Last Modified:			2018-07-06
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore.Common
{
    public class ContentHistoryCommands : IContentHistoryCommands
    {
        public ContentHistoryCommands(ISimpleContentDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly ISimpleContentDbContextFactory _contextFactory;

        public async Task Create(
            string projectId,
            ContentHistory contentHistory,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var _db = _contextFactory.CreateContext())
            {
                _db.ContentHistory.Add(contentHistory);
                int rowsAffected = await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task Delete(
            string projectId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var _db = _contextFactory.CreateContext())
            {
                var itemToRemove = await _db.ContentHistory.SingleOrDefaultAsync(x => x.Id == id && x.ProjectId == projectId).ConfigureAwait(false);
                if (itemToRemove == null) throw new InvalidOperationException("item to delete not found");
                _db.ContentHistory.Remove(itemToRemove);
                int rowsAffected = await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteByContent(
            string projectId,
            string contentId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using(var _db = _contextFactory.CreateContext())
            {
                var itemsToRemove = _db.ContentHistory.Where(x => x.ProjectId == projectId && x.ContentId == contentId);
                _db.ContentHistory.RemoveRange(itemsToRemove);
                int rowsAffected = await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteByContent(
            string projectId,
            string contentId,
            DateTime cutoffDate,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var _db = _contextFactory.CreateContext())
            {
                var itemsToRemove = _db.ContentHistory.Where(x => x.ProjectId == projectId && x.ContentId == contentId && x.ArchivedUtc < cutoffDate);
                _db.ContentHistory.RemoveRange(itemsToRemove);
                int rowsAffected = await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteByProject(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var _db = _contextFactory.CreateContext())
            {
                var itemsToRemove = _db.ContentHistory.Where(x => x.ProjectId == projectId);
                _db.ContentHistory.RemoveRange(itemsToRemove);
                int rowsAffected = await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteOlderThan(
            string projectId,
            DateTime cutoffDate,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var _db = _contextFactory.CreateContext())
            {
                var itemsToRemove = _db.ContentHistory.Where(x => x.ProjectId == projectId && x.ArchivedUtc < cutoffDate);
                _db.ContentHistory.RemoveRange(itemsToRemove);
                int rowsAffected = await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteDraftHistory(
            string projectId,
            string contentId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            using (var _db = _contextFactory.CreateContext())
            {
                var itemsToRemove = _db.ContentHistory.Where(x => x.ProjectId == projectId && x.ContentId == contentId && x.IsDraftHx);
                _db.ContentHistory.RemoveRange(itemsToRemove);
                int rowsAffected = await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }


    }
}
