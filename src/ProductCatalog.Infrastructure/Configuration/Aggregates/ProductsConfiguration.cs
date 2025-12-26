using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration.Aggregates
{
    internal sealed class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(SqlTableNames.Products);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            builder.ComplexProperty(x => x.Price, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("PriceAmount")
                     .HasPrecision(18, 2)
                     .IsRequired();

                money.Property(m => m.Currency)
                     .HasColumnName("PriceCurrency")
                     .HasMaxLength(3)
                     .IsRequired();
            });
            builder.HasIndex(x => x.CategoryId);
        }
    }
}
