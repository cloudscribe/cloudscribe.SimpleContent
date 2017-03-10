using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL.Migrations
{
    public partial class changes20170310 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrelationKey",
                table: "cs_Post",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowRecentPostsOnDefaultPage",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: false)
                .Annotation("MySql:ValueGeneratedOnAdd", true);

            migrationBuilder.CreateIndex(
                name: "IX_cs_Post_CorrelationKey",
                table: "cs_Post",
                column: "CorrelationKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cs_Post_CorrelationKey",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "CorrelationKey",
                table: "cs_Post");

            migrationBuilder.DropColumn(
                name: "ShowRecentPostsOnDefaultPage",
                table: "cs_ContentProject");
        }
    }
}
