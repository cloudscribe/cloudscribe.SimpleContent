using System.Collections.Generic;
using System.Threading.Tasks;


namespace cloudscribe.SimpleContent.Models
{
    public interface IPageService
    {
        Task DeletePage(string projectId, string pageId);
        Task<List<Page>> GetAllPages(
            string projectId,
            string userName,
            string password);
        Task<List<Page>> GetRootPages();
        Task<List<Page>> GetChildPages(string pageId);
        Task<Page> GetPage(string projectId, string pageId, string userName, string password);
        Task<Page> GetPage(string pageId);

        Task<Page> GetPageBySlug(string projectId, string slug);
        Task<bool> PageSlugIsAvailable(string slug);
        Task<bool> PageSlugIsAvailable(string projectId, string slug);
        Task<string> ResolvePageUrl(Page page);

        Task Create(
            Page page,
            bool publish);

        Task Update(
            Page page,
            bool publish);

        Task Create(
            string projectId, 
            string userName,
            string password,
            Page page, 
            bool publish);

        Task Update(
            string projectId,
            string userName,
            string password,
            Page page,
            bool publish);

        Task<bool> SlugIsAvailable(string projectId, string slug);

        void ClearNavigationCache();
    }
}