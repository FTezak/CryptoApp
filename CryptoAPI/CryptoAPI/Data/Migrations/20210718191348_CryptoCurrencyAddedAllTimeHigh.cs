using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class CryptoCurrencyAddedAllTimeHigh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AllTimeHigh",
                table: "CryptoCurrency",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllTimeHigh",
                table: "CryptoCurrency");
        }
    }
}
