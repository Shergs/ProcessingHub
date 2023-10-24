using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.Migrations
{
    public partial class ReciptEmails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppPassword",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptEmail",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipitentEmail",
                table: "CardPayments",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppPassword",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "ReceiptEmail",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "RecipitentEmail",
                table: "CardPayments");
        }
    }
}
