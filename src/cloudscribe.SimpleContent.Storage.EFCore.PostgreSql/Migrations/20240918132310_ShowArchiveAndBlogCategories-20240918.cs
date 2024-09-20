using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class ShowArchiveAndBlogCategories20240918 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "show_archived_posts",
                table: "cs_content_project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_blog_categories",
                table: "cs_content_project",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "show_archived_posts",
                table: "cs_content_project");

            migrationBuilder.DropColumn(
                name: "show_blog_categories",
                table: "cs_content_project");
        }
    }
}
