﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cloudscribe.SimpleContent.Storage.EFCore.MySQL;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    [DbContext(typeof(SimpleContentDbContext))]
    partial class SimpleContentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.ContentHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("char(36)");

                    b.Property<string>("ArchivedBy")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ArchivedUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Author")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CategoriesCsv")
                        .HasColumnType("longtext");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ContentSource")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ContentType")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValue("html");

                    b.Property<string>("CorrelationKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreatedByUser")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DraftAuthor")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DraftContent")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DraftPubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DraftSerializedModel")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDraftHx")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastModifiedByUser")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MetaDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("MetaHtml")
                        .HasColumnType("longtext");

                    b.Property<string>("MetaJson")
                        .HasColumnType("longtext");

                    b.Property<int>("PageOrder")
                        .HasColumnType("int");

                    b.Property<string>("ParentId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ParentSlug")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProjectId")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("PubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SerializedModel")
                        .HasColumnType("longtext");

                    b.Property<string>("Serializer")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Slug")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TeaserOverride")
                        .HasColumnType("longtext");

                    b.Property<string>("TemplateKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ViewRoles")
                        .HasColumnType("longtext");

                    b.Property<bool>("WasDeleted")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("ContentId");

                    b.HasIndex("ContentSource");

                    b.HasIndex("CorrelationKey");

                    b.HasIndex("CreatedByUser");

                    b.HasIndex("LastModifiedByUser");

                    b.HasIndex("Title");

                    b.ToTable("cs_ContentHistory", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.ProjectSettings", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("AboutContent")
                        .HasColumnType("longtext");

                    b.Property<string>("AboutHeading")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("AddBlogToPagesTree")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("BlogMenuLinksToNewestPost")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("BlogPageNavComponentVisibility")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("BlogPagePosition")
                        .HasColumnType("int");

                    b.Property<string>("BlogPageText")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CdnUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ChannelCategoriesCsv")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ChannelRating")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("ChannelTimeToLive")
                        .HasColumnType("int");

                    b.Property<string>("CommentNotificationEmail")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CopyrightNotice")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("DaysToComment")
                        .HasColumnType("int");

                    b.Property<string>("DefaultContentType")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValue("html");

                    b.Property<int>("DefaultFeedItems")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(20);

                    b.Property<string>("DefaultPageSlug")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("DisqusShortName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FacebookAppId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Image")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IncludePubDateInPostUrls")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LanguageCode")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("LocalMediaVirtualPath")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ManagingEditorEmail")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("MaxFeedItems")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1000);

                    b.Property<bool>("ModerateComments")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("PostsPerPage")
                        .HasColumnType("int");

                    b.Property<string>("PubDateFormat")
                        .HasMaxLength(75)
                        .HasColumnType("varchar(75)");

                    b.Property<string>("Publisher")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PublisherEntityType")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PublisherLogoHeight")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("PublisherLogoUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PublisherLogoWidth")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("RecaptchaPrivateKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RecaptchaPublicKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RemoteFeedProcessorUseAgentFragment")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RemoteFeedUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("ShowAboutBox")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<bool>("ShowArchivedPosts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<bool>("ShowBlogCategories")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<bool>("ShowFeaturedPostsOnDefaultPage")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowRecentPostsOnDefaultPage")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowRelatedPosts")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<bool>("ShowTitle")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SiteName")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<byte>("TeaserMode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint unsigned")
                        .HasDefaultValue((byte)0);

                    b.Property<int>("TeaserTruncationLength")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(20);

                    b.Property<byte>("TeaserTruncationMode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint unsigned")
                        .HasDefaultValue((byte)0);

                    b.Property<string>("TimeZoneId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TwitterCreator")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("TwitterPublisher")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("UseDefaultPageAsRootNode")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("WebmasterEmail")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("cs_ContentProject", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageCategory", b =>
                {
                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PageEntityId")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Value", "PageEntityId");

                    b.HasIndex("PageEntityId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("Value");

                    b.ToTable("cs_PageCategory", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageComment", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Author")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Ip")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PageEntityId")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("PubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Website")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PageEntityId");

                    b.HasIndex("ProjectId");

                    b.ToTable("cs_PageComment", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Author")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CategoriesCsv")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("ContentType")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValue("html");

                    b.Property<string>("CorrelationKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreatedByUser")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("DisableEditor")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("DraftAuthor")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DraftContent")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DraftPubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DraftSerializedModel")
                        .HasColumnType("longtext");

                    b.Property<string>("ExternalUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastModifiedByUser")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MenuFilters")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<bool>("MenuOnly")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("MetaDescription")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("MetaHtml")
                        .HasColumnType("longtext");

                    b.Property<string>("MetaJson")
                        .HasColumnType("longtext");

                    b.Property<int>("PageOrder")
                        .HasColumnType("int");

                    b.Property<string>("ParentId")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ParentSlug")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("PubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SerializedModel")
                        .HasColumnType("longtext");

                    b.Property<string>("Serializer")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("ShowCategories")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowComments")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowHeading")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowLastModified")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowMenu")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowPubDate")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TemplateKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ViewRoles")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CorrelationKey");

                    b.HasIndex("ParentId");

                    b.HasIndex("ProjectId");

                    b.ToTable("cs_Page", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageResourceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Environment")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("PageEntityId")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PageEntityId");

                    b.ToTable("cs_PageResource", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostCategory", b =>
                {
                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PostEntityId")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Value", "PostEntityId");

                    b.HasIndex("PostEntityId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("Value");

                    b.ToTable("cs_PostCategory", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostComment", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Author")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Ip")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PostEntityId")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("PubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Website")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PostEntityId");

                    b.HasIndex("ProjectId");

                    b.ToTable("cs_PostComment", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Author")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AutoTeaser")
                        .HasColumnType("longtext");

                    b.Property<string>("BlogId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CategoriesCsv")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<string>("ContentType")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValue("html");

                    b.Property<string>("CorrelationKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreatedByUser")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DraftAuthor")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DraftContent")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DraftPubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DraftSerializedModel")
                        .HasColumnType("longtext");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastModifiedByUser")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MetaDescription")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("MetaHtml")
                        .HasColumnType("longtext");

                    b.Property<string>("MetaJson")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("PubDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SerializedModel")
                        .HasColumnType("longtext");

                    b.Property<string>("Serializer")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<ulong>("ShowComments")
                        .HasColumnType("bit");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("SuppressTeaser")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("TeaserOverride")
                        .HasColumnType("longtext");

                    b.Property<string>("TemplateKey")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ThumbnailUrl")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("CorrelationKey");

                    b.HasIndex("Slug");

                    b.ToTable("cs_Post", (string)null);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageComment", b =>
                {
                    b.HasOne("cloudscribe.SimpleContent.Storage.EFCore.Models.PageEntity", null)
                        .WithMany("PageComments")
                        .HasForeignKey("PageEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageResourceEntity", b =>
                {
                    b.HasOne("cloudscribe.SimpleContent.Storage.EFCore.Models.PageEntity", null)
                        .WithMany("PageResources")
                        .HasForeignKey("PageEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostComment", b =>
                {
                    b.HasOne("cloudscribe.SimpleContent.Storage.EFCore.Models.PostEntity", null)
                        .WithMany("PostComments")
                        .HasForeignKey("PostEntityId");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageEntity", b =>
                {
                    b.Navigation("PageComments");

                    b.Navigation("PageResources");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostEntity", b =>
                {
                    b.Navigation("PostComments");
                });
#pragma warning restore 612, 618
        }
    }
}
