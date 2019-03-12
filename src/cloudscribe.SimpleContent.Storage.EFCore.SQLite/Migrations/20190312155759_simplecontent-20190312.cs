using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    public partial class simplecontent20190312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_cs_PageComment_cs_Page_PageEntityId",
            //    table: "cs_PageComment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_cs_PageResource_cs_Page_PageEntityId",
            //    table: "cs_PageResource");

            //migrationBuilder.AlterColumn<byte>(
            //    name: "TeaserTruncationMode",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: (byte)0,
            //    oldClrType: typeof(byte),
            //    oldDefaultValue: (byte)0)
            //    .OldAnnotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AlterColumn<byte>(
            //    name: "TeaserMode",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: (byte)0,
            //    oldClrType: typeof(byte),
            //    oldDefaultValue: (byte)0)
            //    .OldAnnotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_cs_PageComment_cs_Page_PageEntityId",
            //    table: "cs_PageComment",
            //    column: "PageEntityId",
            //    principalTable: "cs_Page",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_cs_PageResource_cs_Page_PageEntityId",
            //    table: "cs_PageResource",
            //    column: "PageEntityId",
            //    principalTable: "cs_Page",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_cs_PageComment_cs_Page_PageEntityId",
            //    table: "cs_PageComment");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_cs_PageResource_cs_Page_PageEntityId",
            //    table: "cs_PageResource");

            //migrationBuilder.AlterColumn<byte>(
            //    name: "TeaserTruncationMode",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: (byte)0,
            //    oldClrType: typeof(byte),
            //    oldDefaultValue: (byte)0)
            //    .Annotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AlterColumn<byte>(
            //    name: "TeaserMode",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: (byte)0,
            //    oldClrType: typeof(byte),
            //    oldDefaultValue: (byte)0)
            //    .Annotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_cs_PageComment_cs_Page_PageEntityId",
            //    table: "cs_PageComment",
            //    column: "PageEntityId",
            //    principalTable: "cs_Page",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_cs_PageResource_cs_Page_PageEntityId",
            //    table: "cs_PageResource",
            //    column: "PageEntityId",
            //    principalTable: "cs_Page",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
