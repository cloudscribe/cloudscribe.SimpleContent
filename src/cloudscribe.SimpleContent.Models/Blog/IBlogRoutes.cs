
namespace cloudscribe.SimpleContent.Models
{
    public interface IBlogRoutes
    {
        string PostWithDateRouteName { get; }

        string PostWithoutDateRouteName { get; }

        string MostRecentPostRouteName { get; }

        string BlogCategoryRouteName { get; }

        string BlogArchiveRouteName { get; }

        string NewPostRouteName { get; }
        
        string BlogIndexRouteName { get; }

        string PostEditRouteName { get; }

        string PostDeleteRouteName { get; }

        string PostHistoryRouteName { get; }
    }
}
