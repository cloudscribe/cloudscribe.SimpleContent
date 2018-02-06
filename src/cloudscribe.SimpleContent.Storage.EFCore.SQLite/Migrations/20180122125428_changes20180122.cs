using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    public partial class changes20180122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SuppressTeaser",
                table: "cs_Post",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TeaserOverride",
                table: "cs_Post",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TeaserMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "TeaserTruncationLength",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: 20)
                ;

            migrationBuilder.AddColumn<byte>(
                name: "TeaserTruncationMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuppressTeaser",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "TeaserOverride",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "TeaserMode",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "TeaserTruncationLength",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "TeaserTruncationMode",
                table: "cs_ContentProject");
        }
    }
}
