using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.MobilePhones
{
    public sealed class MobilePhoneFilterBrandValidationRule : IValidationRule<MobilePhoneFilterDto>
    {
        private readonly ValidationError brandMustExistInMobilePhonesBrand;

        public MobilePhoneFilterBrandValidationRule()
        {
            brandMustExistInMobilePhonesBrand = new ValidationError
            {
                Message = $"Brand must exist in {nameof(MobilePhonesBrand)} enum.",
                Name = nameof(MobilePhoneFilterBrandValidationRule),
                Entity = nameof(MobilePhoneFilterDto)
            };
        }

        public async Task IsValid(MobilePhoneFilterDto mobilePhoneFilter, ValidationResult validationResult)
        {
            if (mobilePhoneFilter is null || mobilePhoneFilter.Brand is null)
            {
                return;
            }

            var brand = (MobilePhonesBrand)mobilePhoneFilter.Brand.Value;

            if (!Enum.IsDefined(typeof(MobilePhonesBrand), brand))
            {
                validationResult.AddValidationError(brandMustExistInMobilePhonesBrand);
            }

            await Task.CompletedTask;
        }

        public List<ValidationError> Describe()
        {
            return [brandMustExistInMobilePhonesBrand];
        }
    }
}
