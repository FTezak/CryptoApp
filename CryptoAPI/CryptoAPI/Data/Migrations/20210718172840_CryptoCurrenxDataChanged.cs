using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class CryptoCurrenxDataChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CryptoCurrencyData",
                newName: "Open");

            migrationBuilder.AddColumn<decimal>(
                name: "Close",
                table: "CryptoCurrencyData",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Close",
                table: "CryptoCurrencyData");

            migrationBuilder.RenameColumn(
                name: "Open",
                table: "CryptoCurrencyData",
                newName: "Price");
        }
    }
}
