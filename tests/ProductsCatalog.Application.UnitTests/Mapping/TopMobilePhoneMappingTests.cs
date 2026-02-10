using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Mapping;

public sealed class TopMobilePhoneMappingTests
{
    static TopMobilePhoneMappingTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public void MobilePhoneReadModel_ShouldMapTo_TopMobilePhoneDto()
    {
        var readModel = BuildReadModel();

        var dto = readModel.Adapt<TopMobilePhoneDto>();

        dto.Id.ShouldBe(readModel.Id);
        dto.CommonDescription.Name.ShouldBe(readModel.Name);
        dto.CommonDescription.Brand.ShouldBe(readModel.Brand);
        dto.CommonDescription.Description.ShouldBe(readModel.Description);
        dto.CommonDescription.MainPhoto.ShouldBe(readModel.MainPhoto);
        dto.Price.Amount.ShouldBe(readModel.PriceAmount);
        dto.Price.Currency.ShouldBe(readModel.PriceCurrency);
    }

    private static MobilePhoneReadModel BuildReadModel()
    {
        return new MobilePhoneReadModel
        {
            Id = Guid.NewGuid(),
            Name = "Top Model",
            Brand = "Brand Z",
            Description = "Top tier device",
            MainPhoto = "main-photo",
            OtherPhotos = "[]",
            CPU = "CPU",
            GPU = "GPU",
            Ram = "12GB",
            Storage = "256GB",
            DisplayType = "AMOLED",
            RefreshRateHz = 120,
            ScreenSizeInches = 6.5m,
            Width = 70,
            Height = 150,
            BatteryType = "Li-Ion",
            BatteryCapacity = 5000,
            GPS = true,
            AGPS = false,
            Galileo = true,
            GLONASS = false,
            QZSS = true,
            Accelerometer = true,
            Gyroscope = false,
            Proximity = true,
            Compass = false,
            Barometer = true,
            Halla = false,
            AmbientLight = true,
            Has5G = true,
            WiFi = true,
            NFC = true,
            Bluetooth = true,
            Camera = "50 MP",
            FingerPrint = true,
            FaceId = false,
            CategoryId = Guid.NewGuid(),
            PriceAmount = 899.99m,
            PriceCurrency = "USD",
            Description2 = "description 2",
            Description3 = "description 3",
            IsActive = true
        };
    }
}
