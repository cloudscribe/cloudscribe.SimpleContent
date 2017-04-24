using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL.Migrations
{
    public partial class changes20170417 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cs_PageResource",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Environment = table.Column<string>(maxLength: 15, nullable: false),
                    PageEntityId = table.Column<string>(maxLength: 36, nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 10, nullable: false),
                    Url = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cs_PageResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cs_PageResource_cs_Page_PageEntityId",
                        column: x => x.PageEntityId,
                        principalTable: "cs_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cs_PageResource_PageEntityId",
                table: "cs_PageResource",
                column: "PageEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_PageResource");
        }
    }
}
