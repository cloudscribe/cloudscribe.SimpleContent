namespace cloudscribe.SimpleContent.Models
{
    public class BlogEditOptions
    {
        public bool ForceLowerCaseCategories { get; set; } = false;
        public bool AllowMarkdown { get; set; } = true;
        public bool HideUnpublishButton { get; set; }
    }
}
