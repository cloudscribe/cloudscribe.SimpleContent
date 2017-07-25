using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    public partial class changes20170724 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaHtml",
                table: "cs_Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaJson",
                table: "cs_Post",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaHtml",
                table: "cs_Page",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaJson",
                table: "cs_Page",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaHtml",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "MetaJson",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "MetaHtml",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "MetaJson",
                table: "cs_Page");
        }
    }
}
