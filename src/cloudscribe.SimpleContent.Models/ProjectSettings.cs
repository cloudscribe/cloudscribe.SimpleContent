// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-09
// Last Modified:           2016-09-07
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

        public string BlogPageNavComponentVisibility { get; set; } = "topnav";

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

        public static ProjectSettings FromIProjectSettings(IProjectSettings project)
        {
            var p = new ProjectSettings();
            p.AddBlogToPagesTree = project.AddBlogToPagesTree;
            p.BlogMenuLinksToNewestPost = project.BlogMenuLinksToNewestPost;
            p.BlogPageNavComponentVisibility = project.BlogPageNavComponentVisibility;
            p.BlogPagePosition = project.BlogPagePosition;
            p.BlogPageText = project.BlogPageText;
            p.CdnUrl = project.CdnUrl;
            p.ChannelCategoriesCsv = project.ChannelCategoriesCsv;
            p.ChannelRating = project.ChannelRating;
            p.ChannelTimeToLive = project.ChannelTimeToLive;
            p.CommentNotificationEmail = project.CommentNotificationEmail;
            p.CopyrightNotice = project.CopyrightNotice;
            p.DaysToComment = project.DaysToComment;
            p.DefaultPageSlug = project.DefaultPageSlug;
            p.Description = project.Description;
            p.EmailFromAddress = project.EmailFromAddress;
            p.Id = project.Id;
            p.Image = project.Image;
            p.IncludePubDateInPostUrls = project.IncludePubDateInPostUrls;
            p.LanguageCode = project.LanguageCode;
            p.LocalMediaVirtualPath = project.LocalMediaVirtualPath;
            p.ManagingEditorEmail = project.ManagingEditorEmail;
            p.ModerateComments = project.ModerateComments;
            p.PostsPerPage = project.PostsPerPage;
            p.PubDateFormat = project.PubDateFormat;
            p.RecaptchaPrivateKey = project.RecaptchaPrivateKey;
            p.RecaptchaPublicKey = project.RecaptchaPublicKey;
            p.RemoteFeedProcessorUseAgentFragment = project.RemoteFeedProcessorUseAgentFragment;
            p.RemoteFeedUrl = project.RemoteFeedUrl;
            p.ShowTitle = project.ShowTitle;
            p.SmtpPassword = project.SmtpPassword;
            p.SmtpPort = project.SmtpPort;
            p.SmtpPreferredEncoding = project.SmtpPreferredEncoding;
            p.SmtpRequiresAuth = project.SmtpRequiresAuth;
            p.SmtpServer = project.SmtpServer;
            p.SmtpUser = project.SmtpUser;
            p.SmtpUseSsl = project.SmtpUseSsl;
            p.TimeZoneId = project.TimeZoneId;
            p.Title = project.Title;
            p.UseDefaultPageAsRootNode = project.UseDefaultPageAsRootNode;
            p.UseMetaDescriptionInFeed = project.UseMetaDescriptionInFeed;
            p.WebmasterEmail = project.WebmasterEmail;
            

            return p;
        }

    }
}
