using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class simplecontent20180705 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "cs_ContentHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaHtml",
                table: "cs_ContentHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaJson",
                table: "cs_ContentHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageOrder",
                table: "cs_ContentHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "cs_ContentHistory",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentSlug",
                table: "cs_ContentHistory",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serializer",
                table: "cs_ContentHistory",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateKey",
                table: "cs_ContentHistory",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViewRoles",
                table: "cs_ContentHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "MetaHtml",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "MetaJson",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "PageOrder",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "ParentSlug",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "Serializer",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "TemplateKey",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "ViewRoles",
                table: "cs_ContentHistory");
        }
    }
}
