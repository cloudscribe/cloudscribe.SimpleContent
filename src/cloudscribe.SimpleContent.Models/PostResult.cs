namespace cloudscribe.SimpleContent.Models
{
    public class PostResult
    {
        public Post Post { get; set; } = null;

        public Post PreviousPost { get; set; } = null;

        public Post NextPost { get; set; } = null;

    }
}
