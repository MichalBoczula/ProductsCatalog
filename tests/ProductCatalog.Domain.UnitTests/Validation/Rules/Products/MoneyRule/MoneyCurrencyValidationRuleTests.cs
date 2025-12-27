using Moq;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.ReadModels;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Products.MoneyRule
{
    public class MoneyCurrencyValidationRuleTests
    {
        [Fact]
        public async Task IsValid_CurrencyIsEmpty_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, ""), Guid.Empty);
            var currenciesQueriesRepository = new Mock<ICurrenciesQueriesRepository>();
            var currencyReadModels = new List<CurrencyReadModel>
            {
                new CurrencyReadModel
                {
                    Id = Guid.NewGuid(),
                    Code = "USD",
                    Description = "US Dollar",
                    IsActive = true
                }
            }.AsReadOnly();
            currenciesQueriesRepository
                .Setup(r => r.GetCurrencies(It.IsAny<CancellationToken>()))
                .ReturnsAsync(currencyReadModels);
            var rule = new MoneyCurrencyValidationRule(currenciesQueriesRepository.Object);
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(product.Price, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(2);
            var error = validationResult.GetValidatonErrors().First();
            var error2 = validationResult.GetValidatonErrors().Last();
            error.Message.ShouldContain("Currency cannot be null or whitespace.");
            error.Name.ShouldContain("MoneyCurrencyValidationRule");
            error.Entity.ShouldContain("Money");
            error2.Message.ShouldContain("Currency does not exist.");
            error2.Name.ShouldContain("MoneyCurrencyValidationRule");
            error2.Entity.ShouldContain("Money");
            currenciesQueriesRepository.Verify(r => r.GetCurrencies(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task IsValid_CurrencyNotExists_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "eur"), Guid.Empty);
            var currenciesQueriesRepository = new Mock<ICurrenciesQueriesRepository>();
            var currencyReadModels = new List<CurrencyReadModel>
            {
                new CurrencyReadModel
                {
                    Id = Guid.NewGuid(),
                    Code = "USD",
                    Description = "US Dollar",
                    IsActive = true
                }
            }.AsReadOnly();
            currenciesQueriesRepository
                .Setup(r => r.GetCurrencies(It.IsAny<CancellationToken>()))
                .ReturnsAsync(currencyReadModels);
            var rule = new MoneyCurrencyValidationRule(currenciesQueriesRepository.Object);
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(product.Price, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Currency does not exist.");
            error.Name.ShouldContain("MoneyCurrencyValidationRule");
            error.Entity.ShouldContain("Money");
            currenciesQueriesRepository.Verify(r => r.GetCurrencies(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task IsValid_ShouldBeValid()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, "USD"), Guid.Empty);
            var currenciesQueriesRepository = new Mock<ICurrenciesQueriesRepository>();
            var currencyReadModels = new List<CurrencyReadModel>
            {
                new CurrencyReadModel
                {
                    Id = Guid.NewGuid(),
                    Code = "USD",
                    Description = "US Dollar",
                    IsActive = true
                }
            }.AsReadOnly();
            currenciesQueriesRepository
                .Setup(r => r.GetCurrencies(It.IsAny<CancellationToken>()))
                .ReturnsAsync(currencyReadModels);
            var rule = new MoneyCurrencyValidationRule(currenciesQueriesRepository.Object);
            var validationResult = new ValidationResult();
            //Act
            await rule.IsValid(product.Price, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(0);
            currenciesQueriesRepository.Verify(r => r.GetCurrencies(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var currenciesQueriesRepository = new Mock<ICurrenciesQueriesRepository>();
            var currencyReadModels = new List<CurrencyReadModel>
            {
                new CurrencyReadModel
                {
                    Id = Guid.NewGuid(),
                    Code = "USD",
                    Description = "US Dollar",
                    IsActive = true
                }
            }.AsReadOnly();
            currenciesQueriesRepository
                .Setup(r => r.GetCurrencies(It.IsAny<CancellationToken>()))
                .ReturnsAsync(currencyReadModels);

            var rule = new MoneyCurrencyValidationRule(currenciesQueriesRepository.Object);
            var nullOrEmpty = new ValidationError
            {
                Message = "Currency cannot be null or whitespace.",
                Name = "MoneyCurrencyValidationRule",
                Entity = nameof(Money)
            };
            var currencyNotExists = new ValidationError
            {
                Message = "Currency does not exist.",
                Name = nameof(MoneyCurrencyValidationRule),
                Entity = nameof(Money)
            };

            //Act
            var result = rule.Describe();
            //Assert
            result.Count.ShouldBe(2);
            var desc = result.First();
            var desc2 = result.Last();
            desc.Message.ShouldBe(nullOrEmpty.Message);
            desc.Name.ShouldBe(nullOrEmpty.Name);
            desc2.Message.ShouldBe(currencyNotExists.Message);
            desc2.Name.ShouldBe(currencyNotExists.Name);
        }
    }
}
