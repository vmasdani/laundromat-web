using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class inventorysummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Inventory",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "Inventory",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Qty",
                table: "Inventory",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Inventory",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Inventory",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ItemId",
                table: "Inventory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StoreId",
                table: "Inventory",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Items_ItemId",
                table: "Inventory",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Stores_StoreId",
                table: "Inventory",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Items_ItemId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Stores_StoreId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_ItemId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_StoreId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Mode",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Inventory");
        }
    }
}
