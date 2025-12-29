using Moq;
using ProductCatalog.Application.Features.Products.Commands.UpdateProduct;
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

public class UpdateProductCommandHandlerTests
{
    static UpdateProductCommandHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeDependenciesInOrderAndReturnUpdatedDto()
    {
        // Arrange
        var existingProduct = new Product("Phone", "Nice phone", new Money(99.99m, "usd"), Guid.NewGuid());
        var productId = existingProduct.Id;
        var command = new UpdateProductCommand(
            productId,
            new UpdateProductExternalDto(
                "Updated Phone",
                "Even nicer phone",
                new UpdateMoneyExternalDto(199.99m, "eur"),
                Guid.NewGuid()));

        var productRepositoryMock = new Mock<IProductsCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<Product>>(MockBehavior.Strict);

        var validationResult = new ValidationResult();
        var sequence = new MockSequence();

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(It.Is<Product>(p =>
                p.Name == command.product.Name &&
                p.Description == command.product.Description &&
                p.CategoryId == command.product.CategoryId)))
            .ReturnsAsync(validationResult);

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.GetProductById(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(existingProduct))
            .ReturnsAsync(validationResult);

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.Update(existingProduct));

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.WriteHistory(It.Is<ProductsHistory>(history =>
                history.Operation == Operation.Updated &&
                history.ProductId == productId &&
                history.Name == command.product.Name &&
                history.CategoryId == command.product.CategoryId)));

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductCommandHandler(
            productRepositoryMock.Object,
            validationPolicyMock.Object,
            new UpdateProductCommandFlowDescribtor());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        validationPolicyMock.Verify(
            policy => policy.Validate(It.IsAny<Product>()),
            Times.Exactly(2));

        productRepositoryMock.Verify(
            repo => repo.GetProductById(productId, It.IsAny<CancellationToken>()),
            Times.Once);

        productRepositoryMock.Verify(
            repo => repo.Update(It.Is<Product>(p =>
                p.Name == command.product.Name &&
                p.Description == command.product.Description &&
                p.CategoryId == command.product.CategoryId &&
                p.Price.Amount == command.product.Price.Amount &&
                p.Price.Currency == command.product.Price.Currency.ToUpperInvariant())),
            Times.Once);

        productRepositoryMock.Verify(
            repo => repo.WriteHistory(It.IsAny<ProductsHistory>()),
            Times.Once);

        productRepositoryMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(productId);
        result.Name.ShouldBe(command.product.Name);
        result.Description.ShouldBe(command.product.Description);
        result.CategoryId.ShouldBe(command.product.CategoryId);
        result.Price.Amount.ShouldBe(command.product.Price.Amount);
        result.Price.Currency.ShouldBe(command.product.Price.Currency.ToUpperInvariant());
        result.IsActive.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            new UpdateProductExternalDto(
                "Updated Phone",
                "Even nicer phone",
                new UpdateMoneyExternalDto(199.99m, "eur"),
                Guid.NewGuid()));

        var productRepositoryMock = new Mock<IProductsCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<Product>>(MockBehavior.Strict);

        var invalidResult = new ValidationResult();
        invalidResult.AddValidationError(new ValidationError
        {
            Entity = nameof(Product),
            Name = nameof(Product.Name),
            Message = "Invalid name"
        });

        validationPolicyMock
            .Setup(policy => policy.Validate(It.IsAny<Product>()))
            .ReturnsAsync(invalidResult);

        var handler = new UpdateProductCommandHandler(
            productRepositoryMock.Object,
            validationPolicyMock.Object,
            new UpdateProductCommandFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        productRepositoryMock.Verify(repo => repo.GetProductById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
        productRepositoryMock.Verify(repo => repo.WriteHistory(It.IsAny<ProductsHistory>()), Times.Never);
        productRepositoryMock.Verify(repo => repo.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
    }
}
