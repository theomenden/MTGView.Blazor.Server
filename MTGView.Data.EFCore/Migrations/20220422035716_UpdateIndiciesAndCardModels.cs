using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.EFCore.Migrations
{
    public partial class UpdateIndiciesAndCardModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MagicCards_PersonalCardMapping_Id",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalCollection_PersonalCardMapping_PersonalCardMappingId",
                table: "PersonalCollection");

            migrationBuilder.DropIndex(
                name: "IX_PersonalCollection_PersonalCardMappingId",
                table: "PersonalCollection");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CollectionCardMappingId",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UUID",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "PersonalCardMappingId",
                table: "PersonalCollection");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PersonalCollection",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PersonalCollection",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "manaValue",
                schema: "MTG",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "life",
                schema: "MTG",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isTimeshifted",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isTextless",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isStorySpotlight",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isStarter",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isReserved",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isReprint",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isRebalanced",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isPromo",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isOversized",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isOnlineOnly",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isFunny",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isFullArt",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isAlternative",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "hasContentWarning",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "hasAlternativeDeckLimit",
                schema: "MTG",
                table: "Cards",
                type: "bit",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "edhrecRank",
                schema: "MTG",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CollectionCardMappingId",
                schema: "MTG",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCardMapping_CardId",
                table: "PersonalCardMapping",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCardMapping_PersonalCollectionId",
                table: "PersonalCardMapping",
                column: "PersonalCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CollectionCardMapping_Id",
                schema: "MTG",
                table: "Cards",
                column: "CollectionCardMappingId")
                .Annotation("SqlServer:Include", new[] { "name", "manaCost", "setCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SetCode",
                schema: "MTG",
                table: "Cards",
                column: "setCode")
                .Annotation("SqlServer:Include", new[] { "colorIdentity", "scryfallId", "manaCost" });

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_PersonalCardMapping_CollectionCardMappingId",
                schema: "MTG",
                table: "Cards",
                column: "CollectionCardMappingId",
                principalTable: "PersonalCardMapping",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalCardMapping_PersonalCard_CardId",
                table: "PersonalCardMapping",
                column: "CardId",
                principalTable: "PersonalCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalCardMapping_PersonalCollection_PersonalCollectionId",
                table: "PersonalCardMapping",
                column: "PersonalCollectionId",
                principalTable: "PersonalCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_PersonalCardMapping_CollectionCardMappingId",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalCardMapping_PersonalCard_CardId",
                table: "PersonalCardMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalCardMapping_PersonalCollection_PersonalCollectionId",
                table: "PersonalCardMapping");

            migrationBuilder.DropTable(
                name: "PersonalCard");

            migrationBuilder.DropIndex(
                name: "IX_PersonalCardMapping_CardId",
                table: "PersonalCardMapping");

            migrationBuilder.DropIndex(
                name: "IX_PersonalCardMapping_PersonalCollectionId",
                table: "PersonalCardMapping");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CollectionCardMapping_Id",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_SetCode",
                schema: "MTG",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PersonalCollection");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PersonalCollection");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonalCardMappingId",
                table: "PersonalCollection",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<short>(
                name: "manaValue",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "life",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isTimeshifted",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isTextless",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isStorySpotlight",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isStarter",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isReserved",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isReprint",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isRebalanced",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isPromo",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isOversized",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isOnlineOnly",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isFunny",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isFullArt",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "isAlternative",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "hasContentWarning",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "hasAlternativeDeckLimit",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "edhrecRank",
                schema: "MTG",
                table: "Cards",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CollectionCardMappingId",
                schema: "MTG",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCollection_PersonalCardMappingId",
                table: "PersonalCollection",
                column: "PersonalCardMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CollectionCardMappingId",
                schema: "MTG",
                table: "Cards",
                column: "CollectionCardMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UUID",
                schema: "MTG",
                table: "Cards",
                column: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_MagicCards_PersonalCardMapping_Id",
                schema: "MTG",
                table: "Cards",
                column: "CollectionCardMappingId",
                principalTable: "PersonalCardMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalCollection_PersonalCardMapping_PersonalCardMappingId",
                table: "PersonalCollection",
                column: "PersonalCardMappingId",
                principalTable: "PersonalCardMapping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
