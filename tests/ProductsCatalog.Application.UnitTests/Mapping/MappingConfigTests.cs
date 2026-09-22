using Mapster;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.ReadModels;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Mapping
{
    public sealed class MappingConfigTests
    {
        static MappingConfigTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public void CreateMoneyExternalDto_ShouldMapTo_Money()
        {
            var dto = new CreateMoneyExternalDto(12.34m, "usd");

            var money = dto.Adapt<Money>();

            money.Amount.ShouldBe(dto.Amount);
            money.Currency.ShouldBe(dto.Currency.ToUpperInvariant());
        }

        [Fact]
        public void Money_ShouldMapTo_MoneyDto()
        {
            var money = new Money(56.78m, "eur");

            var dto = money.Adapt<MoneyDto>();

            dto.Amount.ShouldBe(money.Amount);
            dto.Currency.ShouldBe(money.Currency);
        }

        [Fact]
        public void CommonDescriptionExtrernalDto_ShouldMapTo_CommonDescription()
        {
            var dto = new CommonDescriptionExtrernalDto("Name", "Brand", "Description", "main", new List<string> { "a", "b" });

            var description = dto.Adapt<CommonDescription>();

            description.Name.ShouldBe(dto.Name);
            description.Description.ShouldBe(dto.Description);
            description.MainPhoto.ShouldBe(dto.MainPhoto);
            description.OtherPhotos.ShouldBe(dto.OtherPhotos);
        }

        [Fact]
        public void CommonDescription_ShouldMapTo_CommonDescriptionDto()
        {
            var description = new CommonDescription("Name", "brand", "Desc", "main-photo", new List<string> { "1" });

            var dto = description.Adapt<CommonDescriptionDto>();

            dto.Name.ShouldBe(description.Name);
            dto.Description.ShouldBe(description.Description);
            dto.MainPhoto.ShouldBe(description.MainPhoto);
            dto.OtherPhotos.ShouldBe(description.OtherPhotos);
        }

        [Fact]
        public void CreateElectronicDetailsExternalDto_ShouldMapTo_ElectronicDetails()
        {
            var dto = new CreateElectronicDetailsExternalDto("CPU", "GPU", "8GB", "256GB", "AMOLED", 120, 6.5m, 70, 150, "Li-Ion", 5000);

            var details = dto.Adapt<ElectronicDetails>();

            details.CPU.ShouldBe(dto.CPU);
            details.GPU.ShouldBe(dto.GPU);
            details.Ram.ShouldBe(dto.Ram);
            details.Storage.ShouldBe(dto.Storage);
            details.DisplayType.ShouldBe(dto.DisplayType);
            details.RefreshRateHz.ShouldBe(dto.RefreshRateHz);
            details.ScreenSizeInches.ShouldBe(dto.ScreenSizeInches);
            details.Width.ShouldBe(dto.Width);
            details.Height.ShouldBe(dto.Height);
            details.BatteryType.ShouldBe(dto.BatteryType);
            details.BatteryCapacity.ShouldBe(dto.BatteryCapacity);
        }

        [Fact]
        public void UpdateElectronicDetailsExternalDto_ShouldMapTo_ElectronicDetails()
        {
            var dto = new UpdateElectronicDetailsExternalDto("CPU2", "GPU2", "12GB", "512GB", "IPS", 90, 6.1m, 68, 140, "Li-Po", 4500);

            var details = dto.Adapt<ElectronicDetails>();

            details.CPU.ShouldBe(dto.CPU);
            details.GPU.ShouldBe(dto.GPU);
            details.Ram.ShouldBe(dto.Ram);
            details.Storage.ShouldBe(dto.Storage);
            details.DisplayType.ShouldBe(dto.DisplayType);
            details.RefreshRateHz.ShouldBe(dto.RefreshRateHz);
            details.ScreenSizeInches.ShouldBe(dto.ScreenSizeInches);
            details.Width.ShouldBe(dto.Width);
            details.Height.ShouldBe(dto.Height);
            details.BatteryType.ShouldBe(dto.BatteryType);
            details.BatteryCapacity.ShouldBe(dto.BatteryCapacity);
        }

        [Fact]
        public void ElectronicDetails_ShouldMapTo_ElectronicDetailsDto()
        {
            var details = new ElectronicDetails("CPU3", "GPU3", "16GB", "1TB", "OLED", 144, 6.9m, 75, 155, "NiMH", 5200);

            var dto = details.Adapt<ElectronicDetailsDto>();

            dto.CPU.ShouldBe(details.CPU);
            dto.GPU.ShouldBe(details.GPU);
            dto.Ram.ShouldBe(details.Ram);
            dto.Storage.ShouldBe(details.Storage);
            dto.DisplayType.ShouldBe(details.DisplayType);
            dto.RefreshRateHz.ShouldBe(details.RefreshRateHz);
            dto.ScreenSizeInches.ShouldBe(details.ScreenSizeInches);
            dto.Width.ShouldBe(details.Width);
            dto.Height.ShouldBe(details.Height);
            dto.BatteryType.ShouldBe(details.BatteryType);
            dto.BatteryCapacity.ShouldBe(details.BatteryCapacity);
        }

        [Fact]
        public void CreateConnectivityExternalDto_ShouldMapTo_Connectivity()
        {
            var dto = new CreateConnectivityExternalDto(true, false, true, false);

            var connectivity = dto.Adapt<Connectivity>();

            connectivity.Has5G.ShouldBe(dto.Has5G);
            connectivity.WiFi.ShouldBe(dto.WiFi);
            connectivity.NFC.ShouldBe(dto.NFC);
            connectivity.Bluetooth.ShouldBe(dto.Bluetooth);
        }

        [Fact]
        public void UpdateConnectivityExternalDto_ShouldMapTo_Connectivity()
        {
            var dto = new UpdateConnectivityExternalDto(false, true, false, true);

            var connectivity = dto.Adapt<Connectivity>();

            connectivity.Has5G.ShouldBe(dto.Has5G);
            connectivity.WiFi.ShouldBe(dto.WiFi);
            connectivity.NFC.ShouldBe(dto.NFC);
            connectivity.Bluetooth.ShouldBe(dto.Bluetooth);
        }

        [Fact]
        public void Connectivity_ShouldMapTo_ConnectivityDto()
        {
            var connectivity = new Connectivity(true, true, true, false);

            var dto = connectivity.Adapt<ConnectivityDto>();

            dto.Has5G.ShouldBe(connectivity.Has5G);
            dto.WiFi.ShouldBe(connectivity.WiFi);
            dto.NFC.ShouldBe(connectivity.NFC);
            dto.Bluetooth.ShouldBe(connectivity.Bluetooth);
        }

        [Fact]
        public void CreateSatelliteNavigationSystemExternalDto_ShouldMapTo_SatelliteNavigationSystem()
        {
            var dto = new CreateSatelliteNavigationSystemExternalDto(true, true, false, true, false);

            var navigation = dto.Adapt<SatelliteNavigationSystem>();

            navigation.GPS.ShouldBe(dto.GPS);
            navigation.AGPS.ShouldBe(dto.AGPS);
            navigation.Galileo.ShouldBe(dto.Galileo);
            navigation.GLONASS.ShouldBe(dto.GLONASS);
            navigation.QZSS.ShouldBe(dto.QZSS);
        }

        [Fact]
        public void UpdateSatelliteNavigationSystemExternalDto_ShouldMapTo_SatelliteNavigationSystem()
        {
            var dto = new UpdateSatelliteNavigationSystemExternalDto(false, false, true, false, true);

            var navigation = dto.Adapt<SatelliteNavigationSystem>();

            navigation.GPS.ShouldBe(dto.GPS);
            navigation.AGPS.ShouldBe(dto.AGPS);
            navigation.Galileo.ShouldBe(dto.Galileo);
            navigation.GLONASS.ShouldBe(dto.GLONASS);
            navigation.QZSS.ShouldBe(dto.QZSS);
        }

        [Fact]
        public void SatelliteNavigationSystem_ShouldMapTo_SatelliteNavigationSystemDto()
        {
            var navigation = new SatelliteNavigationSystem(true, false, true, false, true);

            var dto = navigation.Adapt<SatelliteNavigationSystemDto>();

            dto.GPS.ShouldBe(navigation.GPS);
            dto.AGPS.ShouldBe(navigation.AGPS);
            dto.Galileo.ShouldBe(navigation.Galileo);
            dto.GLONASS.ShouldBe(navigation.GLONASS);
            dto.QZSS.ShouldBe(navigation.QZSS);
        }

        [Fact]
        public void CreateSensorsExternalDto_ShouldMapTo_Sensors()
        {
            var dto = new CreateSensorsExternalDto(true, false, true, false, true, false, true);

            var sensors = dto.Adapt<Sensors>();

            sensors.Accelerometer.ShouldBe(dto.Accelerometer);
            sensors.Gyroscope.ShouldBe(dto.Gyroscope);
            sensors.Proximity.ShouldBe(dto.Proximity);
            sensors.Compass.ShouldBe(dto.Compass);
            sensors.Barometer.ShouldBe(dto.Barometer);
            sensors.Halla.ShouldBe(dto.Halla);
            sensors.AmbientLight.ShouldBe(dto.AmbientLight);
        }

        [Fact]
        public void UpdateSensorsExternalDto_ShouldMapTo_Sensors()
        {
            var dto = new UpdateSensorsExternalDto(false, true, false, true, false, true, false);

            var sensors = dto.Adapt<Sensors>();

            sensors.Accelerometer.ShouldBe(dto.Accelerometer);
            sensors.Gyroscope.ShouldBe(dto.Gyroscope);
            sensors.Proximity.ShouldBe(dto.Proximity);
            sensors.Compass.ShouldBe(dto.Compass);
            sensors.Barometer.ShouldBe(dto.Barometer);
            sensors.Halla.ShouldBe(dto.Halla);
            sensors.AmbientLight.ShouldBe(dto.AmbientLight);
        }

        [Fact]
        public void Sensors_ShouldMapTo_SensorsDto()
        {
            var sensors = new Sensors(false, true, false, true, false, true, false);

            var dto = sensors.Adapt<SensorsDto>();

            dto.Accelerometer.ShouldBe(sensors.Accelerometer);
            dto.Gyroscope.ShouldBe(sensors.Gyroscope);
            dto.Proximity.ShouldBe(sensors.Proximity);
            dto.Compass.ShouldBe(sensors.Compass);
            dto.Barometer.ShouldBe(sensors.Barometer);
            dto.Halla.ShouldBe(sensors.Halla);
            dto.AmbientLight.ShouldBe(sensors.AmbientLight);
        }

        [Fact]
        public void ProductReadModel_ShouldMapTo_ProductDtoWithPrice()
        {
            var priceAmount = 23.5m;
            var priceCurrency = "EUR";
            var readModel = new ProductReadModel
            {
                Id = Guid.NewGuid(),
                Name = "Monitor",
                Description = "4k",
                PriceAmount = priceAmount,
                PriceCurrency = priceCurrency,
                IsActive = true,
            };

            var dto = readModel.Adapt<ProductDto>();

            dto.Id.ShouldBe(readModel.Id);
            dto.Name.ShouldBe(readModel.Name);
            dto.Description.ShouldBe(readModel.Description);
            dto.Price.Amount.ShouldBe(priceAmount);
            dto.Price.Currency.ShouldBe(priceCurrency);
            dto.IsActive.ShouldBe(readModel.IsActive);
        }

    }
}
