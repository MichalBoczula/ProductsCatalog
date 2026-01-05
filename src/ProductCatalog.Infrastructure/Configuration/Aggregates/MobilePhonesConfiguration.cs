using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
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

            builder.ComplexProperty(x => x.ElectronicDetails, desc =>
            {
                desc.Property(p => p.CPU).HasColumnName("CPU").HasMaxLength(200).IsRequired();
                desc.Property(p => p.GPU).HasColumnName("GPU").HasMaxLength(200).IsRequired();
                desc.Property(p => p.Ram).HasColumnName("Ram").HasMaxLength(200).IsRequired();
                desc.Property(p => p.Storage).HasColumnName("Storage").HasMaxLength(200).IsRequired();
                desc.Property(p => p.DisplayType).HasColumnName("DisplayType").HasMaxLength(200).IsRequired();
                desc.Property(p => p.RefreshRateHz).HasColumnName("RefreshRateHz").IsRequired();
                desc.Property(p => p.ScreenSizeInches).HasColumnName("ScreenSizeInches").IsRequired();
                desc.Property(p => p.BatteryType).HasColumnName("BatteryType").HasMaxLength(200).IsRequired();
                desc.Property(p => p.BatteryCapacity).HasColumnName("BatteryCapacity").IsRequired();
                desc.Property(p => p.Width).HasColumnName("Width").IsRequired();
                desc.Property(p => p.Height).HasColumnName("Height").IsRequired();
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
