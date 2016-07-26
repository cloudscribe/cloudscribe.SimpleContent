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
        
        
        Task<int> GetCount(string category);
        Task<int> GetCount(
            string projectId,
            int year,
            int month = 0,
            int day = 0);
        Task<Post> GetPost(string postId);

        Task<Post> GetPost(
            string projectId, 
            string postId,
            string userName,
            string password
            );

        Task<PostResult> GetPostBySlug(string slug);
        Task<List<Post>> GetRecentPosts(int numberToGet);
          
        Task<List<Post>> GetVisiblePosts();
        Task<PagedResult<Post>> GetVisiblePosts(string category, int pageNumber);
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
            int pageSize = 10);

        Task<Dictionary<string, int>> GetCategories();
        Task<Dictionary<string, int>> GetCategories(
            string projectId,
            string userName,
            string password
            );
        Task<Dictionary<string, int>> GetArchives();

        Task<bool> SlugIsAvailable(string projectId, string slug);

        Task Delete(string postId);

        Task Delete(
            string projectId, 
            string postId,
            string userName,
            string password);

        Task Save(Post post, bool isNew);
        Task Save(
            string projectId,
            string userName,
            string password,
            Post post, 
            bool isNew, 
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