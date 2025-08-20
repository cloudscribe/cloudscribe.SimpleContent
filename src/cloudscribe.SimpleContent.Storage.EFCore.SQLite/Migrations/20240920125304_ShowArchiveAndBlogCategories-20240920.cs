using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class ShowArchiveAndBlogCategories20240920 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowArchivedPosts",
                table: "cs_ContentProject",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowBlogCategories",
                table: "cs_ContentProject",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowArchivedPosts",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowBlogCategories",
                table: "cs_ContentProject");
        }
    }
}
