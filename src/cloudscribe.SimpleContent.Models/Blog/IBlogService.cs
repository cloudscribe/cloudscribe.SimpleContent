using System;
using System.Collections.Generic;
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

        Task<IPost> GetPost(
            string projectId, 
            string postId,
            string userName,
            string password
            );

        Task<PostResult> GetPostBySlug(string slug);
        Task<List<IPost>> GetRecentPosts(int numberToGet);
        Task<List<IPost>> GetFeaturedPosts(int numberToGet);

        Task<List<IPost>> GetPosts(bool includeUnpublished);
        Task<PagedPostResult> GetPosts(string category, int pageNumber, bool includeUnpublished);
        Task<string> ResolveBlogUrl(IProjectSettings blog);
        Task<string> ResolveMediaUrl(string fileName);
        Task<string> ResolvePostUrl(IPost post);
        
        
        Task<List<IPost>> GetRecentPosts(
            string projectId,
            string userName,
            string password,
            int numberToGet
            );

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
        Task<Dictionary<string, int>> GetCategories(
            string projectId,
            string userName,
            string password
            );
        Task<Dictionary<string, int>> GetArchives(bool includeUnpublished);

        Task<bool> SlugIsAvailable(string projectId, string slug);

        Task Delete(string postId);

        Task Delete(
            string projectId, 
            string postId,
            string userName,
            string password);

        Task Create(IPost post);

        Task Update(IPost post);
        Task Create(
            string projectId,
            string userName,
            string password,
            IPost post, 
            bool publish
            );

        Task Update(
            string projectId,
            string userName,
            string password,
            IPost post,
            bool publish
            );

        Task SaveMedia(
            string projectId,
            string userName,
            string password,
            byte[] bytes, 
            string fileName);

        //Task HandlePubDateAboutToChange(IPost post, DateTime newPubDate);
    }
}