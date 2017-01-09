using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class Page : IPage
    {
        public Page()
        {
            Categories = new List<string>();
            Comments = new List<IComment>();
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

        public bool MenuOnly { get; set; } = false;

        public string ViewRoles { get; set; } = string.Empty;

        public List<string> Categories { get; set; }
        public List<IComment> Comments { get; set; }

        public bool ShowHeading { get; set; } = true;
        public bool ShowPubDate { get; set; } = false;
        public bool ShowLastModified { get; set; } = false;
        public bool ShowCategories { get; set; } = false;
        public bool ShowComments { get; set; } = false;

        public static Page FromIPage(IPage page)
        {
            var p = new Page();
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
            p.MenuOnly = page.MenuOnly;
            p.ShowLastModified = page.ShowLastModified;
            p.ShowPubDate = page.ShowPubDate;
            p.Slug = page.Slug;
            p.Title = page.Title;
            p.ViewRoles = page.ViewRoles;
            
            return p;
        }
    }
}
