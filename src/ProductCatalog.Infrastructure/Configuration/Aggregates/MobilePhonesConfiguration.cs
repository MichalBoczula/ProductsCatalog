using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration.Aggregates
{
    internal class MobilePhonesConfiguration : IEntityTypeConfiguration<MobilePhone>
    {
        public void Configure(EntityTypeBuilder<MobilePhone> builder)
        {
            builder.ToTable(SqlTableNames.MobilePhones);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FingerPrint).IsRequired();
            builder.Property(x => x.FaceId).IsRequired();
            builder.ComplexProperty(x => x.CommonDescription, desc =>
            {
                desc.Property(p => p.Name).HasColumnName("Name").HasMaxLength(200).IsRequired();
                desc.Property(p => p.Description).HasColumnName("Description").HasMaxLength(200).IsRequired();
                desc.Property(p => p.MainPhoto).HasColumnName("MainPhoto").HasMaxLength(200).IsRequired();
                desc.Property(p => p.OtherPhotos).HasColumnName("OtherPhotos").HasColumnType("nvarchar(4000)").IsRequired();
            });

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
            builder.HasKey(x => x.Id);
        }
    }
}
