//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace cloudscribe.SimpleContent.Models
//{
//    /// <summary>
//    /// these model classes are a representation of "Category" used for relational db modeling for EF
//    /// I considered putting this model in the EF Storage project but decided it might also be useful for 
//    /// implementing other relation db sotrage later so might as well put it here so it can be re-used
//    /// I named it Tag rather than category to make it clear to developers implementing storage that this model
//    /// is not really used directly as Category, instead the relational storage should use this to retrieve data but then use 
//    /// the data to populate the List<string> Category property on Post or Page
//    /// for EF storage I think I will also use a shadow property to store categories as a csv on the post/page so that I can avoid
//    /// the extra db hit to retrieve categories when retrieving a post/page
//    /// but still need to keep a separate storage for populating the category list without processing all posts/pages
//    /// 
//    /// the difference is with documentdb type storage such as with NoDb the categories are stored as part of the post/page document
//    /// not as separate documents. also true of Comments when using NoDb
//    /// 
//    /// a class to represent Category string, since Post has List<string> rather than a true model for category
//    /// named it tag in case later a Category class is introduced in models
//    /// </summary>
//    public class Tag
//    {
//        /// <summary>
//        /// the actual category aka tag is the key
//        /// </summary>
//        public string Value { get; set; }

//        public string ProjectId { get; set; } // so we can retrieve by project and delete if the project is deleted

//        //public string ContentType { get; set; } = "Post";

       
//        //public string Value { get; set; }

//        //public int CountOfItems { get; set; }
//    }

//    /// <summary>
//    /// actually for EF storage I'm only using TagItem, not using Tag
//    /// TagValue and ContentId are used for composite key
//    /// </summary>
//    public class TagItem
//    {
//        public string TagValue { get; set; }

//        /// <summary>
//        /// the id of post or page that is tagged
//        /// </summary>
//        public string ContentId { get; set; }

//        public string ProjectId { get; set; } // so we can retrieve by project and delete if the project is deleted

//        public string ContentType { get; set; } = "Post";


//    }
//}
