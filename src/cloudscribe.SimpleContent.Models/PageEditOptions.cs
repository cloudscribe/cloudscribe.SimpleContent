namespace cloudscribe.SimpleContent.Models
{
    public class PageEditOptions
    {
        public bool AlwaysShowDeveloperLink { get; set; } = false;
        public string DeveloperAllowedRole { get; set; } = string.Empty;

        public bool ShowDisableEditorOption { get; set; }
        public bool AllowMarkdown { get; set; } = true;
    }
}
