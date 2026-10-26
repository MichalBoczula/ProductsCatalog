using ProductCatalog.Domain.Validation.Common;
using Mapster;
using ProductCatalog.Application.Common.Dtos.MobilePhones;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.ReadModel;
using ProductCatalog.Domain.AggregatesModel.MobilePhoneAggregate.Repositories;

namespace ProductCatalog.Application.Features.MobilePhones.Queries.GetTopMobilePhones
{
    [Documentation("products.flow.top", "Read the three most recently changed active phones; top is recency, not sales popularity.")]
    [FlowOutcome("products.branch.top.found", "Active phones are available.", "Return at most three summaries in stable recency/Id order, excluding inactive phones; no writes.")]
    [FlowOutcome("products.branch.top.empty", "No active phones match, or the defensive result is null.", "Return an empty array as 200 without writes.")]
    [FlowOutcome("products.branch.top.repository-failure", "The top-phone repository read throws.", "Return safe internal_error 500 without writes.")]
    internal class GetTopMobilePhonesQueryFlowDescribtor : FlowDescriberBase<GetTopMobilePhonesQuery>
    {
        [FlowStep(1, "load", "Query the three active phones ordered by ChangedAt descending then Id descending, including stored description and mainPhoto.")]
        public virtual Task<IReadOnlyList<MobilePhoneReadModel>> GetTopMobilePhones(
            IMobilePhonesQueriesRepository mobilePhonesQueriesRepository,
            CancellationToken cancellationToken)
        {
            return mobilePhonesQueriesRepository.GetTop(cancellationToken);
        }

        [FlowStep(2, "check-result", "Keep a valid empty list; normalize a defensive null repository result to an empty list.")]
        public virtual IReadOnlyList<MobilePhoneReadModel> EnsureTopMobilePhonesFound(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones ?? [];
        }

        [FlowStep(3, "map-response", "Return the three-or-fewer top summaries with description and price as 200; do not write.")]
        public virtual IReadOnlyList<TopMobilePhoneDto> MapTopMobilePhonesToDto(IReadOnlyList<MobilePhoneReadModel> mobilePhones)
        {
            return mobilePhones.Adapt<List<TopMobilePhoneDto>>().AsReadOnly();
        }
    }
}
