

namespace cloudscribe.MetaWeblog.Models
{
    /// <summary>
    /// MetaWeblog Category struct
    ///     returned as an array from GetCategories
    /// </summary>
    public struct CategoryStruct
    {
        public string description;
        public string htmlUrl;
        public string id;
        public string parentId;
        public string rssUrl;
        public string title;

    }
}
