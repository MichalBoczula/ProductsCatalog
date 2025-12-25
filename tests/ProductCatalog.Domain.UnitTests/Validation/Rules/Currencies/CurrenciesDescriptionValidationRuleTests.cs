using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Currencies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Currencies
{
    public class CurrenciesDescriptionValidationRuleTests
    {
        [Fact]
        public void IsValid_DescriptionIsEmpty_ShouldReturnError()
        {
            //Arrange
            var currency = new Currency("code", "");
            var rule = new CurrenciesDescriptionValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(currency, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Description cannot be null or whitespace.");
            error.Name.ShouldContain("CurrenciesDescriptionValidationRule");
            error.Entity.ShouldContain("Currency");
        }

        [Fact]
        public void IsValid_DescriptionIsNull_ShouldReturnError()
        {
            //Arrange
            var currency = new Currency("code", null);
            var rule = new CurrenciesDescriptionValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(currency, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Description cannot be null or whitespace.");
            error.Name.ShouldContain("CurrenciesDescriptionValidationRule");
            error.Entity.ShouldContain("Currency");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new CurrenciesDescriptionValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Description cannot be null or whitespace.",
                Name = "CurrenciesDescriptionValidationRule",
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
