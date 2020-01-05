using System;
using System.Collections.Generic;


namespace cloudscribe.MetaWeblog.Models
{
    /// <summary>
    /// MetaWeblog Post struct
    ///     used in newPost, editPost, getPost, recentPosts
    ///     not all properties are used everytime.
    /// </summary>
    public struct PostStruct
    {
        /// <summary>
        ///     wp_author_id
        /// </summary>
        public string author;

        /// <summary>
        ///     List of Categories assigned for Blog Post
        /// </summary>
        public List<string> categories;

        /// <summary>
        ///     CommentPolicy (Allow/Deny)
        ///     this would only be used on outgoing structs to indicate to the client
        ///     whether comments are allowed
        /// </summary>
        public string commentPolicy;

        /// <summary>
        ///     Content of Blog Post
        /// </summary>
        public string description;

        /// <summary>
        ///     Excerpt
        /// </summary>
        public string excerpt;

        /// <summary>
        ///     Link to Blog Post
        ///     this would only be populated on outgoing structs
        ///     we pass this to the cient, the client doesn't pass it to us
        /// </summary>
        public string link;

        /// <summary>
        ///     Display date of Blog Post (DateCreated)
        /// </summary>
        public DateTime postDate;

        //public DateTime dateCreated;

        /// <summary>
        ///     PostID Guid in string format
        /// </summary>
        public string postId;

        /// <summary>
        ///     Whether the Post is published or not.
        /// </summary>
        public bool publish;

        /// <summary>
        ///     Slug of post
        /// </summary>
        public string slug;

        /// <summary>
        ///     List of Tags assigned for Blog Post
        ///     
        ///     cloudscribe.SimpleContent is not currently supporting tags as a separate concept from categories
        ///     caregories are essentially similar to tags
        ///     tags maps to wordpress keywords, I'm guessin that was once upon a time used to populate
        ///     meta but has been ignored and devalued by search engines for many years so
        ///     nobody does that anymore afaik
        /// </summary>
        public List<string> tags;

        /// <summary>
        ///     Title of Blog Post
        /// </summary>
        public string title;
    }
}
