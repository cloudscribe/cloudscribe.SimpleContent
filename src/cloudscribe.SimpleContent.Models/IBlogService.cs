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
        Task<bool> Delete(string postId);
        
        Task<int> GetCount(string category);
        Task<int> GetCount(
            string projectId,
            int year,
            int month = 0,
            int day = 0);
        Task<Post> GetPost(string postId);
        
        Task<Post> GetPostBySlug(string slug);
        Task<List<Post>> GetRecentPosts(int numberToGet);
          
        Task<List<Post>> GetVisiblePosts();
        Task<List<Post>> GetVisiblePosts(string category, int pageNumber);
        Task<string> ResolveBlogUrl(ProjectSettings blog);
        Task<string> ResolveMediaUrl(string fileName);
        Task<string> ResolvePostUrl(Post post);
        
        Task<Post> GetPost(string projectId, string postId);
        Task<List<Post>> GetRecentPosts(string projectId, int numberToGet);
        Task<List<Post>> GetPosts(
            string projectId,
            int year,
            int month = 0,
            int day = 0,
            int pageNumber = 1,
            int pageSize = 10);

        Task<Dictionary<string, int>> GetCategories();
        Task<Dictionary<string, int>> GetCategories(string projectId, bool userIsOwner);
        Task<Dictionary<string, int>> GetArchives();

        Task<bool> SlugIsAvailable(string projectId, string slug);
        Task<bool> Delete(string projectId, string postId);
        Task Save(Post post, bool isNew);
        Task Save(string projectId, Post post, bool isNew, bool publish);
        Task SaveMedia(string projectId, byte[] bytes, string fileName);
        Task HandlePubDateAboutToChange(Post post, DateTime newPubDate);
    }
}