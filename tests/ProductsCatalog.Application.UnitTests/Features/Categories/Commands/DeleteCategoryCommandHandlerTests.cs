using Moq;
using ProductCatalog.Application.Features.Categories.Commands.DeleteCategory;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Categories.Commands
{
    public class DeleteCategoryCommandHandlerTests
    {
        static DeleteCategoryCommandHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldDeactivateAndReturnDto()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var command = new DeleteCategoryCommand(categoryId);

            var category = new Category("elec", "Electronics");

            var repoMock = new Mock<ICategoriesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Category>>(MockBehavior.Strict);
            var sequence = new MockSequence();

            var validationResult = new ValidationResult();

            repoMock.InSequence(sequence)
                .Setup(r => r.GetCategoryById(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            validationPolicyMock.InSequence(sequence)
                .Setup(v => v.Validate(category))
                .ReturnsAsync(validationResult);

            repoMock.InSequence(sequence)
                .Setup(r => r.Update(category));

            repoMock.InSequence(sequence)
                .Setup(r => r.WriteHistory(It.IsAny<CategoriesHistory>()));

            repoMock.InSequence(sequence)
                .Setup(r => r.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new DeleteCategoryCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new DeleteCategoryCommandFlowDescribtor());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            validationPolicyMock.Verify(v => v.Validate(category), Times.Once);
            repoMock.Verify(r => r.Update(category), Times.Once);
            repoMock.Verify(r => r.WriteHistory(It.IsAny<CategoriesHistory>()), Times.Once);
            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            result.IsActive.ShouldBeFalse();
            result.Code.ShouldBe(category.Code);
            result.Name.ShouldBe(category.Name);
        }

        [Fact]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var command = new DeleteCategoryCommand(categoryId);

            var category = new Category("elec", "Electronics");

            var repoMock = new Mock<ICategoriesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Category>>(MockBehavior.Strict);

            repoMock
                .Setup(r => r.GetCategoryById(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            var invalid = new ValidationResult();
            invalid.AddValidationError(new ValidationError
            {
                Entity = nameof(Category),
                Name = nameof(Category.Name),
                Message = "Invalid name"
            });

            validationPolicyMock
                .Setup(v => v.Validate(category))
                .ReturnsAsync(invalid);

            var handler = new DeleteCategoryCommandHandler(
                repoMock.Object,
                validationPolicyMock.Object,
                new DeleteCategoryCommandFlowDescribtor());

            // Act & Assert
            await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            repoMock.Verify(r => r.Update(It.IsAny<Category>()), Times.Never);
            repoMock.Verify(r => r.WriteHistory(It.IsAny<CategoriesHistory>()), Times.Never);
            repoMock.Verify(r => r.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}