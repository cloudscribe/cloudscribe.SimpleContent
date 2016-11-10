using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cs_ContentProject",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    AddBlogToPagesTree = table.Column<bool>(nullable: false),
                    BlogMenuLinksToNewestPost = table.Column<bool>(nullable: false),
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
                    IncludePubDateInPostUrls = table.Column<bool>(nullable: false),
                    LanguageCode = table.Column<string>(maxLength: 10, nullable: true),
                    LocalMediaVirtualPath = table.Column<string>(maxLength: 255, nullable: true),
                    ManagingEditorEmail = table.Column<string>(maxLength: 100, nullable: true),
                    ModerateComments = table.Column<bool>(nullable: false),
                    PostsPerPage = table.Column<int>(nullable: false),
                    PubDateFormat = table.Column<string>(maxLength: 75, nullable: true),
                    RecaptchaPrivateKey = table.Column<string>(maxLength: 255, nullable: true),
                    RecaptchaPublicKey = table.Column<string>(maxLength: 255, nullable: true),
                    RemoteFeedProcessorUseAgentFragment = table.Column<string>(maxLength: 255, nullable: true),
                    RemoteFeedUrl = table.Column<string>(maxLength: 255, nullable: true),
                    ShowTitle = table.Column<bool>(nullable: false),
                    SmtpPassword = table.Column<string>(nullable: true),
                    SmtpPort = table.Column<int>(nullable: false),
                    SmtpPreferredEncoding = table.Column<string>(maxLength: 20, nullable: true),
                    SmtpRequiresAuth = table.Column<bool>(nullable: false),
                    SmtpServer = table.Column<string>(maxLength: 100, nullable: true),
                    SmtpUseSsl = table.Column<bool>(nullable: false),
                    SmtpUser = table.Column<string>(maxLength: 500, nullable: true),
                    TimeZoneId = table.Column<string>(maxLength: 100, nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: true),
                    UseDefaultPageAsRootNode = table.Column<bool>(nullable: false),
                    UseMetaDescriptionInFeed = table.Column<bool>(nullable: false),
                    WebmasterEmail = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_ContentProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cs_PageCategory",
                columns: table => new
                {
                    Value = table.Column<string>(maxLength: 50, nullable: false),
                    PageEntityId = table.Column<string>(maxLength: 36, nullable: false),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_PageCategory", x => new { x.Value, x.PageEntityId });
                });

            migrationBuilder.CreateTable(
                name: "cs_Page",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    CategoriesCsv = table.Column<string>(maxLength: 500, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    MetaDescription = table.Column<string>(maxLength: 500, nullable: true),
                    PageOrder = table.Column<int>(nullable: false),
                    ParentId = table.Column<string>(maxLength: 36, nullable: true),
                    ParentSlug = table.Column<string>(maxLength: 255, nullable: true),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false),
                    PubDate = table.Column<DateTime>(nullable: false),
                    ShowCategories = table.Column<bool>(nullable: false),
                    ShowComments = table.Column<bool>(nullable: false),
                    ShowHeading = table.Column<bool>(nullable: false),
                    ShowLastModified = table.Column<bool>(nullable: false),
                    ShowPubDate = table.Column<bool>(nullable: false),
                    Slug = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    ViewRoles = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_Page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cs_PostCategory",
                columns: table => new
                {
                    Value = table.Column<string>(maxLength: 50, nullable: false),
                    PostEntityId = table.Column<string>(maxLength: 36, nullable: false),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_PostCategory", x => new { x.Value, x.PostEntityId });
                });

            migrationBuilder.CreateTable(
                name: "cs_Post",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    BlogId = table.Column<string>(maxLength: 36, nullable: false),
                    CategoriesCsv = table.Column<string>(maxLength: 500, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
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
                name: "cs_PageComment",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Ip = table.Column<string>(maxLength: 100, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    PageEntityId = table.Column<string>(maxLength: 36, nullable: true),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false),
                    PubDate = table.Column<DateTime>(nullable: false),
                    UserAgent = table.Column<string>(maxLength: 255, nullable: true),
                    Website = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_PageComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cs_PageComment_cs_Page_PageEntityId",
                        column: x => x.PageEntityId,
                        principalTable: "cs_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cs_PostComment",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Ip = table.Column<string>(maxLength: 100, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    PostEntityId = table.Column<string>(maxLength: 36, nullable: true),
                    ProjectId = table.Column<string>(maxLength: 36, nullable: false),
                    PubDate = table.Column<DateTime>(nullable: false),
                    UserAgent = table.Column<string>(maxLength: 255, nullable: true),
                    Website = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_PostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cs_PostComment_cs_Post_PostEntityId",
                        column: x => x.PostEntityId,
                        principalTable: "cs_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cs_PageCategory_PageEntityId",
                table: "cs_PageCategory",
                column: "PageEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PageCategory_ProjectId",
                table: "cs_PageCategory",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PageCategory_Value",
                table: "cs_PageCategory",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PageComment_PageEntityId",
                table: "cs_PageComment",
                column: "PageEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PageComment_ProjectId",
                table: "cs_PageComment",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Page_ParentId",
                table: "cs_Page",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Page_ProjectId",
                table: "cs_Page",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PostCategory_PostEntityId",
                table: "cs_PostCategory",
                column: "PostEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PostCategory_ProjectId",
                table: "cs_PostCategory",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PostCategory_Value",
                table: "cs_PostCategory",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PostComment_PostEntityId",
                table: "cs_PostComment",
                column: "PostEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_PostComment_ProjectId",
                table: "cs_PostComment",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Post_BlogId",
                table: "cs_Post",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_Post_Slug",
                table: "cs_Post",
                column: "Slug");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_ContentProject");

            migrationBuilder.DropTable(
                name: "cs_PageCategory");

            migrationBuilder.DropTable(
                name: "cs_PageComment");

            migrationBuilder.DropTable(
                name: "cs_PostCategory");

            migrationBuilder.DropTable(
                name: "cs_PostComment");

            migrationBuilder.DropTable(
                name: "cs_Page");

            migrationBuilder.DropTable(
                name: "cs_Post");
        }
    }
}
