using Mapster;
using MapsterMapper;

namespace ProductsCatalog.Application.UnitTests.Mapping
{
    internal static class AddMapsterForTests
    {
        public static Mapper GetMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(ProductCatalog.Application.DependencyInjection).Assembly);
            return new Mapper(config);
        }
    }
}
