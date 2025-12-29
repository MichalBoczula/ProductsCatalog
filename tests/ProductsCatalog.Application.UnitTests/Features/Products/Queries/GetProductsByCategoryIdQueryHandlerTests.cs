using System.Linq;
using Moq;
using ProductCatalog.Application.Features.Products.Queries.GetProductsByCategoryId;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Products.Queries;

public class GetProductsByCategoryIdQueryHandlerTests
{
    static GetProductsByCategoryIdQueryHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepositoryAndMapProducts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var products = new List<ProductReadModel>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Phone",
                Description = "Nice phone",
                PriceAmount = 99.99m,
                PriceCurrency = "usd",
                IsActive = true,
                CategoryId = categoryId
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Tablet",
                Description = "Nice tablet",
                PriceAmount = 199.99m,
                PriceCurrency = "usd",
                IsActive = true,
                CategoryId = categoryId
            }
        }.AsReadOnly();

        var query = new GetProductsByCategoryIdQuery(categoryId);

        var queriesRepositoryMock = new Mock<IProductsQueriesRepository>(MockBehavior.Strict);
        queriesRepositoryMock
            .Setup(repo => repo.GetByCategoryId(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var handler = new GetProductsByCategoryIdQueryHandler(
            queriesRepositoryMock.Object,
            new GetProductsByCategoryIdQueryFlowDescribtor());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        queriesRepositoryMock.Verify(
            repo => repo.GetByCategoryId(categoryId, It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result!.Count.ShouldBe(products.Count);
        result.Select(p => p.Id).ShouldBe(products.Select(p => p.Id));
        result.Select(p => p.Name).ShouldBe(products.Select(p => p.Name));
    }
}
