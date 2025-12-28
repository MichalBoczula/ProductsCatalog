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

namespace ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency
{
    internal sealed class UpdateCurrencyCommandFlowDescribtor : FlowDescriberBase<UpdateCurrencyCommand>
    {
        [FlowStep(1)]
        public Currency MapRequestToCurrencyAggregate(UpdateCurrencyCommand command)
        {
            var currency = command.currency.Adapt<Currency>();
            return currency;
        }

        [FlowStep(2)]
        public async Task<ValidationResult> ValidateIncomingCurrency(Currency currency, IValidationPolicy<Currency> validationPolicy)
        {
            var validationResult = await validationPolicy.Validate(currency);
            return validationResult;
        }

        [FlowStep(3)]
        public void ThrowValidationExceptionIfIncomingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(4)]
        public async Task<Currency> LoadExistingCurrency(Guid currencyId, ICurrenciesCommandsRepository currencyCommandsRepository, CancellationToken cancellationToken)
        {
            return await currencyCommandsRepository.GetCurrencyById(currencyId, cancellationToken);
        }

        [FlowStep(5)]
        public async Task<ValidationResult> ValidateExistingCurrency(Currency currency, IValidationPolicy<Currency> validationPolicy)
        {
            var validationResult = await validationPolicy.Validate(currency);
            return validationResult;
        }

        [FlowStep(6)]
        public void ThrowValidationExceptionIfExistingInvalid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        [FlowStep(7)]
        public void AssignNewCurrencyInformation(Currency currency, Currency incomingCurrency)
        {
            currency.AssigneNewCurrencyInformation(incomingCurrency);
        }

        [FlowStep(8)]
        public void UpdateCurrencyInRepository(Currency currency, ICurrenciesCommandsRepository currencyCommandsRepository)
        {
            currencyCommandsRepository.Update(currency);
        }

        [FlowStep(9)]
        public CurrenciesHistory CreateCurrencyHistoryEntry(Currency currency)
        {
            var currenciesHistory = currency.BuildAdapter()
                .AddParameters("operation", Operation.Updated)
                .AdaptToType<CurrenciesHistory>();

            return currenciesHistory;
        }

        [FlowStep(10)]
        public void WriteHistoryToRepository(ICurrenciesCommandsRepository currencyCommandsRepository, CurrenciesHistory currenciesHistory)
        {
            currencyCommandsRepository.WriteHistory(currenciesHistory);
        }

        [FlowStep(11)]
        public async Task SaveChanges(ICurrenciesCommandsRepository currencyCommandsRepository, CancellationToken cancellationToken)
        {
            await currencyCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(12)]
        public CurrencyDto MapCurrencyToCurrencyDto(Currency currency)
        {
            var currencyDto = currency.Adapt<CurrencyDto>();
            return currencyDto;
        }
    }
}