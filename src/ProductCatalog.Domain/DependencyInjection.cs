using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Concrete.Policies;

namespace ProductCatalog.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(
            this IServiceCollection services)
        {
            services.AddScoped<IValidationPolicy<Product>, ProductsValidationPolicy>();
            return services;
        }
    }
}
