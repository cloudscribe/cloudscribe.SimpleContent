using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class ShowArchivedAndBlogCategories20240918 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowArchivedPosts",
                table: "cs_ContentProject",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowBlogCategories",
                table: "cs_ContentProject",
                type: "bit",
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
