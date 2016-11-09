using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class SimpleContentTableNames : ISimpleContentTableNames
    {
        public SimpleContentTableNames()
        {

        }

        public string TablePrefix { get; set; } = "cs_";
        public string ProjectTableName { get; set; } = "ContentProject";
        public string PostTableName { get; set; } = "Post";
        public string PageTableName { get; set; } = "Page";
        public string PostCommentTableName { get; set; } = "PostComment";

        //public string TagTableName { get; set; } = "Tag";

        public string PostCategoryTableName { get; set; } = "PostCategory";

        public string PageCommentTableName { get; set; } = "PageComment";

        public string PageCategoryTableName { get; set; } = "PageCategory";

    }
}
