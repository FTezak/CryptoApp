using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class CryptoCurrencyDecimalPrecisionChangedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Open",
                table: "CryptoCurrencyData",
                type: "decimal(38,18)",
                precision: 38,
                scale: 18,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Close",
                table: "CryptoCurrencyData",
                type: "decimal(38,18)",
                precision: 38,
                scale: 18,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "AllTimeHigh",
                table: "CryptoCurrency",
                type: "decimal(38,18)",
                precision: 38,
                scale: 18,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Open",
                table: "CryptoCurrencyData",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)",
                oldPrecision: 38,
                oldScale: 18);

            migrationBuilder.AlterColumn<decimal>(
                name: "Close",
                table: "CryptoCurrencyData",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)",
                oldPrecision: 38,
                oldScale: 18);

            migrationBuilder.AlterColumn<decimal>(
                name: "AllTimeHigh",
                table: "CryptoCurrency",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)",
                oldPrecision: 38,
                oldScale: 18);
        }
    }
}
