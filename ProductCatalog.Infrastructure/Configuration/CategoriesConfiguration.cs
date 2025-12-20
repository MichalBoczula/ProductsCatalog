using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;

namespace ProductCatalog.Infrastructure.Configuration
{
    internal class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("TB_Categories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            
            builder.Property(x => x.Code).HasMaxLength(200).IsRequired();
            builder.HasIndex(x => x.Code);

        }
    }
}