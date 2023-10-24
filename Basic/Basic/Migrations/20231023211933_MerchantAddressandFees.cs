using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basic.Migrations
{
    public partial class MerchantAddressandFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FeeName",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "FeePercent",
                table: "Merchants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TaxRate",
                table: "Merchants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Merchants",
                type: "nvarchar(255)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "FeeName",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "FeePercent",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Merchants");
        }
    }
}
