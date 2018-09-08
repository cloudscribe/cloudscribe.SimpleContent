using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite.Migrations
{
    public partial class simplecontent20180908 : Migration
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

            //migrationBuilder.AlterColumn<int>(
            //    name: "MaxFeedItems",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: 1000,
            //    oldClrType: typeof(int))
            //    .Annotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "DefaultFeedItems",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    defaultValue: 20,
            //    oldClrType: typeof(int))
            //    .Annotation("Sqlite:Autoincrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            //migrationBuilder.AlterColumn<int>(
            //    name: "MaxFeedItems",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldDefaultValue: 1000)
            //    .OldAnnotation("Sqlite:Autoincrement", true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "DefaultFeedItems",
            //    table: "cs_ContentProject",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldDefaultValue: 20)
            //    .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
