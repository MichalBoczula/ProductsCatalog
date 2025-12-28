using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency
{
    internal sealed record DeleteCurrencyCommandHandler
        (ICurrenciesCommandsRepository _currencyCommandsRepository,
         IValidationPolicy<Currency> validationPolicy,
         DeleteCurrencyCommandFlowDescribtor _deleteCurrencyCommandFlowDescribtor)
        : IRequestHandler<DeleteCurrencyCommand, CurrencyDto>
    {
        public async Task<CurrencyDto> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await _deleteCurrencyCommandFlowDescribtor
               .LoadCurrency(request.currencyId, _currencyCommandsRepository, cancellationToken);

            var validationResult = await _deleteCurrencyCommandFlowDescribtor
               .ValidateCurrency(currency, validationPolicy);
            _deleteCurrencyCommandFlowDescribtor.ThrowValidationExceptionIfNotValid(validationResult);

            _deleteCurrencyCommandFlowDescribtor.DeactivateCurrency(currency);
            _deleteCurrencyCommandFlowDescribtor.UpdateCurrencyInRepository(currency, _currencyCommandsRepository);

            var currenciesHistory = _deleteCurrencyCommandFlowDescribtor.CreateCurrencyHistoryEntry(currency);
            _deleteCurrencyCommandFlowDescribtor.WriteHistoryToRepository(_currencyCommandsRepository, currenciesHistory);

            await _deleteCurrencyCommandFlowDescribtor.SaveChanges(_currencyCommandsRepository, cancellationToken);
            return _deleteCurrencyCommandFlowDescribtor.MapCurrencyToCurrencyDto(currency);
        }
    }
}
