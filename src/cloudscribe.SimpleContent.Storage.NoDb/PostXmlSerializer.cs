// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-25
// Last Modified:           2016-04-25
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PostXmlSerializer : IStringSerializer<Post>
    {
        public PostXmlSerializer(
            ILogger<PostXmlSerializer> logger)
        {
            log = logger;
        }

        private ILogger log;

        public string ExpectedFileExtension { get; } = ".xml";

        public string Serialize(Post post)
        {
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
            if (post.Comments != null)
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
                    catch (Exception ex)
                    {
                        log.LogError("error adding comment", ex);
                    }

                }
            }

            using (StringWriter s = new StringWriter())
            {
                doc.Save(s, SaveOptions.None);

                return s.GetStringBuilder().ToString();
            }

        }

        public Post Deserialize(string xmlString, string key)
        {
            // we need the key passed in here because the xml format adopted from MiniBlog/BlogEngine
            // does not store the post id in the xml but only as part of the file name

            var doc = XDocument.Parse(xmlString);

            Post post = new Post()
            {
                Id = key,
                Title = ReadValue(doc.Root, "title"),
                Author = ReadValue(doc.Root, "author"),
                MetaDescription = ReadValue(doc.Root, "excerpt"),
                Content = ReadValue(doc.Root, "content"),
                Slug = ReadValue(doc.Root, "slug").ToLowerInvariant(),
                PubDate = DateTime.Parse(ReadValue(doc.Root, "pubDate")),
                LastModified = DateTime.Parse(ReadValue(doc.Root, "lastModified", DateTime.UtcNow.ToString())),
                IsPublished = bool.Parse(ReadValue(doc.Root, "ispublished", "true")),
            };

            LoadCategories(post, doc.Root);
            LoadComments(post, doc.Root);

            return post;

        }

        protected void LoadCategories(Post post, XElement doc)
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

        protected void LoadComments(Post post, XElement doc)
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

        protected string ReadValue(XElement doc, XName name, string defaultValue = "")
        {
            if (doc.Element(name) != null)
                return doc.Element(name).Value;

            return defaultValue;
        }

        protected string ReadAttribute(XElement element, XName name, string defaultValue = "")
        {
            if (element.Attribute(name) != null)
                return element.Attribute(name).Value;

            return defaultValue;
        }
    }
}
