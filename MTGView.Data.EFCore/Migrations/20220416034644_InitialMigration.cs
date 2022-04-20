﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.EFCore.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MTG");

            migrationBuilder.CreateTable(
                name: "legalities",
                schema: "MTG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    format = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legalities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Meta",
                schema: "MTG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    version = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meta", x => x.id);
                });
            
            migrationBuilder.CreateTable(
                name: "rulings",
                schema: "MTG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rulings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sets",
                schema: "MTG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    baseSetSize = table.Column<int>(type: "int", nullable: false),
                    block = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    booster = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isFoilOnly = table.Column<bool>(type: "bit", nullable: false),
                    isForeignOnly = table.Column<bool>(type: "bit", nullable: false),
                    isNonFoilOnly = table.Column<bool>(type: "bit", nullable: false),
                    isOnlineOnly = table.Column<bool>(type: "bit", nullable: false),
                    isPartialPreview = table.Column<bool>(type: "bit", nullable: false),
                    keyruneCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mcmId = table.Column<double>(type: "float", nullable: true),
                    mcmIdExtras = table.Column<decimal>(type: "decimal(5,1)", nullable: true),
                    mcmName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mtgoCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    parentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    releaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sealedProduct = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    tcgplayerGroupId = table.Column<double>(type: "float", nullable: true),
                    totalSetSize = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "MTG",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    index = table.Column<int>(type: "int", nullable: true),
                    artist = table.Column<string>(type: "varchar(390)", unicode: false, maxLength: 390, nullable: true),
                    asciiName = table.Column<string>(type: "varchar(220)", unicode: false, maxLength: 220, nullable: true),
                    availability = table.Column<string>(type: "varchar(170)", unicode: false, maxLength: 170, nullable: true),
                    borderColor = table.Column<string>(type: "varchar(110)", unicode: false, maxLength: 110, nullable: true),
                    cardParts = table.Column<string>(type: "varchar(840)", unicode: false, maxLength: 840, nullable: true),
                    colorIdentity = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: true),
                    colorIndicator = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    colors = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: true),
                    duelDeck = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    edhrecRank = table.Column<short>(type: "smallint", nullable: true),
                    faceFlavorName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    finishes = table.Column<string>(type: "varchar(130)", unicode: false, maxLength: 130, nullable: true),
                    flavorText = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    frameEffects = table.Column<string>(type: "varchar(390)", unicode: false, maxLength: 390, nullable: true),
                    frameVersion = table.Column<string>(type: "varchar(600)", unicode: false, maxLength: 600, nullable: true),
                    hasAlternativeDeckLimit = table.Column<short>(type: "smallint", nullable: true),
                    hasContentWarning = table.Column<short>(type: "smallint", nullable: true),
                    isAlternative = table.Column<short>(type: "smallint", nullable: true),
                    isFullArt = table.Column<short>(type: "smallint", nullable: true),
                    isFunny = table.Column<short>(type: "smallint", nullable: true),
                    isOnlineOnly = table.Column<short>(type: "smallint", nullable: true),
                    isOversized = table.Column<short>(type: "smallint", nullable: true),
                    isPromo = table.Column<short>(type: "smallint", nullable: true),
                    isRebalanced = table.Column<short>(type: "smallint", nullable: true),
                    isReprint = table.Column<short>(type: "smallint", nullable: true),
                    isReserved = table.Column<short>(type: "smallint", nullable: true),
                    isStarter = table.Column<short>(type: "smallint", nullable: true),
                    isStorySpotlight = table.Column<short>(type: "smallint", nullable: true),
                    isTextless = table.Column<short>(type: "smallint", nullable: true),
                    isTimeshifted = table.Column<short>(type: "smallint", nullable: true),
                    keywords = table.Column<string>(type: "varchar(590)", unicode: false, maxLength: 590, nullable: true),
                    layout = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: true),
                    life = table.Column<short>(type: "smallint", nullable: true),
                    loyalty = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    manaCost = table.Column<string>(type: "varchar(330)", unicode: false, maxLength: 330, nullable: true),
                    manaValue = table.Column<short>(type: "smallint", nullable: true),
                    mtgjsonV4Id = table.Column<string>(type: "varchar(390)", unicode: false, maxLength: 390, nullable: true),
                    mtgoFoilId = table.Column<int>(type: "int", nullable: true),
                    mtgoId = table.Column<int>(type: "int", nullable: true),
                    multiverseId = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "varchar(620)", unicode: false, maxLength: 620, nullable: true),
                    number = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    originalPrintings = table.Column<string>(type: "varchar(800)", unicode: false, maxLength: 800, nullable: true),
                    originalReleaseDate = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    otherFaceIds = table.Column<string>(type: "varchar(800)", unicode: false, maxLength: 800, nullable: true),
                    power = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    printings = table.Column<string>(type: "varchar(1700)", unicode: false, maxLength: 1700, nullable: true),
                    promoTypes = table.Column<string>(type: "varchar(220)", unicode: false, maxLength: 220, nullable: true),
                    purchaseUrls = table.Column<string>(type: "varchar(2760)", unicode: false, maxLength: 2760, nullable: true),
                    rarity = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    rebalancedPrintings = table.Column<string>(type: "varchar(390)", unicode: false, maxLength: 390, nullable: true),
                    scryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    scryfallIllustrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    scryfallOracleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    securityStamp = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    setCode = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    side = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    signature = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    subtypes = table.Column<string>(type: "varchar(310)", unicode: false, maxLength: 310, nullable: true),
                    supertypes = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    text = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    toughness = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: true),
                    type = table.Column<string>(type: "varchar(560)", unicode: false, maxLength: 560, nullable: true),
                    types = table.Column<string>(type: "varchar(220)", unicode: false, maxLength: 220, nullable: true),
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    variations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    watermark = table.Column<string>(type: "varchar(130)", unicode: false, maxLength: 130, nullable: true),
                    CollectionCardMappingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_MagicCards_PersonalCardMapping_Id",
                        column: x => x.CollectionCardMappingId,
                        principalTable: "PersonalCardMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalCollection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalCardMappingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalCollection_PersonalCardMapping_PersonalCardMappingId",
                        column: x => x.PersonalCardMappingId,
                        principalTable: "PersonalCardMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Legalities_status",
                schema: "MTG",
                table: "legalities",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Legalities_Uuid",
                schema: "MTG",
                table: "legalities",
                column: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalCollection_PersonalCardMappingId",
                table: "PersonalCollection",
                column: "PersonalCardMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_Rulings_Index",
                schema: "MTG",
                table: "rulings",
                column: "index");

            migrationBuilder.CreateIndex(
                name: "IX_Rulings_Uuid",
                schema: "MTG",
                table: "rulings",
                column: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Mtg_Sets_Code",
                schema: "MTG",
                table: "sets",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_Mtg_Sets_Type",
                schema: "MTG",
                table: "sets",
                column: "type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards",
                schema: "MTG");

            migrationBuilder.DropTable(
                name: "legalities",
                schema: "MTG");

            migrationBuilder.DropTable(
                name: "Meta",
                schema: "MTG");

            migrationBuilder.DropTable(
                name: "PersonalCollection");

            migrationBuilder.DropTable(
                name: "rulings",
                schema: "MTG");

            migrationBuilder.DropTable(
                name: "sets",
                schema: "MTG");

            migrationBuilder.DropTable(
                name: "PersonalCardMapping");
        }
    }
}
