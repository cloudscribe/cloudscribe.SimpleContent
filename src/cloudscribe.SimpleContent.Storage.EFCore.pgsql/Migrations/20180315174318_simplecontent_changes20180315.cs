using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
{
    public partial class simplecontent_changes20180315 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailFromAddress",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SmtpPassword",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SmtpPort",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SmtpPreferredEncoding",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SmtpRequiresAuth",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SmtpServer",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SmtpUseSsl",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SmtpUser",
                table: "cs_ContentProject");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailFromAddress",
                table: "cs_ContentProject",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpPassword",
                table: "cs_ContentProject",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmtpPort",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmtpPreferredEncoding",
                table: "cs_ContentProject",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmtpRequiresAuth",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SmtpServer",
                table: "cs_ContentProject",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmtpUseSsl",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SmtpUser",
                table: "cs_ContentProject",
                maxLength: 500,
                nullable: true);
        }
    }
}
