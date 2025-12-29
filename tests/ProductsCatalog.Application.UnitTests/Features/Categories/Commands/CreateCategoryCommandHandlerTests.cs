using Moq;
using ProductCatalog.Application.Features.Categories.Commands.CreateCategory;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Categories.Commands;

public class CreateCategoryCommandHandlerTests
{
    static CreateCategoryCommandHandlerTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Fact]
    public async Task Handle_ShouldInvokeDependenciesInOrderAndReturnDto()
    {
        // Arrange
        var command = new CreateCategoryCommand(
            new CreateCategoryExternalDto(
                "Phones",
                "All kinds of phones"));

        var categoriesRepoMock = new Mock<ICategoriesCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<Category>>(MockBehavior.Strict);

        var validationResult = new ValidationResult();
        var sequence = new MockSequence();

        validationPolicyMock.InSequence(sequence)
            .Setup(policy => policy.Validate(It.IsAny<Category>()))
            .ReturnsAsync(validationResult);

        categoriesRepoMock.InSequence(sequence)
            .Setup(repo => repo.Add(It.IsAny<Category>()));

        categoriesRepoMock.InSequence(sequence)
            .Setup(repo => repo.WriteHistory(It.IsAny<CategoriesHistory>()));

        categoriesRepoMock.InSequence(sequence)
            .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateCategoryCommandHandler(
            categoriesRepoMock.Object,
            validationPolicyMock.Object,
            new CreateCategoryCommandFlowDescribtor());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        validationPolicyMock.Verify(policy => policy.Validate(It.IsAny<Category>()), Times.Once);

        categoriesRepoMock.Verify(
            repo => repo.Add(It.Is<Category>(c =>
                c.Name == command.category.Name &&
                c.Code == command.category.Code)),
            Times.Once);

        categoriesRepoMock.Verify(
            repo => repo.WriteHistory(It.Is<CategoriesHistory>(history =>
                history.Operation == Operation.Inserted &&
                history.CategoryId != Guid.Empty &&
                history.Name == command.category.Name)),
            Times.Once);

        categoriesRepoMock.Verify(
            repo => repo.SaveChanges(It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(command.category.Name);
        result.Code.ShouldBe(command.category.Code);
        result.IsActive.ShouldBeTrue();
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateCategoryCommand(
            new CreateCategoryExternalDto(
                "Phones",
                "All kinds of phones"));

        var categoriesRepoMock = new Mock<ICategoriesCommandsRepository>(MockBehavior.Strict);
        var validationPolicyMock = new Mock<IValidationPolicy<Category>>(MockBehavior.Strict);

        var invalidResult = new ValidationResult();
        invalidResult.AddValidationError(new ValidationError
        {
            Entity = nameof(Category),
            Name = nameof(Category.Name),
            Message = "Invalid name"
        });

        validationPolicyMock
            .Setup(policy => policy.Validate(It.IsAny<Category>()))
            .ReturnsAsync(invalidResult);

        var handler = new CreateCategoryCommandHandler(
            categoriesRepoMock.Object,
            validationPolicyMock.Object,
            new CreateCategoryCommandFlowDescribtor());

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        categoriesRepoMock.Verify(repo => repo.Add(It.IsAny<Category>()), Times.Never);
        categoriesRepoMock.Verify(repo => repo.WriteHistory(It.IsAny<CategoriesHistory>()), Times.Never);
        categoriesRepoMock.Verify(repo => repo.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
    }
}
