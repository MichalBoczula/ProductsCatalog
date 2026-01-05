using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterMobilePhone3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AGPS",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Accelerometer",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AmbientLight",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Barometer",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Compass",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GLONASS",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GPS",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Galileo",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Gyroscope",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Halla",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Proximity",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QZSS",
                table: "TB_MobilePhones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AGPS",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Accelerometer",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "AmbientLight",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Barometer",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Compass",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "GLONASS",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "GPS",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Galileo",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Gyroscope",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Halla",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "Proximity",
                table: "TB_MobilePhones");

            migrationBuilder.DropColumn(
                name: "QZSS",
                table: "TB_MobilePhones");
        }
    }
}
