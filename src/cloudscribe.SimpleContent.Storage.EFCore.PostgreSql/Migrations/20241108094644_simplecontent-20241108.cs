using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class simplecontent20241108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "show_created_by",
                table: "cs_page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_created_date",
                table: "cs_page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_last_modified_by",
                table: "cs_page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_last_modified_date",
                table: "cs_page",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_created_by",
                table: "cs_content_project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_created_date",
                table: "cs_content_project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_last_modified_by",
                table: "cs_content_project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_last_modified_date",
                table: "cs_content_project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "show_created_by",
                table: "cs_content_history",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_created_date",
                table: "cs_content_history",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_last_modified_by",
                table: "cs_content_history",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_last_modified_date",
                table: "cs_content_history",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "show_created_by",
                table: "cs_page");

            migrationBuilder.DropColumn(
                name: "show_created_date",
                table: "cs_page");

            migrationBuilder.DropColumn(
                name: "show_last_modified_by",
                table: "cs_page");

            migrationBuilder.DropColumn(
                name: "show_last_modified_date",
                table: "cs_page");

            migrationBuilder.DropColumn(
                name: "show_created_by",
                table: "cs_content_project");

            migrationBuilder.DropColumn(
                name: "show_created_date",
                table: "cs_content_project");

            migrationBuilder.DropColumn(
                name: "show_last_modified_by",
                table: "cs_content_project");

            migrationBuilder.DropColumn(
                name: "show_last_modified_date",
                table: "cs_content_project");

            migrationBuilder.DropColumn(
                name: "show_created_by",
                table: "cs_content_history");

            migrationBuilder.DropColumn(
                name: "show_created_date",
                table: "cs_content_history");

            migrationBuilder.DropColumn(
                name: "show_last_modified_by",
                table: "cs_content_history");

            migrationBuilder.DropColumn(
                name: "show_last_modified_date",
                table: "cs_content_history");
        }
    }
}
