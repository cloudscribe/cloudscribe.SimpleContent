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
            bool isAllowed, 
            bool isBlogOwner)
        {
            this.displayName = displayName;
            this.blogId = blogId;
            this.isAllowed = isAllowed;
            this.isBlogOwner = isBlogOwner;
        }

        private string displayName = string.Empty;
        private string blogId = string.Empty;
        private bool isAllowed = false;
        private bool isBlogOwner = false;

        public string DisplayName
        {
            get { return displayName; }
        }

        public string BlogId
        {
            get { return blogId; }
        }


        public bool IsAllowed
        {
            get { return isAllowed; }
        }

        public bool IsBlogOwner
        {
            get { return isBlogOwner; }
        }
    }
}
