// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-07
// Last Modified:           2016-03-18
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace cloudscribe.SimpleContent.Storage.Xml
{
    public class XmlPostRepository : IPostRepository
    {
        public XmlPostRepository(
            ProjectFilePathResolver pathResolver,
            IXmlPersister persister,
            ILogger<XmlPostRepository> logger)
        {
            filePersister = persister;
            this.pathResolver = pathResolver;
            log = logger;
        }

        private ILogger log;
        private IXmlPersister filePersister;
        private ProjectFilePathResolver pathResolver;

        public async Task<List<Post>> GetAllPosts(
            string blogId,
            CancellationToken cancellationToken)
        {
            //TODO: caching
            //if (HttpRuntime.Cache["posts"] == null)

            var posts = await LoadPosts(blogId);

            return posts;

            //if (HttpRuntime.Cache["posts"] != null)
            //{
            //    return (List<Post>)HttpRuntime.Cache["posts"];
            //}
            //return new List<Post>();
        }

        public async Task<List<Post>> GetVisiblePosts(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var posts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);
            
            posts = posts.Where(p =>
                      (
                      (
                      p.IsPublished 
                      && p.PubDate <= DateTime.UtcNow
                      )
                      || userIsBlogOwner)
                      ).ToList<Post>();

            return posts;
        }

        public async Task<List<Post>> GetVisiblePosts(
            string blogId,
            string category,
            bool userIsBlogOwner,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var posts = await GetVisiblePosts(blogId, userIsBlogOwner, cancellationToken);

            if (!string.IsNullOrEmpty(category))
            {
                //var i = posts as IEnumerable<Post>;

                posts = posts.Where(
                    p => p.Categories.Any(
                        c => string.Equals(c, category, StringComparison.OrdinalIgnoreCase))
                        ).ToList<Post>();
               
            }

            if (pageSize > 0)
            {
               
                var offset = 0;
                if(pageNumber > 1) { offset = pageSize * (pageNumber - 1); }
                posts = posts.Skip(offset).Take(pageSize).ToList<Post>();

               
            }

            return posts;
        }

        public async Task<int> GetCount(
            string blogId,
            string category,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var posts = await GetVisiblePosts(blogId, userIsBlogOwner, cancellationToken);

            
            if (!string.IsNullOrEmpty(category))
            {
                posts = posts.Where(
                    p => p.Categories.Any(
                        c => string.Equals(c, category, StringComparison.OrdinalIgnoreCase))
                        ).ToList<Post>();

                
            }

            

            return posts.Count();
        }

        public async Task<List<Post>> GetRecentPosts(
            string blogId,
            int numberToGet,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var posts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);
            
            return  posts.Take(numberToGet).ToList<Post>();

        }

        
        public async Task<List<Post>> GetPosts(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var posts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);
            
            if(day > 0 && month > 0)
            {
                posts = posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                && x.PubDate.Day == day
                )
                .ToList<Post>();
            } else if(month > 0)
            {
                posts= posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                )
                .ToList<Post>();

            }
            else
            {
                posts = posts.Where(
                x => x.PubDate.Year == year
                )
                .ToList<Post>();
            }

            if (pageSize > 0)
            {
                var offset = 0;
                if (pageNumber > 1) { offset = pageSize * (pageNumber - 1); }
                posts = posts.Skip(offset).Take(pageSize).ToList<Post>();

            }

            return posts;

            //return posts.Where(
            //    x => x.PubDate.Year == year
            //    )
            //    .Take(pageSize).ToList<Post>();

        }

        public async Task<int> GetCount(
            string blogId,
            int year,
            int month = 0,
            int day = 0,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var posts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);

            if (day > 0 && month > 0)
            {
                return posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                && x.PubDate.Day == day
                )
                .Count();
            }
            else if (month > 0)
            {
                return  posts.Where(
                x => x.PubDate.Year == year
                && x.PubDate.Month == month
                )
                .Count();

            }
            
            return posts.Where(
                x => x.PubDate.Year == year
                )
                .Count();

        }

        public async Task HandlePubDateAboutToChange(Post post, DateTime newPubDate)
        {
            // because with the filesystem storage we are storing posts in a year folder
            // if the year changes we need to delete the old file and save the updated post to the
            // new year folder
            await filePersister.DeletePostFile(post.BlogId, post.Id, post.PubDate).ConfigureAwait(false);
            
        }

        // Can this be done async?
        public async Task Save(
            string blogId,
            Post post, 
            bool isNew)
        {
            //string file = Path.Combine(_folder, post.Id + ".xml");
            post.LastModified = DateTime.UtcNow;

            var doc = new XDocument(
                            new XElement("post",
                                new XElement("title", post.Title),
                                new XElement("slug", post.Slug),
                                new XElement("author", post.Author),
                                //new XElement("pubDate", post.PubDate.ToString("yyyy-MM-dd HH:mm:ss")),
                                new XElement("pubDate", post.PubDate.ToString("O")),
                                //new XElement("lastModified", post.LastModified.ToString("yyyy-MM-dd HH:mm:ss")),
                                new XElement("lastModified", post.LastModified.ToString("O")),
                                new XElement("excerpt", post.MetaDescription),
                                new XElement("content", post.Content),
                                new XElement("ispublished", post.IsPublished),
                                new XElement("categories", string.Empty),
                                new XElement("comments", string.Empty)
                            ));

            //XElement categories = doc.XPathSelectElement("post/categories");
            var categories = doc.Descendants("categories").FirstOrDefault();
            foreach (string category in post.Categories)
            {
                categories.Add(new XElement("category", category));
            }

            //XElement comments = doc.XPathSelectElement("post/comments");
            if(post.Comments != null)
            {
                var comments = doc.Descendants("comments").FirstOrDefault();
                foreach (Comment comment in post.Comments)
                {
                    try
                    {
                        comments.Add(
                        new XElement("comment",
                            new XElement("author", comment.Author),
                            new XElement("email", comment.Email),
                            new XElement("website", comment.Website),
                            new XElement("ip", comment.Ip),
                            new XElement("userAgent", comment.UserAgent),
                            //new XElement("date", comment.PubDate.ToString("yyyy-MM-dd HH:m:ss")),
                            new XElement("date", comment.PubDate.ToString("O")),
                            new XElement("content", comment.Content),
                            new XAttribute("isAdmin", comment.IsAdmin),
                            new XAttribute("isApproved", comment.IsApproved),
                            new XAttribute("id", comment.Id)
                        ));
                    }
                    catch(Exception ex)
                    {
                        log.LogError("error adding comment", ex);
                    }
                    
                }
            }

            if (string.IsNullOrEmpty(post.Id)) { post.Id = Guid.NewGuid().ToString(); }
            
            if (isNew) // New post
            {
                var posts = await GetAllPosts(
                    blogId,
                    CancellationToken.None).ConfigureAwait(false);
                posts.Insert(0, post);
                posts.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));
                //HttpRuntime.Cache.Insert("posts", posts);
            }
            else
            {
                //Blog.ClearStartPageCache();
            }

            await filePersister.SavePostFile(blogId, post.Id, post.PubDate, doc);

        }

        

        public async Task<Post> GetPost(
            string blogId,
            string postId,
            CancellationToken cancellationToken)
        {
            var allPosts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);
            return allPosts.FirstOrDefault(p => p.Id == postId);
            
        }

        public async Task<Post> GetPostBySlug(
            string blogId,
            string slug,
            CancellationToken cancellationToken)
        {
            var allPosts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);
            return allPosts.FirstOrDefault(p => p.Slug == slug);

        }

        public async Task<bool> SlugIsAvailable(
            string blogId,
            string slug,
            CancellationToken cancellationToken)
        {
            var allPosts = await GetAllPosts(blogId, cancellationToken).ConfigureAwait(false);

            var isInUse = allPosts.Any(
                p => string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));

            return !isInUse;
        }

        public async Task<bool> Delete(string blogId, string postId)
        {  
            var post = await GetPost(blogId, postId, CancellationToken.None);
            if(post != null)
            {
                var allPosts = await GetAllPosts(blogId, CancellationToken.None).ConfigureAwait(false);
                await filePersister.DeletePostFile(blogId, postId, post.PubDate).ConfigureAwait(false);
                allPosts.Remove(post);
                return true;
                //Blog.ClearStartPageCache();
            }
            return false;
  
        }

        public async Task<Dictionary<string, int>> GetCategories(
            string blogId, 
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, int>();

            var visiblePosts = await GetVisiblePosts(
                blogId, 
                userIsBlogOwner, 
                cancellationToken).ConfigureAwait(false);

            foreach (var category in visiblePosts.SelectMany(post => post.Categories))
            {
                if (!result.ContainsKey(category))
                {
                    result.Add(category, 0);
                }

                result[category] = result[category] + 1;
            }

            // TODO: cache this 
            var sorted = new SortedDictionary<string, int>(result);

            return sorted.OrderBy(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) as Dictionary<string, int>;
        }

        public async Task<Dictionary<string, int>> GetArchives(
            string blogId,
            bool userIsBlogOwner,
            CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, int>();

            // since we are storing the files on disk grouped by year and month folders
            // we could derive this list from the file system
            // currently we are loading all posts into memory but for a blog with years of frequent posts
            // maybe we don't need to keep all the old posts in memory we could load the recent year(s) by default and
            // load the old years on demand if requests come in for them
            // at any rate I think the way of retrieving posts needs more review and thought
            // about efficient strategies to use the minimum resources needed

            var visiblePosts = await GetVisiblePosts(
                blogId,
                userIsBlogOwner,
                cancellationToken).ConfigureAwait(false);

            var grouped = from p in visiblePosts
                          group p by new { month = p.PubDate.Month, year = p.PubDate.Year } into d
                          select new {
                              key = d.Key.year.ToString() + "/" + d.Key.month.ToString("00")
                              , count = d.Count() };

            foreach(var item in grouped)
            {
                result.Add(item.key, item.count);
            }

            return result;

            //foreach (var category in visiblePosts.SelectMany(post => post.Categories))
            //{
            //    if (!result.ContainsKey(category))
            //    {
            //        result.Add(category, 0);
            //    }

            //    result[category] = result[category] + 1;
            //}

            // TODO: cache this 
            //var sorted = new SortedDictionary<string, int>(result);

            //return sorted.OrderBy(x => x.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) as Dictionary<string, int>;
        }


        private async Task<List<Post>> LoadPosts(string blogId)
        {
            await pathResolver.EnsureInitialized(blogId).ConfigureAwait(false);
            var folder = pathResolver.GetPostRootFolderPath();
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            List<Post> list = new List<Post>();
            
            foreach (string file in Directory.EnumerateFiles(folder, "*.xml", SearchOption.AllDirectories))
            {
                XElement doc = XElement.Load(file);

                Post post = new Post()
                {
                    Id = Path.GetFileNameWithoutExtension(file),
                    Title = ReadValue(doc, "title"),
                    Author = ReadValue(doc, "author"),
                    MetaDescription = ReadValue(doc, "excerpt"),
                    Content = ReadValue(doc, "content"),
                    Slug = ReadValue(doc, "slug").ToLowerInvariant(),
                    PubDate = DateTime.Parse(ReadValue(doc, "pubDate")),
                    LastModified = DateTime.Parse(ReadValue(doc, "lastModified", DateTime.UtcNow.ToString())),
                    IsPublished = bool.Parse(ReadValue(doc, "ispublished", "true")),
                };

                LoadCategories(post, doc);
                LoadComments(post, doc);
                list.Add(post);
            }

            if (list.Count > 0)
            {
                list.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));
                //HttpRuntime.Cache.Insert("posts", list);
            }

            return list;
        }

        private void LoadCategories(Post post, XElement doc)
        {
            XElement categories = doc.Element("categories");
            if (categories == null)
                return;

            List<string> list = new List<string>();

            foreach (var node in categories.Elements("category"))
            {
                list.Add(node.Value);
            }

            post.Categories = list;
        }
        private void LoadComments(Post post, XElement doc)
        {
            var comments = doc.Element("comments");

            if (comments == null)
                return;

            foreach (var node in comments.Elements("comment"))
            {
                Comment comment = new Comment()
                {
                    Id = ReadAttribute(node, "id"),
                    Author = ReadValue(node, "author"),
                    Email = ReadValue(node, "email"),
                    Website = ReadValue(node, "website"),
                    Ip = ReadValue(node, "ip"),
                    UserAgent = ReadValue(node, "userAgent"),
                    IsAdmin = bool.Parse(ReadAttribute(node, "isAdmin", "false")),
                    IsApproved = bool.Parse(ReadAttribute(node, "isApproved", "true")),
                    Content = ReadValue(node, "content").Replace("\n", "<br />"),
                    PubDate = DateTime.Parse(ReadValue(node, "date", "2000-01-01")),
                };

                post.Comments.Add(comment);
            }
        }

        private string ReadValue(XElement doc, XName name, string defaultValue = "")
        {
            if (doc.Element(name) != null)
                return doc.Element(name).Value;

            return defaultValue;
        }

        private string ReadAttribute(XElement element, XName name, string defaultValue = "")
        {
            if (element.Attribute(name) != null)
                return element.Attribute(name).Value;

            return defaultValue;
        }

    }
}
