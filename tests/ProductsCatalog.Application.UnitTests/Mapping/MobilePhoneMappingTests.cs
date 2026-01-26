using Mapster;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.Common.Enums;
using Shouldly;
using Xunit;
using System.Text.Json;

namespace ProductsCatalog.Application.UnitTests.Mapping
{
    public sealed class MobilePhoneMappingTests
    {
        static MobilePhoneMappingTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public void CreateMobilePhoneExternalDto_ShouldMapTo_MobilePhone()
        {
            //Arrange
            var commonDescription = new CommonDescriptionExtrernalDto("Phone", "Brand", "Good phone", "main-photo", new List<string> { "photo1" });
            var electronicDetails = new CreateElectronicDetailsExternalDto("CPU1", "GPU1", "8GB", "128GB", "AMOLED", 120, 6.7m, 70, 150, "Li-Ion", 5000);
            var connectivity = new CreateConnectivityExternalDto(true, true, true, true);
            var navigation = new CreateSatelliteNavigationSystemExternalDto(true, true, true, false, true);
            var sensors = new CreateSensorsExternalDto(true, true, true, false, true, false, true);
            var price = new CreateMoneyExternalDto(250.5m, "usd");
            var dto = new CreateMobilePhoneExternalDto(
                commonDescription,
                electronicDetails,
                connectivity,
                navigation,
                sensors,
                "50 MP",
                true,
                false,
                Guid.NewGuid(),
                price,
                "desc2",
                "desc3");

            //Act
            var mobilePhone = dto.Adapt<MobilePhone>();

            //Assert
            mobilePhone.Id.ShouldNotBe(Guid.Empty);
            mobilePhone.CommonDescription.Name.ShouldBe(commonDescription.Name);
            mobilePhone.CommonDescription.Brand.ShouldBe(commonDescription.Brand);
            mobilePhone.CommonDescription.Description.ShouldBe(commonDescription.Description);
            mobilePhone.CommonDescription.MainPhoto.ShouldBe(commonDescription.MainPhoto);
            mobilePhone.CommonDescription.OtherPhotos.ShouldBe(commonDescription.OtherPhotos);
            mobilePhone.ElectronicDetails.CPU.ShouldBe(electronicDetails.CPU);
            mobilePhone.ElectronicDetails.GPU.ShouldBe(electronicDetails.GPU);
            mobilePhone.ElectronicDetails.Ram.ShouldBe(electronicDetails.Ram);
            mobilePhone.ElectronicDetails.Storage.ShouldBe(electronicDetails.Storage);
            mobilePhone.ElectronicDetails.DisplayType.ShouldBe(electronicDetails.DisplayType);
            mobilePhone.ElectronicDetails.RefreshRateHz.ShouldBe(electronicDetails.RefreshRateHz);
            mobilePhone.ElectronicDetails.ScreenSizeInches.ShouldBe(electronicDetails.ScreenSizeInches);
            mobilePhone.ElectronicDetails.Width.ShouldBe(electronicDetails.Width);
            mobilePhone.ElectronicDetails.Height.ShouldBe(electronicDetails.Height);
            mobilePhone.ElectronicDetails.BatteryType.ShouldBe(electronicDetails.BatteryType);
            mobilePhone.ElectronicDetails.BatteryCapacity.ShouldBe(electronicDetails.BatteryCapacity);
            mobilePhone.Connectivity.Has5G.ShouldBe(connectivity.Has5G);
            mobilePhone.Connectivity.WiFi.ShouldBe(connectivity.WiFi);
            mobilePhone.Connectivity.NFC.ShouldBe(connectivity.NFC);
            mobilePhone.Connectivity.Bluetooth.ShouldBe(connectivity.Bluetooth);
            mobilePhone.SatelliteNavigationSystems.GPS.ShouldBe(navigation.GPS);
            mobilePhone.SatelliteNavigationSystems.AGPS.ShouldBe(navigation.AGPS);
            mobilePhone.SatelliteNavigationSystems.Galileo.ShouldBe(navigation.Galileo);
            mobilePhone.SatelliteNavigationSystems.GLONASS.ShouldBe(navigation.GLONASS);
            mobilePhone.SatelliteNavigationSystems.QZSS.ShouldBe(navigation.QZSS);
            mobilePhone.Sensors.Accelerometer.ShouldBe(sensors.Accelerometer);
            mobilePhone.Sensors.Gyroscope.ShouldBe(sensors.Gyroscope);
            mobilePhone.Sensors.Proximity.ShouldBe(sensors.Proximity);
            mobilePhone.Sensors.Compass.ShouldBe(sensors.Compass);
            mobilePhone.Sensors.Barometer.ShouldBe(sensors.Barometer);
            mobilePhone.Sensors.Halla.ShouldBe(sensors.Halla);
            mobilePhone.Sensors.AmbientLight.ShouldBe(sensors.AmbientLight);
            mobilePhone.Camera.ShouldBe(dto.Camera);
            mobilePhone.FingerPrint.ShouldBeTrue();
            mobilePhone.FaceId.ShouldBeFalse();
            mobilePhone.CategoryId.ShouldBe(dto.CategoryId);
            mobilePhone.Price.Amount.ShouldBe(price.Amount);
            mobilePhone.Price.Currency.ShouldBe(price.Currency.ToUpperInvariant());
            mobilePhone.IsActive.ShouldBeTrue();
            mobilePhone.ChangedAt.ShouldNotBe(DateTime.MinValue);
        }

