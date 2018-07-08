// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-21
// Last Modified:           2018-07-08
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.SiteMap;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class BlogSiteMapNodeService : ISiteMapNodeService
    {
        public BlogSiteMapNodeService(
            IProjectService projectService,
            IBlogService blogService,
            IBlogUrlResolver blogUrlResolver,
            IHttpContextAccessor contextAccessor,
            ILogger<BlogSiteMapNodeService> logger)
        {
            _projectService = projectService;
            _blogService = blogService;
            _blogUrlResolver = blogUrlResolver;
            _contextAccessor = contextAccessor;
            _log = logger;
        }

        private readonly IProjectService _projectService;
        private readonly IBlogService _blogService;
        private readonly IBlogUrlResolver _blogUrlResolver;
        private readonly ILogger _log;
        private readonly IHttpContextAccessor _contextAccessor;
        private string _baseUrl = string.Empty;
        private List<string> _addedUrls = new List<string>();

        public string BaseUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_baseUrl))
                {
                    _baseUrl = string.Concat(_contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        _contextAccessor.HttpContext.Request.Host.ToUriComponent());
                }
                return _baseUrl;
            }
        }


        public async Task<IEnumerable<ISiteMapNode>> GetSiteMapNodes(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var mapNodes = new List<SiteMapNode>();
            var includeUnpublished = false;
            var posts = await _blogService.GetPosts(includeUnpublished).ConfigureAwait(false);
            if(posts == null)
            {
                _log.LogWarning("post list came back null so returning empty list of sitemapnodes");
                return mapNodes;
            }

            var project = await _projectService.GetCurrentProjectSettings();
            if (project == null)
            {
                _log.LogWarning("projectsettings back null so returning empty list of sitemapnodes");
                return mapNodes;
            }

            foreach (var post in posts)
            {
                if (!post.IsPublished) continue;
                if (post.PubDate > DateTime.UtcNow) continue;
                var url = await ResolveUrl(post, project).ConfigureAwait(false);

                if (string.IsNullOrEmpty(url))
                {
                    _log.LogWarning("failed to resolve url for post " + post.Id + ", skipping this post for sitemap");
                    continue;
                }

                if (_addedUrls.Contains(url)) continue;

                mapNodes.Add(
                            new SiteMapNode(url)
                            {
                                LastModified = post.LastModified  
                            });

                _addedUrls.Add(url);
            }

            return mapNodes;
        }

        private async Task<string> ResolveUrl(IPost post, IProjectSettings project)
        {
            if (string.IsNullOrWhiteSpace(post.Slug)) return string.Empty;
            var url = await _blogUrlResolver.ResolvePostUrl(post, project).ConfigureAwait(false);
            if (url == null) return string.Empty;
            if (url.StartsWith("http")) return url;

            return BaseUrl + url;
        }

    }
}
