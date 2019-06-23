using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Integration.SimpleContent
{
    public class BlogPreCacheItemProvider : IPreCacheItemProvider
    {
        public BlogPreCacheItemProvider(
            IProjectService projectService,
            IBlogService blogService,
            IBlogUrlResolver blogUrlResolver,
            IHttpContextAccessor contextAccessor
            )
        {
            _projectService = projectService;
            _blogService = blogService;
            _blogUrlResolver = blogUrlResolver;
            _contextAccessor = contextAccessor;
        }

        private readonly IProjectService _projectService;
        private readonly IBlogService _blogService;
        private readonly IBlogUrlResolver _blogUrlResolver;
        private readonly IHttpContextAccessor _contextAccessor;

        public async Task<List<PreCacheItem>> GetItems()
        {
            var result = new List<PreCacheItem>();

            var includeUnpublished = false;
            var posts = await _blogService.GetPosts(includeUnpublished).ConfigureAwait(false);
            if (posts == null) return result;
            var project = await _projectService.GetCurrentProjectSettings();
            if (project == null) return result;

            foreach (var post in posts)
            {
                if (!post.IsPublished) continue;
                if (post.PubDate > DateTime.UtcNow) continue;
                var url = await ResolveUrl(post, project).ConfigureAwait(false);

                if (string.IsNullOrEmpty(url))
                {
                   // _log.LogWarning("failed to resolve url for post " + post.Id + ", skipping this post for sitemap");
                    continue;
                }

                //if (_addedUrls.Contains(url)) continue;
                result.Add(new PreCacheItem()
                {
                    Url = url,
                    LastModifiedUtc = post.LastModified,
                    Revision = post.LastModified.ToString("s")
                });
               

                //_addedUrls.Add(url);
            }




            return result;

        }


        private async Task<string> ResolveUrl(IPost post, IProjectSettings project)
        {
            if (string.IsNullOrWhiteSpace(post.Slug)) return string.Empty;
            var url = await _blogUrlResolver.ResolvePostUrl(post, project).ConfigureAwait(false);
            if (url == null) return string.Empty;
            if (url.StartsWith("http")) return url;

            return url;
        }
    }
}
