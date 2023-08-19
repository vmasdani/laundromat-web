using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class defaultstoreadminconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultStoreId",
                table: "AdminConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminConfigs_DefaultStoreId",
                table: "AdminConfigs",
                column: "DefaultStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminConfigs_Stores_DefaultStoreId",
                table: "AdminConfigs",
                column: "DefaultStoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminConfigs_Stores_DefaultStoreId",
                table: "AdminConfigs");

            migrationBuilder.DropIndex(
                name: "IX_AdminConfigs_DefaultStoreId",
                table: "AdminConfigs");

            migrationBuilder.DropColumn(
                name: "DefaultStoreId",
                table: "AdminConfigs");
        }
    }
}
