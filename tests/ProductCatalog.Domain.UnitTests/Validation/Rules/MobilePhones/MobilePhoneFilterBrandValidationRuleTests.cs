using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Common;
using ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones;
using Shouldly;

namespace ProductCatalog.Domain.UnitTests.Validation.Rules.MobilePhones
{
    public class MobilePhoneFilterBrandValidationRuleTests
    {
        [Fact]
        public async Task IsValid_WhenBrandIsInvalidEnumValue_ShouldReturnError()
        {
            var rule = new MobilePhoneFilterBrandValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { Brand = (MobilePhonesBrand)100 };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(1);
            validationResult.GetValidatonErrors().ShouldContain(error => error.Message == "Brand must exist in MobilePhonesBrand enum.");
        }

        [Fact]
        public async Task IsValid_WhenBrandIsValidEnumValue_ShouldNotReturnErrors()
        {
            var rule = new MobilePhoneFilterBrandValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { Brand = MobilePhonesBrand.Apple };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }

        [Fact]
        public async Task IsValid_WhenBrandIsNull_ShouldNotReturnErrors()
        {
            var rule = new MobilePhoneFilterBrandValidationRule();
            var validationResult = new ValidationResult();
            var filter = new MobilePhoneFilterDto { Brand = null };

            await rule.IsValid(filter, validationResult);

            validationResult.GetValidatonErrors().Count.ShouldBe(0);
        }
    }
}
