using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    public partial class simplecontent20190212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "AboutHeading",
                table: "cs_ContentProject",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowAboutBox",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowRelatedPosts",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutHeading",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowAboutBox",
                table: "cs_ContentProject");

            migrationBuilder.DropColumn(
                name: "ShowRelatedPosts",
                table: "cs_ContentProject");

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
        }
    }
}
