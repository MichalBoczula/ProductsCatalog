using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterMobilePhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatteryCapacity",
                table: "TB_MobilePhones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BatteryType",
                table: "TB_MobilePhones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CPU",
                table: "TB_MobilePhones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DisplayType",
                table: "TB_MobilePhones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GPU",
                table: "TB_MobilePhones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "TB_MobilePhones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ram",
                table: "TB_MobilePhones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RefreshRateHz",
                table: "TB_MobilePhones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ScreenSizeInches",
                table: "TB_MobilePhones",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "TB_MobilePhones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "TB_MobilePhones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatteryCapacity",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "BatteryType",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "CPU",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "DisplayType",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "GPU",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "RefreshRateHz",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "ScreenSizeInches",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "TB_MobilePhones");
        }
    }
}
