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
        public async Task InitializeInfrastructureAsync_ShouldRetainMobilePhoneSeed()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(_fixture.ConnectionString);

            using var context = new ProductsContext(optionsBuilder.Options);

            // Act
            var mobilePhonesCount = await context.MobilePhones.CountAsync();
            var mobilePhonesHistoryCount = await context.MobilePhonesHistories.CountAsync();

            mobilePhonesCount.ShouldBe(15);
            mobilePhonesHistoryCount.ShouldBe(15);
        }
    }
}