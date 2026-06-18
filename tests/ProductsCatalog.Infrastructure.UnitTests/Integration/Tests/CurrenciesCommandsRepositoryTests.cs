using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Infrastructure.Contexts.Commands;
using ProductCatalog.Infrastructure.Repositories.Currencies;
using ProductsCatalog.Infrastructure.UnitTests.Integration.Configuration;
using Shouldly;

namespace ProductsCatalog.Infrastructure.UnitTests.Integration.Tests
{
    public class CurrenciesCommandsRepositoryTests : IClassFixture<MsSqlDbTestFixture>
    {
        private readonly MsSqlDbTestFixture _fixture;

        public CurrenciesCommandsRepositoryTests(MsSqlDbTestFixture fixture)
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
        public async Task Add_ShouldPersistNewCurrencyInDatabase_WhenSaveChangesIsCalled()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CurrenciesCommandsRepository(context);

            var currency = new Currency("CHF", "Swiss Franc");

            // Act
            repository.Add(currency);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var result = await assertContext.Currencies.FirstOrDefaultAsync(c => c.Code == "CHF");

            result.ShouldNotBeNull();
            result.Id.ShouldBe(currency.Id);
            result.Code.ShouldBe("CHF");
            result.Description.ShouldBe("Swiss Franc");
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task Update_ShouldModifyExistingCurrencyInDatabase_WhenSaveChangesIsCalled()
        {
            // Arrange
            Guid currencyId;
            using (var setupContext = CreateContext())
            {
                var setupRepository = new CurrenciesCommandsRepository(setupContext);
                var initialCurrency = new Currency("GBP", "Great British Pound");
                setupRepository.Add(initialCurrency);
                await setupRepository.SaveChanges(CancellationToken.None);
                currencyId = initialCurrency.Id;
            }

            // Act
            using var context = CreateContext();
            var repository = new CurrenciesCommandsRepository(context);
            var currencyToUpdate = await repository.GetCurrencyById(currencyId, CancellationToken.None);

            currencyToUpdate.ShouldNotBeNull();

            var newInfo = new Currency("GBP", "British Pound Sterling");
            currencyToUpdate.AssigneNewCurrencyInformation(newInfo);

            repository.Update(currencyToUpdate);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var result = await assertContext.Currencies.FirstOrDefaultAsync(c => c.Id == currencyId);

            result.ShouldNotBeNull();
            result.Description.ShouldBe("British Pound Sterling");
            result.ChangedAt.ShouldBeGreaterThan(DateTime.MinValue);
        }

        [Fact]
        public async Task AddWithHistory_ShouldPersistBothRecordsWithMatchingData()
        {
            // Arrange
            using var context = CreateContext();
            var repository = new CurrenciesCommandsRepository(context);

            var currency = new Currency("JPY", "Japanese Yen");

            var history = CreateTestHistory(
                currencyId: currency.Id,
                code: currency.Code,
                description: currency.Description,
                isActive: currency.IsActive,
                operation: Operation.Inserted,
                changedAt: currency.ChangedAt
            );

            // Act
            repository.Add(currency);
            repository.WriteHistory(history);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var currencyResult = await assertContext.Currencies.FirstOrDefaultAsync(c => c.Id == currency.Id);
            var historyResult = await assertContext.CurrenciesHistories.FirstOrDefaultAsync(h => h.CurrencyId == currency.Id);

            currencyResult.ShouldNotBeNull();
            historyResult.ShouldNotBeNull();
            historyResult.Id.ShouldNotBe(Guid.Empty); 
            historyResult.CurrencyId.ShouldBe(currencyResult.Id);
            historyResult.Code.ShouldBe(currencyResult.Code);
            historyResult.Description.ShouldBe(currencyResult.Description);
            historyResult.IsActive.ShouldBe(currencyResult.IsActive);
            historyResult.ChangedAt.ShouldBe(currencyResult.ChangedAt);
            historyResult.Operation.ShouldBe(Operation.Inserted);
        }

        [Fact]
        public async Task UpdateWithHistory_ShouldPersistBothUpdatedRecordsWithMatchingData()
        {
            // Arrange
            Guid currencyId;
            using (var setupContext = CreateContext())
            {
                var setupRepository = new CurrenciesCommandsRepository(setupContext);
                var initialCurrency = new Currency("CAD", "Canadian Dollar");
                setupRepository.Add(initialCurrency);
                await setupRepository.SaveChanges(CancellationToken.None);
                currencyId = initialCurrency.Id;
            }

            // Act
            using var context = CreateContext();
            var repository = new CurrenciesCommandsRepository(context);
            var currencyToUpdate = await repository.GetCurrencyById(currencyId, CancellationToken.None);

            currencyToUpdate.ShouldNotBeNull();

            var newInfo = new Currency("CAD", "Updated Canadian Dollar");
            currencyToUpdate.AssigneNewCurrencyInformation(newInfo);

            var history = CreateTestHistory(
                currencyId: currencyToUpdate.Id,
                code: currencyToUpdate.Code,
                description: currencyToUpdate.Description,
                isActive: currencyToUpdate.IsActive,
                operation: Operation.Updated,
                changedAt: currencyToUpdate.ChangedAt
            );

            repository.Update(currencyToUpdate);
            repository.WriteHistory(history);
            await repository.SaveChanges(CancellationToken.None);

            // Assert
            using var assertContext = CreateContext();
            var currencyResult = await assertContext.Currencies.FirstOrDefaultAsync(c => c.Id == currencyId);
            var historyResult = await assertContext.CurrenciesHistories
                .FirstOrDefaultAsync(h => h.CurrencyId == currencyId && h.Operation == Operation.Updated);

            currencyResult.ShouldNotBeNull();
            historyResult.ShouldNotBeNull();
            historyResult.Id.ShouldNotBe(Guid.Empty);

            historyResult.CurrencyId.ShouldBe(currencyResult.Id);
            historyResult.Code.ShouldBe(currencyResult.Code);
            historyResult.Description.ShouldBe(currencyResult.Description);
            historyResult.IsActive.ShouldBe(currencyResult.IsActive);
            historyResult.ChangedAt.ShouldBe(currencyResult.ChangedAt);
            historyResult.Operation.ShouldBe(Operation.Updated);
        }

        private static CurrenciesHistory CreateTestHistory(Guid currencyId, string code, string description, bool isActive, Operation operation, DateTime changedAt)
        {
            return new CurrenciesHistory
            {
                CurrencyId = currencyId,
                Code = code,
                Description = description,
                IsActive = isActive,
                ChangedAt = changedAt,
                Operation = operation
            };
        }
    }
}