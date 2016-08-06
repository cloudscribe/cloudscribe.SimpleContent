

namespace cloudscribe.SimpleContent.Models
{
    public class DefaultBlogRoutes : IBlogRoutes
    {
        public string PostWithDateRouteName { get; } = ProjectConstants.PostWithDateRouteName;

        public string PostWithoutDateRouteName { get; } = ProjectConstants.PostWithoutDateRouteName;

    }
}
