namespace cloudscribe.SimpleContent.Models
{
    public class CommentNotificationModel
    {
        public CommentNotificationModel(
            IProjectSettings project,
            IPost post,
            Comment comment,
            string postUrl)
        {
            Project = project;
            Post = post;
            Comment = comment;
            PostUrl = postUrl;
        }

        public IProjectSettings Project { get; private set; }
        public IPost Post { get; private set; }
        public Comment Comment { get; private set; }
        public string PostUrl { get; private set; }

    }
}
