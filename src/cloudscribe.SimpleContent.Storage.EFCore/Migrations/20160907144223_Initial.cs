using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cs_Page",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    CategoryCsv = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastModified = table.Column<DateTime>(nullable: false),
                    MetaDescription = table.Column<string>(maxLength: 500, nullable: true),
                    PageOrder = table.Column<int>(nullable: false),
                    ParentId = table.Column<string>(nullable: true),
                    ParentSlug = table.Column<string>(maxLength: 255, nullable: true),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false),
                    PubDate = table.Column<DateTime>(nullable: false),
                    ShowCategories = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShowComments = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShowHeading = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ShowLastModified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShowPubDate = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Slug = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    ViewRoles = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_Page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cs_Post",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    BlogId = table.Column<string>(maxLength: 36, nullable: false),
                    CategoryCsv = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastModified = table.Column<DateTime>(nullable: false),
                    MetaDescription = table.Column<string>(maxLength: 500, nullable: true),
                    PubDate = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_Post", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cs_ContentProject",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    AddBlogToPagesTree = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    BlogMenuLinksToNewestPost = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    BlogPageNavComponentVisibility = table.Column<string>(maxLength: 255, nullable: true),
                    BlogPagePosition = table.Column<int>(nullable: false),
                    BlogPageText = table.Column<string>(maxLength: 255, nullable: true),
                    CdnUrl = table.Column<string>(maxLength: 255, nullable: true),
                    ChannelCategoriesCsv = table.Column<string>(maxLength: 255, nullable: true),
                    ChannelRating = table.Column<string>(maxLength: 100, nullable: true),
                    ChannelTimeToLive = table.Column<int>(nullable: false),
                    CommentNotificationEmail = table.Column<string>(maxLength: 100, nullable: true),
                    CopyrightNotice = table.Column<string>(maxLength: 255, nullable: true),
                    DaysToComment = table.Column<int>(nullable: false),
                    DefaultPageSlug = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmailFromAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Image = table.Column<string>(maxLength: 255, nullable: true),
                    IncludePubDateInPostUrls = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LanguageCode = table.Column<string>(maxLength: 10, nullable: true),
                    LocalMediaVirtualPath = table.Column<string>(maxLength: 255, nullable: true),
                    ManagingEditorEmail = table.Column<string>(maxLength: 100, nullable: true),
                    ModerateComments = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PostsPerPage = table.Column<int>(nullable: false),
                    PubDateFormat = table.Column<string>(maxLength: 75, nullable: true),
                    RecaptchaPrivateKey = table.Column<string>(maxLength: 255, nullable: true),
                    RecaptchaPublicKey = table.Column<string>(maxLength: 255, nullable: true),
                    RemoteFeedProcessorUseAgentFragment = table.Column<string>(maxLength: 255, nullable: true),
                    RemoteFeedUrl = table.Column<string>(maxLength: 255, nullable: true),
                    ShowTitle = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SmtpPassword = table.Column<string>(nullable: true),
                    SmtpPort = table.Column<int>(nullable: false),
                    SmtpPreferredEncoding = table.Column<string>(maxLength: 20, nullable: true),
                    SmtpRequiresAuth = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SmtpServer = table.Column<string>(maxLength: 100, nullable: true),
                    SmtpUseSsl = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SmtpUser = table.Column<string>(maxLength: 500, nullable: true),
                    TimeZoneId = table.Column<string>(maxLength: 100, nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: true),
                    UseDefaultPageAsRootNode = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UseMetaDescriptionInFeed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    WebmasterEmail = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_ContentProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cs_TagItem",
                columns: table => new
                {
                    TagValue = table.Column<string>(maxLength: 50, nullable: false),
                    ContentId = table.Column<string>(maxLength: 36, nullable: false),
                    ContentType = table.Column<string>(maxLength: 20, nullable: false),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_TagItem", x => new { x.TagValue, x.ContentId });
                });

            migrationBuilder.CreateTable(
                name: "cs_Comment",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ContentId = table.Column<string>(maxLength: 36, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Ip = table.Column<string>(maxLength: 100, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    PostId = table.Column<string>(maxLength: 36, nullable: true),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false),
                    PubDate = table.Column<DateTime>(nullable: false),
                    UserAgent = table.Column<string>(maxLength: 255, nullable: true),
                    Website = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cs_Comment_cs_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "cs_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cs_Comment_ContentId",
                table: "cs_Comment",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Comment_PostId",
                table: "cs_Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Comment_ProjectId",
                table: "cs_Comment",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Page_ProjectId",
                table: "cs_Page",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Post_BlogId",
                table: "cs_Post",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Post_Slug",
                table: "cs_Post",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_cs_TagItem_ContentId",
                table: "cs_TagItem",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_TagItem_ContentType",
                table: "cs_TagItem",
                column: "ContentType");

            migrationBuilder.CreateIndex(
                name: "IX_cs_TagItem_ProjectId",
                table: "cs_TagItem",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_TagItem_TagValue",
                table: "cs_TagItem",
                column: "TagValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_Comment");

            migrationBuilder.DropTable(
                name: "cs_Page");

            migrationBuilder.DropTable(
                name: "cs_ContentProject");

            migrationBuilder.DropTable(
                name: "cs_TagItem");

            migrationBuilder.DropTable(
                name: "cs_Post");
        }
    }
}
