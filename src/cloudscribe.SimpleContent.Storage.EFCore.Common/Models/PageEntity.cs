// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-08
// Last Modified:			2016-09-11
// 

using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.SimpleContent.Storage.EFCore.Models
{
    public class PageEntity : IPage
    {
        public PageEntity()
        {
            categories = new List<string>();
            comments = new List<IComment>();
            pageComments = new List<PageComment>();
        }

        public string Id { get; set; } = string.Empty;

        public string ProjectId { get; set; } = string.Empty;

        public string ParentId { get; set; } = string.Empty;

        public string ParentSlug { get; set; } = string.Empty;

        public int PageOrder { get; set; } = 3;

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

        public string ViewRoles { get; set; } = string.Empty;
        
        public bool ShowHeading { get; set; } = true;
        public bool ShowPubDate { get; set; } = false;
        public bool ShowLastModified { get; set; } = false;
        public bool ShowCategories { get; set; } = false;
        public bool ShowComments { get; set; } = false;


        private List<string> categories;
        public List<string> Categories
        {
            get
            {
                var list = CategoriesCsv.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim().ToLower()).ToList();

                categories.AddRange(list.Where(p2 =>
                  categories.All(p1 => p1 != p2)));

                return categories;
            }
            set
            {
                categories = value;
                CategoriesCsv = string.Join(",", categories);
            }
        }

        public string CategoriesCsv { get; set; } = string.Empty;
        
        private List<IComment> comments;
        public List<IComment> Comments
        {
            get
            {
                if (comments.Count == 0)
                {
                    comments.AddRange(PageComments);
                }
                return comments;
            }
            set
            {
                comments = value;
                if (comments.Count > 0)
                {
                    pageComments.Clear();
                    foreach (var c in comments)
                    {
                        pageComments.Add(PageComment.FromIComment(c));
                    }
                }
            }
        }

        private List<PageComment> pageComments;
        public List<PageComment> PageComments
        {
            get { return pageComments; }
            set { pageComments = value; }
        }

        public static PageEntity FromIPage(IPage page)
        {
            var p = new PageEntity();
            p.Author = page.Author;
            p.Categories = page.Categories;
            p.Comments = page.Comments;
            p.Content = page.Content;
            p.Id = page.Id;
            p.IsPublished = page.IsPublished;
            p.LastModified = page.LastModified;
            p.MetaDescription = page.MetaDescription;
            p.PageOrder = page.PageOrder;
            p.ParentId = page.ParentId;
            p.ParentSlug = page.ParentSlug;
            p.ProjectId = page.ProjectId;
            p.PubDate = page.PubDate;
            p.ShowCategories = page.ShowCategories;
            p.ShowComments = page.ShowComments;
            p.ShowHeading = page.ShowHeading;
            p.ShowLastModified = page.ShowLastModified;
            p.ShowPubDate = page.ShowPubDate;
            p.Slug = page.Slug;
            p.Title = page.Title;
            p.ViewRoles = page.ViewRoles;

            return p;
        }
    }
}
