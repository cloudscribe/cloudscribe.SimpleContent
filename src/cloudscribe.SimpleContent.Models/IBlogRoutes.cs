
namespace cloudscribe.SimpleContent.Models
{
    public interface IBlogRoutes
    {
        string PostWithDateRouteName { get; }

        string PostWithoutDateRouteName { get; }

        string MostRecentPostRouteName { get; }
    }
}
