using Moq;
using ProductCatalog.Application.Features.Categories.Commands.UpdateCategory;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Categories.Commands
{
    public class UpdateCategoryCommandHandlerTests
    {
        static UpdateCategoryCommandHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldUpdateAndReturnDto()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var command = new UpdateCategoryCommand(
                categoryId,
                new UpdateCategoryExternalDto("Phones v2", "Updated description"));

            var categoriesRepoMock = new Mock<ICategoriesCommandsRepository>();
            var validationPolicyMock = new Mock<IValidationPolicy<Category>>();
            var sequence = new MockSequence();

            var incomingValidationResult = new ValidationResult();
            var existingValidationResult = new ValidationResult();

            validationPolicyMock.InSequence(sequence)
                .Setup(policy => policy.Validate(It.IsAny<Category>()))
                .ReturnsAsync(incomingValidationResult);

            categoriesRepoMock.InSequence(sequence)
                .Setup(repo => repo.GetCategoryById(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category("Old", "Phones"));

            validationPolicyMock.InSequence(sequence)
                .Setup(policy => policy.Validate(It.IsAny<Category>()))
                .ReturnsAsync(existingValidationResult);

            categoriesRepoMock.InSequence(sequence)
                .Setup(repo => repo.Update(It.Is<Category>(c =>
                c.Id == categoryId &&
                c.Name == command.category.Name &&
                c.Code == command.category.Code)));

            categoriesRepoMock.InSequence(sequence)
                .Setup(repo => repo.WriteHistory(It.IsAny<CategoriesHistory>()));

            categoriesRepoMock.InSequence(sequence)
                .Setup(repo => repo.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new UpdateCategoryCommandHandler(
                categoriesRepoMock.Object,
                validationPolicyMock.Object,
                new UpdateCategoryCommandFlowDescribtor());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            validationPolicyMock.Verify(policy => policy.Validate(It.IsAny<Category>()), Times.Exactly(2));

            categoriesRepoMock.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Once);

            categoriesRepoMock.Verify(repo => repo.WriteHistory(It.IsAny<CategoriesHistory>()), Times.Once);

            categoriesRepoMock.Verify(repo => repo.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            result.Name.ShouldBe(command.category.Name);
            result.Code.ShouldBe(command.category.Code.ToUpper());
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task Handle_WhenIncomingValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var command = new UpdateCategoryCommand(
                categoryId,
                new UpdateCategoryExternalDto("Phones v2", "Updated description"));

            var categoriesRepoMock = new Mock<ICategoriesCommandsRepository>(MockBehavior.Strict);
            var validationPolicyMock = new Mock<IValidationPolicy<Category>>(MockBehavior.Strict);

            var incomingInvalid = new ValidationResult();
            incomingInvalid.AddValidationError(new ValidationError
            {
                Entity = nameof(Category),
                Name = nameof(Category.Name),
                Message = "Invalid name"
            });

            validationPolicyMock
                .Setup(policy => policy.Validate(It.IsAny<Category>()))
                .ReturnsAsync(incomingInvalid);

            var handler = new UpdateCategoryCommandHandler(
                categoriesRepoMock.Object,
                validationPolicyMock.Object,
                new UpdateCategoryCommandFlowDescribtor());

            // Act & Assert
            await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            categoriesRepoMock.Verify(repo => repo.GetCategoryById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            categoriesRepoMock.Verify(repo => repo.Update(It.IsAny<Category>()), Times.Never);
            categoriesRepoMock.Verify(repo => repo.WriteHistory(It.IsAny<CategoriesHistory>()), Times.Never);
            categoriesRepoMock.Verify(repo => repo.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}