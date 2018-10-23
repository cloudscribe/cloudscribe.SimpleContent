using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        
        public Task<Dictionary<string, int>> GetArchives(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, int>> GetCategories(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(string category, bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(string projectId, int year, int month = 0, int day = 0, bool includeUnpublished = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IPost> GetPost(string postId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<PostResult> GetPostBySlug(string slug, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetPosts(string blogId, int numberToGet, int year, int month = 0, int day = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<PagedPostResult> GetPosts(
            string blogId, 
            int year, 
            int month = 0, 
            int day = 0, 
            int pageNumber = 1, 
            int pageSize = 10, 
            bool includeUnpublished = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetRecentPosts(int numberToGet, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<List<IPost>> GetFeaturedPosts(int numberToGet, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        
        public Task<List<IPost>> GetPosts(bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<PagedPostResult> GetPosts(string category, int pageNumber, bool includeUnpublished, CancellationToken cancellationToken = default(CancellationToken))
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
        
        //public Task SaveMedia(string blogId, byte[] bytes, string fileName)
        //{
        //    throw new NotImplementedException();
        //}
        
        public Task<bool> SlugIsAvailable(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlugIsAvailable(string blogId, string slug)
        {
            throw new NotImplementedException();
        }

        public Task FirePublishEvent(IPost post)
        {
            throw new NotImplementedException();
        }

        public Task FireUnPublishEvent(IPost post)
        {
            throw new NotImplementedException();
        }

        public Task PublishReadyDrafts(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

    }
}
