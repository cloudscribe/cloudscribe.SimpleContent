using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class simplecontent20241108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedBy",
                table: "cs_Page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedDate",
                table: "cs_Page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedBy",
                table: "cs_Page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedDate",
                table: "cs_Page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedBy",
                table: "cs_ContentProject",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedDate",
                table: "cs_ContentProject",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedBy",
                table: "cs_ContentProject",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedDate",
                table: "cs_ContentProject",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedBy",
                table: "cs_ContentHistory",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedDate",
                table: "cs_ContentHistory",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedBy",
                table: "cs_ContentHistory",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedDate",
                table: "cs_ContentHistory",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowCreatedBy",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "ShowCreatedDate",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "ShowLastModifiedBy",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "ShowLastModifiedDate",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "ShowCreatedBy",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowCreatedDate",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowLastModifiedBy",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowLastModifiedDate",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowCreatedBy",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "ShowCreatedDate",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "ShowLastModifiedBy",
                table: "cs_ContentHistory");

            migrationBuilder.DropColumn(
                name: "ShowLastModifiedDate",
                table: "cs_ContentHistory");
        }
    }
}
