// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-25
// Last Modified:           2018-07-03
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
                                new XElement("id", post.Id),
                                new XElement("title", post.Title),
                                new XElement("slug", post.Slug),
                                new XElement("correlationkey", post.CorrelationKey),
                                new XElement("author", post.Author),
                                new XElement("pubDate", post.PubDate?.ToString("O")),
                                new XElement("lastModified", post.LastModified.ToString("s")),
                                new XElement("excerpt", post.MetaDescription),
                                new XElement("content", post.Content),
                                new XElement("contentType", post.ContentType),
                                new XElement("imageUrl", post.ImageUrl),
                                new XElement("thumbnailUrl", post.ThumbnailUrl),

                                new XElement("createdUtc", post.CreatedUtc.ToString("s")),
                                new XElement("createdByUser", post.CreatedByUser),
                                new XElement("lastModifiedByUser", post.LastModifiedByUser),
                                new XElement("draftAuthor", post.DraftAuthor),
                                new XElement("draftContent", post.DraftContent),
                                new XElement("draftPubDate", post.DraftPubDate?.ToString("O")),
                                new XElement("templateKey", post.TemplateKey),
                                new XElement("serializedModel", post.SerializedModel),
                                new XElement("draftSerializedModel", post.DraftSerializedModel),
                                new XElement("serializer", post.Serializer),
                                new XElement("blogId", post.BlogId),


                                new XElement("ispublished", post.IsPublished),
                                new XElement("isFeatured", post.IsFeatured),
                                new XElement("teaserOverride", post.TeaserOverride),
                                new XElement("suppressTeaser", post.SuppressTeaser),
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
                BlogId = ReadValue(doc.Root, "blogId"),
                Title = ReadValue(doc.Root, "title"),
                Author = ReadValue(doc.Root, "author"),
                CorrelationKey = ReadValue(doc.Root, "correlationkey"),
                MetaDescription = ReadValue(doc.Root, "excerpt"),
                Content = ReadValue(doc.Root, "content"),
                ContentType = ReadValue(doc.Root, "contentType"),
                Slug = ReadValue(doc.Root, "slug").ToLowerInvariant(),
                ImageUrl = ReadValue(doc.Root, "imageUrl"),
                ThumbnailUrl = ReadValue(doc.Root, "thumbnailUrl"),
                PubDate = GetNullableDate(doc.Root, "pubDate"),
                LastModified = GetDate(doc.Root, "lastModified"),
                IsPublished = bool.Parse(ReadValue(doc.Root, "ispublished", "true")),
                IsFeatured = bool.Parse(ReadValue(doc.Root, "isFeatured", "false")),
                TeaserOverride = ReadValue(doc.Root, "teaserOverride"),
                SuppressTeaser = bool.Parse(ReadValue(doc.Root, "suppressTeaser", "false")),

                CreatedUtc = GetDate(doc.Root, "createdUtc"),
                CreatedByUser = ReadValue(doc.Root, "createdByUser"),
                LastModifiedByUser = ReadValue(doc.Root, "lastModifiedByUser"),
                DraftAuthor = ReadValue(doc.Root, "draftAuthor"),
                DraftContent = ReadValue(doc.Root, "draftContent"),
                DraftPubDate = GetNullableDate(doc.Root, "draftPubDate"),
                TemplateKey = ReadValue(doc.Root, "templateKey"),
                SerializedModel = ReadValue(doc.Root, "serializedModel"),
                DraftSerializedModel = ReadValue(doc.Root, "draftSerializedModel"),
                Serializer = ReadValue(doc.Root, "serializer"),

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

        protected DateTime GetDate(XElement doc, XName name)
        {
            if (doc.Element(name) == null)
                return DateTime.UtcNow;

            DateTime d;
            try
            {
                d = DateTime.ParseExact(
                    ReadValue(doc, name, DateTime.UtcNow.ToString()),
                    "O",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal |
                    DateTimeStyles.AdjustToUniversal
                );
            }
            catch (FormatException)
            {
                try
                {
                    d = DateTime.Parse(ReadValue(doc, name));
                }
                catch(Exception ex)
                {
                    log.LogError($"failed to parse date so returning current date/time error: {ex.Message}: {ex.StackTrace}");
                    d = DateTime.UtcNow;
                }
                

            }

            return d;
        }

        protected DateTime? GetNullableDate(XElement doc, XName name)
        {
            if (doc.Element(name) == null)
                return null;

            var val = ReadValue(doc, name);
            if(string.IsNullOrWhiteSpace(val)) { return null; }

            DateTime? d;
            try
            {
                d = DateTime.ParseExact(
                    val,
                    "O",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal |
                    DateTimeStyles.AdjustToUniversal
                );
            }
            catch (FormatException)
            {
                try
                {
                    d = DateTime.Parse(val);
                }
                catch (Exception ex)
                {
                    log.LogError($"failed to parse date from {val} so returning null error: {ex.Message}: {ex.StackTrace}");
                    d = null;
                }


            }

            return d;
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
