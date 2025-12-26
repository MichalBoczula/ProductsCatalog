using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration.Histories
{
    internal sealed class ProductsHistoryConfiguration : IEntityTypeConfiguration<ProductsHistory>
    {
        public void Configure(EntityTypeBuilder<ProductsHistory> builder)
        {
            builder.ToTable(SqlTableNames.ProductsHistory);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.PriceAmount).IsRequired();
            builder.Property(x => x.PriceCurrency).IsRequired();
            builder.Property(x => x.ChangedAt).IsRequired();
            builder.Property(x => x.Operation).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.ChangedAt);
        }
    }
}
