using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
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
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_PageResource",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_Page",
                type: "nvarchar(max)",
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
