using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    public partial class simplecontentchanges20180206 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "AutoTeaserMode",
            //    table: "cs_ContentProject");

            // https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations

            //migrationBuilder.DropColumn(
            //    name: "UseMetaDescriptionInFeed",
            //    table: "cs_ContentProject");

            //migrationBuilder.RenameColumn(
            //    name: "SuppressAutoTeaser",
            //    table: "cs_Post",
            //    newName: "SuppressTeaser");

            //migrationBuilder.DropColumn(
            //    name: "SuppressAutoTeaser",
            //    table: "cs_Post");

            //migrationBuilder.AddColumn<bool>(
            //    name: "SuppressTeaser",
            //    table: "cs_Post",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AlterColumn<byte>(
            //    name: "TeaserTruncationMode",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: (byte)0,
            //    oldClrType: typeof(byte),
            //    oldDefaultValue: (byte)0)
            //    .OldAnnotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AddColumn<byte>(
            //    name: "TeaserMode",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "TeaserMode",
            //    table: "cs_ContentProject");

            //migrationBuilder.DropColumn(
            //    name: "SuppressTeaser",
            //    table: "cs_Post");

            //migrationBuilder.AddColumn<bool>(
            //    name: "SuppressAutoTeaser",
            //    table: "cs_Post",
            //    nullable: false,
            //    defaultValue: false);

            ////migrationBuilder.RenameColumn(
            ////    name: "SuppressTeaser",
            ////    table: "cs_Post",
            ////    newName: "SuppressAutoTeaser");

            ////migrationBuilder.AlterColumn<byte>(
            ////    name: "TeaserTruncationMode",
            ////    table: "cs_ContentProject",
            ////    nullable: false,
            ////    defaultValue: (byte)0,
            ////    oldClrType: typeof(byte),
            ////    oldDefaultValue: (byte)0)
            ////    .Annotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AddColumn<byte>(
            //    name: "AutoTeaserMode",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: (byte)0)
            //    ;

            //migrationBuilder.AddColumn<bool>(
            //    name: "UseMetaDescriptionInFeed",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: false);
        }
    }
}
