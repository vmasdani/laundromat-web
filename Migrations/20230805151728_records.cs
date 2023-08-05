using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class records : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePerWeight",
                table: "Stores",
                type: "REAL",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LaundryRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: true),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    Weight = table.Column<double>(type: "REAL", nullable: true),
                    PriceSnapshot = table.Column<double>(type: "REAL", nullable: true),
                    IsDiscount = table.Column<bool>(type: "INTEGER", nullable: true),
                    DiscountPrice = table.Column<double>(type: "REAL", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaundryRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaundryRecords_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LaundryRecords_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecordItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RecordID = table.Column<int>(type: "INTEGER", nullable: true),
                    Qty = table.Column<double>(type: "REAL", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordItems_LaundryRecords_RecordID",
                        column: x => x.RecordID,
                        principalTable: "LaundryRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecordItems_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaundryRecords_CreatedById",
                table: "LaundryRecords",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LaundryRecords_CustomerId",
                table: "LaundryRecords",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordItems_CreatedById",
                table: "RecordItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RecordItems_RecordID",
                table: "RecordItems",
                column: "RecordID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordItems");

            migrationBuilder.DropTable(
                name: "LaundryRecords");

            migrationBuilder.DropColumn(
                name: "PricePerWeight",
                table: "Stores");
        }
    }
}
