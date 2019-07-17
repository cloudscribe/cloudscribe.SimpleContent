using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.SimpleContent
{
    public class ContentSettingsUIConfig
    {
        public bool ShowBlogMenuOptions { get; set; } = true;
        public bool ShowBlogSettings { get; set; } = true;
        public bool ShowPageSettings { get; set; } = true;
        public bool ShowDefaultContentType { get; set; } = false;

        public bool ShowCommentSettings { get; set; } = true;
    }
}
