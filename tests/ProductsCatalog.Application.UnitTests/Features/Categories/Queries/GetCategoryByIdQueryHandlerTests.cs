using Moq;
using ProductCatalog.Application.Features.Categories.Queries.GetCategoryById;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Common;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Categories.Queries
{
    public class GetCategoryByIdQueryHandlerTests
    {
        static GetCategoryByIdQueryHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldInvokeRepositoryAndMapResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var readModel = new CategoryReadModel
            {
                Id = categoryId,
                Code = "ELEC",
                Name = "Electronics",
                IsActive = true
            };

            var query = new GetCategoryByIdQuery(categoryId);
            var repoMock = new Mock<ICategoriesQueriesRepository>(MockBehavior.Strict);

            repoMock
                .Setup(r => r.GetById(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(readModel);

            var handler = new GetCategoryByIdQueryHandler(
                repoMock.Object,
                new GetCategoryByIdQueryFlowDescribtor());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            repoMock.Verify(r => r.GetById(categoryId, It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(readModel.Id);
            result.Code.ShouldBe(readModel.Code);
            result.Name.ShouldBe(readModel.Name);
            result.IsActive.ShouldBe(readModel.IsActive);
        }

        [Fact]
        public async Task Handle_WhenCategoryNotFound_ShouldThrowResourceNotFoundException()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var query = new GetCategoryByIdQuery(categoryId);
            var repoMock = new Mock<ICategoriesQueriesRepository>(MockBehavior.Strict);

            repoMock
                .Setup(r => r.GetById(categoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CategoryReadModel?)null);

            var handler = new GetCategoryByIdQueryHandler(
                repoMock.Object,
                new GetCategoryByIdQueryFlowDescribtor());

            // Act & Assert
            await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(query, CancellationToken.None));

            repoMock.Verify(r => r.GetById(categoryId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
