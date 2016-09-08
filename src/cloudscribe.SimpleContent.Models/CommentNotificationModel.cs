namespace cloudscribe.SimpleContent.Models
{
    public class CommentNotificationModel
    {
        public CommentNotificationModel(
            IProjectSettings project,
            IPost post,
            IComment comment,
            string postUrl)
        {
            Project = project;
            Post = post;
            Comment = comment;
            PostUrl = postUrl;
        }

        public IProjectSettings Project { get; private set; }
        public IPost Post { get; private set; }
        public IComment Comment { get; private set; }
        public string PostUrl { get; private set; }

    }
}
