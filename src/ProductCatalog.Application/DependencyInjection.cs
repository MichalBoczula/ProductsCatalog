using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.RemoveProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
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

            services.AddScoped<UpdateProductCommandFlowDescribtor>();
            services.AddScoped<IFlowDescriber<UpdateProductCommand>, UpdateProductCommandFlowDescribtor>();

            services.AddScoped<RemoveProductCommandFlowDescribtor>();
            services.AddScoped<IFlowDescriber<RemoveProductCommand>, RemoveProductCommandFlowDescribtor>();

            return services;
        }
    }
}
