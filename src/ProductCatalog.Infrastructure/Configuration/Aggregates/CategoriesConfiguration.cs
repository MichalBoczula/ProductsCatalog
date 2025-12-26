using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration.Aggregates
{
    internal sealed class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(SqlTableNames.Categories);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(200).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.HasIndex(x => x.Code).IsUnique();
        }
    }
}