using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    public partial class AddPostShowCommentsSwitch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowComments",
                table: "cs_Post",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<byte>(
                name: "TeaserTruncationMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)0)
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<byte>(
                name: "TeaserMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)0)
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowComments",
                table: "cs_Post");

            migrationBuilder.AlterColumn<byte>(
                name: "TeaserTruncationMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<byte>(
                name: "TeaserMode",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)0)
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
