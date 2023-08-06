using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class recorditem_storeitemid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "RecordItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "RecordItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecordItems_ItemId",
                table: "RecordItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordItems_StoreId",
                table: "RecordItems",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordItems_Items_ItemId",
                table: "RecordItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordItems_Stores_StoreId",
                table: "RecordItems",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordItems_Items_ItemId",
                table: "RecordItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RecordItems_Stores_StoreId",
                table: "RecordItems");

            migrationBuilder.DropIndex(
                name: "IX_RecordItems_ItemId",
                table: "RecordItems");

            migrationBuilder.DropIndex(
                name: "IX_RecordItems_StoreId",
                table: "RecordItems");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "RecordItems");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "RecordItems");
        }
    }
}