        [Fact]
        public void UpdateMobilePhoneExternalDto_ShouldMapTo_MobilePhone()
        {
            //Arrange
            var commonDescription = new CommonDescriptionExtrernalDto("Phone 2", "Brand", "Better phone", "main-photo2", new List<string>());
            var electronicDetails = new UpdateElectronicDetailsExternalDto("CPU2", "GPU2", "12GB", "256GB", "IPS", 90, 6.2m, 68, 140, "Li-Po", 4000);
            var connectivity = new UpdateConnectivityExternalDto(false, true, false, true);
            var navigation = new UpdateSatelliteNavigationSystemExternalDto(false, true, false, true, false);
            var sensors = new UpdateSensorsExternalDto(false, true, false, true, false, true, false);
            var price = new UpdateMoneyExternalDto(300.25m, "eur");
            var dto = new UpdateMobilePhoneExternalDto(
                commonDescription,
                electronicDetails,
                connectivity,
                navigation,
                sensors,
                "108 MP",
                false,
                true,
                Guid.NewGuid(),
                price,
                "desc2",
                "desc3");

            //Act
            var mobilePhone = dto.Adapt<MobilePhone>();

            //Assert
            mobilePhone.Id.ShouldNotBe(Guid.Empty);
            mobilePhone.CommonDescription.Name.ShouldBe(commonDescription.Name);
            mobilePhone.CommonDescription.Brand.ShouldBe(commonDescription.Brand);
            mobilePhone.CommonDescription.Description.ShouldBe(commonDescription.Description);
            mobilePhone.CommonDescription.MainPhoto.ShouldBe(commonDescription.MainPhoto);
            mobilePhone.CommonDescription.OtherPhotos.ShouldBe(commonDescription.OtherPhotos);
            mobilePhone.ElectronicDetails.CPU.ShouldBe(electronicDetails.CPU);
            mobilePhone.ElectronicDetails.GPU.ShouldBe(electronicDetails.GPU);
            mobilePhone.ElectronicDetails.Ram.ShouldBe(electronicDetails.Ram);
            mobilePhone.ElectronicDetails.Storage.ShouldBe(electronicDetails.Storage);
            mobilePhone.ElectronicDetails.DisplayType.ShouldBe(electronicDetails.DisplayType);
            mobilePhone.ElectronicDetails.RefreshRateHz.ShouldBe(electronicDetails.RefreshRateHz);
            mobilePhone.ElectronicDetails.ScreenSizeInches.ShouldBe(electronicDetails.ScreenSizeInches);
            mobilePhone.ElectronicDetails.Width.ShouldBe(electronicDetails.Width);
            mobilePhone.ElectronicDetails.Height.ShouldBe(electronicDetails.Height);
            mobilePhone.ElectronicDetails.BatteryType.ShouldBe(electronicDetails.BatteryType);
            mobilePhone.ElectronicDetails.BatteryCapacity.ShouldBe(electronicDetails.BatteryCapacity);
            mobilePhone.Connectivity.Has5G.ShouldBe(connectivity.Has5G);
            mobilePhone.Connectivity.WiFi.ShouldBe(connectivity.WiFi);
            mobilePhone.Connectivity.NFC.ShouldBe(connectivity.NFC);
            mobilePhone.Connectivity.Bluetooth.ShouldBe(connectivity.Bluetooth);
            mobilePhone.SatelliteNavigationSystems.GPS.ShouldBe(navigation.GPS);
            mobilePhone.SatelliteNavigationSystems.AGPS.ShouldBe(navigation.AGPS);
            mobilePhone.SatelliteNavigationSystems.Galileo.ShouldBe(navigation.Galileo);
            mobilePhone.SatelliteNavigationSystems.GLONASS.ShouldBe(navigation.GLONASS);
            mobilePhone.SatelliteNavigationSystems.QZSS.ShouldBe(navigation.QZSS);
            mobilePhone.Sensors.Accelerometer.ShouldBe(sensors.Accelerometer);
            mobilePhone.Sensors.Gyroscope.ShouldBe(sensors.Gyroscope);
            mobilePhone.Sensors.Proximity.ShouldBe(sensors.Proximity);
            mobilePhone.Sensors.Compass.ShouldBe(sensors.Compass);
            mobilePhone.Sensors.Barometer.ShouldBe(sensors.Barometer);
            mobilePhone.Sensors.Halla.ShouldBe(sensors.Halla);
            mobilePhone.Sensors.AmbientLight.ShouldBe(sensors.AmbientLight);
            mobilePhone.Camera.ShouldBe(dto.Camera);
            mobilePhone.FingerPrint.ShouldBeFalse();
            mobilePhone.FaceId.ShouldBeTrue();
            mobilePhone.CategoryId.ShouldBe(dto.CategoryId);
            mobilePhone.Price.Amount.ShouldBe(price.Amount);
            mobilePhone.Price.Currency.ShouldBe(price.Currency.ToUpperInvariant());
            mobilePhone.IsActive.ShouldBeTrue();
            mobilePhone.ChangedAt.ShouldNotBe(DateTime.MinValue);
        }

