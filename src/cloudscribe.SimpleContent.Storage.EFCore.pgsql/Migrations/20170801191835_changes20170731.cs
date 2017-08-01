using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
{
    public partial class changes20170731 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisableEditor",
                table: "cs_Page",
                type: "bool",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowRecentPostsOnDefaultPage",
                table: "cs_ContentProject",
                type: "bool",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisableEditor",
                table: "cs_Page");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowRecentPostsOnDefaultPage",
                table: "cs_ContentProject",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bool",
                oldDefaultValue: false);
        }
    }
}
