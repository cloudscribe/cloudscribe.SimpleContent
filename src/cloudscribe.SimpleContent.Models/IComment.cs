using System;

namespace cloudscribe.SimpleContent.Models
{
    public interface IComment
    {
        string Author { get; set; }
        string Content { get; set; }
        string ContentId { get; set; }
        string Email { get; set; }
        string Id { get; set; }
        string Ip { get; set; }
        bool IsAdmin { get; set; }
        bool IsApproved { get; set; }
        string ProjectId { get; set; }
        DateTime PubDate { get; set; }
        string UserAgent { get; set; }
        string Website { get; set; }
    }
}