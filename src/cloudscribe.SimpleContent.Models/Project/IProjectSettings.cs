// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					
// Last Modified:			2019-02-10
// 
namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectSettings
    {
        bool AddBlogToPagesTree { get; set; }
        bool BlogMenuLinksToNewestPost { get; set; }
        string BlogPageNavComponentVisibility { get; set; }
        int BlogPagePosition { get; set; }
        string BlogPageText { get; set; }
        string CdnUrl { get; set; }
        string ChannelCategoriesCsv { get; set; }
        string ChannelRating { get; set; }
        int ChannelTimeToLive { get; set; }
        string CommentNotificationEmail { get; set; }
        string CopyrightNotice { get; set; }
        int DaysToComment { get; set; }
        string DefaultPageSlug { get; set; }
        string Description { get; set; }
        //string EmailFromAddress { get; set; }
        string Id { get; set; }
        string Image { get; set; }
        bool IncludePubDateInPostUrls { get; set; }
        string LanguageCode { get; set; }
        string LocalMediaVirtualPath { get; set; }
        string ManagingEditorEmail { get; set; }

        int DefaultFeedItems { get; set; }
        int MaxFeedItems { get; set; }
        bool ModerateComments { get; set; }
        int PostsPerPage { get; set; }
        string Publisher { get; set; }
        string PublisherLogoUrl { get; set; }
        string PubDateFormat { get; set; }
        string RecaptchaPrivateKey { get; set; }
        string RecaptchaPublicKey { get; set; }
        string RemoteFeedProcessorUseAgentFragment { get; set; }
        string RemoteFeedUrl { get; set; }
        bool ShowTitle { get; set; }
        //string SmtpPassword { get; set; }
        //int SmtpPort { get; set; }
        //string SmtpPreferredEncoding { get; set; }
        //bool SmtpRequiresAuth { get; set; }
        //string SmtpServer { get; set; }
        //string SmtpUser { get; set; }
        //bool SmtpUseSsl { get; set; }
        string TimeZoneId { get; set; }
        string Title { get; set; }
        bool UseDefaultPageAsRootNode { get; set; }
        //bool UseMetaDescriptionInFeed { get; set; }
        string WebmasterEmail { get; set; }
        string PublisherLogoWidth { get; set; } 
        string PublisherLogoHeight { get; set; }
        string PublisherEntityType { get; set; }
        string DisqusShortName { get; set; }
        bool ShowRecentPostsOnDefaultPage { get; set; }
        bool ShowFeaturedPostsOnDefaultPage { get; set; }

        string FacebookAppId { get; set; }
        string SiteName { get; set; }
        string TwitterPublisher { get; set; }
        string TwitterCreator { get; set; }
        string DefaultContentType { get; set; }

        bool ShowRelatedPosts { get; set; }

        bool ShowAboutBox { get; set; } 
        string AboutHeading { get; set; }
        string AboutContent { get; set; }

        // Teaser properties.
        /// <summary>
        /// Specifies whether SimpleContent should show teasers for blog posts on index/listing views.
        /// The default is OFF (show entire post).
        /// </summary>
        TeaserMode TeaserMode { get; set; }
        /// <summary>
        /// Specifies how SimpleContent will truncate blog posts to create teasers for index/listing views.
        /// </summary>
        TeaserTruncationMode TeaserTruncationMode { get; set; }
        /// <summary>
        /// Specifies the length in characters/words/paragraphs that a post will be truncated to create a teaser for index/listing views.
        /// </summary>
        int TeaserTruncationLength { get; set; }
    }
}