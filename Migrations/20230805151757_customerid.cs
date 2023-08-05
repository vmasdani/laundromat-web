using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class customerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordItems_LaundryRecords_RecordID",
                table: "RecordItems");

            migrationBuilder.RenameColumn(
                name: "RecordID",
                table: "RecordItems",
                newName: "RecordId");

            migrationBuilder.RenameIndex(
                name: "IX_RecordItems_RecordID",
                table: "RecordItems",
                newName: "IX_RecordItems_RecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordItems_LaundryRecords_RecordId",
                table: "RecordItems",
                column: "RecordId",
                principalTable: "LaundryRecords",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordItems_LaundryRecords_RecordId",
                table: "RecordItems");

            migrationBuilder.RenameColumn(
                name: "RecordId",
                table: "RecordItems",
                newName: "RecordID");

            migrationBuilder.RenameIndex(
                name: "IX_RecordItems_RecordId",
                table: "RecordItems",
                newName: "IX_RecordItems_RecordID");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordItems_LaundryRecords_RecordID",
                table: "RecordItems",
                column: "RecordID",
                principalTable: "LaundryRecords",
                principalColumn: "Id");
        }
    }
}
