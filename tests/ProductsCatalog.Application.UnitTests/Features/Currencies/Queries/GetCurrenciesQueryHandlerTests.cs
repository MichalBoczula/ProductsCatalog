using Moq;
using ProductCatalog.Application.Features.Currencies.Queries.GetCurrencies;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Currencies.Queries
{
    public class GetCurrenciesQueryHandlerTests
    {
        static GetCurrenciesQueryHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldInvokeRepositoryAndMapCurrencies()
        {
            // Arrange
            var currencies = new List<CurrencyReadModel>
            {
                new() { Id = Guid.NewGuid(), Code = "USD", Description = "US Dollar", IsActive = true },
                new() { Id = Guid.NewGuid(), Code = "EUR", Description = "Euro", IsActive = false }
            }.AsReadOnly();

            var query = new GetCurrenciesQuery();
            var repoMock = new Mock<ICurrenciesQueriesRepository>(MockBehavior.Strict);

            repoMock
                .Setup(r => r.GetCurrencies(It.IsAny<CancellationToken>()))
                .ReturnsAsync(currencies);

            var handler = new GetCurrenciesQueryHandler(
                repoMock.Object,
                new GetCurrenciesQueryFlowDescribtor());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            repoMock.Verify(r => r.GetCurrencies(It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(currencies.Count);
            result.Select(c => c.Id).ShouldBe(currencies.Select(c => c.Id));
            result.Select(c => c.Code).ShouldBe(currencies.Select(c => c.Code));
            result.Select(c => c.Description).ShouldBe(currencies.Select(c => c.Description));
        }
    }
}