        [Fact]
        public void MobilePhone_ShouldMapTo_MobilePhoneDto()
        {
            //Arrange
            var categoryId = Guid.NewGuid();
            var mobilePhone = new MobilePhone(
                new CommonDescription("Phone", "brand", "Good phone", "main-photo", new List<string>()),
                new ElectronicDetails("CPU", "GPU", "6GB", "128GB", "AMOLED", 120, 6.5m, 70, 150, "Li-Ion", 4500),
                new Connectivity(true, true, false, true),
                new SatelliteNavigationSystem(true, false, true, true, false),
                new Sensors(true, true, false, true, false, false, true),
                "12 MP",
                true,
                true,
                categoryId,
                new Money(199.99m, "usd"),
                "desc2",
                "desc3");

            //Act
            var dto = mobilePhone.Adapt<MobilePhoneDetailsDto>();

            //Assert
            dto.Id.ShouldBe(mobilePhone.Id);
            dto.CommonDescription.Name.ShouldBe(mobilePhone.CommonDescription.Name);
            dto.CommonDescription.Brand.ShouldBe(mobilePhone.CommonDescription.Brand);
            dto.CommonDescription.Description.ShouldBe(mobilePhone.CommonDescription.Description);
            dto.CommonDescription.MainPhoto.ShouldBe(mobilePhone.CommonDescription.MainPhoto);
            dto.CommonDescription.OtherPhotos.ShouldBe(mobilePhone.CommonDescription.OtherPhotos);
            dto.ElectronicDetails.CPU.ShouldBe(mobilePhone.ElectronicDetails.CPU);
            dto.ElectronicDetails.GPU.ShouldBe(mobilePhone.ElectronicDetails.GPU);
            dto.ElectronicDetails.Ram.ShouldBe(mobilePhone.ElectronicDetails.Ram);
            dto.ElectronicDetails.Storage.ShouldBe(mobilePhone.ElectronicDetails.Storage);
            dto.ElectronicDetails.DisplayType.ShouldBe(mobilePhone.ElectronicDetails.DisplayType);
            dto.ElectronicDetails.RefreshRateHz.ShouldBe(mobilePhone.ElectronicDetails.RefreshRateHz);
            dto.ElectronicDetails.ScreenSizeInches.ShouldBe(mobilePhone.ElectronicDetails.ScreenSizeInches);
            dto.ElectronicDetails.Width.ShouldBe(mobilePhone.ElectronicDetails.Width);
            dto.ElectronicDetails.Height.ShouldBe(mobilePhone.ElectronicDetails.Height);
            dto.ElectronicDetails.BatteryType.ShouldBe(mobilePhone.ElectronicDetails.BatteryType);
            dto.ElectronicDetails.BatteryCapacity.ShouldBe(mobilePhone.ElectronicDetails.BatteryCapacity);
            dto.Connectivity.Has5G.ShouldBe(mobilePhone.Connectivity.Has5G);
            dto.Connectivity.WiFi.ShouldBe(mobilePhone.Connectivity.WiFi);
            dto.Connectivity.NFC.ShouldBe(mobilePhone.Connectivity.NFC);
            dto.Connectivity.Bluetooth.ShouldBe(mobilePhone.Connectivity.Bluetooth);
            dto.SatelliteNavigationSystems.GPS.ShouldBe(mobilePhone.SatelliteNavigationSystems.GPS);
            dto.SatelliteNavigationSystems.AGPS.ShouldBe(mobilePhone.SatelliteNavigationSystems.AGPS);
            dto.SatelliteNavigationSystems.Galileo.ShouldBe(mobilePhone.SatelliteNavigationSystems.Galileo);
            dto.SatelliteNavigationSystems.GLONASS.ShouldBe(mobilePhone.SatelliteNavigationSystems.GLONASS);
            dto.SatelliteNavigationSystems.QZSS.ShouldBe(mobilePhone.SatelliteNavigationSystems.QZSS);
            dto.Sensors.Accelerometer.ShouldBe(mobilePhone.Sensors.Accelerometer);
            dto.Sensors.Gyroscope.ShouldBe(mobilePhone.Sensors.Gyroscope);
            dto.Sensors.Proximity.ShouldBe(mobilePhone.Sensors.Proximity);
            dto.Sensors.Compass.ShouldBe(mobilePhone.Sensors.Compass);
            dto.Sensors.Barometer.ShouldBe(mobilePhone.Sensors.Barometer);
            dto.Sensors.Halla.ShouldBe(mobilePhone.Sensors.Halla);
            dto.Sensors.AmbientLight.ShouldBe(mobilePhone.Sensors.AmbientLight);
            dto.Camera.ShouldBe(mobilePhone.Camera);
            dto.FingerPrint.ShouldBe(mobilePhone.FingerPrint);
            dto.FaceId.ShouldBe(mobilePhone.FaceId);
            dto.CategoryId.ShouldBe(categoryId);
            dto.Price.Amount.ShouldBe(mobilePhone.Price.Amount);
            dto.Price.Currency.ShouldBe(mobilePhone.Price.Currency);
        }

