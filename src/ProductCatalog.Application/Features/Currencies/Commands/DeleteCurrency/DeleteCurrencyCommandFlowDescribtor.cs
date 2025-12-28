using Mapster;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Application.Common.FlowDescriptors.Abstract;
using ProductCatalog.Application.Common.FlowDescriptors.Common;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency
{
    internal sealed class DeleteCurrencyCommandFlowDescribtor : FlowDescriberBase<DeleteCurrencyCommand>
    {
        [FlowStep(1)]
        public Task<Currency> LoadCurrency(Guid currencyId, ICurrenciesCommandsRepository currencyCommandsRepository, CancellationToken cancellationToken)
        {
            return currencyCommandsRepository.GetCurrencyById(currencyId, cancellationToken);
        }

        [FlowStep(2)]
        public async Task<ValidationResult> ValidateCurrency(Currency currency, IValidationPolicy<Currency> validationPolicy)
        {
            var validationResult = await validationPolicy.Validate(currency);
            return validationResult;
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfNotValid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public void DeactivateCurrency(Currency currency)
        {
            currency.Deactivate();
        }

        [FlowStep(5)]
        public void UpdateCurrencyInRepository(Currency currency, ICurrenciesCommandsRepository currencyCommandsRepository)
        {
            currencyCommandsRepository.Update(currency);
        }

        [FlowStep(6)]
        public CurrenciesHistory CreateCurrencyHistoryEntry(Currency currency)
        {
            var currenciesHistory = currency.BuildAdapter()
                .AddParameters("operation", Operation.Deleted)
                .AdaptToType<CurrenciesHistory>();

            return currenciesHistory;
        }

        [FlowStep(7)]
        public void WriteHistoryToRepository(ICurrenciesCommandsRepository currencyCommandsRepository, CurrenciesHistory currenciesHistory)
        {
            currencyCommandsRepository.WriteHistory(currenciesHistory);
        }

        [FlowStep(8)]
        public async Task SaveChanges(ICurrenciesCommandsRepository currencyCommandsRepository, CancellationToken cancellationToken)
        {
            await currencyCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(9)]
        public CurrencyDto MapCurrencyToCurrencyDto(Currency currency)
        {
            var currencyDto = currency.Adapt<CurrencyDto>();
            return currencyDto;
        }
    }
}