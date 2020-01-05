using System;


namespace cloudscribe.MetaWeblog.Models
{
    /// <summary>
    /// wp Page Struct
    /// </summary> 
    public struct PageStruct
    {
        /// <summary>
        ///     Content of Blog Post
        /// </summary>
        public string description;

        /// <summary>
        ///     Link to Blog Post
        /// </summary>
        public string link;

        //wp_slug
        public string slug;

        /// <summary>
        ///     Convert Breaks
        /// </summary>
        public string mt_convert_breaks;

        /// <summary>
        ///     Page keywords
        /// </summary>
        public string mt_keywords;

        /// <summary>
        ///     Display date of Blog Post (DateCreated)
        /// </summary>
        public DateTime pageDate;

        public DateTime pageUtcDate;

        /// <summary>
        ///     PostID Guid in string format
        /// </summary>
        public string pageId;

        /// <summary>
        ///     Page Parent ID
        /// </summary>
        public string pageParentId;

        public string parentTitle;

        //string page_status

        /// <summary>
        ///     PageOrder
        /// </summary>
        public string pageOrder;

        /// <summary>
        ///     Title of Blog Post
        /// </summary>
        public string title;

        /// <summary>
        ///     CommentPolicy (Allow/Deny)
        /// </summary>
        public string commentPolicy;


        public string published; //publish or draft

        //http://codex.wordpress.org/XML-RPC_wp
        //TODO: implement support for custom fields
        // ? will live writer round trip these?
        // we need a place to store the module id of the hmtl item
        //array custom_fields : struct string id string key string value

        //public string moduleId;

        //public string itemId;

        public string pageEditRoles;

        public string moduleEditRoles;
    }
}