        [Fact]
        public void MobilePhoneReadModel_ShouldMapTo_MobilePhoneDetailsDto()
        {
            //Arrange
            var otherPhotos = new List<string> { "photo1", "photo2" };
            var readModel = new MobilePhoneReadModel
            {
                Id = Guid.NewGuid(),
                Name = "Model X",
                Brand = "Brand Y",
                Description = "Flagship device",
                MainPhoto = "main-photo",
                OtherPhotos = JsonSerializer.Serialize<IReadOnlyList<string>>(otherPhotos),
                CPU = "CPU",
                GPU = "GPU",
                Ram = "12GB",
                Storage = "256GB",
                DisplayType = "AMOLED",
                RefreshRateHz = 120,
                ScreenSizeInches = 6.8m,
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
                Camera = "12 MP",
                FingerPrint = true,
                FaceId = false,
                CategoryId = Guid.NewGuid(),
                PriceAmount = 799.99m,
                PriceCurrency = "USD",
                Description2 = "description 2",
                Description3 = "description 3",
                IsActive = true
            };

            //Act
            var dto = readModel.Adapt<MobilePhoneDetailsDto>();

            //Assert
            dto.Id.ShouldBe(readModel.Id);
            dto.CommonDescription.Name.ShouldBe(readModel.Name);
            dto.CommonDescription.Brand.ShouldBe(readModel.Brand);
            dto.CommonDescription.Description.ShouldBe(readModel.Description);
            dto.CommonDescription.MainPhoto.ShouldBe(readModel.MainPhoto);
            dto.CommonDescription.OtherPhotos.ShouldBe(otherPhotos);
            dto.ElectronicDetails.CPU.ShouldBe(readModel.CPU);
            dto.ElectronicDetails.GPU.ShouldBe(readModel.GPU);
            dto.ElectronicDetails.Ram.ShouldBe(readModel.Ram);
            dto.ElectronicDetails.Storage.ShouldBe(readModel.Storage);
            dto.ElectronicDetails.DisplayType.ShouldBe(readModel.DisplayType);
            dto.ElectronicDetails.RefreshRateHz.ShouldBe(readModel.RefreshRateHz);
            dto.ElectronicDetails.ScreenSizeInches.ShouldBe(readModel.ScreenSizeInches);
            dto.ElectronicDetails.Width.ShouldBe(readModel.Width);
            dto.ElectronicDetails.Height.ShouldBe(readModel.Height);
            dto.ElectronicDetails.BatteryType.ShouldBe(readModel.BatteryType);
            dto.ElectronicDetails.BatteryCapacity.ShouldBe(readModel.BatteryCapacity);
            dto.SatelliteNavigationSystems.GPS.ShouldBe(readModel.GPS);
            dto.SatelliteNavigationSystems.AGPS.ShouldBe(readModel.AGPS);
            dto.SatelliteNavigationSystems.Galileo.ShouldBe(readModel.Galileo);
            dto.SatelliteNavigationSystems.GLONASS.ShouldBe(readModel.GLONASS);
            dto.SatelliteNavigationSystems.QZSS.ShouldBe(readModel.QZSS);
            dto.Sensors.Accelerometer.ShouldBe(readModel.Accelerometer);
            dto.Sensors.Gyroscope.ShouldBe(readModel.Gyroscope);
            dto.Sensors.Proximity.ShouldBe(readModel.Proximity);
            dto.Sensors.Compass.ShouldBe(readModel.Compass);
            dto.Sensors.Barometer.ShouldBe(readModel.Barometer);
            dto.Sensors.Halla.ShouldBe(readModel.Halla);
            dto.Sensors.AmbientLight.ShouldBe(readModel.AmbientLight);
            dto.Connectivity.Has5G.ShouldBe(readModel.Has5G);
            dto.Connectivity.WiFi.ShouldBe(readModel.WiFi);
            dto.Connectivity.NFC.ShouldBe(readModel.NFC);
            dto.Connectivity.Bluetooth.ShouldBe(readModel.Bluetooth);
            dto.Camera.ShouldBe(readModel.Camera);
            dto.FingerPrint.ShouldBe(readModel.FingerPrint);
            dto.FaceId.ShouldBe(readModel.FaceId);
            dto.CategoryId.ShouldBe(readModel.CategoryId);
            dto.Price.Amount.ShouldBe(readModel.PriceAmount);
            dto.Price.Currency.ShouldBe(readModel.PriceCurrency);
        }

