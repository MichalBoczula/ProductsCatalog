using Mapster;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency;
using ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            CreateMappingForProducts();
            CreateMappingForMoney();
            CreateMappingForCategories();
            CreateMappingForReadModels();
            CreateMappingForCurrencies();
            CreateMappingForHistory();
        }

        private static void CreateMappingForProducts()
        {
            TypeAdapterConfig<CreateProductExternalDto, Product>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<UpdateProductExternalDto, Product>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<Product, ProductDto>
                .NewConfig();
        }

        private static void CreateMappingForMoney()
        {
            TypeAdapterConfig<CreateMoneyExternalDto, Money>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<Money, MoneyDto>
                .NewConfig();
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
