using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration
{
    internal sealed class CurrenciesConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable(SqlTableNames.Currencies);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).HasMaxLength(3).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}
