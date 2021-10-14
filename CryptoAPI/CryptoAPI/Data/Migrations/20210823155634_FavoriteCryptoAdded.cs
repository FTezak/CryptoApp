using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class FavoriteCryptoAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserCryptoCurrency",
                columns: table => new
                {
                    FavoriteCryptoId = table.Column<int>(type: "int", nullable: false),
                    FavoriteOfUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserCryptoCurrency", x => new { x.FavoriteCryptoId, x.FavoriteOfUserId });
                    table.ForeignKey(
                        name: "FK_AppUserCryptoCurrency_AspNetUsers_FavoriteOfUserId",
                        column: x => x.FavoriteOfUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserCryptoCurrency_CryptoCurrency_FavoriteCryptoId",
                        column: x => x.FavoriteCryptoId,
                        principalTable: "CryptoCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserCryptoCurrency_FavoriteOfUserId",
                table: "AppUserCryptoCurrency",
                column: "FavoriteOfUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserCryptoCurrency");
        }
    }
}
