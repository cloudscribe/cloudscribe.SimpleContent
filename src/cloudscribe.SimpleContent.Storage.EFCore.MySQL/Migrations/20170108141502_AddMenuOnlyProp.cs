using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    public partial class AddMenuOnlyProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_Post",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PostComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_Page",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AddColumn<bool>(
                name: "MenuOnly",
                table: "cs_Page",
                nullable: false,
                defaultValue: false)
                .Annotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PageComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_ContentProject",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("MySql:ValueGeneratedOnAdd", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuOnly",
                table: "cs_Page");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_Post",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PostComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_Page",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PageComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_ContentProject",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("MySql:ValueGeneratedOnAdd", true);
        }
    }
}
