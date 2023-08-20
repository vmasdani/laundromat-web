using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class extraservices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecordExtraservices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RecordId = table.Column<int>(type: "INTEGER", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordExtraservices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordExtraservices_LaundryRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "LaundryRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecordExtraservices_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecordExtraservices_CreatedById",
                table: "RecordExtraservices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RecordExtraservices_RecordId",
                table: "RecordExtraservices",
                column: "RecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordExtraservices");
        }
    }
}
