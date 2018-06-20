
using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentHistoryQueries
    {
        Task<ContentHistory> Fetch(
            string projectId,
            Guid levelId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<PagedResult<ContentHistory>> GetList(
            string projectId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
