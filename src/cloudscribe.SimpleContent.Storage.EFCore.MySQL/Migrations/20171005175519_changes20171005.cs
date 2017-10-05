using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    public partial class changes20171005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "cs_Post",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "cs_Post",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "cs_Post",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowFeaturedPostsOnDefaultPage",
                table: "cs_ContentProject",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "ShowFeaturedPostsOnDefaultPage",
                table: "cs_ContentProject");
        }
    }
}
