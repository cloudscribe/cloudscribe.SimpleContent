
using cloudscribe.MetaWeblog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{
    public class MetaWeblogResult
    {
        public MetaWeblogResult()
        {
            Blogs = new List<BlogInfoStruct>();
            Categories = new List<CategoryStruct>();
            Keywords = new List<string>();
            Posts = new List<PostStruct>();
            Pages = new List<PageStruct>();
            Authors = new List<AuthorStruct>();

        }

        public string Method { get; set; }

       
        public List<AuthorStruct> Authors { get; set; }

        
        public List<BlogInfoStruct> Blogs { get; set; }

        
        public List<CategoryStruct> Categories { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether function call was completed and successful.  
        ///     Used by metaWeblog.editPost and blogger.deletePost.
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        ///     Gets or sets Fault Struct. Used by API to return error information
        /// </summary>
        public FaultStruct Fault { get; set; }

        /// <summary>
        ///     Gets or sets List of Tags.  Used by wp.getTags.
        /// </summary>
        public List<string> Keywords { get; set; }

        
        public MediaInfoStruct MediaInfo { get; set; }

        
        public PageStruct Page { get; set; }

        /// <summary>
        ///     Gets or sets Id of page that was just added.
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        ///     Gets or sets List of Page Structs
        /// </summary>
        public List<PageStruct> Pages { get; set; }

        /// <summary>
        ///     Gets or sets Metaweblog Post Struct. Used by metaWeblog.getPost
        /// </summary>
        public PostStruct Post { get; set; }

        /// <summary>
        ///     Gets or sets Id of post that was just added.  Used by metaWeblog.newPost
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        ///     Gets or sets List of Metaweblog Post Structs.  Used by metaWeblog.getRecentPosts
        /// </summary>
        public List<PostStruct> Posts { get; set; }

        /// <summary>
        ///     Gets or sets Id of Category that was just added.  Used by wp.newCategory
        /// </summary>
        public string CategoryId { get; set; }

    }
}
