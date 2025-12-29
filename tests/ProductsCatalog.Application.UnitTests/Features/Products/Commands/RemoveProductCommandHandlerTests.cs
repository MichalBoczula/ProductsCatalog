using Moq;
using ProductCatalog.Application.Features.Products.Commands.RemoveProduct;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Products.Commands;

public class RemoveProductCommandHandlerTests
{
    static RemoveProductCommandHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeDependenciesInOrderAndReturnDeactivatedDto()
    {
        // Arrange
        var product = new Product("Phone", "Nice phone", new Money(99.99m, "usd"), Guid.NewGuid());
        var productId = product.Id;
        var command = new RemoveProductCommand(productId);

        var productRepositoryMock = new Mock<IProductsCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<Product>>(MockBehavior.Strict);

        var validationResult = new ValidationResult();
        var sequence = new MockSequence();

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.GetProductById(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(product))
            .ReturnsAsync(validationResult);

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.Update(It.Is<Product>(p => !p.IsActive)));

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.WriteHistory(It.Is<ProductsHistory>(history =>
                history.Operation == Operation.Deleted &&
                history.ProductId == productId &&
                history.Name == product.Name &&
                history.CategoryId == product.CategoryId)));

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new RemoveProductCommandHandler(
            productRepositoryMock.Object,
            validationPolicyMock.Object,
            new RemoveProductCommandFlowDescribtor());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        productRepositoryMock.Verify(
            repo => repo.GetProductById(productId, It.IsAny<CancellationToken>()),
            Times.Once);

        validationPolicyMock.Verify(policy => policy.Validate(product), Times.Once);

        productRepositoryMock.Verify(
            repo => repo.Update(It.Is<Product>(p => p.IsActive == false)),
            Times.Once);

        productRepositoryMock.Verify(
            repo => repo.WriteHistory(It.IsAny<ProductsHistory>()),
            Times.Once);

        productRepositoryMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(productId);
        result.IsActive.ShouldBeFalse();
        result.Name.ShouldBe(product.Name);
        result.Description.ShouldBe(product.Description);
    }
}
