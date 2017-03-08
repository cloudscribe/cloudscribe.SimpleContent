using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170308 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisqusShortName",
                table: "cs_ContentProject",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublisherLogoHeight",
                table: "cs_ContentProject",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublisherLogoWidth",
                table: "cs_ContentProject",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisqusShortName",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "PublisherLogoHeight",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "PublisherLogoWidth",
                table: "cs_ContentProject");
        }
    }
}
