using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cloudscribe.SimpleContent.Storage.EFCore;

namespace cloudscribe.SimpleContent.Storage.EFCore.Migrations
{
    [DbContext(typeof(SimpleContentDbContext))]
    [Migration("20160909191651_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("PageCategories");

                    b.HasAnnotation("SqlServer:TableName", "cs_PageCategory");
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

                    b.ToTable("PageComments");

                    b.HasAnnotation("SqlServer:TableName", "cs_PageComment");
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

                    b.Property<bool>("IsPublished")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ColumnType", "bit")
                        .HasAnnotation("SqlServer:DefaultValue", true);

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

                    b.HasIndex("ParentId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Pages");

                    b.HasAnnotation("SqlServer:TableName", "cs_Page");
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

                    b.ToTable("PostCategories");

                    b.HasAnnotation("SqlServer:TableName", "cs_PostCategory");
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

                    b.ToTable("Comments");

                    b.HasAnnotation("SqlServer:TableName", "cs_PostComment");
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
