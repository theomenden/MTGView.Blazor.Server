using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.EFCore.Migrations
{
    public partial class AddKeywordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keywords",
                schema: "MTG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword_Id", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Type",
                schema: "MTG",
                table: "Keywords",
                column: "Type")
                .Annotation("SqlServer:Include", new[] { "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Keywords",
                schema: "MTG");
        }
    }
}
