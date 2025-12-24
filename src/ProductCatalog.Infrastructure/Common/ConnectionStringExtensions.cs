using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Infrastructure.Common
{
    internal static class  ConnectionStringExtensions
    {
        internal static string Initialize(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("ProductCatalogDb")
                ?? throw new InvalidOperationException("Connection string 'ProductCatalogDb' is not configured.");
        }
    }
}
