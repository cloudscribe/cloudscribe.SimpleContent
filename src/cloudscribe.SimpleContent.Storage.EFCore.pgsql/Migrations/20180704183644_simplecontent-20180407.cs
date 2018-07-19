using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
{
    public partial class simplecontent20180407 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PubDate",
                table: "cs_Post",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUser",
                table: "cs_Post",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "cs_Post",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DraftAuthor",
                table: "cs_Post",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DraftContent",
                table: "cs_Post",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DraftPubDate",
                table: "cs_Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DraftSerializedModel",
                table: "cs_Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUser",
                table: "cs_Post",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerializedModel",
                table: "cs_Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serializer",
                table: "cs_Post",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateKey",
                table: "cs_Post",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PubDate",
                table: "cs_Page",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUser",
                table: "cs_Page",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "cs_Page",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DraftAuthor",
                table: "cs_Page",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DraftContent",
                table: "cs_Page",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DraftPubDate",
                table: "cs_Page",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DraftSerializedModel",
                table: "cs_Page",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedByUser",
                table: "cs_Page",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerializedModel",
                table: "cs_Page",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serializer",
                table: "cs_Page",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateKey",
                table: "cs_Page",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultFeedItems",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxFeedItems",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "cs_ContentHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(maxLength: 36, nullable: false),
                    ContentId = table.Column<string>(maxLength: 36, nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    ContentSource = table.Column<string>(maxLength: 50, nullable: false),
                    ContentType = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "html"),
                    Slug = table.Column<string>(maxLength: 255, nullable: true),
                    IsDraftHx = table.Column<bool>(nullable: false),
                    WasDeleted = table.Column<bool>(nullable: false),
                    ArchivedUtc = table.Column<DateTime>(nullable: false),
                    ArchivedBy = table.Column<string>(maxLength: 255, nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    Author = table.Column<string>(maxLength: 255, nullable: true),
                    CorrelationKey = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PubDate = table.Column<DateTime>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedByUser = table.Column<string>(maxLength: 100, nullable: true),
                    LastModifiedByUser = table.Column<string>(maxLength: 100, nullable: true),
                    DraftContent = table.Column<string>(nullable: true),
                    DraftAuthor = table.Column<string>(maxLength: 255, nullable: true),
                    DraftPubDate = table.Column<DateTime>(nullable: true),
                    SerializedModel = table.Column<string>(nullable: true),
                    DraftSerializedModel = table.Column<string>(nullable: true),
                    TeaserOverride = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_ContentHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cs_ContentHistory_ContentId",
                table: "cs_ContentHistory",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_cs_ContentHistory_ContentSource",
                table: "cs_ContentHistory",
                column: "ContentSource");

            migrationBuilder.CreateIndex(
                name: "IX_cs_ContentHistory_CorrelationKey",
                table: "cs_ContentHistory",
                column: "CorrelationKey");

            migrationBuilder.CreateIndex(
                name: "IX_cs_ContentHistory_CreatedByUser",
                table: "cs_ContentHistory",
                column: "CreatedByUser");

            migrationBuilder.CreateIndex(
                name: "IX_cs_ContentHistory_LastModifiedByUser",
                table: "cs_ContentHistory",
                column: "LastModifiedByUser");

            migrationBuilder.CreateIndex(
                name: "IX_cs_ContentHistory_Title",
                table: "cs_ContentHistory",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "CreatedByUser",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "DraftAuthor",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "DraftContent",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "DraftPubDate",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "DraftSerializedModel",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUser",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "SerializedModel",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "Serializer",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "TemplateKey",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "CreatedByUser",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "DraftAuthor",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "DraftContent",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "DraftPubDate",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "DraftSerializedModel",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUser",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "SerializedModel",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "Serializer",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "TemplateKey",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "DefaultFeedItems",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "MaxFeedItems",
                table: "cs_ContentProject");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PubDate",
                table: "cs_Post",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PubDate",
                table: "cs_Page",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
