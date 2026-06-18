using Microsoft.Extensions.Configuration;
using ProductCatalog.Infrastructure.Repositories.Categories;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class CategoriesQueriesRepositoryTests : IClassFixture<MsSqlDbTestFixture>
    {
        private readonly MsSqlDbTestFixture _fixture;

        public CategoriesQueriesRepositoryTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetCategories_ShouldReturnAllSeededCategoriesWithValidReadModels()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = _fixture.ConnectionString
                })
                .Build();

            var repository = new CategoriesQueriesRepository(configuration);

            // Act
            var result = await repository.GetCategories(CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);

            var mobile = result.FirstOrDefault(x => x.Code == "MOBILE");
            mobile.ShouldNotBeNull();
            mobile.Id.ShouldNotBe(Guid.Empty);
            mobile.Name.ShouldBe("Mobile");
            mobile.IsActive.ShouldBeTrue();

            var pc = result.FirstOrDefault(x => x.Code == "PC");
            pc.ShouldNotBeNull();
            pc.Id.ShouldNotBe(Guid.Empty);
            pc.Name.ShouldBe("Personal Computer");
            pc.IsActive.ShouldBeTrue();

            var tablet = result.FirstOrDefault(x => x.Code == "TABLET");
            tablet.ShouldNotBeNull();
            tablet.Id.ShouldNotBe(Guid.Empty);
            tablet.Name.ShouldBe("Tablet");
            tablet.IsActive.ShouldBeTrue();

            foreach (var category in result)
            {
                category.Id.ShouldNotBe(Guid.Empty);
                category.Code.ShouldNotBeNullOrWhiteSpace();
                category.Name.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }
}
