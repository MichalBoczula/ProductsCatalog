using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Categories_History",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Categories_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Currencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Currencies_History",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Currencies_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Products_History",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PriceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Products_History", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TB_Categories",
                columns: new[] { "Id", "ChangedAt", "Code", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("587480bb-c126-4f9b-b531-b0244daa4ba4"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2867), "MOBILE", true, "Mobile" },
                    { new Guid("9656c5c5-8ed9-46e1-a5df-025f5d7885d4"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2870), "PC", true, "Personal Computer" },
                    { new Guid("f5fd7b52-275e-4710-a578-40a522ac139c"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2871), "TABLET", true, "Tablet" }
                });

            migrationBuilder.InsertData(
                table: "TB_Categories_History",
                columns: new[] { "Id", "CategoryId", "ChangedAt", "Code", "IsActive", "Name", "Operation" },
                values: new object[,]
                {
                    { new Guid("24daf035-652f-404a-8fd7-785fae68b341"), new Guid("587480bb-c126-4f9b-b531-b0244daa4ba4"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2867), "MOBILE", true, "Mobile", 0 },
                    { new Guid("b3d060f4-81f4-4d64-b4ac-77b3f39b1e29"), new Guid("9656c5c5-8ed9-46e1-a5df-025f5d7885d4"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2870), "PC", true, "Personal Computer", 0 },
                    { new Guid("fc500d67-46a9-492a-a861-77fcd73f1bfc"), new Guid("f5fd7b52-275e-4710-a578-40a522ac139c"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(2871), "TABLET", true, "Tablet", 0 }
                });

            migrationBuilder.InsertData(
                table: "TB_Currencies",
                columns: new[] { "Id", "ChangedAt", "Code", "Description", "IsActive" },
                values: new object[,]
                {
                    { new Guid("12da255e-6408-4b28-a5b1-84758f889348"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3044), "EUR", "Euro", true },
                    { new Guid("1a017544-890c-4219-891f-cd5549473d4e"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3042), "USD", "US Dollar", true },
                    { new Guid("e73b3ef4-ec2c-4262-81ef-0ac21fbc1ec3"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3043), "PLN", "Polish Złoty", true }
                });

            migrationBuilder.InsertData(
                table: "TB_Currencies_History",
                columns: new[] { "Id", "ChangedAt", "Code", "CurrencyId", "Description", "IsActive", "Operation" },
                values: new object[,]
                {
                    { new Guid("1b0d0b8c-2926-415b-a1b8-1843fc189747"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3042), "USD", new Guid("1a017544-890c-4219-891f-cd5549473d4e"), "US Dollar", true, 0 },
                    { new Guid("58ad897f-178c-45c9-a8f0-3302a265a9aa"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3043), "PLN", new Guid("e73b3ef4-ec2c-4262-81ef-0ac21fbc1ec3"), "Polish Złoty", true, 0 },
                    { new Guid("5bbe9f3d-6299-40b6-a9e6-e851de397563"), new DateTime(2025, 12, 26, 19, 19, 17, 970, DateTimeKind.Utc).AddTicks(3044), "EUR", new Guid("12da255e-6408-4b28-a5b1-84758f889348"), "Euro", true, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_Categories_Code",
                table: "TB_Categories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_Categories_History_CategoryId",
                table: "TB_Categories_History",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Categories_History_ChangedAt",
                table: "TB_Categories_History",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Currencies_Code",
                table: "TB_Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_Currencies_History_ChangedAt",
                table: "TB_Currencies_History",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Currencies_History_CurrencyId",
                table: "TB_Currencies_History",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Products_CategoryId",
                table: "TB_Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Products_History_ChangedAt",
                table: "TB_Products_History",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TB_Products_History_ProductId",
                table: "TB_Products_History",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Categories");

            migrationBuilder.DropTable(
                name: "TB_Categories_History");

            migrationBuilder.DropTable(
                name: "TB_Currencies");

            migrationBuilder.DropTable(
                name: "TB_Currencies_History");

            migrationBuilder.DropTable(
                name: "TB_Products");

            migrationBuilder.DropTable(
                name: "TB_Products_History");
        }
    }
}
