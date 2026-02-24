using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Concrete.Policies;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Policies
{
    public class MobilePhoneFilterValidationPolicyTests
    {
        [Fact]
        public async Task Validate_WhenFilterPricesAreNegative_ShouldReturnErrors()
        {
            var policy = new MobilePhoneFilterValidationPolicy();
            var filter = new MobilePhoneFilterDto { MinimalPrice = -10m, MaximalPrice = -1m };

            var result = await policy.Validate(filter);

            result.GetValidatonErrors().Count.ShouldBe(2);
            result.GetValidatonErrors().ShouldContain(error => error.Name == "MobilePhoneFilterPricesValidationRule");
        }

        [Fact]
        public async Task Validate_WhenFilterPricesAreValid_ShouldBeValid()
        {
            var policy = new MobilePhoneFilterValidationPolicy();
            var filter = new MobilePhoneFilterDto { MinimalPrice = 0m, MaximalPrice = 10m };

            var result = await policy.Validate(filter);

            result.GetValidatonErrors().Count.ShouldBe(0);
        }


        [Fact]
        public async Task Validate_WhenMinimalPriceIsNotLowerThanMaximalPrice_ShouldReturnError()
        {
            var policy = new MobilePhoneFilterValidationPolicy();
            var filter = new MobilePhoneFilterDto { MinimalPrice = 10m, MaximalPrice = 10m };

            var result = await policy.Validate(filter);

            result.GetValidatonErrors().Count.ShouldBe(1);
            result.GetValidatonErrors().ShouldContain(error => error.Name == "MobilePhoneFilterPriceRangeValidationRule");
        }


        [Fact]
        public async Task Validate_WhenBrandIsNotInEnum_ShouldReturnError()
        {
            var policy = new MobilePhoneFilterValidationPolicy();
            var filter = new MobilePhoneFilterDto { Brand = (ProductCatalog.Domain.Common.Enums.MobilePhonesBrand)100, MinimalPrice = 0m, MaximalPrice = 10m };

            var result = await policy.Validate(filter);

            result.GetValidatonErrors().Count.ShouldBe(1);
            result.GetValidatonErrors().ShouldContain(error => error.Name == "MobilePhoneFilterBrandValidationRule");
        }

        [Fact]
        public void Describe_ShouldReturnDescriptionForAllRules()
        {
            var policy = new MobilePhoneFilterValidationPolicy();

            var result = policy.Describe();

            result.Rules.Count.ShouldBe(3);
            result.PolicyName.ShouldBe("MobilePhoneFilterValidationPolicy");
            result.Rules.ShouldContain(rule => rule.RuleName == "MobilePhoneFilterPricesValidationRule");
            result.Rules.ShouldContain(rule => rule.RuleName == "MobilePhoneFilterPriceRangeValidationRule");
            result.Rules.ShouldContain(rule => rule.RuleName == "MobilePhoneFilterBrandValidationRule");
        }
    }
}
