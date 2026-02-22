using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.MobilePhones
{
    public class MobilePhoneFilterPriceRangeValidationRuleTests
    {
        [Fact]
        public async Task IsValid_WhenMinimalPriceIsGreaterThanMaximalPrice_ShouldReturnError()
        {
            var rule = new MobilePhoneFilterPriceRangeValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { MinimalPrice = 100m, MaximalPrice = 10m };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            validationResult.GetValidatonErrors().ShouldContain(error => error.Message == "Minimal price must be lower than maximal price.");
        }

        [Fact]
        public async Task IsValid_WhenMinimalPriceEqualsMaximalPrice_ShouldReturnError()
        {
            var rule = new MobilePhoneFilterPriceRangeValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { MinimalPrice = 10m, MaximalPrice = 10m };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            validationResult.GetValidatonErrors().ShouldContain(error => error.Message == "Minimal price must be lower than maximal price.");
        }

        [Fact]
        public async Task IsValid_WhenMinimalPriceIsLowerThanMaximalPrice_ShouldNotReturnErrors()
        {
            var rule = new MobilePhoneFilterPriceRangeValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { MinimalPrice = 10m, MaximalPrice = 100m };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }
    }
}
