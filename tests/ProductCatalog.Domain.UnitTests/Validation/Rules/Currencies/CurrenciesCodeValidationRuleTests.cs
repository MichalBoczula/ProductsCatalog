using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Currencies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Currencies
{
    public class CurrenciesCodeValidationRuleTests
    {
        [Fact]
        public void IsValid_CodeIsEmpty_ShouldReturnError()
        {
            //Arrange
            var currency = new Currency("", "name");
            var rule = new CurrenciesCodeValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(currency, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Code cannot be null or whitespace.");
            error.Name.ShouldContain("CurrenciesCodeValidationRule");
            error.Entity.ShouldContain("Currency");
        }

        [Fact]
        public void IsValid_CodeIsNull_ShouldReturnError()
        {
            //Arrange
            var currency = new Currency(null, "name");
            var rule = new CurrenciesCodeValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(currency, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Code cannot be null or whitespace.");
            error.Name.ShouldContain("CurrenciesCodeValidationRule");
            error.Entity.ShouldContain("Currency");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new CurrenciesCodeValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Code cannot be null or whitespace.",
                Name = "CurrenciesCodeValidationRule",
                Entity = nameof(Currency)
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
