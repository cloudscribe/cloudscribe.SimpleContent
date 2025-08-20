using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class removepageresourcescript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "script",
                table: "cs_page_resource");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "script",
                table: "cs_page_resource",
                type: "text",
                nullable: true);
        }
    }
}
