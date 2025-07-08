using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPriceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsPresentToMapDet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Mappings_Oanda_IsPresent",
                table: "Assets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Mappings_Simulation_IsPresent",
                table: "Assets",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mappings_Oanda_IsPresent",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Mappings_Simulation_IsPresent",
                table: "Assets");
        }
    }
}