        [Fact]
        public void MobilePhone_ShouldMapTo_MobilePhonesHistory()
        {
            //Arrange
            var categoryId = Guid.NewGuid();
            var otherPhotos = new List<string> { "photo1", "photo2" };
            var mobilePhone = new MobilePhone(
                new CommonDescription("Phone", "brand", "Good phone", "main-photo", otherPhotos),
                new ElectronicDetails("CPU", "GPU", "6GB", "128GB", "AMOLED", 120, 6.5m, 70, 150, "Li-Ion", 4500),
                new Connectivity(true, true, false, true),
                new SatelliteNavigationSystem(true, false, true, true, false),
                new Sensors(true, true, false, true, false, false, true),
                "12 MP",
                true,
                true,
                categoryId,
                new Money(199.99m, "usd"),
                "desc2",
                "desc3");
            var operation = Operation.Updated;

            //Act
            var history = mobilePhone.BuildAdapter()
                .AddParameters("operation", operation)
                .AdaptToType<MobilePhonesHistory>();

            //Assert
            history.Id.ShouldNotBe(Guid.Empty);
            history.MobilePhoneId.ShouldBe(mobilePhone.Id);
            history.Name.ShouldBe(mobilePhone.CommonDescription.Name);
            history.Brand.ShouldBe(mobilePhone.CommonDescription.Brand);
            history.Description.ShouldBe(mobilePhone.CommonDescription.Description);
            history.MainPhoto.ShouldBe(mobilePhone.CommonDescription.MainPhoto);
            history.OtherPhotos.ShouldBe(JsonSerializer.Serialize<IReadOnlyList<string>>(otherPhotos));
            history.CPU.ShouldBe(mobilePhone.ElectronicDetails.CPU);
            history.GPU.ShouldBe(mobilePhone.ElectronicDetails.GPU);
            history.Ram.ShouldBe(mobilePhone.ElectronicDetails.Ram);
            history.Storage.ShouldBe(mobilePhone.ElectronicDetails.Storage);
            history.DisplayType.ShouldBe(mobilePhone.ElectronicDetails.DisplayType);
            history.RefreshRateHz.ShouldBe(mobilePhone.ElectronicDetails.RefreshRateHz);
            history.ScreenSizeInches.ShouldBe(mobilePhone.ElectronicDetails.ScreenSizeInches);
            history.Width.ShouldBe(mobilePhone.ElectronicDetails.Width);
            history.Height.ShouldBe(mobilePhone.ElectronicDetails.Height);
            history.BatteryType.ShouldBe(mobilePhone.ElectronicDetails.BatteryType);
            history.BatteryCapacity.ShouldBe(mobilePhone.ElectronicDetails.BatteryCapacity);
            history.GPS.ShouldBe(mobilePhone.SatelliteNavigationSystems.GPS);
            history.AGPS.ShouldBe(mobilePhone.SatelliteNavigationSystems.AGPS);
            history.Galileo.ShouldBe(mobilePhone.SatelliteNavigationSystems.Galileo);
            history.GLONASS.ShouldBe(mobilePhone.SatelliteNavigationSystems.GLONASS);
            history.QZSS.ShouldBe(mobilePhone.SatelliteNavigationSystems.QZSS);
            history.Accelerometer.ShouldBe(mobilePhone.Sensors.Accelerometer);
            history.Gyroscope.ShouldBe(mobilePhone.Sensors.Gyroscope);
            history.Proximity.ShouldBe(mobilePhone.Sensors.Proximity);
            history.Compass.ShouldBe(mobilePhone.Sensors.Compass);
            history.Barometer.ShouldBe(mobilePhone.Sensors.Barometer);
            history.Halla.ShouldBe(mobilePhone.Sensors.Halla);
            history.AmbientLight.ShouldBe(mobilePhone.Sensors.AmbientLight);
            history.Has5G.ShouldBe(mobilePhone.Connectivity.Has5G);
            history.WiFi.ShouldBe(mobilePhone.Connectivity.WiFi);
            history.NFC.ShouldBe(mobilePhone.Connectivity.NFC);
            history.Bluetooth.ShouldBe(mobilePhone.Connectivity.Bluetooth);
            history.Camera.ShouldBe(mobilePhone.Camera);
            history.FingerPrint.ShouldBe(mobilePhone.FingerPrint);
            history.FaceId.ShouldBe(mobilePhone.FaceId);
            history.CategoryId.ShouldBe(mobilePhone.CategoryId);
            history.PriceAmount.ShouldBe(mobilePhone.Price.Amount);
            history.PriceCurrency.ShouldBe(mobilePhone.Price.Currency);
            history.IsActive.ShouldBe(mobilePhone.IsActive);
            history.ChangedAt.ShouldBe(mobilePhone.ChangedAt);
            history.Operation.ShouldBe(operation);
        }

