

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public static class ModelExtensions
    {
        public static int ApprovedCommentCount(this IPost post)
        {
            if(post.Comments == null) { return 0; }
            return post.Comments.Where(c => c.IsApproved == true).Count();
        }

        public static int CommentCount(this IPost post)
        {
            if (post.Comments == null) { return 0; }
            return post.Comments.Count();
        }

        public static int ApprovedCommentCount(this IPage page)
        {
            if (page.Comments == null) { return 0; }
            return page.Comments.Where(c => c.IsApproved == true).Count();
        }
    }
}
