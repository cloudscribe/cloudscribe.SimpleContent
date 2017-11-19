using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class changes20171118 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "cs_Post",
                maxLength: 50,
                nullable: true,
                defaultValue: "html");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "cs_Page",
                maxLength: 50,
                nullable: true,
                defaultValue: "html");

            migrationBuilder.AddColumn<string>(
                name: "DefaultContentType",
                table: "cs_ContentProject",
                maxLength: 50,
                nullable: true,
                defaultValue: "html");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "DefaultContentType",
                table: "cs_ContentProject");
        }
    }
}
