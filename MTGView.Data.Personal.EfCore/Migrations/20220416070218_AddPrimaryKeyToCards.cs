using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.Personal.EfCore.Migrations
{
    public partial class AddPrimaryKeyToCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                schema: "Personal",
                table: "Cards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                schema: "Personal",
                table: "Cards",
                column: "id")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SetCode",
                schema: "Personal",
                table: "Cards",
                column: "setCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                schema: "Personal",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_SetCode",
                schema: "Personal",
                table: "Cards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                schema: "Personal",
                table: "Cards",
                column: "id");
        }
    }
}
