using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class userscript20250423 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_Post",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_PageResource",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_Page",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Script",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "Script",
                table: "cs_PageResource");

            migrationBuilder.DropColumn(
                name: "Script",
                table: "cs_Page");
        }
    }
}
