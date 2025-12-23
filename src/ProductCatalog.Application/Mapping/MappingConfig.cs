using Mapster;
using ProductCatalog.Application.Common.Dtos;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Mapping
{
    internal class MappingConfig
    {
        public static void RegisterMappings()
        {
            CreateMapppingForProducts();
            CreateMapppingForMoney();
            CreateMapppingForReadModels();
        }

        private static void CreateMapppingForProducts()
        {
            TypeAdapterConfig<CreateProductExternalDto, Product>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<Product, ProductDto>
                .NewConfig();
        }

        private static void CreateMapppingForMoney()
        {
            TypeAdapterConfig<CreateMoneyExternalDto, Money>
                .NewConfig()
                .MapToConstructor(true);
            TypeAdapterConfig<Money, MoneyDto>
                .NewConfig();
        }

        private static void CreateMapppingForReadModels()
        {
            TypeAdapterConfig<ProductReadModel, ProductDto>
                .NewConfig()
                .Map(dest => dest.Price,
                    src => new MoneyDto
                    {
                        Amount = src.PriceAmount,
                        Currency = src.PriceCurrency
                    }); ;
        }
    }
}
