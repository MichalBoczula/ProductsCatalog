using ProductCatalog.Domain.AggregatesModel.Common.ValueObjects;
using ProductCatalog.Domain.AggregatesModel.ProductAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Products.MoneyRule;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Products.MoneyRule
{
    public class MoneyAmountValidationRuleTests
    {
        [Fact]
        public void IsValid_AmountEqual0_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(0, "usd"), Guid.Empty);
            var rule = new MoneyAmountValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product.Price, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Amount cannot be zero or below.");
            error.Name.ShouldContain("MoneyAmountValidationRule");
            error.Entity.ShouldContain("Money");
        }

        [Fact]
        public void IsValid_AmountBelow0_ShouldReturnError()
        {
            //Arrange
            var product = new Product("test", "desc", new Money(-10, "usd"), Guid.Empty);
            var rule = new MoneyAmountValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(product.Price, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Amount cannot be zero or below.");
            error.Name.ShouldContain("MoneyAmountValidationRule");
            error.Entity.ShouldContain("Money");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new MoneyAmountValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Amount cannot be zero or below.",
                Name = "MoneyAmountValidationRule",
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
