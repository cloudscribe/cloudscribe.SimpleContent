using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
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
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_PageResource",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "cs_Page",
                type: "text",
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
