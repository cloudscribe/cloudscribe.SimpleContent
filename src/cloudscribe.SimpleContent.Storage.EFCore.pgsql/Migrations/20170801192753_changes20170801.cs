using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170801 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowPubDate",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowMenu",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLastModified",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowHeading",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowComments",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCategories",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "MenuOnly",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowRecentPostsOnDefaultPage",
                table: "cs_ContentProject",
                type: "bool",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowPubDate",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowMenu",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowLastModified",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowHeading",
                table: "cs_Page",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowComments",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCategories",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "MenuOnly",
                table: "cs_Page",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublished",
                table: "cs_Page",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bool");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowRecentPostsOnDefaultPage",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bool");
        }
    }
}
