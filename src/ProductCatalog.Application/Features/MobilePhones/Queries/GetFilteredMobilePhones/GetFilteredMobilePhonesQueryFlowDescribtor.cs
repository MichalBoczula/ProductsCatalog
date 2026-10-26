using Mapster;
using System;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Common.Filters;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetFilteredMobilePhones
{
    [Documentation("products.flow.filter", "Validate optional filter values, then return matching active phones in stable name/Id order.")]
    [FlowOutcome("products.branch.filter.found", "The valid filter matches active phones.", "Return matching phones ordered by Name and Id as 200 without writes.")]
    [FlowOutcome("products.branch.filter.empty", "The valid filter matches no active phones.", "Return an empty array as 200 without writes.")]
    [FlowOutcome("products.branch.filter.invalid-filter", "A supplied brand or price constraint fails the filter policy.", "Return validation_failed 400 before SQL.", "products.rule.filter-brand", "products.rule.filter-prices", "products.rule.filter-range")]
    [FlowOutcome("products.branch.filter.repository-failure", "The filtered repository read throws.", "Return safe internal_error 500 without writes.")]
    internal class GetFilteredMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetFilteredMobilePhonesQuery>
    {
        [FlowStep(1, "validate-filter", "Run brand, nonnegative-bound and strict price-range rules; null bounds mean no bound.")]
        public virtual Task<ValidationResult> ValidateFilter(MobilePhoneFilterDto mobilePhoneFilter, IValidationPolicy<MobilePhoneFilterDto> validationPolicy)
        {
            return validationPolicy.Validate(mobilePhoneFilter);
        }

        [FlowStep(2, "reject-filter", "If a filter rule fails, return validation_failed 400 before building or executing SQL.")]
        public virtual void ThrowValidationExceptionIfFilterInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(3, "normalize-brand", "Translate a defined enum brand to its canonical name; leave a null brand without a brand filter.")]
        public virtual MobilePhoneReadFilterDto CastBrandToString(MobilePhoneFilterDto mobilePhoneFilter)
        {
            var brand = mobilePhoneFilter.Brand is null
                ? null
                : Enum.GetName(typeof(MobilePhonesBrand), mobilePhoneFilter.Brand.Value);

            return new MobilePhoneReadFilterDto
            {
                BrandName = brand,
                MinimalPrice = mobilePhoneFilter.MinimalPrice,
                MaximalPrice = mobilePhoneFilter.MaximalPrice
            };
        }

        [FlowStep(4, "load", "Query active phones with optional inclusive lower/upper price bounds and brand, ordered by Name then Id.")]
        public virtual Task<IReadOnlyList<MobilePhoneReadModel>> GetFilteredMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            MobilePhoneReadFilterDto mobilePhoneFilter,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetFilteredPhones(mobilePhoneFilter, cancellationToken);
        }

        [FlowStep(5, "map-response", "Return matching list summaries or an empty array as 200 without writes.")]
        public virtual IList<MobilePhoneDto> MapMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<IList<MobilePhoneDto>>();
        }
    }
}
