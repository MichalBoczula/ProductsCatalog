using Mapster;
using ProductCatalog.Application.Features.Products;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;

namespace ProductCatalog.Application.Mapping
{
    internal class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CreateProductExternalDto, Product>
                .NewConfig();
        }
    }
}
