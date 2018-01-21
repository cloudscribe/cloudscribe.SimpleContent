// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2017-12-22
// 
namespace cloudscribe.SimpleContent.Models
{

    public class ProjectSettings : IProjectSettings
    {
        public ProjectSettings()
        {
        }

        public string Id { get; set; }
        public string Title { get; set; } = "Blog";
        public string Description { get; set; } = string.Empty;
        public string CopyrightNotice { get; set; } = string.Empty;

        public string Publisher { get; set; } = string.Empty;

        public string PublisherLogoUrl { get; set; } = string.Empty;

        public string PublisherLogoWidth { get; set; } = "500px";
        public string PublisherLogoHeight { get; set; } = "500px";

        public string PublisherEntityType { get; set; } = "Organization";

        public string DisqusShortName { get; set; } = string.Empty;

        public int PostsPerPage { get; set; } = 5;
        public int DaysToComment { get; set; } = -1;
        public bool ModerateComments { get; set; } = true;
        public string CommentNotificationEmail { get; set; }

        /// <summary>
        /// by default the blog menu item links to the post list
        /// if this is set to true, it should link/redirect to the newest post instead
        /// </summary>
        public bool BlogMenuLinksToNewestPost { get; set; } = false;
        public string LocalMediaVirtualPath { get; set; } = "/media/images/";
        //public string LocalMediaAbsoluteBaseUrl { get; set; } = string.Empty;
        public string CdnUrl { get; set; } = string.Empty;
        
        public string PubDateFormat { get; set; } = "MMMM d. yyyy";
        public bool IncludePubDateInPostUrls { get; set; } = true;

        private string timeZoneId = "America/New_York";
        public string TimeZoneId
        {
            get { return timeZoneId ?? "America/New_York"; }
            set { timeZoneId = value; }
        }
        public string RecaptchaPublicKey { get; set; }
        public string RecaptchaPrivateKey { get; set; }
        public string DefaultPageSlug { get; set; } = "home";
        public bool UseDefaultPageAsRootNode { get; set; } = true;
        
        public bool ShowTitle { get; set; } = false;

        // if true automatically add the blog index
        public bool AddBlogToPagesTree { get; set; } = true;
        public int BlogPagePosition { get; set; } = 2; // right after home page
        public string BlogPageText { get; set; } = "Blog";

        //public string HomePageText { get; set; } = "Home";
        public string BlogPageNavComponentVisibility { get; set; }

        // feed settings

        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// default is false, set to true if you want to use the metadescription instead of the full 
        /// post content in the feed.
        /// </summary>
        public bool UseMetaDescriptionInFeed { get; set; } = false;
        public int ChannelTimeToLive { get; set; } = 60;
        public string LanguageCode { get; set; } = "en-US";
        public string ChannelCategoriesCsv { get; set; } = string.Empty;
        public string ManagingEditorEmail { get; set; } = string.Empty;
        public string ChannelRating { get; set; } = string.Empty;
        public string WebmasterEmail { get; set; } = string.Empty;

        /// <summary>
        /// ie feedburner url
        /// </summary>
        public string RemoteFeedUrl { get; set; } = string.Empty;

        /// <summary>
        /// ie Feedburner User Agent fragment "FeedBurner"
        /// </summary>
        public string RemoteFeedProcessorUseAgentFragment { get; set; } = string.Empty;

        //smtp settings
        public string EmailFromAddress { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public int SmtpPort { get; set; } = 25;
        public string SmtpPreferredEncoding { get; set; }
        public bool SmtpRequiresAuth { get; set; } = false;
        public bool SmtpUseSsl { get; set; } = false;

        public bool ShowRecentPostsOnDefaultPage { get; set; }
        public bool ShowFeaturedPostsOnDefaultPage { get; set; }

        public string FacebookAppId { get; set; }
        public string SiteName { get; set; }
        public string TwitterPublisher { get; set; }
        public string TwitterCreator { get; set; }

        public string DefaultContentType { get; set; } = "html";

        public AutoTeaserMode AutoTeaserMode { get; set; }
        public TeaserTruncationMode TeaserTruncationMode { get; set; }
        public int TeaserTruncationLength { get; set; } = 20;   // Default 20 words.

        public static ProjectSettings FromIProjectSettings(IProjectSettings project)
        {
            var p = new ProjectSettings();
            project.CopyTo(p);
            return p;
        }

    }
}
