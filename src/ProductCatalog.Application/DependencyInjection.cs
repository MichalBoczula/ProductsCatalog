using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Mapping;

namespace ProductCatalog.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            MappingConfig.RegisterMappings();

            services.AddScoped<CreateProductCommandFlowDescribtor>();
            services.AddScoped<IFlowDescriber<CreateProductCommand>, CreateProductCommandFlowDescribtor>();

            return services;
        }
    }
}
