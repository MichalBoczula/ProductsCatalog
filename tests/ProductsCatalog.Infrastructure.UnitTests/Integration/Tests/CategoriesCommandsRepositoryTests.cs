using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate;
using ProductCatalog.Domain.AggregatesModel.CategoryAggregate.History;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.Categories;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class CategoriesCommandsRepositoryTests : IClassFixture<MsSqlDbTestFixture>
    {
        private readonly MsSqlDbTestFixture _fixture;

        public CategoriesCommandsRepositoryTests(MsSqlDbTestFixture fixture)
        {
            _fixture = fixture;
        }

        private ProductsContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(_fixture.ConnectionString);
            return new ProductsContext(optionsBuilder.Options);
        }

        [Fact]
        public async Task Add_ShouldPersistNewCategoryInDatabase_WhenSaveChangesIsCalled()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoriesCommandsRepository(context);

            var category = new Category("TST", "Test Category");

            // Act
            repository.Add(category);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var result = await assertContext.Categories.FirstOrDefaultAsync(c => c.Code == "TST");

            result.ShouldNotBeNull();
            result.Id.ShouldBe(category.Id);
            result.Code.ShouldBe("TST");
            result.Name.ShouldBe("Test Category");
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task Update_ShouldModifyExistingCategoryInDatabase_WhenSaveChangesIsCalled()
        {
            // Arrange
            Guid categoryId;
            using (var setupContext = CreateContext())
            {
                var setupRepository = new CategoriesCommandsRepository(setupContext);
                var initialCategory = new Category("OLD", "Old Category");
                setupRepository.Add(initialCategory);
                await setupRepository.SaveChanges(CancellationToken.None);
                categoryId = initialCategory.Id;
            }

            // Act
            using var context = CreateContext();
            var repository = new CategoriesCommandsRepository(context);
            var categoryToUpdate = await repository.GetCategoryById(categoryId, CancellationToken.None);

            categoryToUpdate.ShouldNotBeNull();

            var newInfo = new Category("new", "New Category");
            categoryToUpdate.AssigneNewCategoryInformation(newInfo);

            repository.Update(categoryToUpdate);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var result = await assertContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            result.ShouldNotBeNull();
            result.Code.ShouldBe("NEW");
            result.Name.ShouldBe("New Category");
            result.ChangedAt.ShouldBeGreaterThan(DateTime.MinValue);
        }

        [Fact]
        public async Task AddWithHistory_ShouldPersistBothRecordsWithMatchingData()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CategoriesCommandsRepository(context);

            var category = new Category("HST", "History Category");

            var history = CreateTestHistory(
                categoryId: category.Id,
                code: category.Code,
                name: category.Name,
                isActive: category.IsActive,
                operation: Operation.Inserted,
                changedAt: category.ChangedAt
            );

            // Act
            repository.Add(category);
            repository.WriteHistory(history);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var categoryResult = await assertContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            var historyResult = await assertContext.CategoriesHistories.FirstOrDefaultAsync(h => h.CategoryId == category.Id);

            categoryResult.ShouldNotBeNull();
            historyResult.ShouldNotBeNull();
            historyResult.Id.ShouldNotBe(Guid.Empty);
            historyResult.CategoryId.ShouldBe(categoryResult.Id);
            historyResult.Code.ShouldBe(categoryResult.Code);
            historyResult.Name.ShouldBe(categoryResult.Name);
            historyResult.IsActive.ShouldBe(categoryResult.IsActive);
            historyResult.ChangedAt.ShouldBe(categoryResult.ChangedAt);
            historyResult.Operation.ShouldBe(Operation.Inserted);
        }

        [Fact]
        public async Task UpdateWithHistory_ShouldPersistBothUpdatedRecordsWithMatchingData()
        {
            // Arrange
            Guid categoryId;
            using (var setupContext = CreateContext())
            {
                var setupRepository = new CategoriesCommandsRepository(setupContext);
                var initialCategory = new Category("CAT", "Initial Category");
                setupRepository.Add(initialCategory);
                await setupRepository.SaveChanges(CancellationToken.None);
                categoryId = initialCategory.Id;
            }

            // Act
            using var context = CreateContext();
            var repository = new CategoriesCommandsRepository(context);
            var categoryToUpdate = await repository.GetCategoryById(categoryId, CancellationToken.None);

            categoryToUpdate.ShouldNotBeNull();

            var newInfo = new Category("upd", "Updated Category");
            categoryToUpdate.AssigneNewCategoryInformation(newInfo);

            var history = CreateTestHistory(
                categoryId: categoryToUpdate.Id,
                code: categoryToUpdate.Code,
                name: categoryToUpdate.Name,
                isActive: categoryToUpdate.IsActive,
                operation: Operation.Updated,
                changedAt: categoryToUpdate.ChangedAt
            );

            repository.Update(categoryToUpdate);
            repository.WriteHistory(history);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var categoryResult = await assertContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            var historyResult = await assertContext.CategoriesHistories
                .FirstOrDefaultAsync(h => h.CategoryId == categoryId && h.Operation == Operation.Updated);

            categoryResult.ShouldNotBeNull();
            historyResult.ShouldNotBeNull();
            historyResult.Id.ShouldNotBe(Guid.Empty);

            historyResult.CategoryId.ShouldBe(categoryResult.Id);
            historyResult.Code.ShouldBe(categoryResult.Code);
            historyResult.Name.ShouldBe(categoryResult.Name);
            historyResult.IsActive.ShouldBe(categoryResult.IsActive);
            historyResult.ChangedAt.ShouldBe(categoryResult.ChangedAt);
            historyResult.Operation.ShouldBe(Operation.Updated);
        }

        private static CategoriesHistory CreateTestHistory(Guid categoryId, string code, string name, bool isActive, Operation operation, DateTime changedAt)
        {
            return new CategoriesHistory
            {
                CategoryId = categoryId,
                Code = code,
                Name = name,
                IsActive = isActive,
                ChangedAt = changedAt,
                Operation = operation
            };
        }
    }
}
