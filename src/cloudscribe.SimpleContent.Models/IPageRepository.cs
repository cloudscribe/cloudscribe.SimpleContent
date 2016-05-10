

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPageRepository
    {
        Task Save(
            string blogId,
            Page page,
            bool isNew);

        Task Delete(string blogId, string pageId);

        Task<bool> SlugIsAvailable(
            string blogId,
            string slug,
            CancellationToken cancellationToken
            );

        Task<Page> GetPage(
            string blogId,
            string postId,
            CancellationToken cancellationToken
            );

        Task<Page> GetPageBySlug(
            string blogId,
            string slug,
            CancellationToken cancellationToken
            );

        Task<List<Page>> GetAllPages(
            string blogId,
            CancellationToken cancellationToken);

        Task<List<Page>> GetRootPages(
            string blogId,
            CancellationToken cancellationToken
            );

        Task<List<Page>> GetChildPages(
            string blogId,
            string pageId,
            CancellationToken cancellationToken
            );
    }
}
