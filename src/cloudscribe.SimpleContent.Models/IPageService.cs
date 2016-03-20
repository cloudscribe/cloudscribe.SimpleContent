using System.Collections.Generic;
using System.Threading.Tasks;


namespace cloudscribe.SimpleContent.Models
{
    public interface IPageService
    {
        Task<bool> DeletePage(string blogId, string pageId);
        Task<List<Page>> GetAllPages(string blogId);
        Task<List<Page>> GetRootPages();
        Task<List<Page>> GetChildPages(string pageId);
        Task<Page> GetPage(string blogId, string pageId);
        Task<Page> GetPageBySlug(string blogId, string slug);
        Task<bool> PageSlugIsAvailable(string slug);
        Task<bool> PageSlugIsAvailable(string blogId, string slug);
        Task<string> ResolvePageUrl(Page page);
        Task Save(string blogId, Page page, bool isNew, bool publish);
        Task<bool> SlugIsAvailable(string projectId, string slug);
    }
}