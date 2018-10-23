using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public class NotImplementedPageService : IPageService
    {
        public void ClearNavigationCache()
        {
            
        }

        public Task PublishReadyDrafts(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeletePage(string pageId)
        {
            throw new NotImplementedException();
        }
        
        public Task<List<IPage>> GetAllPages(string projectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<List<IPage>> GetChildPages(string pageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IPage> GetPage(string pageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        
        public Task<IPage> GetPageBySlug(string slug, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<List<IPage>> GetRootPages(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        
        public Task Create(IPage page)
        {
            throw new NotImplementedException();
        }

        public Task Update(IPage page)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlugIsAvailable(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPageTreeJson(ClaimsPrincipal user, Func<IPage, string> urlResolver, string node = "root", CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<PageActionResult> Move(PageMoveModel model)
        {
            throw new NotImplementedException();
        }

        public Task<PageActionResult> SortChildPagesAlpha(string pageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNextChildPageOrder(string pageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task FirePublishEvent(IPage page)
        {
            throw new NotImplementedException();
        }

        public Task FireUnPublishEvent(IPage page)
        {
            throw new NotImplementedException();
        }
    }
}
