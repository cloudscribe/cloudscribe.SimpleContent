

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration.ViewModels
{
    public class ProjectViewModel
    {

        

        public string Title { get; set; } = "Blog";

        public bool ShowTitle { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        public string CopyrightNotice { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int PostsPerPage { get; set; } = 5;

        public string PubDateFormat { get; set; } = "MMMM d. yyyy";
        public bool IncludePubDateInPostUrls { get; set; } = true;

       

        //public string LocalMediaVirtualPath { get; set; } = "/media/images/";

       

        public int DaysToComment { get; set; } = -1;
        public bool ModerateComments { get; set; } = true;
        public string CommentNotificationEmail { get; set; } = string.Empty;
        
        public int GravatarSize { get; set; } = 50;

        
        //public string DefaultPageSlug { get; set; } = "home";
        //public bool UseDefaultPageAsRootNode { get; set; } = true;

        

        //public string AllowedEditRoles { get; set; } = "Administrators";
        

        // if true automatically add the blog index
        //public bool AddBlogToPagesTree { get; set; } = true;
        //public int BlogPagePosition { get; set; } = 2; // right after home page
        //public string BlogPageText { get; set; } = "Blog";
        //public string BlogPageNavComponentVisibility { get; set; } = "topnav";


        
        public string RemoteFeedUrl { get; set; } = string.Empty;

        /// <summary>
        /// ie Feedburner User Agent fragment "FeedBurner"
        /// </summary>
        public string RemoteFeedProcessorUseAgentFragment { get; set; } = string.Empty;
        public bool UseMetaDescriptionInFeed { get; set; } = false;
        public int ChannelTimeToLive { get; set; } = 60;
        public string LanguageCode { get; set; } = "en-US";
        public string ChannelCategoriesCsv { get; set; } = string.Empty;
        public string ManagingEditorEmail { get; set; } = string.Empty;
        public string ChannelRating { get; set; } = string.Empty;
        public string WebmasterEmail { get; set; } = string.Empty;

        
    }
}
