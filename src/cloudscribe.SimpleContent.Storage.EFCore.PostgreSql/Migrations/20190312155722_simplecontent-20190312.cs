using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    public partial class simplecontent20190312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cs_page_comment_cs_page_page_entity_id",
                table: "cs_page_comment");

            migrationBuilder.DropForeignKey(
                name: "fk_cs_page_resource_cs_page_page_entity_id",
                table: "cs_page_resource");

            migrationBuilder.AddForeignKey(
                name: "fk_cs_page_comment_cs_page_page_entity_id",
                table: "cs_page_comment",
                column: "page_entity_id",
                principalTable: "cs_page",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cs_page_resource_cs_page_page_entity_id",
                table: "cs_page_resource",
                column: "page_entity_id",
                principalTable: "cs_page",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cs_page_comment_cs_page_page_entity_id",
                table: "cs_page_comment");

            migrationBuilder.DropForeignKey(
                name: "fk_cs_page_resource_cs_page_page_entity_id",
                table: "cs_page_resource");

            migrationBuilder.AddForeignKey(
                name: "fk_cs_page_comment_cs_page_page_entity_id",
                table: "cs_page_comment",
                column: "page_entity_id",
                principalTable: "cs_page",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_cs_page_resource_cs_page_page_entity_id",
                table: "cs_page_resource",
                column: "page_entity_id",
                principalTable: "cs_page",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
