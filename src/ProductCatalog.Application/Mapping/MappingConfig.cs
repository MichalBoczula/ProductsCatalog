using Mapster;
using ProductCatalog.Application.Common.Dtos.Categories;
using ProductCatalog.Application.Common.Dtos.Products;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.ReadModels;

namespace ProductCatalog.Application.Mapping
{
    internal class MappingConfig
    {
        public static void RegisterMappings()
        {
            CreateMappingForProducts();
            CreateMappingForMoney();
            CreateMappingForCategories();
            CreateMappingForReadModels();
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
    }
}
