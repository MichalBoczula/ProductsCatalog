using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.MobilePhones
{
    public class MobilePhoneFilterPricesValidationRuleTests
    {
        [Fact]
        public async Task IsValid_WhenMinimalPriceIsNegative_ShouldReturnError()
        {
            var rule = new MobilePhoneFilterPricesValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { MinimalPrice = -1m, MaximalPrice = 1m };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            validationResult.GetValidatonErrors().ShouldContain(error => error.Message == "Minimal price must be greater than or equal to zero.");
        }

        [Fact]
        public async Task IsValid_WhenMaximalPriceIsNegative_ShouldReturnError()
        {
            var rule = new MobilePhoneFilterPricesValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { MinimalPrice = 1m, MaximalPrice = -1m };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            validationResult.GetValidatonErrors().ShouldContain(error => error.Message == "Maximal price must be greater than or equal to zero.");
        }

        [Fact]
        public async Task IsValid_WhenPricesAreValid_ShouldNotReturnErrors()
        {
            var rule = new MobilePhoneFilterPricesValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { MinimalPrice = 0m, MaximalPrice = 100m };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }
    }
}
