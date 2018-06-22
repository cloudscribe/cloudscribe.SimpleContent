namespace cloudscribe.SimpleContent.Models
{
    public class DefaultPageRoutes : IPageRoutes
    {
        public string PageRouteName { get; } = ProjectConstants.PageIndexRouteName;
        public string PageEditRouteName { get; } = ProjectConstants.PageEditRouteName;
        public string PageEditWithTemplateRouteName { get; } = ProjectConstants.PageEditWithTemplateRouteName;
        public string PageDeleteRouteName { get; } = ProjectConstants.PageDeleteRouteName;

        public string PageDevelopRouteName { get; } = ProjectConstants.PageDevelopRouteName;

        public string PageTreeRouteName { get; } = ProjectConstants.PageTreeRouteName;
        public string NewPageRouteName { get; } = ProjectConstants.NewPageRouteName;
    }
}
