using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMobilePhoneHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_MobilePhones_History",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MobilePhoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MainPhoto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OtherPhotos = table.Column<string>(type: "nvarchar(4000)", nullable: false),
                    CPU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GPU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ram = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Storage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RefreshRateHz = table.Column<int>(type: "int", nullable: false),
                    ScreenSizeInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    BatteryType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BatteryCapacity = table.Column<int>(type: "int", nullable: false),
                    GPS = table.Column<bool>(type: "bit", nullable: false),
                    AGPS = table.Column<bool>(type: "bit", nullable: false),
                    Galileo = table.Column<bool>(type: "bit", nullable: false),
                    GLONASS = table.Column<bool>(type: "bit", nullable: false),
                    QZSS = table.Column<bool>(type: "bit", nullable: false),
                    Accelerometer = table.Column<bool>(type: "bit", nullable: false),
                    Gyroscope = table.Column<bool>(type: "bit", nullable: false),
                    Proximity = table.Column<bool>(type: "bit", nullable: false),
                    Compass = table.Column<bool>(type: "bit", nullable: false),
                    Barometer = table.Column<bool>(type: "bit", nullable: false),
                    Halla = table.Column<bool>(type: "bit", nullable: false),
                    AmbientLight = table.Column<bool>(type: "bit", nullable: false),
                    Has5G = table.Column<bool>(type: "bit", nullable: false),
                    WiFi = table.Column<bool>(type: "bit", nullable: false),
                    NFC = table.Column<bool>(type: "bit", nullable: false),
                    Bluetooth = table.Column<bool>(type: "bit", nullable: false),
                    FingerPrint = table.Column<bool>(type: "bit", nullable: false),
                    FaceId = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MobilePhones_History", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_MobilePhones_History_ChangedAt",
                table: "TB_MobilePhones_History",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MobilePhones_History_MobilePhoneId",
                table: "TB_MobilePhones_History",
                column: "MobilePhoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_MobilePhones_History");
        }
    }
}
