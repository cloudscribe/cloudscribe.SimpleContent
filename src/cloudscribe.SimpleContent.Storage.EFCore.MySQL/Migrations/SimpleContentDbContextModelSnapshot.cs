using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.SimpleContent.Storage.EFCore.MySQL;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    [DbContext(typeof(SimpleContentDbContext))]
    partial class SimpleContentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.ProjectSettings", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<bool>("AddBlogToPagesTree");

                    b.Property<bool>("BlogMenuLinksToNewestPost");

                    b.Property<string>("BlogPageNavComponentVisibility")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("BlogPagePosition");

                    b.Property<string>("BlogPageText")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("CdnUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ChannelCategoriesCsv")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ChannelRating")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("ChannelTimeToLive");

                    b.Property<string>("CommentNotificationEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("CopyrightNotice")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("DaysToComment");

                    b.Property<string>("DefaultPageSlug")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Description");

                    b.Property<string>("EmailFromAddress")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Image")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("IncludePubDateInPostUrls");

                    b.Property<string>("LanguageCode")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("LocalMediaVirtualPath")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ManagingEditorEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("ModerateComments");

                    b.Property<int>("PostsPerPage");

                    b.Property<string>("PubDateFormat")
                        .HasAnnotation("MaxLength", 75);

                    b.Property<string>("RecaptchaPrivateKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RecaptchaPublicKey")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RemoteFeedProcessorUseAgentFragment")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("RemoteFeedUrl")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("ShowTitle");

                    b.Property<string>("SmtpPassword");

                    b.Property<int>("SmtpPort");

                    b.Property<string>("SmtpPreferredEncoding")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<bool>("SmtpRequiresAuth");

                    b.Property<string>("SmtpServer")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("SmtpUseSsl");

                    b.Property<string>("SmtpUser")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("TimeZoneId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Title")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("UseDefaultPageAsRootNode");

                    b.Property<bool>("UseMetaDescriptionInFeed");

                    b.Property<string>("WebmasterEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.ToTable("cs_ContentProject");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageCategory", b =>
                {
                    b.Property<string>("Value")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("PageEntityId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.HasKey("Value", "PageEntityId");

                    b.HasIndex("PageEntityId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("Value");

                    b.ToTable("cs_PageCategory");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageComment", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Author")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Content");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Ip")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("PageEntityId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("PubDate");

                    b.Property<string>("UserAgent")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Website")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("PageEntityId");

                    b.HasIndex("ProjectId");

                    b.ToTable("cs_PageComment");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Author")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("CategoriesCsv")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("Content");

                    b.Property<bool>("IsPublished");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("MetaDescription")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<int>("PageOrder");

                    b.Property<string>("ParentId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("ParentSlug")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("PubDate");

                    b.Property<bool>("ShowCategories");

                    b.Property<bool>("ShowComments");

                    b.Property<bool>("ShowHeading");

                    b.Property<bool>("ShowLastModified");

                    b.Property<bool>("ShowPubDate");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ViewRoles");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("ProjectId");

                    b.ToTable("cs_Page");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostCategory", b =>
                {
                    b.Property<string>("Value")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("PostEntityId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.HasKey("Value", "PostEntityId");

                    b.HasIndex("PostEntityId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("Value");

                    b.ToTable("cs_PostCategory");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostComment", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Author")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Content");

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Ip")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("PostEntityId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("PubDate");

                    b.Property<string>("UserAgent")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Website")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("PostEntityId");

                    b.HasIndex("ProjectId");

                    b.ToTable("cs_PostComment");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Author")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("BlogId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("CategoriesCsv")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("Content");

                    b.Property<bool>("IsPublished");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("MetaDescription")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<DateTime>("PubDate");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("Slug");

                    b.ToTable("cs_Post");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageComment", b =>
                {
                    b.HasOne("cloudscribe.SimpleContent.Storage.EFCore.Models.PageEntity")
                        .WithMany("PageComments")
                        .HasForeignKey("PageEntityId");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostComment", b =>
                {
                    b.HasOne("cloudscribe.SimpleContent.Storage.EFCore.Models.PostEntity")
                        .WithMany("PostComments")
                        .HasForeignKey("PostEntityId");
                });
        }
    }
}
