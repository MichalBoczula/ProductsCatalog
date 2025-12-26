using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration.Histories
{
    internal sealed class CategoriesHistoryConfiguration : IEntityTypeConfiguration<CategoriesHistory>
    {
        public void Configure(EntityTypeBuilder<CategoriesHistory> builder)
        {
            builder.ToTable(SqlTableNames.CategoriesHistory);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ChangedAt).IsRequired();
            builder.Property(x => x.Operation).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => x.ChangedAt);
        }
    }
}
