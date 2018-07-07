using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IBlogService
    {
        
        Task<bool> CommentsAreOpen(IPost post, bool userIsOwner);
        string CreateSlug(string title);
        Task<bool> SlugIsAvailable(string slug);
        
        
        Task<int> GetCount(string category, bool includeUnpublished);
        Task<int> GetCount(
            string projectId,
            int year,
            int month = 0,
            int day = 0,
            bool includeUnpublished = false
            );
        Task<IPost> GetPost(string postId);
        
        Task<PostResult> GetPostBySlug(string slug);
        Task<List<IPost>> GetRecentPosts(int numberToGet);
        Task<List<IPost>> GetFeaturedPosts(int numberToGet);

        Task<List<IPost>> GetPosts(bool includeUnpublished);
        Task<PagedPostResult> GetPosts(string category, int pageNumber, bool includeUnpublished);
        Task<string> ResolveBlogUrl(IProjectSettings blog);
        Task<string> ResolveMediaUrl(string fileName);
        Task<string> ResolvePostUrl(IPost post);
        
        Task<PagedPostResult> GetPosts(
            string projectId,
            int year,
            int month = 0,
            int day = 0,
            int pageNumber = 1,
            int pageSize = 10,
            bool includeUnpublished = false
            );

        Task<Dictionary<string, int>> GetCategories(bool includeUnpublished);
        
        Task<Dictionary<string, int>> GetArchives(bool includeUnpublished);

        Task<bool> SlugIsAvailable(string projectId, string slug);

        Task Delete(string postId);
        
        Task Create(IPost post, bool convertToRelativeUrls = false);

        Task Update(IPost post, bool convertToRelativeUrls = false);
        
        Task SaveMedia(
            string projectId,
            byte[] bytes, 
            string fileName);
        
        Task FirePublishEvent(
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}