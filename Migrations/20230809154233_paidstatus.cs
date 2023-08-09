using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laundromatweb.Migrations
{
    public partial class paidstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "LaundryRecords",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "LaundryRecords");
        }
    }
}
