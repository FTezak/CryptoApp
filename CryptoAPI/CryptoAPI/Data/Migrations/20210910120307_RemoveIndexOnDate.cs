using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class RemoveIndexOnDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CryptoCurrencyData_Date",
                table: "CryptoCurrencyData");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoCurrencyData_Date_CryptoCurrencyId",
                table: "CryptoCurrencyData",
                columns: new[] { "Date", "CryptoCurrencyId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CryptoCurrencyData_Date_CryptoCurrencyId",
                table: "CryptoCurrencyData");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoCurrencyData_Date",
                table: "CryptoCurrencyData",
                column: "Date")
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
