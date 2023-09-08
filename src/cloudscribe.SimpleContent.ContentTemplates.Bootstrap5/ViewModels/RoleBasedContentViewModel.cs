namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class RoleBasedContentViewModel
    {
        public string AuthenticatedContent { get; set; }
        public string UnauthenticatedContent { get; set; }
        public string RoleBasedContent { get; set; }
        public string Roles { get; set; }
        public bool RoleContentReplacesAuthenticatedContent { get; set; } = false;
    }
}
