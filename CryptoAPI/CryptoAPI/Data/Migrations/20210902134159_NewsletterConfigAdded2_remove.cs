using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class NewsletterConfigAdded2_remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_NewsletterConfig_NewsletterId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_NewsletterConfig_userId",
                table: "NewsletterConfig");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NewsletterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NewsletterId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterConfig_userId",
                table: "NewsletterConfig",
                column: "userId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewsletterConfig_userId",
                table: "NewsletterConfig");

            migrationBuilder.AddColumn<int>(
                name: "NewsletterId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsletterConfig_userId",
                table: "NewsletterConfig",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NewsletterId",
                table: "AspNetUsers",
                column: "NewsletterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_NewsletterConfig_NewsletterId",
                table: "AspNetUsers",
                column: "NewsletterId",
                principalTable: "NewsletterConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
