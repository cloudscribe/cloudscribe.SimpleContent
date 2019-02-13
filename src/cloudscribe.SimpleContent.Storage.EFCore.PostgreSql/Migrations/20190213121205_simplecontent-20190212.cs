using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    public partial class simplecontent20190212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "about_heading",
                table: "cs_content_project",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_about_box",
                table: "cs_content_project",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_related_posts",
                table: "cs_content_project",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "about_heading",
                table: "cs_content_project");

            migrationBuilder.DropColumn(
                name: "show_about_box",
                table: "cs_content_project");

            migrationBuilder.DropColumn(
                name: "show_related_posts",
                table: "cs_content_project");
        }
    }
}
