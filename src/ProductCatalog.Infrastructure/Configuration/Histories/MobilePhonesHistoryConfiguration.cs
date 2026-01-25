using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Infrastructure.Common;

namespace ProductCatalog.Infrastructure.Configuration.Histories
{
    internal sealed class MobilePhonesHistoryConfiguration : IEntityTypeConfiguration<MobilePhonesHistory>
    {
        public void Configure(EntityTypeBuilder<MobilePhonesHistory> builder)
        {
            builder.ToTable(SqlTableNames.MobilePhonesHistory);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MobilePhoneId).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Brand).HasColumnName("Brand").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.MainPhoto).HasMaxLength(200).IsRequired();
            builder.Property(x => x.OtherPhotos).HasColumnType("nvarchar(4000)").IsRequired();
            builder.Property(x => x.CPU).HasMaxLength(200).IsRequired();
            builder.Property(x => x.GPU).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Ram).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Storage).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DisplayType).HasMaxLength(200).IsRequired();
            builder.Property(x => x.RefreshRateHz).IsRequired();
            builder.Property(x => x.ScreenSizeInches).IsRequired();
            builder.Property(x => x.Width).IsRequired();
            builder.Property(x => x.Height).IsRequired();
            builder.Property(x => x.BatteryType).HasMaxLength(200).IsRequired();
            builder.Property(x => x.BatteryCapacity).IsRequired();
            builder.Property(x => x.GPS).IsRequired();
            builder.Property(x => x.AGPS).IsRequired();
            builder.Property(x => x.Galileo).IsRequired();
            builder.Property(x => x.GLONASS).IsRequired();
            builder.Property(x => x.QZSS).IsRequired();
            builder.Property(x => x.Accelerometer).IsRequired();
            builder.Property(x => x.Gyroscope).IsRequired();
            builder.Property(x => x.Proximity).IsRequired();
            builder.Property(x => x.Compass).IsRequired();
            builder.Property(x => x.Barometer).IsRequired();
            builder.Property(x => x.Halla).IsRequired();
            builder.Property(x => x.AmbientLight).IsRequired();
            builder.Property(x => x.Has5G).IsRequired();
            builder.Property(x => x.WiFi).IsRequired();
            builder.Property(x => x.NFC).IsRequired();
            builder.Property(x => x.Bluetooth).IsRequired();
            builder.Property(x => x.Camera).HasMaxLength(200).IsRequired();
            builder.Property(x => x.FingerPrint).IsRequired();
            builder.Property(x => x.FaceId).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.PriceAmount).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.PriceCurrency).HasMaxLength(3).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.ChangedAt).IsRequired();
            builder.Property(x => x.Operation).IsRequired(); 
            builder.Property(x => x.Description2).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.Description3).HasMaxLength(2000).IsRequired();

            builder.HasIndex(x => x.MobilePhoneId);
            builder.HasIndex(x => x.ChangedAt);
        }
    }
}
