using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class changes20170301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrelationKey",
                table: "cs_Page",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "cs_Page",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowMenu",
                table: "cs_Page",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "cs_ContentProject",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublisherLogoUrl",
                table: "cs_ContentProject",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cs_Page_CorrelationKey",
                table: "cs_Page",
                column: "CorrelationKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cs_Page_CorrelationKey",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "CorrelationKey",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "ShowMenu",
                table: "cs_Page");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "PublisherLogoUrl",
                table: "cs_ContentProject");
        }
    }
}
