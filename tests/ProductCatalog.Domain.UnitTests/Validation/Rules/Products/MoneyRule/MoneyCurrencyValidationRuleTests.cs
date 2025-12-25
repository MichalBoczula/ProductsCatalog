using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate.ValueObjects;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Products.MoneyRule
{
    public class MoneyCurrencyValidationRuleTests
    {
        [Fact]
        public void IsValid_CurrencyIsEmpty_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, ""), Guid.Empty);
            var rule = new MoneyCurrencyValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product.Price, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Currency cannot be null or whitespace.");
            error.Name.ShouldContain("MoneyCurrencyValidationRule");
            error.Entity.ShouldContain("Money");
        }

        [Fact]
        public void IsValid_CurrencyIsnull_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(10, null), Guid.Empty);
            var rule = new MoneyCurrencyValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product.Price, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Currency cannot be null or whitespace.");
            error.Name.ShouldContain("MoneyCurrencyValidationRule");
            error.Entity.ShouldContain("Money");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new MoneyCurrencyValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Currency cannot be null or whitespace.",
                Name = "MoneyCurrencyValidationRule",
                Entity = nameof(Money)
            };

            //Act
            var result = rule.Describe();
            //Assert
            result.Count.ShouldBe(1);
            var desc = result.First();
            desc.Message.ShouldBe(nullOrEmpty.Message);
            desc.Name.ShouldBe(nullOrEmpty.Name);
        }
    }
}
