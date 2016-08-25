using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public static class ProjectConstants
    {
        public const string PostWithDateRouteName = "postwithdate";
        public const string PostWithoutDateRouteName = "postwithoutdate";
        public const string BlogCategoryRouteName = "blogcategory";
        public const string BlogArchiveRouteName = "blogarchive";
        public const string BlogIndexRouteName = "blogindex";
        public const string NewPostRouteName = "newpost";
        public const string MostRecentPostRouteName = "mostrecentpost";
        public const string PageIndexRouteName = "pageindex";

        public const string FolderPostWithDateRouteName = "folderpostwithdate";
        public const string FolderPostWithoutDateRouteName = "folderpostwithoutdate";
        public const string FolderBlogCategoryRouteName = "folderblogcategory";
        public const string FolderBlogArchiveRouteName = "folderblogarchive";
        public const string FolderBlogIndexRouteName = "folderblogindex";
        public const string FolderNewPostRouteName = "foldernewpost";
        public const string FolderMostRecentPostRouteName = "foldermostrecentpost";
        public const string FolderPageIndexRouteName = "folderpageindex";

        public const string ContentEditorClaimType = "ContentEditor";
        public const string PageEditorClaimType = "PageEditor";
        public const string BlogEditorClaimType = "BlogEditor";

        public const string PageEditPolicy = "PageEditPolicy";
        public const string BlogEditPolicy = "BlogEditPolicy";

    }
}
