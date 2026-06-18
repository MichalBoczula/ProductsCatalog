using Microsoft.Extensions.Configuration;
using ProductCatalog.Infrastructure.Repositories.Currencies;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class CurrenciesQueriesRepositoryTests : IClassFixture<MsSqlDbTestFixture>
    {
        private readonly MsSqlDbTestFixture _fixture;

        public CurrenciesQueriesRepositoryTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetCurrencies_ShouldReturnAllSeededCurrenciesWithValidReadModels()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:ProductCatalogDb"] = _fixture.ConnectionString
                })
                .Build();

            var repository = new CurrenciesQueriesRepository(configuration);

            // Act
            var result = await repository.GetCurrencies(CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(3);

            var pln = result.FirstOrDefault(x => x.Code == "PLN");
            pln.ShouldNotBeNull();
            pln.Id.ShouldNotBe(Guid.Empty);
            pln.Description.ShouldBe("Polish Złoty");
            pln.IsActive.ShouldBeTrue();

            var usd = result.FirstOrDefault(x => x.Code == "USD");
            usd.ShouldNotBeNull();
            usd.Id.ShouldNotBe(Guid.Empty);
            usd.Description.ShouldBe("US Dollar");
            usd.IsActive.ShouldBeTrue();

            var eur = result.FirstOrDefault(x => x.Code == "EUR");
            eur.ShouldNotBeNull();
            eur.Id.ShouldNotBe(Guid.Empty);
            eur.Description.ShouldBe("Euro");
            eur.IsActive.ShouldBeTrue();

            foreach (var currency in result)
            {
                currency.Id.ShouldNotBe(Guid.Empty);
                currency.Code.ShouldNotBeNullOrWhiteSpace();
                currency.Description.ShouldNotBeNullOrWhiteSpace();
            }
        }
    }
}