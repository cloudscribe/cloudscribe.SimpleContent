using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowPubDate",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLastModified",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "ShowHeading",
                table: "cs_Page",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "ShowComments",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCategories",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "cs_Page",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));

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

            migrationBuilder.AlterColumn<bool>(
                name: "ShowPubDate",
                table: "cs_Page",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLastModified",
                table: "cs_Page",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowHeading",
                table: "cs_Page",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowComments",
                table: "cs_Page",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCategories",
                table: "cs_Page",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "cs_Page",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }
    }
}
