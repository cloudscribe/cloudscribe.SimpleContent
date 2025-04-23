using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class userscript20250423 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "script",
                table: "cs_post",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "script",
                table: "cs_page_resource",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "script",
                table: "cs_page",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "script",
                table: "cs_post");

            migrationBuilder.DropColumn(
                name: "script",
                table: "cs_page_resource");

            migrationBuilder.DropColumn(
                name: "script",
                table: "cs_page");
        }
    }
}
