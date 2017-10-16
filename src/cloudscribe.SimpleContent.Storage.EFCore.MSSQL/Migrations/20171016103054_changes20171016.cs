using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class changes20171016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookAppId",
                table: "cs_ContentProject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                table: "cs_ContentProject",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterCreator",
                table: "cs_ContentProject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterPublisher",
                table: "cs_ContentProject",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookAppId",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "TwitterCreator",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "TwitterPublisher",
                table: "cs_ContentProject");
        }
    }
}
