using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    public partial class changes20170801b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowHeading",
                table: "cs_Page",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ShowHeading",
                table: "cs_Page",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
