using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterMobilePhone2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "5G",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Bluetooth",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NFC",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WiFi",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "5G",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Bluetooth",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "NFC",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "WiFi",
                table: "TB_MobilePhones");
        }
    }
}
