using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;

namespace ProductCatalog.Performance.BenchmarkTests.Categories.Infrastructure.Common
{
    internal static class CategoriesInfrastructureDataFactory
    {
        public static Category Create(Guid id)
        {
            var category = new Category(
                code: $"BENCH-{id:N}"[..20].ToUpper(),
                name: "Benchmark Category");

            var idProperty = typeof(Category).GetProperty("Id")
                             ?? typeof(Category).BaseType?.GetProperty("Id");
            idProperty?.SetValue(category, id);

            return category;
        }

        public static Category Create() => Create(Guid.NewGuid());
    }
}
