using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public class NotImplementedPageService : IPageService
    {
        public Task<bool> DeletePage(string blogId, string pageId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Page>> GetAllPages(string blogId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Page>> GetChildPages(string pageId)
        {
            throw new NotImplementedException();
        }

        public Task<Page> GetPage(string blogId, string pageId)
        {
            throw new NotImplementedException();
        }

        public Task<Page> GetPageBySlug(string blogId, string slug)
        {
            throw new NotImplementedException();
        }

        public Task<List<Page>> GetRootPages()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PageSlugIsAvailable(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PageSlugIsAvailable(string blogId, string slug)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResolvePageUrl(Page page)
        {
            throw new NotImplementedException();
        }

        public Task Save(string blogId, Page page, bool isNew, bool publish)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlugIsAvailable(string projectId, string slug)
        {
            throw new NotImplementedException();
        }
    }
}
