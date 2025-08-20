// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-08
// Last Modified:			2018-08-20
// 


using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.SimpleContent.Storage.EFCore.Models
{
    public class PostEntity : IPost
    {
        public PostEntity()
        {
            categories = new List<string>();
            postComments = new List<PostComment>();

            comments = new List<IComment>();
        }

        public string Id { get; set; }

        public string BlogId { get; set; }

        public string Title { get; set; }

        public string CorrelationKey { get; set; } = string.Empty;

        public string Author { get; set; }

        public string Slug { get; set; }

        public string MetaDescription { get; set; }
        public string MetaJson { get; set; }
        public string MetaHtml { get; set; }

        public string Content { get; set; }

        public DateTime? PubDate { get; set; } 

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }

        public bool IsFeatured { get; set; }

        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        //added to support TalkAbout Comments switch
        public bool ShowComments { get; set; } = false;

        private List<string> categories;
        public List<string> Categories
        {
            get {
                //if(categories.Count == 0)
                var list = CategoriesCsv.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                categories.AddRange(list.Where(p2 =>
                  categories.All(p1 => p1 != p2)));

                return categories;
            }
            set {
                categories = value;
                CategoriesCsv = string.Join(",", categories.Distinct(StringComparer.OrdinalIgnoreCase));
            }
        }

        public string CategoriesCsv { get; set; } = string.Empty;

        //public List<PostCategory> Cats { get; set; }

        private List<IComment> comments;
        public List<IComment> Comments
        {
            get {
                if(comments.Count == 0)
                {
                    comments.AddRange(PostComments);
                }
                return comments;
            }
            set {
                comments = value;
                postComments.Clear();
                if (comments.Count > 0)
                {       
                    foreach(var c in comments)
                    {
                        postComments.Add(PostComment.FromIComment(c));
                    }
                }
            }
        }

        private List<PostComment> postComments;
        public List<PostComment> PostComments
        {
            get { return postComments; }
            set { postComments = value; }
        }

        public string ContentType { get; set; } = "html";

        // Teaser.
        public string AutoTeaser { get; set; }
        public string TeaserOverride { get; set; }
        public bool SuppressTeaser { get; set; }

        // new fields 2018-06-20
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public string CreatedByUser { get; set; }
        public string LastModifiedByUser { get; set; }
        public string DraftContent { get; set; }
        public string DraftAuthor { get; set; }
        public DateTime? DraftPubDate { get; set; }

        public string TemplateKey { get; set; }
        public string SerializedModel { get; set; }
        public string DraftSerializedModel { get; set; }
        public string Serializer { get; set; }

        public static PostEntity FromIPost(IPost post)
        {
            var p = new PostEntity();
            post.CopyTo(p);
            return p;
        }
        public string Script { get; set; }
    }
}
