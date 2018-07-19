namespace cloudscribe.SimpleContent.Models
{
    public interface IPageRoutes
    {
        string PageRouteName { get; }
        string PageEditRouteName { get; }
        string PageEditWithTemplateRouteName { get; }
        string PageDeleteRouteName { get; }

        string PageDevelopRouteName { get; }
        string PageTreeRouteName { get; }
        string NewPageRouteName { get; }

        string PageHistoryRouteName { get; }
    }
}
