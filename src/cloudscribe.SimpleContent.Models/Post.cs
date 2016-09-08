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
            
            return p;
        }

    }
}
