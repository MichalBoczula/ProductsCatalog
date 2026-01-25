using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterMobilePhoneTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "TB_MobilePhones_History",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "TB_MobilePhones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "TB_MobilePhones_History");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "TB_MobilePhones");
        }
    }
}
