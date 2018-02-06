using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class simplecontentchanges20180206 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "AutoTeaserMode",
               table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "UseMetaDescriptionInFeed",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "SuppressAutoTeaser",
                table: "cs_Post");

            migrationBuilder.AddColumn<bool>(
                name: "SuppressTeaser",
                table: "cs_Post",
                nullable: false,
                defaultValue: false);
            
            migrationBuilder.AddColumn<byte>(
                name: "TeaserMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuppressTeaser",
                table: "cs_Post");

            migrationBuilder.AddColumn<bool>(
                name: "SuppressAutoTeaser",
                table: "cs_Post",
                nullable: false,
                defaultValue: false);

          

            migrationBuilder.DropColumn(
                name: "TeaserMode",
                table: "cs_ContentProject");

            

            migrationBuilder.AddColumn<byte>(
                name: "AutoTeaserMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "UseMetaDescriptionInFeed",
                table: "cs_ContentProject",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
