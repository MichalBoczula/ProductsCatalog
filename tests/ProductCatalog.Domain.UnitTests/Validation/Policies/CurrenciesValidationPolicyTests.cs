using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class CurrenciesValidationPolicyTests
    {
        [Fact]
        public async Task Validate_ShouldReturnErrors()
        {
            //Arrange
            var currency = new Currency("", "");
            var policy = new CurrenciesValidationPolicy();
            //Act
            var result = await policy.Validate(currency);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(2);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CurrenciesCodeValidationRule");
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CurrenciesDescriptionValidationRule");
        }

        [Fact]
        public async Task Validate_CategoryIsNull_ShouldReturnErrorInRulesSet()
        {
            //Arrange
            Currency currency = null;
            var policy = new CurrenciesValidationPolicy();
            //Act
            var result = await policy.Validate(currency);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(1);
            result.GetValidatonErrors().ShouldContain(e => e.Name == "CurrencyIsNullValidationRule");
        }

        [Fact]
        public async Task Validate_ShouldBeValid()
        {
            //Arrange
            var currency = new Currency("code", "desc");
            var policy = new CurrenciesValidationPolicy();
            //Act
            var result = await policy.Validate(currency);
            //Assert
            result.GetValidatonErrors().Count().ShouldBe(0);
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRoles()
        {
            //Arrange
            var policy = new CurrenciesValidationPolicy();
            //Act
            var result = policy.Describe();
            //Assert
            result.Rules.Count.ShouldBe(3);
            result.PolicyName.ShouldBe("CurrenciesValidationPolicy");
            result.Rules.ShouldContain(r => r.RuleName == "CurrenciesCodeValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "CurrenciesDescriptionValidationRule");
            result.Rules.ShouldContain(r => r.RuleName == "CurrencyIsNullValidationRule");
        }
    }
}