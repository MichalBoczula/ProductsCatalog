using Moq;
using ProductCatalog.Application.Features.Categories.Queries.GetCategories;
using ProductCatalog.Application.Mapping;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.Repositories;
using ProductCatalog.Domain.ReadModels;
using Shouldly;

namespace ProductsCatalog.Application.UnitTests.Features.Categories.Queries
{
    public class GetCategoriesQueryHandlerTests
    {
        static GetCategoriesQueryHandlerTests()
        {
            MappingConfig.RegisterMappings();
        }

        [Fact]
        public async Task Handle_ShouldInvokeRepositoryAndMapCategories()
        {
            // Arrange
            var categories = new List<CategoryReadModel>
        {
            new() { Id = Guid.NewGuid(), Code = "ELEC", Name = "Electronics", IsActive = true },
            new() { Id = Guid.NewGuid(), Code = "HOME", Name = "Home", IsActive = false }
        }.AsReadOnly();

            var query = new GetCategoriesQuery();
            var repoMock = new Mock<ICategoriesQueriesRepository>(MockBehavior.Strict);

            repoMock
                .Setup(r => r.GetCategories(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            var handler = new GetCategoriesQueryHandler(
                repoMock.Object,
                new GetCategoriesQueryFlowDescribtor());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            repoMock.Verify(r => r.GetCategories(It.IsAny<CancellationToken>()), Times.Once);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(categories.Count);
            result.Select(c => c.Id).ShouldBe(categories.Select(c => c.Id));
            result.Select(c => c.Code).ShouldBe(categories.Select(c => c.Code));
            result.Select(c => c.Name).ShouldBe(categories.Select(c => c.Name));
        }
    }
}