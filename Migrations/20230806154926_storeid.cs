using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class storeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "LaundryRecords",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaundryRecords_StoreId",
                table: "LaundryRecords",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaundryRecords_Customers_StoreId",
                table: "LaundryRecords",
                column: "StoreId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaundryRecords_Customers_StoreId",
                table: "LaundryRecords");

            migrationBuilder.DropIndex(
                name: "IX_LaundryRecords_StoreId",
                table: "LaundryRecords");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "LaundryRecords");
        }
    }
}
