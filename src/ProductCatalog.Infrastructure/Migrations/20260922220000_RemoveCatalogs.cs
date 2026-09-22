using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductCatalog.Infrastructure.Contexts.Commands;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    [DbContext(typeof(ProductsContext))]
    [Migration("20260922220000_RemoveCatalogs")]
    public sealed class RemoveCatalogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TB_Categories_History");
            migrationBuilder.DropTable(name: "TB_Currencies_History");
            migrationBuilder.DropTable(name: "TB_Categories");
            migrationBuilder.DropTable(name: "TB_Currencies");

            migrationBuilder.DropIndex(name: "IX_TB_MobilePhones_CategoryId", table: "TB_MobilePhones");
            migrationBuilder.DropColumn(name: "CategoryId", table: "TB_MobilePhones");
            migrationBuilder.DropColumn(name: "CategoryId", table: "TB_MobilePhones_History");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(name: "CategoryId", table: "TB_MobilePhones", type: "uniqueidentifier", nullable: false, defaultValue: Guid.Empty);
            migrationBuilder.AddColumn<Guid>(name: "CategoryId", table: "TB_MobilePhones_History", type: "uniqueidentifier", nullable: false, defaultValue: Guid.Empty);

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
                constraints: table => table.PrimaryKey("PK_TB_Categories", x => x.Id));

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
                constraints: table => table.PrimaryKey("PK_TB_Categories_History", x => x.Id));

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
                constraints: table => table.PrimaryKey("PK_TB_Currencies", x => x.Id));

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
                constraints: table => table.PrimaryKey("PK_TB_Currencies_History", x => x.Id));

            migrationBuilder.CreateIndex(name: "IX_TB_Categories_Code", table: "TB_Categories", column: "Code", unique: true);
            migrationBuilder.CreateIndex(name: "IX_TB_Categories_History_CategoryId", table: "TB_Categories_History", column: "CategoryId");
            migrationBuilder.CreateIndex(name: "IX_TB_Categories_History_ChangedAt", table: "TB_Categories_History", column: "ChangedAt");
            migrationBuilder.CreateIndex(name: "IX_TB_Currencies_Code", table: "TB_Currencies", column: "Code", unique: true);
            migrationBuilder.CreateIndex(name: "IX_TB_Currencies_History_CurrencyId", table: "TB_Currencies_History", column: "CurrencyId");
            migrationBuilder.CreateIndex(name: "IX_TB_Currencies_History_ChangedAt", table: "TB_Currencies_History", column: "ChangedAt");
            migrationBuilder.CreateIndex(name: "IX_TB_MobilePhones_CategoryId", table: "TB_MobilePhones", column: "CategoryId");
        }
    }
}