        [Fact]
        public void MobilePhonesHistory_ShouldMapTo_MobilePhoneHistoryDto()
        {
            //Arrange
            var otherPhotos = new List<string> { "photo1", "photo2" };
            var history = new MobilePhonesHistory
            {
                MobilePhoneId = Guid.NewGuid(),
                Name = "Phone",
                Brand = "brand",
                Description = "History phone",
                MainPhoto = "main-photo",
                OtherPhotos = JsonSerializer.Serialize<IReadOnlyList<string>>(otherPhotos),
                CPU = "CPU",
                GPU = "GPU",
                Ram = "8GB",
                Storage = "256GB",
                DisplayType = "AMOLED",
                RefreshRateHz = 120,
                ScreenSizeInches = 6.8m,
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
                Camera = "12 MP",
                FingerPrint = true,
                FaceId = false,
                CategoryId = Guid.NewGuid(),
                PriceAmount = 799.99m,
                PriceCurrency = "USD",
                Description2 = "description 2",
                Description3 = "description 3",
                IsActive = true,
                ChangedAt = DateTime.UtcNow,
                Operation = Operation.Inserted
            };

            //Act
            var dto = history.Adapt<MobilePhoneHistoryDto>();

            //Assert
            dto.Id.ShouldNotBe(Guid.Empty);
            dto.MobilePhoneId.ShouldBe(history.MobilePhoneId);
            dto.CommonDescription.Name.ShouldBe(history.Name);
            dto.CommonDescription.Brand.ShouldBe(history.Brand);
            dto.CommonDescription.Description.ShouldBe(history.Description);
            dto.CommonDescription.MainPhoto.ShouldBe(history.MainPhoto);
            dto.CommonDescription.OtherPhotos.ShouldBe(otherPhotos);
            dto.ElectronicDetails.CPU.ShouldBe(history.CPU);
            dto.ElectronicDetails.GPU.ShouldBe(history.GPU);
            dto.ElectronicDetails.Ram.ShouldBe(history.Ram);
            dto.ElectronicDetails.Storage.ShouldBe(history.Storage);
            dto.ElectronicDetails.DisplayType.ShouldBe(history.DisplayType);
            dto.ElectronicDetails.RefreshRateHz.ShouldBe(history.RefreshRateHz);
            dto.ElectronicDetails.ScreenSizeInches.ShouldBe(history.ScreenSizeInches);
            dto.ElectronicDetails.Width.ShouldBe(history.Width);
            dto.ElectronicDetails.Height.ShouldBe(history.Height);
            dto.ElectronicDetails.BatteryType.ShouldBe(history.BatteryType);
            dto.ElectronicDetails.BatteryCapacity.ShouldBe(history.BatteryCapacity);
            dto.Connectivity.Has5G.ShouldBe(history.Has5G);
            dto.Connectivity.WiFi.ShouldBe(history.WiFi);
            dto.Connectivity.NFC.ShouldBe(history.NFC);
            dto.Connectivity.Bluetooth.ShouldBe(history.Bluetooth);
            dto.Camera.ShouldBe(history.Camera);
            dto.SatelliteNavigationSystems.GPS.ShouldBe(history.GPS);
            dto.SatelliteNavigationSystems.AGPS.ShouldBe(history.AGPS);
            dto.SatelliteNavigationSystems.Galileo.ShouldBe(history.Galileo);
            dto.SatelliteNavigationSystems.GLONASS.ShouldBe(history.GLONASS);
            dto.SatelliteNavigationSystems.QZSS.ShouldBe(history.QZSS);
            dto.Sensors.Accelerometer.ShouldBe(history.Accelerometer);
            dto.Sensors.Gyroscope.ShouldBe(history.Gyroscope);
            dto.Sensors.Proximity.ShouldBe(history.Proximity);
            dto.Sensors.Compass.ShouldBe(history.Compass);
            dto.Sensors.Barometer.ShouldBe(history.Barometer);
            dto.Sensors.Halla.ShouldBe(history.Halla);
            dto.Sensors.AmbientLight.ShouldBe(history.AmbientLight);
            dto.FingerPrint.ShouldBe(history.FingerPrint);
            dto.FaceId.ShouldBe(history.FaceId);
            dto.CategoryId.ShouldBe(history.CategoryId);
            dto.Price.Amount.ShouldBe(history.PriceAmount);
            dto.Price.Currency.ShouldBe(history.PriceCurrency);
            dto.IsActive.ShouldBe(history.IsActive);
            dto.ChangedAt.ShouldBe(history.ChangedAt);
            dto.Operation.ShouldBe(history.Operation);
        }

