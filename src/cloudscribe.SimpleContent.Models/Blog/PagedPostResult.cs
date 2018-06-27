using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models.Blog
{
    public class PagedPostResult
    {
        public PagedPostResult()
        {
            Data = new List<IPost>();
        }
        public List<IPost> Data { get; set; }
        public int TotalItems { get; set; } = 0;
    }
}
