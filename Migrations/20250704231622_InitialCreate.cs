using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPriceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Kind = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TickSize = table.Column<double>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    BaseCurrency = table.Column<string>(type: "TEXT", nullable: false),
                    Mappings_Simulation_Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Mappings_Simulation_Exchange = table.Column<string>(type: "TEXT", nullable: false),
                    Mappings_Simulation_DefaultOrderSize = table.Column<int>(type: "INTEGER", nullable: false),
                    Mappings_Simulation_MaxOrderSize = table.Column<int>(type: "INTEGER", nullable: true),
                    Mappings_Simulation_TradingHours_RegularStart = table.Column<string>(type: "TEXT", nullable: true),
                    Mappings_Simulation_TradingHours_RegularEnd = table.Column<string>(type: "TEXT", nullable: true),
                    Mappings_Simulation_TradingHours_ElectronicStart = table.Column<string>(type: "TEXT", nullable: true),
                    Mappings_Simulation_TradingHours_ElectronicEnd = table.Column<string>(type: "TEXT", nullable: true),
                    Mappings_Oanda_Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Mappings_Oanda_Exchange = table.Column<string>(type: "TEXT", nullable: false),
                    Mappings_Oanda_DefaultOrderSize = table.Column<int>(type: "INTEGER", nullable: false),
                    Mappings_Oanda_MaxOrderSize = table.Column<int>(type: "INTEGER", nullable: true),
                    Mappings_Oanda_TradingHours_RegularStart = table.Column<string>(type: "TEXT", nullable: true),
                    Mappings_Oanda_TradingHours_RegularEnd = table.Column<string>(type: "TEXT", nullable: true),
                    Mappings_Oanda_TradingHours_ElectronicStart = table.Column<string>(type: "TEXT", nullable: true),
                    Mappings_Oanda_TradingHours_ElectronicEnd = table.Column<string>(type: "TEXT", nullable: true),
                    Profile_Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");
        }
    }
}
