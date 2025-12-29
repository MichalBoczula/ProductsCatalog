using Moq;
using ProductCatalog.Application.Features.Products.Commands.CreateProduct;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.History;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Products.Commands;

public class CreateProductCommandHandlerTests
{
    static CreateProductCommandHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeDependenciesInOrderAndReturnDto()
    {
        // Arrange
        var command = new CreateProductCommand(
            new CreateProductExternalDto(
                "Phone",
                "Nice phone",
                new CreateMoneyExternalDto(99.99m, "usd"),
                Guid.NewGuid()));

        var productRepositoryMock = new Mock<IProductsCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<Product>>(MockBehavior.Strict);

        var validationResult = new ValidationResult();
        var sequence = new MockSequence();

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(It.IsAny<Product>()))
            .ReturnsAsync(validationResult);

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.Add(It.IsAny<Product>()));

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.WriteHistory(It.IsAny<ProductsHistory>()));

        productRepositoryMock.InSequence(sequence)
            .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateProductCommandHandler(
            productRepositoryMock.Object,
            validationPolicyMock.Object,
            new CreateProductCommandFlowDescribtor());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        validationPolicyMock.Verify(policy => policy.Validate(It.IsAny<Product>()), Times.Once);

        productRepositoryMock.Verify(
            repo => repo.Add(It.Is<Product>(p =>
                p.Name == command.product.Name &&
                p.Description == command.product.Description &&
                p.CategoryId == command.product.CategoryId)),
            Times.Once);

        productRepositoryMock.Verify(
            repo => repo.WriteHistory(It.Is<ProductsHistory>(history =>
                history.Operation == Operation.Inserted &&
                history.ProductId != Guid.Empty &&
                history.Name == command.product.Name &&
                history.CategoryId == command.product.CategoryId)),
            Times.Once);

        productRepositoryMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(command.product.Name);
        result.Description.ShouldBe(command.product.Description);
        result.CategoryId.ShouldBe(command.product.CategoryId);
        result.Price.Amount.ShouldBe(command.product.Price.Amount);
        result.Price.Currency.ShouldBe(command.product.Price.Currency.ToUpperInvariant());
        result.IsActive.ShouldBeTrue();
    }
}
