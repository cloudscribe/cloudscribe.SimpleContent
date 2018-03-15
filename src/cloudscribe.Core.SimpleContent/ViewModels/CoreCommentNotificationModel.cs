using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.SimpleContent.ViewModels
{
    public class CoreCommentNotificationModel
    {
        public CoreCommentNotificationModel(
            ISiteContext site,
            IProjectSettings project,
            IPost post,
            IComment comment,
            string postUrl)
        {
            Site = site;
            Project = project;
            Post = post;
            Comment = comment;
            PostUrl = postUrl;
        }

        public ISiteContext Site { get; private set; }
        public IProjectSettings Project { get; private set; }
        public IPost Post { get; private set; }
        public IComment Comment { get; private set; }
        public string PostUrl { get; private set; }

    }
}
