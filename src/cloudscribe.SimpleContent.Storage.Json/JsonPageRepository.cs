// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-17
// Last Modified:           2016-03-20
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public class JsonPageRepository : IPageRepository
    {
        public JsonPageRepository(
            ProjectFilePathResolver pathResolver,
            IJsonPersister persister,
            ILogger<JsonPageRepository> logger)
        {
            log = logger;
            filePersister = persister;
            this.pathResolver = pathResolver;
        }

        private ILogger log;
        private IJsonPersister filePersister;
        private ProjectFilePathResolver pathResolver;

        public async Task<bool> Delete(string projectId, string pageId)
        {
            
            var page = await GetPage(projectId, pageId, CancellationToken.None);
            if (page != null)
            {
                var pages = await GetAllPages(projectId, CancellationToken.None).ConfigureAwait(false);
                await filePersister.DeletePageFile(projectId, pageId).ConfigureAwait(false);
                pages.Remove(page);
                return true;
                //Blog.ClearStartPageCache();
            }
            return false;

        }


        public async Task<List<Page>> GetAllPages(
            string projectId,
            CancellationToken cancellationToken)
        {
            //TODO: caching
            //if (HttpRuntime.Cache["posts"] == null)

            var pages = await LoadAllPages(projectId, cancellationToken);

            //if (HttpRuntime.Cache["posts"] != null)
            //{
            //    return (List<Post>)HttpRuntime.Cache["posts"];
            //}
            //return new List<Post>();

            return pages;
        }

        private async Task<List<Page>> LoadAllPages(
            string projectId,
            CancellationToken cancellationToken)
        {
            await pathResolver.EnsureInitialized(projectId).ConfigureAwait(false);
            var folder = pathResolver.GetPagesFolderPath();
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var list = new List<Page>();

            foreach (string file in Directory.EnumerateFiles(folder, "*.json", SearchOption.TopDirectoryOnly))
            {
                using (StreamReader reader = File.OpenText(file))
                {
                    var payload = reader.ReadToEnd();

                    var page = JsonConvert.DeserializeObject<Page>(payload);
                    if (page != null)
                    {
                        list.Add(page);
                    }
                }
            }

            if (list.Count > 0)
            {
                list.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));
                //HttpRuntime.Cache.Insert("posts", list);
            }

            return list;

        }

        public Task<Dictionary<string, int>> GetCategories(
            string projectId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken
            )
        {
            var result = new Dictionary<string, int>();


            return Task.FromResult(result);
        }

        
        public async Task<Page> GetPage(
            string projectId,
            string pageId,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.FirstOrDefault(p => p.Id == pageId);
           
        }

        public async Task<List<Page>> GetRootPages(
            string projectId,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.Where(p => p.ParentId == "0").OrderBy(p => p.PageOrder).ToList();

        }

        public async Task<List<Page>> GetChildPages(
            string projectId,
            string pageId,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.Where(p => p.ParentId == pageId).OrderBy(p => p.PageOrder).ToList();

        }

        public async Task<Page> GetPageBySlug(
            string projectId,
            string slug,
            CancellationToken cancellationToken
            )
        {

            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);
            return allPages.FirstOrDefault(p => p.Slug == slug);
        }

        //Task<List<Post>> GetRecentPosts(
        //    string projectId,
        //    int numberToGet,
        //    CancellationToken cancellationToken);

        //Task<List<Post>> GetVisiblePosts(
        //    string projectId,
        //    bool userIsBlogOwner,
        //    CancellationToken cancellationToken);

        //Task<List<Post>> GetVisiblePosts(
        //    string projectId,
        //    string category,
        //    bool userIsBlogOwner,
        //    int pageNumber,
        //    int pageSize,
        //    CancellationToken cancellationToken);

        public Task<int> GetCount(
            string projectId,
            string category,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public async Task Save(
            string projectId,
            Page page,
            bool isNew)
        {

            if (string.IsNullOrEmpty(page.Id)) { page.Id = Guid.NewGuid().ToString(); }
            page.LastModified = DateTime.UtcNow;
            if (isNew) // New page
            {
                page.PubDate = DateTime.UtcNow;

                var pages = await GetAllPages(
                    projectId,
                    CancellationToken.None).ConfigureAwait(false);
                pages.Insert(0, page);
                pages.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));

                //HttpRuntime.Cache.Insert("posts", posts);
            }
            else
            {
                //Blog.ClearStartPageCache();
            }

            var json = JsonConvert.SerializeObject(
                page,
                Formatting.None,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }
                );

            

            await filePersister.SavePageFile(projectId, page.Id, json);

            
            
        }

        public async Task<bool> SlugIsAvailable(
            string projectId,
            string slug,
            CancellationToken cancellationToken
            )
        {
            var allPages = await GetAllPages(projectId, cancellationToken).ConfigureAwait(false);

            var isInUse = allPages.Any(
                p => string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));

            return !isInUse;


        }

        
        
    }
}


