using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class NewsletterConfigAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsletterConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    frequency = table.Column<int>(type: "int", nullable: false),
                    walletData = table.Column<bool>(type: "bit", nullable: false),
                    favoriteData = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsletterConfig_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterConfig_userId",
                table: "NewsletterConfig",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsletterConfig");
        }
    }
}
