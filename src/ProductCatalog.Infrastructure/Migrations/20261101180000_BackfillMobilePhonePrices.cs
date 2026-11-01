using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductCatalog.Infrastructure.Contexts.Commands;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    [DbContext(typeof(ProductsContext))]
    [Migration("20261101180000_BackfillMobilePhonePrices")]
    public sealed class BackfillMobilePhonePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string resourceName = "ProductCatalog.Infrastructure.Migrations.Scripts.20261101180000_BackfillMobilePhonePrices.sql";
            using var stream = typeof(BackfillMobilePhonePrices).Assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Missing embedded migration script: {resourceName}");
            using var reader = new StreamReader(stream);
            migrationBuilder.Sql(reader.ReadToEnd());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotSupportedException(
                "The seed price correction cannot be reversed automatically. Restore reviewed data from a backup or apply a forward corrective migration; do not reset prices or delete history.");
        }
    }
}
