using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bingo.BackEnd.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ImportOriginalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitorCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitorCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitorCategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ImportNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Name1 = table.Column<string>(type: "TEXT", nullable: false),
                    Name2 = table.Column<string>(type: "TEXT", nullable: false),
                    Name3 = table.Column<string>(type: "TEXT", nullable: false),
                    Name4 = table.Column<string>(type: "TEXT", nullable: false),
                    Nationality = table.Column<string>(type: "TEXT", nullable: false),
                    Team = table.Column<string>(type: "TEXT", nullable: false),
                    Boat = table.Column<string>(type: "TEXT", nullable: false),
                    Engine = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ImportName = table.Column<string>(type: "TEXT", nullable: false),
                    Length = table.Column<decimal>(type: "TEXT", nullable: false),
                    TargetAverageSpeed = table.Column<decimal>(type: "TEXT", nullable: false),
                    MinTimeTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    MaxTimeTicks = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SectorTimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompetitorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SectorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PenaltyTimeTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    PenaltyPositions = table.Column<int>(type: "INTEGER", nullable: false),
                    PenaltyNote = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionSectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SectorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RaceEnabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionSectors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitorCategories");

            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "SectorTimes");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "SessionSectors");
        }
    }
}
