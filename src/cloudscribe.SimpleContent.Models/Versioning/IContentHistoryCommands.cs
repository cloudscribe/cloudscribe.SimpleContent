using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IContentHistoryCommands
    {
        Task Create(
            string projectId,
            ContentHistory contentHistory,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Delete(
            string projectId,
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task DeleteByContent(
            string projectId,
            string contentId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task DeleteByContent(
            string projectId,
            string contentId,
            DateTime cutoffDate,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task DeleteByProject(
            string projectId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task DeleteOlderThan(
            string projectId,
            DateTime cutoffDate,
            CancellationToken cancellationToken = default(CancellationToken)
            );


        /// <summary>
        /// we keep draft history of pages until published
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="contentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteDraftHistory(
            string projectId,
            string contentId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }
}
