using System;

namespace cloudscribe.SimpleContent.Models
{
    public class Comment : IComment
    {

        public string Id { get; set; }
        public string ContentId { get; set; }

        public string ProjectId { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Content { get; set; }
        public DateTime PubDate { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsApproved { get; set; }

        //public string GravatarUrl { get; set; }

    }

    //https://ef.readthedocs.io/en/latest/modeling/relational/inheritance.html
    //public class PageComment : Comment
    //{

    //}
}
