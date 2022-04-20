using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.Personal.EfCore.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Personal");

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "Personal",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(620)", unicode: false, maxLength: 620, nullable: false),
                    setCode = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                schema: "Personal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    PersonalCardMappingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mappings",
                schema: "Personal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newsequentialid())"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    PersonalCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mappings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collections_PersonalCardMappingId",
                schema: "Personal",
                table: "Collections",
                column: "PersonalCardMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCardMapping_CardId",
                schema: "Personal",
                table: "Mappings",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCardMapping_PersonalCollectionId",
                schema: "Personal",
                table: "Mappings",
                column: "PersonalCollectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards",
                schema: "Personal");

            migrationBuilder.DropTable(
                name: "Collections",
                schema: "Personal");

            migrationBuilder.DropTable(
                name: "Mappings",
                schema: "Personal");
        }
    }
}
