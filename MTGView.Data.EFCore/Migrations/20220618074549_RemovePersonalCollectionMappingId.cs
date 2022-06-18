using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.EFCore.Migrations
{
    public partial class RemovePersonalCollectionMappingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_PersonalCardMapping_CollectionCardMappingId",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "PersonalCardMapping");

            migrationBuilder.DropTable(
                name: "PersonalCard");

            migrationBuilder.DropTable(
                name: "PersonalCollection");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CollectionCardMapping_Id",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CollectionCardMappingId",
                schema: "MTG",
                table: "Cards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CollectionCardMappingId",
                schema: "MTG",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersonalCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalCollection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCollection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalCardMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    PersonalCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCardMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalCardMapping_PersonalCard_CardId",
                        column: x => x.CardId,
                        principalTable: "PersonalCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalCardMapping_PersonalCollection_PersonalCollectionId",
                        column: x => x.PersonalCollectionId,
                        principalTable: "PersonalCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CollectionCardMapping_Id",
                schema: "MTG",
                table: "Cards",
                column: "CollectionCardMappingId")
                .Annotation("SqlServer:Include", new[] { "name", "manaCost", "setCode" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCardMapping_CardId",
                table: "PersonalCardMapping",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCardMapping_PersonalCollectionId",
                table: "PersonalCardMapping",
                column: "PersonalCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_PersonalCardMapping_CollectionCardMappingId",
                schema: "MTG",
                table: "Cards",
                column: "CollectionCardMappingId",
                principalTable: "PersonalCardMapping",
                principalColumn: "Id");
        }
    }
}
