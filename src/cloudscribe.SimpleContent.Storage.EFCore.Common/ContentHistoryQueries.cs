using cloudscribe.Pagination.Models;
using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore.Common
{
    public class ContentHistoryQueries : IContentHistoryQueries, IContentHistoryQueriesSingleton
    {
        public ContentHistoryQueries(ISimpleContentDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly ISimpleContentDbContextFactory _contextFactory;

        public async Task<ContentHistory> Fetch(
            string projectId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                return await _db.ContentHistory.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id && p.ProjectId == projectId).ConfigureAwait(false);
            }
        }

        public async Task<PagedResult<ContentHistory>> GetByContent(
            string projectId,
            string contentId,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var result = new PagedResult<ContentHistory>
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            using (var _db = _contextFactory.CreateContext())
            {
                var query = _db.ContentHistory.Where(x => 
                    x.ProjectId == projectId
                    && x.ContentId == contentId
                    )
                    .OrderByDescending(x => x.ArchivedUtc)
                    .Select(x => x)
                    .Skip(offset)
                    .Take(pageSize)
                    ;

                result.Data = await query.AsNoTracking().AsSingleQuery().ToListAsync<ContentHistory>(cancellationToken).ConfigureAwait(false);
                result.TotalItems = await _db.ContentHistory
                    .Where(x =>
                        x.ProjectId == projectId
                        && x.ContentId == contentId
                    )
                    .CountAsync<ContentHistory>(cancellationToken).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<PagedResult<ContentHistory>> GetList(
            string projectId,
            string contentSource = null,
            string editorQuery = null,
            int pageNumber = 1,
            int pageSize = 10,
            int sortMode = 0, //TBD
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var result = new PagedResult<ContentHistory>
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            using (var _db = _contextFactory.CreateContext())
            {
                var query = _db.ContentHistory.Where(x =>
                    x.ProjectId == projectId
                    && (contentSource == null || x.ContentSource == contentSource)
                     && (editorQuery == null || x.CreatedByUser == editorQuery || x.LastModifiedByUser == editorQuery)
                    )
                    .OrderByDescending(x => x.ArchivedUtc)
                    .ThenBy(x => x.ContentSource)
                    .ThenBy(x => x.Title)
                    .Select(x => x)
                    .Skip(offset)
                    .Take(pageSize)
                    ;

                result.Data = await query.AsNoTracking().AsSingleQuery().ToListAsync<ContentHistory>(cancellationToken).ConfigureAwait(false);
                result.TotalItems = await _db.ContentHistory
                    .Where(x =>
                        x.ProjectId == projectId
                        && (contentSource == null || x.ContentSource == contentSource)
                        && (editorQuery == null || x.CreatedByUser == editorQuery || x.LastModifiedByUser == editorQuery)
                    )
                    .CountAsync<ContentHistory>(cancellationToken).ConfigureAwait(false);
            }

            return result;
        }

    }

}
