using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IBlogService
    {
        

        Task<bool> CommentsAreOpen(Post post, bool userIsOwner);
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
        Task<Post> GetPost(string postId);

        Task<Post> GetPost(
            string projectId, 
            string postId,
            string userName,
            string password
            );

        Task<PostResult> GetPostBySlug(string slug);
        Task<List<Post>> GetRecentPosts(int numberToGet);
          
        Task<List<Post>> GetPosts(bool includeUnpublished);
        Task<PagedResult<Post>> GetPosts(string category, int pageNumber, bool includeUnpublished);
        Task<string> ResolveBlogUrl(ProjectSettings blog);
        Task<string> ResolveMediaUrl(string fileName);
        Task<string> ResolvePostUrl(Post post);
        
        
        Task<List<Post>> GetRecentPosts(
            string projectId,
            string userName,
            string password,
            int numberToGet
            );

        Task<PagedResult<Post>> GetPosts(
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

        Task Create(Post post);

        Task Update(Post post);
        Task Create(
            string projectId,
            string userName,
            string password,
            Post post, 
            bool publish
            );

        Task Update(
            string projectId,
            string userName,
            string password,
            Post post,
            bool publish
            );

        Task SaveMedia(
            string projectId,
            string userName,
            string password,
            byte[] bytes, 
            string fileName);

        Task HandlePubDateAboutToChange(Post post, DateTime newPubDate);
    }
}