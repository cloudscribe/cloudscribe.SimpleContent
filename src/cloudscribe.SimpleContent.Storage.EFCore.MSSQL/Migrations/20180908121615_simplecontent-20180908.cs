using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class simplecontent20180908 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxFeedItems",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: 1000,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DefaultFeedItems",
                table: "cs_ContentProject",
                nullable: false,
                defaultValue: 20,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxFeedItems",
                table: "cs_ContentProject",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "DefaultFeedItems",
                table: "cs_ContentProject",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 20);
        }
    }
}
