using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.SimpleContent.Storage.EFCore.MSSQL;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    [DbContext(typeof(SimpleContentDbContext))]
    [Migration("20170301144919_changes20170301")]
    partial class changes20170301
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.ProjectSettings", b =>
            {
                b.Property<string>("Id")
                    .HasMaxLength(36);

                b.Property<bool>("AddBlogToPagesTree")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(true);

                b.Property<bool>("BlogMenuLinksToNewestPost")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<string>("BlogPageNavComponentVisibility")
                    .HasMaxLength(255);

                b.Property<int>("BlogPagePosition");

                b.Property<string>("BlogPageText")
                    .HasMaxLength(255);

                b.Property<string>("CdnUrl")
                    .HasMaxLength(255);

                b.Property<string>("ChannelCategoriesCsv")
                    .HasMaxLength(255);

                b.Property<string>("ChannelRating")
                    .HasMaxLength(100);

                b.Property<int>("ChannelTimeToLive");

                b.Property<string>("CommentNotificationEmail")
                    .HasMaxLength(100);

                b.Property<string>("CopyrightNotice")
                    .HasMaxLength(255);

                b.Property<int>("DaysToComment");

                b.Property<string>("DefaultPageSlug")
                    .HasMaxLength(255);

                b.Property<string>("Description");

                b.Property<string>("EmailFromAddress")
                    .HasMaxLength(100);

                b.Property<string>("Image")
                    .HasMaxLength(255);

                b.Property<bool>("IncludePubDateInPostUrls")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(true);

                b.Property<string>("LanguageCode")
                    .HasMaxLength(10);

                b.Property<string>("LocalMediaVirtualPath")
                    .HasMaxLength(255);

                b.Property<string>("ManagingEditorEmail")
                    .HasMaxLength(100);

                b.Property<bool>("ModerateComments")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(true);

                b.Property<int>("PostsPerPage");

                b.Property<string>("PubDateFormat")
                    .HasMaxLength(75);

                b.Property<string>("Publisher")
                       .HasMaxLength(255);

                b.Property<string>("PublisherLogoUrl")
                    .HasMaxLength(255);

                b.Property<string>("RecaptchaPrivateKey")
                    .HasMaxLength(255);

                b.Property<string>("RecaptchaPublicKey")
                    .HasMaxLength(255);

                b.Property<string>("RemoteFeedProcessorUseAgentFragment")
                    .HasMaxLength(255);

                b.Property<string>("RemoteFeedUrl")
                    .HasMaxLength(255);

                b.Property<bool>("ShowTitle")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<string>("SmtpPassword");

                b.Property<int>("SmtpPort");

                b.Property<string>("SmtpPreferredEncoding")
                    .HasMaxLength(20);

                b.Property<bool>("SmtpRequiresAuth")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<string>("SmtpServer")
                    .HasMaxLength(100);

                b.Property<bool>("SmtpUseSsl")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<string>("SmtpUser")
                    .HasMaxLength(500);

                b.Property<string>("TimeZoneId")
                    .HasMaxLength(100);

                b.Property<string>("Title")
                    .HasMaxLength(255);

                b.Property<bool>("UseDefaultPageAsRootNode")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(true);

                b.Property<bool>("UseMetaDescriptionInFeed")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<string>("WebmasterEmail")
                    .HasMaxLength(100);

                b.HasKey("Id");

                b.ToTable("cs_ContentProject");


            });



            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageCategory", b =>
            {
                b.Property<string>("Value")
                    .HasMaxLength(50);

                b.Property<string>("PageEntityId")
                    .HasMaxLength(36);

                b.Property<string>("ProjectId")
                    .IsRequired()
                    .HasMaxLength(36);

                b.HasKey("Value", "PageEntityId");

                b.HasIndex("PageEntityId");

                b.HasIndex("ProjectId");

                b.HasIndex("Value");

                b.ToTable("cs_PageCategory");

            });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageComment", b =>
            {
                b.Property<string>("Id")
                    .HasMaxLength(36);

                b.Property<string>("Author")
                    .HasMaxLength(255);

                b.Property<string>("Content");

                b.Property<string>("Email")
                    .HasMaxLength(100);

                b.Property<string>("Ip")
                    .HasMaxLength(100);

                b.Property<bool>("IsAdmin");

                b.Property<bool>("IsApproved");

                b.Property<string>("PageEntityId")
                    .HasMaxLength(36);

                b.Property<string>("ProjectId")
                    .IsRequired()
                    .HasMaxLength(36);

                b.Property<DateTime>("PubDate");

                b.Property<string>("UserAgent")
                    .HasMaxLength(255);

                b.Property<string>("Website")
                    .HasMaxLength(255);

                b.HasKey("Id");

                b.HasIndex("PageEntityId");

                b.HasIndex("ProjectId");

                b.ToTable("cs_PageComment");


            });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PageEntity", b =>
            {
                b.Property<string>("Id")
                    .HasMaxLength(36);

                b.Property<string>("Author")
                    .HasMaxLength(255);

                b.Property<string>("CategoriesCsv")
                   .HasMaxLength(500);

                b.Property<string>("Content");

                b.Property<string>("CorrelationKey")
                        .HasMaxLength(255);

                b.Property<string>("ExternalUrl")
                    .HasMaxLength(255);

                b.Property<bool>("IsPublished")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(true);

                b.Property<DateTime>("LastModified");

                b.Property<bool>("MenuOnly")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<string>("MetaDescription")
                    .HasMaxLength(500);

                b.Property<int>("PageOrder");

                b.Property<string>("ParentId")
                    .HasMaxLength(36);

                b.Property<string>("ParentSlug")
                    .HasMaxLength(255);

                b.Property<string>("ProjectId")
                    .IsRequired()
                    .HasMaxLength(36);

                b.Property<DateTime>("PubDate");

                b.Property<bool>("ShowCategories")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<bool>("ShowComments")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<bool>("ShowHeading")
                    .ValueGeneratedOnAdd()
                   .HasColumnType("bit")
                    .HasDefaultValue(true);

                b.Property<bool>("ShowLastModified")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<bool>("ShowMenu")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<bool>("ShowPubDate")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(false);

                b.Property<string>("Slug")
                    .IsRequired()
                    .HasMaxLength(255);

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(255);

                b.Property<string>("ViewRoles");

                b.HasKey("Id");

                b.HasIndex("ParentId");

                b.HasIndex("ProjectId");

                b.ToTable("cs_Page");


            });


            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostCategory", b =>
            {
                b.Property<string>("Value")
                    .HasMaxLength(50);

                b.Property<string>("PostEntityId")
                    .HasMaxLength(36);

                b.Property<string>("ProjectId")
                    .IsRequired()
                    .HasMaxLength(36);

                b.HasKey("Value", "PostEntityId");

                b.HasIndex("PostEntityId");

                b.HasIndex("ProjectId");

                b.HasIndex("Value");

                b.ToTable("cs_PostCategory");

            });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostComment", b =>
            {
                b.Property<string>("Id")
                    .HasMaxLength(36);

                b.Property<string>("Author")
                    .HasMaxLength(255);

                b.Property<string>("Content");

                b.Property<string>("Email")
                    .HasMaxLength(100);

                b.Property<string>("Ip")
                    .HasMaxLength(100);

                b.Property<bool>("IsAdmin");

                b.Property<bool>("IsApproved");

                b.Property<string>("PostEntityId")
                    .HasMaxLength(36);

                b.Property<string>("ProjectId")
                    .IsRequired()
                    .HasMaxLength(36);

                b.Property<DateTime>("PubDate");

                b.Property<string>("UserAgent")
                    .HasMaxLength(255);

                b.Property<string>("Website")
                    .HasMaxLength(255);

                b.HasKey("Id");

                b.HasIndex("PostEntityId");

                b.HasIndex("ProjectId");

                b.ToTable("cs_PostComment");


            });

            modelBuilder.Entity("cloudscribe.SimpleContent.Storage.EFCore.Models.PostEntity", b =>
            {
                b.Property<string>("Id")
                    .HasMaxLength(36);

                b.Property<string>("Author")
                    .HasMaxLength(255);

                b.Property<string>("BlogId")
                    .IsRequired()
                    .HasMaxLength(36);

                b.Property<string>("CategoriesCsv")
                    .HasMaxLength(500);

                b.Property<string>("Content");

                b.Property<bool>("IsPublished")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bit")
                    .HasDefaultValue(true);

                b.Property<DateTime>("LastModified");

                b.Property<string>("MetaDescription")
                    .HasMaxLength(500);

                b.Property<DateTime>("PubDate");

                b.Property<string>("Slug")
                    .IsRequired()
                    .HasMaxLength(255);

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(255);

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
