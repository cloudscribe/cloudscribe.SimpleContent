

namespace cloudscribe.MetaWeblog.Models
{
    /// <summary>
    /// MetaWeblog BlogInfo struct
    ///     returned as an array from getUserBlogs
    /// </summary>
    public struct BlogInfoStruct
    {
        public string blogId;
        public string blogName;
        public string url;
        public string xmlrpcUrl;

        //had these in mojoportal not sure we will use them
        public string pageEditRoles;
        public string moduleEditRoles;
        public int editUserId;
        public int pageId;

        
    }
}
