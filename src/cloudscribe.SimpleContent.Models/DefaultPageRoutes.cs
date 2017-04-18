namespace cloudscribe.SimpleContent.Models
{
    public class DefaultPageRoutes : IPageRoutes
    {
        public string PageRouteName { get; } = ProjectConstants.PageIndexRouteName;
        public string PageEditRouteName { get; } = ProjectConstants.PageEditRouteName;
        public string PageDeleteRouteName { get; } = ProjectConstants.PageDeleteRouteName;

        public string PageDevelopRouteName { get; } = ProjectConstants.PageDevelopRouteName;
    }
}
