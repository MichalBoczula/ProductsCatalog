using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
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
            var configuration = new CustomTestConfiguration(_fixture.ConnectionString);
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

        private sealed class CustomTestConfiguration : IConfiguration
        {
            private readonly string _connectionString;

            public CustomTestConfiguration(string connectionString)
            {
                _connectionString = connectionString;
            }

            public string? this[string key]
            {
                get => _connectionString;
                set { }
            }

            public IConfigurationSection GetSection(string key)
            {
                return new CustomTestConfigurationSection(_connectionString);
            }

            public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();
            public IChangeToken GetReloadToken() => throw new NotImplementedException();
        }

        private sealed class CustomTestConfigurationSection : IConfigurationSection
        {
            public string? Value { get => _connectionString; set { } }
            public string Key => "ProductCatalogDb";
            public string Path => "ConnectionStrings:ProductCatalogDb";

            private readonly string _connectionString;

            public CustomTestConfigurationSection(string connectionString)
            {
                _connectionString = connectionString;
            }

            public string? this[string key]
            {
                get => _connectionString;
                set { }
            }

            public IConfigurationSection GetSection(string key) => this;
            public IEnumerable<IConfigurationSection> GetChildren() => Array.Empty<IConfigurationSection>();
            public IChangeToken GetReloadToken() => throw new NotImplementedException();
        }
    }
}