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
        
        
        Task<int> GetCount(string category, bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> GetCount(
            string projectId,
            int year,
            int month = 0,
            int day = 0,
            bool includeUnpublished = false,
            CancellationToken cancellationToken = default(CancellationToken)
            );
        Task<IPost> GetPost(string postId, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<PostResult> GetPostBySlug(string slug, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IPost>> GetRecentPosts(int numberToGet, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<IPost>> GetFeaturedPosts(int numberToGet, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IPost>> GetPosts(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedPostResult> GetPosts(string category, int pageNumber, bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken));

        //Task<string> ResolveMediaUrl(string fileName);
        
        Task<PagedPostResult> GetPosts(
            string projectId,
            int year,
            int month = 0,
            int day = 0,
            int pageNumber = 1,
            int pageSize = 10,
            bool includeUnpublished = false,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<Dictionary<string, int>> GetCategories(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken));
        
        Task<Dictionary<string, int>> GetArchives(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> SlugIsAvailable(string projectId, string slug);

        Task Delete(string postId);
        
        Task Create(IPost post, bool convertToRelativeUrls = false);

        Task Update(IPost post, bool convertToRelativeUrls = false);
        
        Task SaveMedia(
            string projectId,
            byte[] bytes, 
            string fileName);
        
        Task FirePublishEvent(IPost post);
        Task FireUnPublishEvent(IPost post);

    }
}