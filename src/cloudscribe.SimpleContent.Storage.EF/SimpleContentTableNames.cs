using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class SimpleContentTableNames
    {
        public SimpleContentTableNames()
        {

        }

        public string TablePrefix { get; set; } = "cs_";
        public string ProjectTableName { get; set; } = "ContentProject";
        public string PostTableName { get; set; } = "Post";
        public string PageTableName { get; set; } = "Page";
        public string CommentTableName { get; set; } = "Comment";

        public string TagTableName { get; set; } = "Tag";

        public string TagItemTableName { get; set; } = "TagItem";

    }
}
