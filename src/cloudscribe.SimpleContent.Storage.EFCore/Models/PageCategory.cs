namespace cloudscribe.SimpleContent.Storage.EFCore.Models
{
    public class PageCategory
    {
        public string Value { get; set; }

        public string PageEntityId { get; set; }

        public string ProjectId { get; set; } // so we can retrieve by project and delete if the project is deleted
    }
}
