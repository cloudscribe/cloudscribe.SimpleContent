// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2018-03-15
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL
{
    public class SimpleContentDbContext : SimpleContentDbContextBase, ISimpleContentDbContext
    {
        public SimpleContentDbContext(DbContextOptions<SimpleContentDbContext> options):base(options)
        {

        }

        protected SimpleContentDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ISimpleContentTableNames tableNames = new SimpleContentTableNames();
            
            modelBuilder.Entity<ProjectSettings>(entity =>
            {
                entity.ToTable(tableNames.TablePrefix + tableNames.ProjectTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Property(p => p.Title).HasMaxLength(255);

                entity.Property(p => p.Publisher).HasMaxLength(255);

                entity.Property(p => p.PublisherLogoUrl).HasMaxLength(255);

                entity.Property(p => p.CopyrightNotice).HasMaxLength(255);

                entity.Property(p => p.ModerateComments)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.CommentNotificationEmail).HasMaxLength(100);

                entity.Property(p => p.BlogMenuLinksToNewestPost)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.LocalMediaVirtualPath).HasMaxLength(255);

                entity.Property(p => p.CdnUrl).HasMaxLength(255);

                entity.Property(p => p.PubDateFormat).HasMaxLength(75);

                entity.Property(p => p.IncludePubDateInPostUrls)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.TimeZoneId).HasMaxLength(100);

                entity.Property(p => p.RecaptchaPrivateKey).HasMaxLength(255);

                entity.Property(p => p.RecaptchaPublicKey).HasMaxLength(255);

                entity.Property(p => p.DefaultPageSlug).HasMaxLength(255);

                entity.Property(p => p.UseDefaultPageAsRootNode)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.ShowTitle)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.AddBlogToPagesTree)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.BlogPageText)
                .HasMaxLength(255);

                entity.Property(p => p.BlogPageNavComponentVisibility).HasMaxLength(255);

                entity.Property(p => p.Image).HasMaxLength(255);

                //entity.Property(p => p.UseMetaDescriptionInFeed)
                //.IsRequired()
                //.HasColumnType("bit");

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

                //entity.Property(p => p.SmtpRequiresAuth)
                //.IsRequired()
                //.HasColumnType("bit");

                //entity.Property(p => p.SmtpUseSsl)
                //.IsRequired()
                //.HasColumnType("bit");

                entity.Property(p => p.PublisherLogoWidth).HasMaxLength(20);

                entity.Property(p => p.PublisherLogoHeight).HasMaxLength(20);

                entity.Property(p => p.DisqusShortName).HasMaxLength(100);

                entity.Property(p => p.PublisherEntityType).HasMaxLength(50);

                entity.Property(p => p.ShowRecentPostsOnDefaultPage)
                .IsRequired()
                .HasColumnType("bit");

                entity.Property(p => p.ShowFeaturedPostsOnDefaultPage)
               .IsRequired()
               .HasColumnType("bit");

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
                entity.ToTable(tableNames.TablePrefix + tableNames.PostTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Property(p => p.CorrelationKey).HasMaxLength(255);

                entity.HasIndex(p => p.CorrelationKey);

                entity.Property(p => p.BlogId)
                .HasMaxLength(36)
                .IsRequired();

                entity.HasIndex(p => p.BlogId);

                entity.Property(p => p.Title)
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(p => p.Author)
                .HasMaxLength(255);

                entity.Property(p => p.Slug)
                .HasMaxLength(255)
                .IsRequired();

                entity.HasIndex(p => p.Slug);

                entity.Property(p => p.MetaDescription)
                .HasMaxLength(500);

                entity.Property(p => p.IsPublished)
                .IsRequired()
                .HasColumnType("bit");

                entity.Ignore(p => p.Categories);

                entity.Property(p => p.CategoriesCsv)
                .HasMaxLength(500);

                entity.Ignore(p => p.Comments);

                entity.HasMany(p => p.PostComments)
                    .WithOne();

                entity.Property(p => p.ImageUrl)
                .HasMaxLength(250);

                entity.Property(p => p.ThumbnailUrl)
                .HasMaxLength(250);

                entity.Property(p => p.IsFeatured)
                .IsRequired();

                entity.Property(p => p.ContentType)
                   .HasMaxLength(50)
                   .HasDefaultValue("html");

                //entity.Property(p => p.SuppressTeaser)
                //    .HasDefaultValue(false);
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.ToTable(tableNames.TablePrefix + tableNames.PostCommentTableName);

                //entity.HasDiscriminator<string>("comment_type")
                //    .HasValue<Comment>("comment_base")
                //    .HasValue<PageComment>("comment_page");

                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                .HasMaxLength(36)
                ;

                entity.Ignore(p => p.ContentId); //mapped from postid

                entity.Property(p => p.PostEntityId)
                    .HasMaxLength(36);

                entity.HasIndex(p => p.PostEntityId);

                entity.Property(p => p.ProjectId)
                    .HasMaxLength(36)
                    .IsRequired();

                entity.HasIndex(p => p.ProjectId);

                entity.Property(p => p.Author).HasMaxLength(255);

                entity.Property(p => p.Email).HasMaxLength(100);

                entity.Property(p => p.Website).HasMaxLength(255);

                entity.Property(p => p.Ip).HasMaxLength(100);

                entity.Property(p => p.UserAgent).HasMaxLength(255);

            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.ToTable(tableNames.TablePrefix + tableNames.PostCategoryTableName);

                entity.HasKey(p => new { p.Value, p.PostEntityId });

                entity.Property(p => p.Value)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(p => p.Value);

                entity.Property(p => p.PostEntityId)
                    .HasMaxLength(36)
                    .IsRequired();

                entity.HasIndex(p => p.PostEntityId);

                entity.Property(p => p.ProjectId)
                    .HasMaxLength(36)
                    .IsRequired();

                entity.HasIndex(p => p.ProjectId);
            });

            modelBuilder.Entity<PageEntity>(entity =>
            {
                entity.ToTable(tableNames.TablePrefix + tableNames.PageTableName);

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasMaxLength(36);

                entity.Property(p => p.CorrelationKey).HasMaxLength(255);

                entity.HasIndex(p => p.CorrelationKey);

                entity.Property(p => p.ProjectId)
                .HasMaxLength(36)
                .IsRequired();

                entity.HasIndex(p => p.ProjectId);

                entity.Property(p => p.Title)
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(p => p.ParentId).HasMaxLength(36);

                entity.HasIndex(p => p.ParentId);

                entity.Property(p => p.ParentSlug).HasMaxLength(255);

                entity.Property(p => p.Author).HasMaxLength(255);

                entity.Property(p => p.Slug)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(p => p.ExternalUrl).HasMaxLength(255);

                entity.Property(p => p.MetaDescription).HasMaxLength(500);

                entity.Property(p => p.IsPublished)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(p => p.MenuOnly)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(p => p.ShowMenu)
                    .IsRequired()
                    .HasColumnType("bit");
                
                entity.Property(p => p.ShowHeading)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(p => p.ShowPubDate)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(p => p.ShowLastModified)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(p => p.ShowCategories)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(p => p.ShowComments)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.Property(p => p.MenuFilters)
                   .HasMaxLength(500);

                entity.Ignore(p => p.Categories);

                entity.Property(p => p.CategoriesCsv)
                    .HasMaxLength(500);

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

            });

            modelBuilder.Entity<PageComment>(entity =>
            {
                entity.ToTable(tableNames.TablePrefix + tableNames.PageCommentTableName);

                //entity.HasDiscriminator<string>("comment_type")
                //    .HasValue<Comment>("comment_base")
                //    .HasValue<PageComment>("comment_page");

                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                .HasMaxLength(36)
                ;

                entity.Ignore(p => p.ContentId); //mapped from postid

                entity.Property(p => p.PageEntityId)
                .HasMaxLength(36)
                //.IsRequired()
                ;
                entity.HasIndex(p => p.PageEntityId);

                entity.Property(p => p.ProjectId)
                .HasMaxLength(36)
                .IsRequired()
                ;
                entity.HasIndex(p => p.ProjectId);

                entity.Property(p => p.Author)
               .HasMaxLength(255)
               ;

                entity.Property(p => p.Email)
               .HasMaxLength(100)
               ;

                entity.Property(p => p.Website)
               .HasMaxLength(255)
               ;

                entity.Property(p => p.Ip)
               .HasMaxLength(100)
               ;

                entity.Property(p => p.UserAgent)
               .HasMaxLength(255)
               ;
            });

            modelBuilder.Entity<PageCategory>(entity =>
            {
                entity.ToTable(tableNames.TablePrefix + tableNames.PageCategoryTableName);

                entity.HasKey(p => new { p.Value, p.PageEntityId });

                entity.Property(p => p.Value)
                .HasMaxLength(50)
                .IsRequired()
                ;

                entity.HasIndex(p => p.Value);

                entity.Property(p => p.PageEntityId)
                .HasMaxLength(36)
                .IsRequired()
                ;
                entity.HasIndex(p => p.PageEntityId);

                entity.Property(p => p.ProjectId)
                .HasMaxLength(36)
                .IsRequired()
                ;
                entity.HasIndex(p => p.ProjectId);
            });

            modelBuilder.Entity<PageResourceEntity>(entity =>
            {
                entity.ToTable(tableNames.TablePrefix + tableNames.PageResourceTableName);
                
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                .HasMaxLength(36)
                ;

                entity.Ignore(p => p.ContentId); //mapped from pageEntityid

                entity.Property(p => p.PageEntityId)
                .HasMaxLength(36)
                //.IsRequired()
                ;
                entity.HasIndex(p => p.PageEntityId);

                entity.Property(p => p.Environment)
                .HasMaxLength(15)
                .IsRequired()
                ;
                

                entity.Property(p => p.Sort)
               .IsRequired()
               ;

                entity.Property(p => p.Type)
               .HasMaxLength(10)
               .IsRequired()
               ;

                entity.Property(p => p.Url)
               .HasMaxLength(255)
               .IsRequired()
               ;

                
            });

           
            
        }

    }
}
