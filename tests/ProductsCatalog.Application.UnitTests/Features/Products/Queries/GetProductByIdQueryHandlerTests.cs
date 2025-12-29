using System.Linq;
using Moq;
using ProductCatalog.Application.Features.Products.Queries.GetProductById;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Products.Queries;

public class GetProductByIdQueryHandlerTests
{
    static GetProductByIdQueryHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeRepositoryAndMapResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var query = new GetProductByIdQuery(productId);
        var productReadModel = new ProductReadModel
        {
            Id = productId,
            Name = "Phone",
            Description = "Nice phone",
            PriceAmount = 99.99m,
            PriceCurrency = "usd",
            IsActive = true,
            CategoryId = categoryId
        };

        var queriesRepositoryMock = new Mock<IProductsQueriesRepository>(MockBehavior.Strict);

        queriesRepositoryMock
            .Setup(repo => repo.GetById(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productReadModel);

        var handler = new GetProductByIdQueryHandler(
            queriesRepositoryMock.Object,
            new GetProductByIdQueryFlowDescribtor());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        queriesRepositoryMock.Verify(
            repo => repo.GetById(productId, It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(productId);
        result.Name.ShouldBe(productReadModel.Name);
        result.Description.ShouldBe(productReadModel.Description);
        result.CategoryId.ShouldBe(productReadModel.CategoryId);
        result.Price.Amount.ShouldBe(productReadModel.PriceAmount);
        result.Price.Currency.ShouldBe(productReadModel.PriceCurrency);
        result.IsActive.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_WhenProductNotFound_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var query = new GetProductByIdQuery(productId);

        var queriesRepositoryMock = new Mock<IProductsQueriesRepository>(MockBehavior.Strict);

        queriesRepositoryMock
            .Setup(repo => repo.GetById(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductReadModel?)null);

        var handler = new GetProductByIdQueryHandler(
            queriesRepositoryMock.Object,
            new GetProductByIdQueryFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(query, CancellationToken.None));

        queriesRepositoryMock.Verify(
            repo => repo.GetById(productId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
