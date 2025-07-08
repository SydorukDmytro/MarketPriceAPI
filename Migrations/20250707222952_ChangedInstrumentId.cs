using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPriceAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedInstrumentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RealTimePriceSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    instrumentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Bid = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ask = table.Column<decimal>(type: "TEXT", nullable: false),
                    Last = table.Column<decimal>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealTimePriceSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealTimePriceSnapshots_instrumentId",
                table: "RealTimePriceSnapshots",
                column: "instrumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealTimePriceSnapshots");
        }
    }
}
