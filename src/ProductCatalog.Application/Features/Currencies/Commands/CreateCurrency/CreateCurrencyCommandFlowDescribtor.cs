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

namespace ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency
{
    internal sealed class CreateCurrencyCommandFlowDescribtor : FlowDescriberBase<CreateCurrencyCommand>
    {
        [FlowStep(1)]
        public Currency MapRequestToCurrencyAggregate(CreateCurrencyCommand _command)
        {
            var currency = _command.currency.Adapt<Currency>();
            return currency;
        }

        [FlowStep(2)]
        public Task<ValidationResult> ValidateCurrencyAggregate(Currency currency, IValidationPolicy<Currency> validationPolicy)
        {
            var validationResult = validationPolicy.Validate(currency);
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
        public void AddCurrencyToRepository(Currency currency, ICurrenciesCommandsRepository currencyCommandsRepository)
        {
            currencyCommandsRepository.Add(currency);
        }

        [FlowStep(5)]
        public CurrenciesHistory CreateCurrencyHistoryEntry(Currency currency)
        {
            var currenciesHistory = currency.BuildAdapter()
                .AddParameters("operation", Operation.Inserted)
                .AdaptToType<CurrenciesHistory>();

            return currenciesHistory;
        }

        [FlowStep(6)]
        public void WriteHistoryToRepository(ICurrenciesCommandsRepository currencyCommandsRepository, CurrenciesHistory currenciesHistory)
        {
            currencyCommandsRepository.WriteHistory(currenciesHistory);
        }

        [FlowStep(7)]
        public Task SaveChanges(ICurrenciesCommandsRepository currencyCommandsRepository, CancellationToken cancellationToken)
        {
            return currencyCommandsRepository.SaveChanges(cancellationToken);
        }

        [FlowStep(8)]
        public CurrencyDto MapCurrencyToCurrencyDto(Currency currency)
        {
            var currencyDto = currency.Adapt<CurrencyDto>();
            return currencyDto;
        }
    }
}