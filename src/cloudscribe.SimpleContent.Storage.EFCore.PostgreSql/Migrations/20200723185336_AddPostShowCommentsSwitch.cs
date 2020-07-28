using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    public partial class AddPostShowCommentsSwitch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cs_page_comment_cs_page_page_entity_id",
                table: "cs_page_comment");

            migrationBuilder.DropForeignKey(
                name: "fk_cs_post_comment_cs_post_post_entity_id",
                table: "cs_post_comment");

            migrationBuilder.AddColumn<bool>(
                name: "show_comments",
                table: "cs_post",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "fk_cs_page_comment_cs__page_page_entity_id",
                table: "cs_page_comment",
                column: "page_entity_id",
                principalTable: "cs_page",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cs_post_comment_cs__post_post_entity_id",
                table: "cs_post_comment",
                column: "post_entity_id",
                principalTable: "cs_post",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cs_page_comment_cs__page_page_entity_id",
                table: "cs_page_comment");

            migrationBuilder.DropForeignKey(
                name: "fk_cs_post_comment_cs__post_post_entity_id",
                table: "cs_post_comment");

            migrationBuilder.DropColumn(
                name: "show_comments",
                table: "cs_post");

            migrationBuilder.AddForeignKey(
                name: "fk_cs_page_comment_cs_page_page_entity_id",
                table: "cs_page_comment",
                column: "page_entity_id",
                principalTable: "cs_page",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_cs_post_comment_cs_post_post_entity_id",
                table: "cs_post_comment",
                column: "post_entity_id",
                principalTable: "cs_post",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
