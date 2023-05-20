using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.BackEnd.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class MasterDataUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Competitors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Boat = table.Column<string>(type: "TEXT", nullable: false),
                    CompetitorCategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreationTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Engine = table.Column<string>(type: "TEXT", nullable: false),
                    ImportNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LastUpdateTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name1 = table.Column<string>(type: "TEXT", nullable: false),
                    Name2 = table.Column<string>(type: "TEXT", nullable: false),
                    Name3 = table.Column<string>(type: "TEXT", nullable: false),
                    Name4 = table.Column<string>(type: "TEXT", nullable: false),
                    Nationality = table.Column<string>(type: "TEXT", nullable: false),
                    Number = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Team = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.Id);
                });
        }
    }
}
