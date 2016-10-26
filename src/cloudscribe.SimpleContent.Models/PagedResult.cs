using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    //public class PagedResult<T> where T : class
    //{
    //    public PagedResult()
    //    {
    //        Data = new List<T>();
    //    }
    //    public List<T> Data { get; set; }
    //    public int TotalItems { get; set; } = 0;
    //}

    /// <summary>
    /// I needed this once I introduced interfaces like IPost because the above only works for concrete types
    /// </summary>
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
