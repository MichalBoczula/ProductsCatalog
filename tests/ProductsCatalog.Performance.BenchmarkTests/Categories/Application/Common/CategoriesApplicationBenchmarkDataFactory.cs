using ProductCatalog.Application.Features.Categories.Queries.GetCategories;
using ProductCatalog.Application.Features.Categories.Queries.GetCategoryById;
using ProductCatalog.Domain.ReadModels;

namespace ProductsCatalog.Performance.BenchmarkTests.Categories.Application.Common
{
    internal static class CategoriesApplicationBenchmarkDataFactory
    {
        private static readonly Guid CategoryId = Guid.Parse("442dcb59-7d28-450a-b5ac-f8a1b74edfa4");

        public static GetCategoriesQuery CreateQuery()
        {
            return new GetCategoriesQuery();
        }

        public static GetCategoryByIdQuery CreateGetCategoryByIdQuery()
        {
            return new GetCategoryByIdQuery(CategoryId);
        }

        public static IReadOnlyList<CategoryReadModel> CreateReadModels()
        {
            return new List<CategoryReadModel>
            {
                new()
                {
                    Id = CategoryId,
                    Code = "MOBILE",
                    Name = "Mobile phones",
                    IsActive = true
                },
                new()
                {
                    Id = Guid.Parse("0416a39a-260a-4ae9-b8ab-04ac6808f19d"),
                    Code = "TABLET",
                    Name = "Tablets",
                    IsActive = true
                },
                new()
                {
                    Id = Guid.Parse("0ff87c12-f0bc-4291-bd51-0a5726790d0d"),
                    Code = "ACCESSORY",
                    Name = "Accessories",
                    IsActive = true
                }
            };
        }

        public static CategoryReadModel CreateReadModel(Guid id)
        {
            return new CategoryReadModel
            {
                Id = id,
                Code = "MOBILE",
                Name = "Mobile phones",
                IsActive = true
            };
        }
    }
}
