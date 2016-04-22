
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Models
{
    public class CommentNotificationModel
    {
        public CommentNotificationModel(
            ProjectSettings project,
            Post post,
            Comment comment,
            string postUrl)
        {
            Project = project;
            Post = post;
            Comment = comment;
            PostUrl = postUrl;
        }

        public ProjectSettings Project { get; private set; }
        public Post Post { get; private set; }
        public Comment Comment { get; private set; }
        public string PostUrl { get; private set; }

    }
}
