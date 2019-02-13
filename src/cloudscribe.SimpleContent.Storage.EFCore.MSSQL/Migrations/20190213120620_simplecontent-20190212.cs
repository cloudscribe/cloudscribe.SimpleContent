using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class simplecontent20190212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutHeading",
                table: "cs_ContentProject",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAboutBox",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowRelatedPosts",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutHeading",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowAboutBox",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowRelatedPosts",
                table: "cs_ContentProject");
        }
    }
}
