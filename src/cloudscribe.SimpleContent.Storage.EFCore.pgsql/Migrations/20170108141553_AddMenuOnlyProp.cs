using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql.Migrations
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
                .Annotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PostComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_Page",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AddColumn<bool>(
                name: "MenuOnly",
                table: "cs_Page",
                nullable: false,
                defaultValue: false)
                .Annotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PageComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_ContentProject",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .Annotation("Npgsql:ValueGeneratedOnAdd", true);
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
                .OldAnnotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PostComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_Page",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_PageComment",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("Npgsql:ValueGeneratedOnAdd", true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "cs_ContentProject",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 36)
                .OldAnnotation("Npgsql:ValueGeneratedOnAdd", true);
        }
    }
}
