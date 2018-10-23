using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;


namespace cloudscribe.SimpleContent.Models
{
    public interface IPageService
    {
        Task DeletePage(string pageId);
        Task<List<IPage>> GetAllPages(string projectId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IPage>> GetRootPages(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IPage>> GetChildPages(string pageId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IPage> GetPage(string pageId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IPage> GetPageBySlug(string slug, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SlugIsAvailable(string slug);
        
        Task Create(IPage page);

        Task Update(IPage page);
        
        void ClearNavigationCache();

        Task<string> GetPageTreeJson(ClaimsPrincipal user, Func<IPage, string> urlResolver, string node = "root", CancellationToken cancellationToken = default(CancellationToken));

        Task<PageActionResult> Move(PageMoveModel model);

        Task<PageActionResult> SortChildPagesAlpha(string pageId);

        Task<int> GetNextChildPageOrder(string pageSlug, CancellationToken cancellationToken = default(CancellationToken));

        Task FirePublishEvent(IPage page);
        Task FireUnPublishEvent(IPage page);

        Task PublishReadyDrafts(CancellationToken cancellationToken = default(CancellationToken));
    }
}