using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class simplecontentuserscript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_Post",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_PageResource",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_Page",
                type: "TEXT",
                nullable: true);
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
