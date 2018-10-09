
using cloudscribe.Pagination.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentHistoryQueriesSingleton : IContentHistoryQueries
    {

    }

    public interface IContentHistoryQueries
    {
        Task<ContentHistory> Fetch(
            string projectId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<PagedResult<ContentHistory>> GetByContent(
            string projectId,
            string contentId,
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        /// <summary>
        ///  sortMode 0 = Archive Date desc, contentSource, title
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="contentSource"></param>
        /// <param name="editorQuery"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortMode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
