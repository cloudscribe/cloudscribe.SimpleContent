using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite
{
    public class SimpleContentDbContext : SimpleContentDbContextBase, ISimpleContentDbContext
    {
        public SimpleContentDbContext(DbContextOptions<SimpleContentDbContext> options) : base(options)
        {

        }

        protected SimpleContentDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ProjectSettings>(entity =>
            {
                entity.ToTable("cs_ContentProject");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Property(p => p.Title).HasMaxLength(255);

                entity.Property(p => p.Publisher).HasMaxLength(255);

                entity.Property(p => p.PublisherLogoUrl).HasMaxLength(255);

                entity.Property(p => p.CopyrightNotice).HasMaxLength(255);

                entity.Property(p => p.ModerateComments).IsRequired();

                entity.Property(p => p.CommentNotificationEmail).HasMaxLength(100);

                entity.Property(p => p.BlogMenuLinksToNewestPost).IsRequired();

                entity.Property(p => p.LocalMediaVirtualPath).HasMaxLength(255);

                entity.Property(p => p.CdnUrl).HasMaxLength(255);

                entity.Property(p => p.PubDateFormat).HasMaxLength(75);

                entity.Property(p => p.IncludePubDateInPostUrls).IsRequired();

                entity.Property(p => p.TimeZoneId).HasMaxLength(100);

                entity.Property(p => p.RecaptchaPrivateKey).HasMaxLength(255);

                entity.Property(p => p.RecaptchaPublicKey).HasMaxLength(255);

                entity.Property(p => p.DefaultPageSlug).HasMaxLength(255);

                entity.Property(p => p.UseDefaultPageAsRootNode).IsRequired();

                entity.Property(p => p.ShowTitle).IsRequired();

                entity.Property(p => p.AddBlogToPagesTree).IsRequired();

                entity.Property(p => p.BlogPageText).HasMaxLength(255);

                entity.Property(p => p.BlogPageNavComponentVisibility).HasMaxLength(255);

                entity.Property(p => p.Image).HasMaxLength(255);

                //entity.Property(p => p.UseMetaDescriptionInFeed).IsRequired();

                entity.Property(p => p.LanguageCode).HasMaxLength(10);

                entity.Property(p => p.ChannelCategoriesCsv).HasMaxLength(255);

                entity.Property(p => p.ManagingEditorEmail).HasMaxLength(100);

                entity.Property(p => p.ChannelRating).HasMaxLength(100);

                entity.Property(p => p.WebmasterEmail).HasMaxLength(100);

                entity.Property(p => p.RemoteFeedUrl).HasMaxLength(255);

                entity.Property(p => p.RemoteFeedProcessorUseAgentFragment).HasMaxLength(255);

                //entity.Property(p => p.EmailFromAddress).HasMaxLength(100);

                //entity.Property(p => p.SmtpServer).HasMaxLength(100);

                //entity.Property(p => p.SmtpPort);

                //entity.Property(p => p.SmtpUser).HasMaxLength(500);

                //entity.Property(p => p.SmtpPassword);

                //entity.Property(p => p.SmtpPreferredEncoding).HasMaxLength(20);

                //entity.Property(p => p.SmtpRequiresAuth).IsRequired();

                //entity.Property(p => p.SmtpUseSsl).IsRequired();

                entity.Property(p => p.PublisherLogoWidth).HasMaxLength(20);

                entity.Property(p => p.PublisherLogoHeight).HasMaxLength(20);

                entity.Property(p => p.PublisherEntityType).HasMaxLength(50);

                entity.Property(p => p.DisqusShortName).HasMaxLength(100);

                entity.Property(p => p.ShowRecentPostsOnDefaultPage).IsRequired();

                entity.Property(p => p.ShowFeaturedPostsOnDefaultPage).IsRequired();

                entity.Property(p => p.FacebookAppId).HasMaxLength(100);
                entity.Property(p => p.SiteName).HasMaxLength(200);
                entity.Property(p => p.TwitterCreator).HasMaxLength(100);
                entity.Property(p => p.TwitterPublisher).HasMaxLength(100);

                entity.Property(p => p.DefaultContentType)
                .HasMaxLength(50)
                .HasDefaultValue("html")
                ;

                entity.Property(p => p.TeaserMode)
                .HasDefaultValue(TeaserMode.Off);

                entity.Property(p => p.TeaserTruncationMode)
                .HasDefaultValue(TeaserTruncationMode.Word);

                entity.Property(p => p.TeaserTruncationLength)
                .HasDefaultValue(20);

            });

            modelBuilder.Entity<PostEntity>(entity =>
            {
                entity.ToTable("cs_Post");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Property(p => p.CorrelationKey).HasMaxLength(255);

                entity.HasIndex(p => p.CorrelationKey);

                entity.Property(p => p.BlogId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.BlogId);

                entity.Property(p => p.Title).HasMaxLength(255).IsRequired();

                entity.Property(p => p.Author).HasMaxLength(255);

                entity.Property(p => p.Slug).HasMaxLength(255).IsRequired();

                entity.HasIndex(p => p.Slug);

                entity.Property(p => p.MetaDescription).HasMaxLength(500);

                entity.Property(p => p.IsPublished).IsRequired();

                entity.Ignore(p => p.Categories);

                entity.Property(p => p.CategoriesCsv).HasMaxLength(500);

                entity.Ignore(p => p.Comments);

                entity.HasMany(p => p.PostComments)
                    .WithOne();

                entity.Property(p => p.ImageUrl).HasMaxLength(250);

                entity.Property(p => p.ThumbnailUrl).HasMaxLength(250);

                entity.Property(p => p.IsFeatured).IsRequired();

                entity.Property(p => p.ContentType)
                .HasMaxLength(50)
                .HasDefaultValue("html");

                entity.Property(p => p.TemplateKey)
                  .HasMaxLength(255);

                entity.Property(p => p.Serializer)
                  .HasMaxLength(50);

                entity.Property(p => p.CreatedByUser)
                  .HasMaxLength(100);

                entity.Property(p => p.LastModifiedByUser)
                  .HasMaxLength(100);

                entity.Property(p => p.DraftAuthor)
                .HasMaxLength(255);


            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.ToTable("cs_PostComment");

                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Ignore(p => p.ContentId); //mapped from postid

                entity.Property(p => p.PostEntityId).HasMaxLength(36);
                entity.HasIndex(p => p.PostEntityId);

                entity.Property(p => p.ProjectId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.ProjectId);

                entity.Property(p => p.Author).HasMaxLength(255);

                entity.Property(p => p.Email).HasMaxLength(100);

                entity.Property(p => p.Website).HasMaxLength(255);

                entity.Property(p => p.Ip).HasMaxLength(100);

                entity.Property(p => p.UserAgent).HasMaxLength(255);

            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.ToTable("cs_PostCategory");

                entity.HasKey(p => new { p.Value, p.PostEntityId });

                entity.Property(p => p.Value).HasMaxLength(50).IsRequired();

                entity.HasIndex(p => p.Value);

                entity.Property(p => p.PostEntityId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.PostEntityId);

                entity.Property(p => p.ProjectId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.ProjectId);
            });

            modelBuilder.Entity<PageEntity>(entity =>
            {
                entity.ToTable("cs_Page");

                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Property(p => p.ProjectId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.ProjectId);

                entity.Property(p => p.Title).HasMaxLength(255).IsRequired();

                entity.Property(p => p.CorrelationKey).HasMaxLength(255);

                entity.HasIndex(p => p.CorrelationKey);

                entity.Property(p => p.ParentId).HasMaxLength(36);

                entity.HasIndex(p => p.ParentId);

                entity.Property(p => p.ParentSlug).HasMaxLength(255);

                entity.Property(p => p.Author).HasMaxLength(255);

                entity.Property(p => p.Slug).HasMaxLength(255).IsRequired();

                entity.Property(p => p.ExternalUrl).HasMaxLength(255);

                entity.Property(p => p.MetaDescription).HasMaxLength(500);

                entity.Property(p => p.IsPublished).IsRequired();

                entity.Property(p => p.MenuOnly).IsRequired();

                entity.Property(p => p.ShowMenu).IsRequired();


                entity.Property(p => p.ShowHeading).IsRequired();

                entity.Property(p => p.ShowPubDate).IsRequired();

                entity.Property(p => p.ShowLastModified).IsRequired();

                entity.Property(p => p.ShowCategories).IsRequired();

                entity.Property(p => p.ShowComments).IsRequired();

                entity.Ignore(p => p.Categories);

                entity.Property(p => p.CategoriesCsv).HasMaxLength(500);

                entity.Property(p => p.MenuFilters).HasMaxLength(500);

                entity.Ignore(p => p.Comments);

                entity.HasMany(p => p.PageComments)
                    .WithOne();

                // a shadow property to persist the categories/tags as a csv
                //entity.Property<string>("CategoryCsv");
                entity.Ignore(p => p.Resources);

                entity.HasMany(p => p.PageResources)
                    .WithOne();

                entity.Property(p => p.DisableEditor)
                  .IsRequired();

                entity.Property(p => p.ContentType)
               .HasMaxLength(50)
               .HasDefaultValue("html");

                entity.Property(p => p.TemplateKey)
                  .HasMaxLength(255);

                entity.Property(p => p.Serializer)
                  .HasMaxLength(50);

                entity.Property(p => p.CreatedByUser)
                  .HasMaxLength(100);

                entity.Property(p => p.LastModifiedByUser)
                  .HasMaxLength(100);

                entity.Property(p => p.DraftAuthor)
                .HasMaxLength(255);

            });

            modelBuilder.Entity<PageComment>(entity =>
            {
                entity.ToTable("cs_PageComment");

                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Ignore(p => p.ContentId); //mapped from postid

                entity.Property(p => p.PageEntityId).HasMaxLength(36);

                entity.HasIndex(p => p.PageEntityId);

                entity.Property(p => p.ProjectId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.ProjectId);

                entity.Property(p => p.Author).HasMaxLength(255);

                entity.Property(p => p.Email).HasMaxLength(100);

                entity.Property(p => p.Website).HasMaxLength(255);

                entity.Property(p => p.Ip).HasMaxLength(100);

                entity.Property(p => p.UserAgent).HasMaxLength(255);

            });

            modelBuilder.Entity<PageCategory>(entity =>
            {
                entity.ToTable("cs_PageCategory");

                entity.HasKey(p => new { p.Value, p.PageEntityId });

                entity.Property(p => p.Value).HasMaxLength(50).IsRequired();

                entity.HasIndex(p => p.Value);

                entity.Property(p => p.PageEntityId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.PageEntityId);

                entity.Property(p => p.ProjectId).HasMaxLength(36).IsRequired();

                entity.HasIndex(p => p.ProjectId);
            });

            modelBuilder.Entity<PageResourceEntity>(entity =>
            {
                entity.ToTable("cs_PageResource");

                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Ignore(p => p.ContentId); //mapped from pageEntityid

                entity.Property(p => p.PageEntityId).HasMaxLength(36);

                entity.HasIndex(p => p.PageEntityId);

                entity.Property(p => p.Environment).HasMaxLength(15).IsRequired();


                entity.Property(p => p.Sort).IsRequired();

                entity.Property(p => p.Type).HasMaxLength(10).IsRequired();

                entity.Property(p => p.Url).HasMaxLength(255).IsRequired();


            });

            modelBuilder.Entity<ContentHistory>(entity =>
            {
                entity.ToTable("cs_ContentHistory");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Property(p => p.CorrelationKey).HasMaxLength(255);

                entity.HasIndex(p => p.CorrelationKey);

                entity.Property(p => p.ContentId)
                .HasMaxLength(36)
                .IsRequired();

                entity.HasIndex(p => p.ContentId);

                entity.Property(p => p.Title)
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(p => p.Author)
                .HasMaxLength(255);

                entity.Property(p => p.IsPublished)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.ContentType)
                   .HasMaxLength(50)
                   .HasDefaultValue("html");

                entity.Property(p => p.CreatedByUser)
                  .HasMaxLength(100);

                entity.Property(p => p.LastModifiedByUser)
                  .HasMaxLength(100);

                entity.Property(p => p.DraftAuthor)
                .HasMaxLength(255);

                entity.Property(p => p.ContentSource)
                .HasMaxLength(50)
                .IsRequired();

                entity.HasIndex(p => p.ContentSource);

                entity.Property(p => p.DraftAuthor)
                .HasMaxLength(255);

                entity.Property(p => p.ArchivedBy)
                .HasMaxLength(255);


            });


        }

    }
}

