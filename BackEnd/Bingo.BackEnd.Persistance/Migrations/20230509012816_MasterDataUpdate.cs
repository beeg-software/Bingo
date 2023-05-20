using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.BackEnd.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class MasterDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Users",
                newName: "LastUpdateTimeStamp");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTimeStamp",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTimeStamp",
                table: "Competitors",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTimeStamp",
                table: "Competitors",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTimeStamp",
                table: "CompetitorCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTimeStamp",
                table: "CompetitorCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTimeStamp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreationTimeStamp",
                table: "Competitors");

            migrationBuilder.DropColumn(
                name: "LastUpdateTimeStamp",
                table: "Competitors");

            migrationBuilder.DropColumn(
                name: "CreationTimeStamp",
                table: "CompetitorCategories");

            migrationBuilder.DropColumn(
                name: "LastUpdateTimeStamp",
                table: "CompetitorCategories");

            migrationBuilder.RenameColumn(
                name: "LastUpdateTimeStamp",
                table: "Users",
                newName: "TimeStamp");
        }
    }
}
