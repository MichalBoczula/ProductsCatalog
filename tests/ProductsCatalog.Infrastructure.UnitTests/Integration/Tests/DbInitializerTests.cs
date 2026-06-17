using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;
using Xunit;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class DbInitializerTests : IClassFixture<MsSqlDbTestFixture>
    {
        private readonly MsSqlDbTestFixture _fixture;

        public DbInitializerTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task InitializeInfrastructureAsync_ShouldCreateDatabaseAndSeedInitialData()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(_fixture.ConnectionString);

            using var context = new ProductsContext(optionsBuilder.Options);

            // Act
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            var categoriesHistory = await context.CategoriesHistories.AsNoTracking().ToListAsync();
            var currencies = await context.Currencies.AsNoTracking().ToListAsync();
            var currenciesHistory = await context.CurrenciesHistories.AsNoTracking().ToListAsync();

            // Assert
            categories.Count.ShouldBe(3);
            categories.ShouldContain(c => c.Code == "MOBILE" && c.Name == "Mobile");
            categories.ShouldContain(c => c.Code == "PC" && c.Name == "Personal Computer");
            categories.ShouldContain(c => c.Code == "TABLET" && c.Name == "Tablet");

            categoriesHistory.Count.ShouldBe(3);
            categoriesHistory.ShouldContain(ch => ch.Code == "MOBILE");

            currencies.Count.ShouldBe(3);
            currencies.ShouldContain(c => c.Code == "USD" && c.Description == "US Dollar");
            currencies.ShouldContain(c => c.Code == "PLN" && c.Description == "Polish Złoty");
            currencies.ShouldContain(c => c.Code == "EUR" && c.Description == "Euro");

            currenciesHistory.Count.ShouldBe(3);
            currenciesHistory.ShouldContain(ch => ch.Code == "PLN");

            var mobilePhonesCount = await context.MobilePhones.CountAsync();
            var mobilePhonesHistoryCount = await context.MobilePhonesHistories.CountAsync();

            mobilePhonesCount.ShouldBe(15);
            mobilePhonesHistoryCount.ShouldBe(15);
        }
    }
}