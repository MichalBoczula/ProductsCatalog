using Mapster;
using ProductCatalog.Application.Common.Dtos;
using ProductCatalog.Application.Features.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;

namespace ProductCatalog.Application.Mapping
{
    internal class MappingConfig
    {
        public static void RegisterMappings()
        {
            CreateMapppingForProducts();
            CreateMapppingForMoney();
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
    }
}
