

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

        public static void CopyTo(this IPost input, IPost target)
        {
            target.Author = input.Author;
            target.BlogId = input.BlogId;
            target.Categories = input.Categories;
            target.Comments = input.Comments;
            target.Content = input.Content;
            target.ContentType = input.ContentType;
            target.Id = input.Id;
            target.IsPublished = input.IsPublished;
            target.LastModified = input.LastModified;
            target.MetaDescription = input.MetaDescription;
            target.MetaHtml = input.MetaHtml;
            target.MetaJson = input.MetaJson;
            target.PubDate = input.PubDate;
            target.Slug = input.Slug;
            target.Title = input.Title;
            target.CorrelationKey = input.CorrelationKey;
            target.ImageUrl = input.ImageUrl;
            target.ThumbnailUrl = input.ThumbnailUrl;
            target.IsFeatured = input.IsFeatured;
        }

        public static void CopyTo(this IPage input, IPage target)
        {
 
            target.Author = input.Author;
            target.Categories = input.Categories;
            target.Comments = input.Comments;
            target.CorrelationKey = input.CorrelationKey;
            target.Content = input.Content;
            target.ContentType = input.ContentType;
            target.DisableEditor = input.DisableEditor;
            target.ExternalUrl = input.ExternalUrl;
            target.Id = input.Id;
            target.IsPublished = input.IsPublished;
            target.LastModified = input.LastModified;
            target.MenuFilters = input.MenuFilters;
            target.MenuOnly = input.MenuOnly;
            target.MetaDescription = input.MetaDescription;
            target.MetaHtml = input.MetaHtml;
            target.MetaJson = input.MetaJson;
            target.PageOrder = input.PageOrder;
            target.ParentId = input.ParentId;
            target.ParentSlug = input.ParentSlug;
            target.ProjectId = input.ProjectId;
            target.PubDate = input.PubDate;
            target.Resources = input.Resources;
            target.ShowCategories = input.ShowCategories;
            target.ShowComments = input.ShowComments;
            target.ShowHeading = input.ShowHeading;
            target.ShowMenu = input.ShowMenu;
            target.ShowLastModified = input.ShowLastModified;
            target.ShowPubDate = input.ShowPubDate;
            target.Slug = input.Slug;
            target.Title = input.Title;
            target.ViewRoles = input.ViewRoles;
        }

        public static void CopyTo(this IProjectSettings input, IProjectSettings target)
        {

            target.AddBlogToPagesTree = input.AddBlogToPagesTree;
            target.BlogMenuLinksToNewestPost = input.BlogMenuLinksToNewestPost;
            target.BlogPageNavComponentVisibility = input.BlogPageNavComponentVisibility;
            target.BlogPagePosition = input.BlogPagePosition;
            target.BlogPageText = input.BlogPageText;
            target.CdnUrl = input.CdnUrl;
            target.ChannelCategoriesCsv = input.ChannelCategoriesCsv;
            target.ChannelRating = input.ChannelRating;
            target.ChannelTimeToLive = input.ChannelTimeToLive;
            target.CommentNotificationEmail = input.CommentNotificationEmail;
            target.CopyrightNotice = input.CopyrightNotice;
            target.DaysToComment = input.DaysToComment;
            target.DefaultPageSlug = input.DefaultPageSlug;
            target.DefaultContentType = input.DefaultContentType;
            target.Description = input.Description;
            target.EmailFromAddress = input.EmailFromAddress;
            target.FacebookAppId = input.FacebookAppId;
            target.Id = input.Id;
            target.Image = input.Image;
            target.IncludePubDateInPostUrls = input.IncludePubDateInPostUrls;
            target.LanguageCode = input.LanguageCode;
            target.LocalMediaVirtualPath = input.LocalMediaVirtualPath;
            target.ManagingEditorEmail = input.ManagingEditorEmail;
            target.ModerateComments = input.ModerateComments;
            target.PostsPerPage = input.PostsPerPage;
            target.PubDateFormat = input.PubDateFormat;
            target.RecaptchaPrivateKey = input.RecaptchaPrivateKey;
            target.RecaptchaPublicKey = input.RecaptchaPublicKey;
            target.RemoteFeedProcessorUseAgentFragment = input.RemoteFeedProcessorUseAgentFragment;
            target.RemoteFeedUrl = input.RemoteFeedUrl;
            target.ShowTitle = input.ShowTitle;
            target.SiteName = input.SiteName;
            target.SmtpPassword = input.SmtpPassword;
            target.SmtpPort = input.SmtpPort;
            target.SmtpPreferredEncoding = input.SmtpPreferredEncoding;
            target.SmtpRequiresAuth = input.SmtpRequiresAuth;
            target.SmtpServer = input.SmtpServer;
            target.SmtpUser = input.SmtpUser;
            target.SmtpUseSsl = input.SmtpUseSsl;
            target.TimeZoneId = input.TimeZoneId;
            target.Title = input.Title;
            target.TwitterCreator = input.TwitterCreator;
            target.TwitterPublisher = input.TwitterPublisher;
            target.UseDefaultPageAsRootNode = input.UseDefaultPageAsRootNode;
            target.UseMetaDescriptionInFeed = input.UseMetaDescriptionInFeed;
            target.WebmasterEmail = input.WebmasterEmail;
            target.Publisher = input.Publisher;
            target.PublisherLogoUrl = input.PublisherLogoUrl;
            target.PublisherLogoWidth = input.PublisherLogoWidth;
            target.PublisherLogoHeight = input.PublisherLogoHeight;
            target.PublisherEntityType = input.PublisherEntityType;
            target.DisqusShortName = input.DisqusShortName;
            target.ShowRecentPostsOnDefaultPage = input.ShowRecentPostsOnDefaultPage;
            target.ShowFeaturedPostsOnDefaultPage = input.ShowFeaturedPostsOnDefaultPage;
        }


    }
}
