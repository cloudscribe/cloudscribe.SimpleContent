using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public class NotImplementedBlogService : IBlogService
    {
        public Task<bool> CommentsAreOpen(IPost post, bool userIsOwner)
        {
            throw new NotImplementedException();
        }

        public string CreateSlug(string title)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string postId)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string blogId, string postId)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string projectId, string postId, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> GetArchives(bool includeUnpublished)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> GetCategories(bool includeUnpublished)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> GetCategories(string blogId, bool includeUnpublished)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> GetCategories(string projectId, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(string category, bool includeUnpublished)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(string projectId, int year, int month = 0, int day = 0, bool includeUnpublished = false)
        {
            throw new NotImplementedException();
        }

        public Task<IPost> GetPost(string postId)
        {
            throw new NotImplementedException();
        }

        public Task<IPost> GetPost(string blogId, string postId)
        {
            throw new NotImplementedException();
        }

        public Task<IPost> GetPost(string projectId, string postId, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<PostResult> GetPostBySlug(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetPosts(string blogId, int numberToGet, int year, int month = 0, int day = 0)
        {
            throw new NotImplementedException();
        }

        public Task<PagedPostResult> GetPosts(string blogId, int year, int month = 0, int day = 0, int pageNumber = 1, int pageSize = 10, bool includeUnpublished = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetRecentPosts(int numberToGet)
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetFeaturedPosts(int numberToGet)
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetRecentPosts(string blogId, int numberToGet)
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetRecentPosts(string projectId, string userName, string password, int numberToGet)
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetPosts(bool includeUnpublished)
        {
            throw new NotImplementedException();
        }

        public Task<PagedPostResult> GetPosts(string category, int pageNumber, bool includeUnpublished)
        {
            throw new NotImplementedException();
        }

        //public Task HandlePubDateAboutToChange(IPost post, DateTime newPubDate)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<string> ResolveBlogUrl(IProjectSettings blog)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResolveMediaUrl(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResolvePostUrl(IPost post)
        {
            throw new NotImplementedException();
        }

        public Task Create(IPost post)
        {
            throw new NotImplementedException();
        }

        public Task Update(IPost post)
        {
            throw new NotImplementedException();
        }

        //public Task Create(string projectId, Post post, bool publish)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task Update(string projectId, Post post, bool publish)
        //{
        //    throw new NotImplementedException();
        //}

        public Task Create(string projectId, string userName, string password, IPost post, bool publish)
        {
            throw new NotImplementedException();
        }

        public Task Update(string projectId, string userName, string password, IPost post, bool publish)
        {
            throw new NotImplementedException();
        }

        public Task SaveMedia(string blogId, byte[] bytes, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task SaveMedia(string projectId, string userName, string password, byte[] bytes, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlugIsAvailable(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlugIsAvailable(string blogId, string slug)
        {
            throw new NotImplementedException();
        }


    }
}
