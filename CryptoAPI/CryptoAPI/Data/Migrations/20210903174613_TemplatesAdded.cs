using Microsoft.EntityFrameworkCore.Migrations;

namespace CryptoAPI.Data.Migrations
{
    public partial class TemplatesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MailTemplate = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    FavoriteDataTemplate = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    FavoriteTemplate = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    WalletDataTemplate = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    WalletTemplate = table.Column<string>(type: "varchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
