namespace cloudscribe.SimpleContent.Models
{
    public interface IPageRoutes
    {
        string PageRouteName { get; }
        string PageEditRouteName { get; }
        string PageDeleteRouteName { get; }

        string PageDevelopRouteName { get; }
        string PageTreeRouteName { get; }
    }
}
