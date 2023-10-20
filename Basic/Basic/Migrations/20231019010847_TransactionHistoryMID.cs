using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.Migrations
{
    public partial class TransactionHistoryMID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MerchantId",
                table: "TransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "TransactionHistories");
        }
    }
}
