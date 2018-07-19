namespace cloudscribe.SimpleContent.Models
{
    public class PostResult
    {
        public IPost Post { get; set; } = null;

        public IPost PreviousPost { get; set; } = null;

        public IPost NextPost { get; set; } = null;

    }
}
