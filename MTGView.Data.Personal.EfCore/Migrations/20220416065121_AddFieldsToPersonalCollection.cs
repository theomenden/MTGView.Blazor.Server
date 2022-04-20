using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTGView.Data.Personal.EfCore.Migrations
{
    public partial class AddFieldsToPersonalCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Collections",
                schema: "Personal",
                table: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_Collections_PersonalCardMappingId",
                schema: "Personal",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "PersonalCardMappingId",
                schema: "Personal",
                table: "Collections");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Personal",
                table: "Collections",
                type: "datetime2(7)",
                precision: 7,
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Personal",
                table: "Collections",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collections",
                schema: "Personal",
                table: "Collections",
                column: "Id")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Collections",
                schema: "Personal",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Personal",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Personal",
                table: "Collections");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonalCardMappingId",
                schema: "Personal",
                table: "Collections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collections",
                schema: "Personal",
                table: "Collections",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_PersonalCardMappingId",
                schema: "Personal",
                table: "Collections",
                column: "PersonalCardMappingId");
        }
    }
}
