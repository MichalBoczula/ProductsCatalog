using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.Currencies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.Currencies
{
    public class CurrencyIsNullValidationRuleTests
    {
        [Fact]
        public void IsValid_CurrencyIsNull_ShouldReturnError()
        {
            //Arrange
            Currency currency = null;
            var rule = new CurrencyIsNullValidationRule();
            var validationResult = new ValidationResult();
            //Act
            rule.IsValid(currency, validationResult);
            //Assert
            validationResult.GetValidatonErrors().Count().ShouldBe(1);
            var error = validationResult.GetValidatonErrors().First();
            error.Message.ShouldContain("Currency cannot be null.");
            error.Name.ShouldContain("CurrencyIsNullValidationRule");
            error.Entity.ShouldContain("Currency");
        }

        [Fact]
        public void Describe_ShouldReturnCorrectRule()
        {
            //Arrange
            var rule = new CurrencyIsNullValidationRule();
            var nullOrEmpty = new ValidationError
            {
                Message = "Currency cannot be null.",
                Name = "CurrencyIsNullValidationRule",
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
