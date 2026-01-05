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

            TypeAdapterConfig<MobilePhoneReadModel, MobilePhoneDto>
                .NewConfig()
                .Map(dest => dest.CommonDescription,
                    src => new CommonDescriptionDto
                    {
                        Name = src.Name,
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
               .Map(dest => dest.Sensors, src => src.Sensors);

            TypeAdapterConfig<UpdateMobilePhoneExternalDto, MobilePhone>
               .NewConfig()
               .MapToConstructor(true)
               .Map(dest => dest.SatelliteNavigationSystems, src => src.SatelliteNavigationSystems)
               .Map(dest => dest.Sensors, src => src.Sensors);

            TypeAdapterConfig<MobilePhone, MobilePhoneDto>
                .NewConfig();
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
        }
    }
}
