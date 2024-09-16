using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    /// <inheritdoc />
    public partial class ShowHideEditingDetails20240916 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedBy",
                table: "cs_Page",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowCreatedDate",
                table: "cs_Page",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedBy",
                table: "cs_Page",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLastModifiedDate",
                table: "cs_Page",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false);
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
        }
    }
}
