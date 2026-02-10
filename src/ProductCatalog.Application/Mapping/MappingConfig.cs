using Mapster;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Common;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Features.Common;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using ProductCatalog.Application.Features.MobilePhones.Commands.CreateMobilePhone;
using ProductCatalog.Application.Features.MobilePhones.Commands.UpdateMobilePhone;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.History;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.ReadModels;
using System.Text.Json;

namespace ProductCatalog.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            CreateMappingForCommon();
            CreateMappingForMobilePhone();
            CreateMappingForCategories();
            CreateMappingForReadModels();
            CreateMappingForCurrencies();
            CreateMappingForHistory();
        }

        private static void CreateMappingForReadModels()
        {
            TypeAdapterConfig<ProductReadModel, ProductDto>
                .NewConfig()
                .Map(dest => dest.Price,
                    src => new MoneyDto
                    {
                        Amount = src.PriceAmount,
                        Currency = src.PriceCurrency
                    });

            TypeAdapterConfig<MobilePhoneReadModel, MobilePhoneDetailsDto>
                .NewConfig()
                .Map(dest => dest.CommonDescription,
                    src => new CommonDescriptionDto
                    {
                        Name = src.Name,
                        Brand = src.Brand,
                        Description = src.Description,
                        MainPhoto = src.MainPhoto,
                        OtherPhotos = JsonSerializer.Deserialize<IReadOnlyList<string>>(src.OtherPhotos) ?? Array.Empty<string>()
                    })
                .Map(dest => dest.ElectronicDetails,
                    src => new ElectronicDetailsDto
                    {
                        CPU = src.CPU,
                        GPU = src.GPU,
                        Ram = src.Ram,
                        Storage = src.Storage,
                        DisplayType = src.DisplayType,
                        RefreshRateHz = src.RefreshRateHz,
                        ScreenSizeInches = src.ScreenSizeInches,
                        Width = src.Width,
                        Height = src.Height,
                        BatteryType = src.BatteryType,
                        BatteryCapacity = src.BatteryCapacity
                    })
                .Map(dest => dest.Connectivity,
                    src => new ConnectivityDto
                    {
                        Has5G = src.Has5G,
                        WiFi = src.WiFi,
                        NFC = src.NFC,
                        Bluetooth = src.Bluetooth
                    })
                .Map(dest => dest.SatelliteNavigationSystems,
                    src => new SatelliteNavigationSystemDto
                    {
                        GPS = src.GPS,
                        AGPS = src.AGPS,
                        Galileo = src.Galileo,
                        GLONASS = src.GLONASS,
                        QZSS = src.QZSS
                    })
                .Map(dest => dest.Sensors,
                    src => new SensorsDto
                    {
                        Accelerometer = src.Accelerometer,
                        Gyroscope = src.Gyroscope,
                        Proximity = src.Proximity,
                        Compass = src.Compass,
                        Barometer = src.Barometer,
                        Halla = src.Halla,
                        AmbientLight = src.AmbientLight
                    })
                .Map(dest => dest.Price,
                    src => new MoneyDto
                    {
                        Amount = src.PriceAmount,
                        Currency = src.PriceCurrency
                    });

            TypeAdapterConfig<CategoryReadModel, CategoryDto>
                .NewConfig();

            TypeAdapterConfig<CurrencyReadModel, CurrencyDto>
                .NewConfig();
        }

        private static void CreateMappingForCategories()
        {
            TypeAdapterConfig<CreateCategoryExternalDto, Category>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<UpdateCategoryExternalDto, Category>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<Category, CategoryDto>
                .NewConfig();
        }

        private static void CreateMappingForCurrencies()
        {
            TypeAdapterConfig<CreateCurrencyExternalDto, Currency>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<UpdateCurrencyExternalDto, Currency>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<Currency, CurrencyDto>
                .NewConfig();
        }

        private static void CreateMappingForCommon()
        {
            TypeAdapterConfig<CreateMoneyExternalDto, Money>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<Money, MoneyDto>
                .NewConfig();

            TypeAdapterConfig<CommonDescriptionExtrernalDto, CommonDescription>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<CommonDescription, CommonDescriptionDto>
                .NewConfig();

            TypeAdapterConfig<CreateElectronicDetailsExternalDto, ElectronicDetails>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<UpdateElectronicDetailsExternalDto, ElectronicDetails>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<ElectronicDetails, ElectronicDetailsDto>
                .NewConfig();
        }

        private static void CreateMappingForMobilePhone()
        {
            TypeAdapterConfig<CreateConnectivityExternalDto, Connectivity>
              .NewConfig()
              .MapToConstructor(true);

            TypeAdapterConfig<UpdateConnectivityExternalDto, Connectivity>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<Connectivity, ConnectivityDto>
                .NewConfig();

            TypeAdapterConfig<CreateSatelliteNavigationSystemExternalDto, SatelliteNavigationSystem>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<UpdateSatelliteNavigationSystemExternalDto, SatelliteNavigationSystem>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<SatelliteNavigationSystem, SatelliteNavigationSystemDto>
                .NewConfig();

            TypeAdapterConfig<CreateSensorsExternalDto, Sensors>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<UpdateSensorsExternalDto, Sensors>
               .NewConfig()
               .MapToConstructor(true);

            TypeAdapterConfig<Sensors, SensorsDto>
                .NewConfig();

            TypeAdapterConfig<CreateMobilePhoneExternalDto, MobilePhone>
               .NewConfig()
               .MapToConstructor(true)
               .Map(dest => dest.SatelliteNavigationSystems, src => src.SatelliteNavigationSystems)
               .Map(dest => dest.Sensors, src => src.Sensors)
               .Map(dest => dest.Camera, src => src.Camera);

            TypeAdapterConfig<UpdateMobilePhoneExternalDto, MobilePhone>
               .NewConfig()
               .MapToConstructor(true)
               .Map(dest => dest.SatelliteNavigationSystems, src => src.SatelliteNavigationSystems)
               .Map(dest => dest.Sensors, src => src.Sensors)
               .Map(dest => dest.Camera, src => src.Camera);

            TypeAdapterConfig<MobilePhone, MobilePhoneDetailsDto>
                .NewConfig();

            TypeAdapterConfig<MobilePhoneReadModel, MobilePhoneDto>
                .NewConfig()
                .Map(dest => dest.Price.Amount, src => src.PriceAmount)
                .Map(dest => dest.Price.Currency, src => src.PriceCurrency);

            TypeAdapterConfig<MobilePhoneReadModel, TopMobilePhoneDto>
                .NewConfig()
                .Map(dest => dest.CommonDescription, src => new TopMobilePhonesCommonDescriptionDto
                {
                    Name = src.Name,
                    Brand = src.Brand,
                    Description = src.Description,
                    MainPhoto = src.MainPhoto
                })
                .Map(dest => dest.Price.Amount, src => src.PriceAmount)
                .Map(dest => dest.Price.Currency, src => src.PriceCurrency);
        }

        private static void CreateMappingForHistory()
        {
            TypeAdapterConfig<Product, ProductsHistory>
                .NewConfig()
                .Map(dest => dest.ProductId, src => src.Id)
                .Map(dest => dest.Operation, src => (Operation)MapContext.Current!.Parameters["operation"])
                .Ignore(dest => dest.Id);

            TypeAdapterConfig<Category, CategoriesHistory>
                .NewConfig()
                .Map(dest => dest.CategoryId, src => src.Id)
                .Map(dest => dest.Operation, src => (Operation)MapContext.Current!.Parameters["operation"])
                .Ignore(dest => dest.Id);

            TypeAdapterConfig<Currency, CurrenciesHistory>
                .NewConfig()
                .Map(dest => dest.CurrencyId, src => src.Id)
                .Map(dest => dest.Operation, src => (Operation)MapContext.Current!.Parameters["operation"])
                .Ignore(dest => dest.Id);

            TypeAdapterConfig<MobilePhone, MobilePhonesHistory>
                .NewConfig()
                .Map(dest => dest.MobilePhoneId, src => src.Id)
                .Map(dest => dest.Name, src => src.CommonDescription.Name)
                .Map(dest => dest.Brand, src => src.CommonDescription.Brand)
                .Map(dest => dest.Description, src => src.CommonDescription.Description)
                .Map(dest => dest.MainPhoto, src => src.CommonDescription.MainPhoto)
                .Map(dest => dest.OtherPhotos, src => JsonSerializer.Serialize(src.CommonDescription.OtherPhotos))
                .Map(dest => dest.CPU, src => src.ElectronicDetails.CPU)
                .Map(dest => dest.GPU, src => src.ElectronicDetails.GPU)
                .Map(dest => dest.Ram, src => src.ElectronicDetails.Ram)
                .Map(dest => dest.Storage, src => src.ElectronicDetails.Storage)
                .Map(dest => dest.DisplayType, src => src.ElectronicDetails.DisplayType)
                .Map(dest => dest.RefreshRateHz, src => src.ElectronicDetails.RefreshRateHz)
                .Map(dest => dest.ScreenSizeInches, src => src.ElectronicDetails.ScreenSizeInches)
                .Map(dest => dest.Width, src => src.ElectronicDetails.Width)
                .Map(dest => dest.Height, src => src.ElectronicDetails.Height)
                .Map(dest => dest.BatteryType, src => src.ElectronicDetails.BatteryType)
                .Map(dest => dest.BatteryCapacity, src => src.ElectronicDetails.BatteryCapacity)
                .Map(dest => dest.GPS, src => src.SatelliteNavigationSystems.GPS)
                .Map(dest => dest.AGPS, src => src.SatelliteNavigationSystems.AGPS)
                .Map(dest => dest.Galileo, src => src.SatelliteNavigationSystems.Galileo)
                .Map(dest => dest.GLONASS, src => src.SatelliteNavigationSystems.GLONASS)
                .Map(dest => dest.QZSS, src => src.SatelliteNavigationSystems.QZSS)
                .Map(dest => dest.Accelerometer, src => src.Sensors.Accelerometer)
                .Map(dest => dest.Gyroscope, src => src.Sensors.Gyroscope)
                .Map(dest => dest.Proximity, src => src.Sensors.Proximity)
                .Map(dest => dest.Compass, src => src.Sensors.Compass)
                .Map(dest => dest.Barometer, src => src.Sensors.Barometer)
                .Map(dest => dest.Halla, src => src.Sensors.Halla)
                .Map(dest => dest.AmbientLight, src => src.Sensors.AmbientLight)
                .Map(dest => dest.Has5G, src => src.Connectivity.Has5G)
                .Map(dest => dest.WiFi, src => src.Connectivity.WiFi)
                .Map(dest => dest.NFC, src => src.Connectivity.NFC)
                .Map(dest => dest.Bluetooth, src => src.Connectivity.Bluetooth)
                .Map(dest => dest.Camera, src => src.Camera)
                .Map(dest => dest.FingerPrint, src => src.FingerPrint)
                .Map(dest => dest.FaceId, src => src.FaceId)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.PriceAmount, src => src.Price.Amount)
                .Map(dest => dest.PriceCurrency, src => src.Price.Currency)
                .Map(dest => dest.Description2, src => src.Description2)
                .Map(dest => dest.Description3, src => src.Description3)
                .Map(dest => dest.Operation, src => (Operation)MapContext.Current!.Parameters["operation"])
                .Ignore(dest => dest.Id);

            TypeAdapterConfig<MobilePhonesHistory, MobilePhoneHistoryDto>
                .NewConfig()
                .Map(dest => dest.CommonDescription,
                    src => new CommonDescriptionDto
                    {
                        Name = src.Name,
                        Brand = src.Brand,
                        Description = src.Description,
                        MainPhoto = src.MainPhoto,
                        OtherPhotos = JsonSerializer.Deserialize<IReadOnlyList<string>>(src.OtherPhotos) ?? Array.Empty<string>()
                    })
                .Map(dest => dest.ElectronicDetails,
                    src => new ElectronicDetailsDto
                    {
                        CPU = src.CPU,
                        GPU = src.GPU,
                        Ram = src.Ram,
                        Storage = src.Storage,
                        DisplayType = src.DisplayType,
                        RefreshRateHz = src.RefreshRateHz,
                        ScreenSizeInches = src.ScreenSizeInches,
                        Width = src.Width,
                        Height = src.Height,
                        BatteryType = src.BatteryType,
                        BatteryCapacity = src.BatteryCapacity
                    })
                .Map(dest => dest.Connectivity,
                    src => new ConnectivityDto
                    {
                        Has5G = src.Has5G,
                        WiFi = src.WiFi,
                        NFC = src.NFC,
                        Bluetooth = src.Bluetooth
                    })
                .Map(dest => dest.SatelliteNavigationSystems,
                    src => new SatelliteNavigationSystemDto
                    {
                        GPS = src.GPS,
                        AGPS = src.AGPS,
                        Galileo = src.Galileo,
                        GLONASS = src.GLONASS,
                        QZSS = src.QZSS
                    })
                .Map(dest => dest.Sensors,
                    src => new SensorsDto
                    {
                        Accelerometer = src.Accelerometer,
                        Gyroscope = src.Gyroscope,
                        Proximity = src.Proximity,
                        Compass = src.Compass,
                        Barometer = src.Barometer,
                        Halla = src.Halla,
                        AmbientLight = src.AmbientLight
                    })
                .Map(dest => dest.Price,
                    src => new MoneyDto
                    {
                        Amount = src.PriceAmount,
                        Currency = src.PriceCurrency
                    })
                .Map(dest => dest.Camera, src => src.Camera)
                .Map(dest => dest.Description2, src => src.Description2)
                .Map(dest => dest.Description3, src => src.Description3);
        }
    }
}
