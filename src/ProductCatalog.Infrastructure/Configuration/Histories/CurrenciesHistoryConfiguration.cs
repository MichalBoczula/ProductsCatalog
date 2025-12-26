using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration.Histories
{
    internal class CurrenciesHistoryConfiguration : IEntityTypeConfiguration<CurrenciesHistory>
    {
        public void Configure(EntityTypeBuilder<CurrenciesHistory> builder)
        {
            builder.ToTable(SqlTableNames.CurrenciesHistory);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CurrencyId).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ChangedAt).IsRequired();
            builder.Property(x => x.Operation).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasIndex(x => x.CurrencyId);
            builder.HasIndex(x => x.ChangedAt);
        }
    }
}