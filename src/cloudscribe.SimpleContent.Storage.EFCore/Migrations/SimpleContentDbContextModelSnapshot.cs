using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.SimpleContent.Storage.EFCore;

namespace cloudscribe.SimpleContent.Storage.EFCore.Migrations
{
    [DbContext(typeof(SimpleContentDbContext))]
    partial class SimpleContentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Author")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Content");

                    b.Property<string>("ContentId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Ip")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("PostId")
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

                    b.HasIndex("ContentId");

                    b.HasIndex("PostId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Comments");

                    b.HasAnnotation("SqlServer:TableName", "cs_Comment");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.Page", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Author")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("CategoryCsv");

                    b.Property<string>("Content");

                    b.Property<bool>("IsPublished")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("MetaDescription")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<int>("PageOrder");

                    b.Property<string>("ParentId");

                    b.Property<string>("ParentSlug")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<DateTime>("PubDate");

                    b.Property<bool>("ShowCategories")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("ShowComments")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("ShowHeading")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<bool>("ShowLastModified")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<bool>("ShowPubDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ViewRoles");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Pages");

                    b.HasAnnotation("SqlServer:TableName", "cs_Page");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.Post", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("Author")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("BlogId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("CategoryCsv");

                    b.Property<string>("Content");

                    b.Property<bool>("IsPublished")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

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

                    b.ToTable("Posts");

                    b.HasAnnotation("SqlServer:TableName", "cs_Post");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.ProjectSettings", b =>
                {
                    b.Property<string>("Id")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<bool>("AddBlogToPagesTree")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<bool>("BlogMenuLinksToNewestPost")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

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

                    b.Property<bool>("IncludePubDateInPostUrls")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<string>("LanguageCode")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("LocalMediaVirtualPath")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("ManagingEditorEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("ModerateComments")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

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

                    b.Property<bool>("ShowTitle")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("SmtpPassword");

                    b.Property<int>("SmtpPort");

                    b.Property<string>("SmtpPreferredEncoding")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<bool>("SmtpRequiresAuth")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("SmtpServer")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("SmtpUseSsl")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("SmtpUser")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("TimeZoneId")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Title")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<bool>("UseDefaultPageAsRootNode")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

                    b.Property<bool>("UseMetaDescriptionInFeed")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", false);

                    b.Property<string>("WebmasterEmail")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.ToTable("Projects");

                    b.HasAnnotation("SqlServer:TableName", "cs_ContentProject");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.TagItem", b =>
                {
                    b.Property<string>("TagValue")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("ContentId")
                        .HasAnnotation("MaxLength", 36);

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 36);

                    b.HasKey("TagValue", "ContentId");

                    b.HasIndex("ContentId");

                    b.HasIndex("ContentType");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TagValue");

                    b.ToTable("TagItems");

                    b.HasAnnotation("SqlServer:TableName", "cs_TagItem");
                });

            modelBuilder.Entity("cloudscribe.SimpleContent.Models.Comment", b =>
                {
                    b.HasOne("cloudscribe.SimpleContent.Models.Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });
        }
    }
}
