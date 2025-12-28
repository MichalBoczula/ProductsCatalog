using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency
{
    internal sealed class CreateCurrencyCommandHandler
        (ICurrenciesCommandsRepository _currencyCommandsRepository,
          IValidationPolicy<Currency> _validationPolicy,
          CreateCurrencyCommandFlowDescribtor _createCurrencyCommandFlowDescribtor)
        : IRequestHandler<CreateCurrencyCommand, CurrencyDto>
    {
        public async Task<CurrencyDto> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = _createCurrencyCommandFlowDescribtor.MapRequestToCurrencyAggregate(request);
            var validationResult = await _createCurrencyCommandFlowDescribtor
                .ValidateCurrencyAggregate(currency, _validationPolicy);

            _createCurrencyCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);
            _createCurrencyCommandFlowDescribtor.AddCurrencyToRepository(currency, _currencyCommandsRepository);

            var currenciesHistory = _createCurrencyCommandFlowDescribtor.CreateCurrencyHistoryEntry(currency);
            _createCurrencyCommandFlowDescribtor.WriteHistoryToRepository(_currencyCommandsRepository, currenciesHistory);

            await _createCurrencyCommandFlowDescribtor.SaveChanges(_currencyCommandsRepository, cancellationToken);
            return _createCurrencyCommandFlowDescribtor.MapCurrencyToCurrencyDto(currency);
        }
    }
}
