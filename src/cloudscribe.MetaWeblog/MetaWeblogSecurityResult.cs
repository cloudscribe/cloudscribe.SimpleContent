using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{
    public class MetaWeblogSecurityResult
    {
        public MetaWeblogSecurityResult(
            string displayName,
            string blogId,
            bool isAuthenticated, 
            bool canEditPosts,
            bool canEditPages
            )
        {
            this.displayName = displayName;
            this.blogId = blogId;
            this.isAuthenticated = isAuthenticated;
            this.canEditPosts = canEditPosts;
            this.canEditPages = canEditPages;
        }

        private string displayName = string.Empty;
        private string blogId = string.Empty;
        private bool isAuthenticated = false;
        private bool canEditPosts = false;
        private bool canEditPages = false;

        public string DisplayName
        {
            get { return displayName; }
        }

        public string BlogId
        {
            get { return blogId; }
        }


        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
        }

        public bool CanEditPosts
        {
            get { return canEditPosts; }
        }

        public bool CanEditPages
        {
            get { return canEditPages; }
        }
    }
}
