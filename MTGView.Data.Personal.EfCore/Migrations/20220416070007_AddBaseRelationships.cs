using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.Personal.EfCore.Migrations
{
    public partial class AddBaseRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Mappings",
                schema: "Personal",
                table: "Mappings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mappings",
                schema: "Personal",
                table: "Mappings",
                column: "Id")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mappings_Card_Id",
                schema: "Personal",
                table: "Mappings",
                column: "CardId",
                principalSchema: "Personal",
                principalTable: "Cards",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mappings_Collection_Id",
                schema: "Personal",
                table: "Mappings",
                column: "PersonalCollectionId",
                principalSchema: "Personal",
                principalTable: "Collections",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mappings_Card_Id",
                schema: "Personal",
                table: "Mappings");

            migrationBuilder.DropForeignKey(
                name: "FK_Mappings_Collection_Id",
                schema: "Personal",
                table: "Mappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mappings",
                schema: "Personal",
                table: "Mappings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mappings",
                schema: "Personal",
                table: "Mappings",
                column: "Id");
        }
    }
}
