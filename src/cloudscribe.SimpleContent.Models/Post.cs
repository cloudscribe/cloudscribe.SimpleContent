using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class Post : IPost
    {
        public Post()
        {
            Categories = new List<string>();
            Comments = new List<IComment>();
        }

        public string Id { get; set; } 

        public string BlogId { get; set; } 

        public string Title { get; set; }

        /// <summary>
        /// This field is a place to store a surrogate key if needed.
        /// For example in a multi-tenant multi-lanaguage setup, it could be used
        /// to correlate posts between the different language sites
        /// to implement a language switcher. ie the corresponding post in the other
        /// site could be found by looking it up by the correlationkey
        /// </summary>
        public string CorrelationKey { get; set; } = string.Empty;

        public string Author { get; set; }
        
        public string Slug { get; set; }
        
        public string MetaDescription { get; set; }
        
        public string Content { get; set; }

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }
        
        public List<string> Categories { get; set; }
        public List<IComment> Comments { get; set; }

        public static Post FromIPost(IPost post)
        {
            var p = new Post();
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
            p.CorrelationKey = post.CorrelationKey;
            
            return p;
        }

    }
}
