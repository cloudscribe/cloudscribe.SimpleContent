
using cloudscribe.Pagination.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentHistoryQueries
    {
        Task<ContentHistory> Fetch(
            string projectId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<PagedResult<ContentHistory>> GetList(
            string projectId,
            string contentSource = null,
            string editorQuery = null,
            int pageNumber = 1,
            int pageSize = 20,
            int sortMode = 0, //TBD
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