        [Fact]
        public void MobilePhoneReadModel_ShouldMapTo_MobilePhoneDto()
        {
            //Arrange
            var otherPhotos = new List<string> { "photo1", "photo2" };
            var readModel = new MobilePhoneReadModel
            {
                Id = Guid.NewGuid(),
                Name = "Model X",
                Brand = "Brand Y",
                Description = "Flagship device",
                MainPhoto = "main-photo",
                OtherPhotos = JsonSerializer.Serialize<IReadOnlyList<string>>(otherPhotos),
                CPU = "CPU",
                GPU = "GPU",
                Ram = "12GB",
                Storage = "256GB",
                DisplayType = "AMOLED",
                RefreshRateHz = 120,
                ScreenSizeInches = 6.8m,
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
                Camera = "12 MP",
                FingerPrint = true,
                FaceId = false,
                CategoryId = Guid.NewGuid(),
                PriceAmount = 799.99m,
                PriceCurrency = "USD",
                Description2 = "description 2",
                Description3 = "description 3",
                IsActive = true
            };

            //Act
            var dto = readModel.Adapt<MobilePhoneDto>();

            //Assert
            dto.Id.ShouldBe(readModel.Id);
            dto.Name.ShouldBe(readModel.Name);
            dto.Camera.ShouldBe(readModel.Camera);
            dto.DisplayType.ShouldBe(readModel.DisplayType);
            dto.ScreenSizeInches.ShouldBe(readModel.ScreenSizeInches);
            dto.Price.Amount.ShouldBe(readModel.PriceAmount);
            dto.Price.Currency.ShouldBe(readModel.PriceCurrency);
        }
    }
}