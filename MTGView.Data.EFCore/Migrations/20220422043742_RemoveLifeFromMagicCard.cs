using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.EFCore.Migrations
{
    public partial class RemoveLifeFromMagicCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "life",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.AlterColumn<decimal>(
                name: "manaValue",
                schema: "MTG",
                table: "Cards",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "manaValue",
                schema: "MTG",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "life",
                schema: "MTG",
                table: "Cards",
                type: "int",
                nullable: true);
        }
    }
}
