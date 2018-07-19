namespace cloudscribe.SimpleContent.Models
{
    public sealed class ContentSource
    {
        public const string Blog = "Blog";
        public const string Page = "Page";
    }

    public sealed class EnvironmentTarget
    {
        public const string Any = "any";
        public const string Development = "dev";
        public const string Production = "prod";
    }


    public static class ProjectConstants
    {
        public const string PostWithDateRouteName = "postwithdate";
        public const string PostWithoutDateRouteName = "postwithoutdate";
        public const string BlogCategoryRouteName = "blogcategory";
        public const string BlogArchiveRouteName = "blogarchive";
        public const string BlogIndexRouteName = "blogindex";
        public const string NewPostRouteName = "newpost";
        public const string MostRecentPostRouteName = "mostrecentpost";
        public const string PostEditRouteName = "postedit";
        public const string PostEditWithTemplateRouteName = "posteditwithtemplate";
        public const string PostDeleteRouteName = "postdelete";
        public const string PostHistoryRouteName = "posthistory";

        public const string FolderPostWithDateRouteName = "folderpostwithdate";
        public const string FolderPostWithoutDateRouteName = "folderpostwithoutdate";
        public const string FolderBlogCategoryRouteName = "folderblogcategory";
        public const string FolderBlogArchiveRouteName = "folderblogarchive";
        public const string FolderBlogIndexRouteName = "folderblogindex";
        public const string FolderNewPostRouteName = "foldernewpost";
        public const string FolderMostRecentPostRouteName = "foldermostrecentpost";
        public const string FolderPostEditRouteName = "folderpostedit";
        public const string FolderPostEditWithTemplateRouteName = "folderposteditwithtemplate";
        public const string FolderPostDeleteRouteName = "folderpostdelete";
        public const string FolderPostHistoryRouteName = "folderposthistory";

        public const string PageIndexRouteName = "pageindex";
        public const string PageEditRouteName = "pageedit";
        public const string PageEditWithTemplateRouteName = "pageeditwithtemplate";
        public const string PageDeleteRouteName = "pagedelete";
        public const string PageDevelopRouteName = "pagedevelop";
        public const string PageTreeRouteName = "pagetree";
        public const string NewPageRouteName = "newpage";
        public const string PageHistoryRouteName = "pagehistory";
        
        public const string FolderPageIndexRouteName = "folderpageindex";
        public const string FolderPageEditRouteName = "folderpageedit";
        public const string FolderPageEditWithTemplateRouteName = "folderpageeditwithtemplate";
        public const string FolderPageDeleteRouteName = "folderpagedelete";
        public const string FolderPageDevelopRouteName = "folderpagedevelop";
        public const string FolderPageTreeRouteName = "folderpagetree";
        public const string FolderNewPageRouteName = "foldernewpage";
        public const string FolderPageHistoryRouteName = "folderpagehistory";

        public const string ContentEditorClaimType = "ContentEditor";
        public const string PageEditorClaimType = "PageEditor";
        public const string BlogEditorClaimType = "BlogEditor";

        public const string PageEditPolicy = "PageEditPolicy";
        public const string BlogEditPolicy = "BlogEditPolicy";

        public const string BlogFeatureName = "Blog";
        public const string PageFeatureName = "Page";

        public const string HtmlContentType = "html";
        public const string MarkdownContentType = "markdown";


    }

    

}
