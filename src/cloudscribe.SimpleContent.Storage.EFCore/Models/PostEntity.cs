// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-08
// Last Modified:			2016-09-08
// 


using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.Models
{
    public class PostEntity : IPost
    {
        public PostEntity()
        {

        }

        public string Id { get; set; }

        public string BlogId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Slug { get; set; }

        public string MetaDescription { get; set; }

        public string Content { get; set; }

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }

        public List<string> Categories { get; set; }
        public List<IComment> Comments { get; set; }

        public static PostEntity FromIPost(IPost post)
        {
            var p = new PostEntity();
            p.Author = post.Author;
            p.BlogId = post.BlogId;
            p.Categories = post.Categories;
            p.Comments = post.Comments;
            p.Content = post.Content;
            p.Id = post.Id;
            p.IsPublished = post.IsPublished;
            p.LastModified = post.LastModified;
            p.MetaDescription = post.MetaDescription;
            p.PubDate = post.PubDate;
            p.Slug = post.Slug;
            p.Title = post.Title;

            return p;
        }

    }
}
