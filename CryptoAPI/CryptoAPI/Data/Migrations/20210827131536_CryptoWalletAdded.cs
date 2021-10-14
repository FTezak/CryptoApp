using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class CryptoWalletAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptoWallet",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CryptoId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(38,18)", precision: 38, scale: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoWallet", x => new { x.UserId, x.CryptoId });
                    table.ForeignKey(
                        name: "FK_CryptoWallet_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CryptoWallet_CryptoCurrency_CryptoId",
                        column: x => x.CryptoId,
                        principalTable: "CryptoCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryptoWallet_CryptoId",
                table: "CryptoWallet",
                column: "CryptoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoWallet");
        }
    }
}
