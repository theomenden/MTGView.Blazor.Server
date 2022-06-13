using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.EFCore.Migrations
{
    public partial class UpdateMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "manaValue",
                schema: "MTG",
                table: "Cards",
                type: "decimal(9,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "manaValue",
                schema: "MTG",
                table: "Cards",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldNullable: true);
        }
    }
}
