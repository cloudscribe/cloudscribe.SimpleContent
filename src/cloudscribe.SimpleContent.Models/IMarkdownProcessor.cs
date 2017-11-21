namespace cloudscribe.SimpleContent.Models
{
    public interface IMarkdownProcessor
    {
        string ExtractFirstImageUrl(string markdown);
    }
}
