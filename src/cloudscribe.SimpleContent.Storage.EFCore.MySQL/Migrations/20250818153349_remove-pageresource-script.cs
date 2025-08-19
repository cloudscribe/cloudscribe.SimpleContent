using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class removepageresourcescript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Script",
                table: "cs_PageResource");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_PageResource",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
