using Mapster;
using ProductCatalog.Application.Common.Dtos.External;
using ProductCatalog.Application.Common.Dtos.Internal;
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
            TypeAdapterConfig<ProductExternalDto, Product>
                .NewConfig()
                .MapToConstructor(true);

            TypeAdapterConfig<Product, ProductDto>
                .NewConfig();
        }

        private static void CreateMapppingForMoney()
        {
            TypeAdapterConfig<MoneyExternalDto, Money>
                .NewConfig()
                .MapToConstructor(true);
            TypeAdapterConfig<Money, MoneyDto>
                .NewConfig();
        }
    }
}
