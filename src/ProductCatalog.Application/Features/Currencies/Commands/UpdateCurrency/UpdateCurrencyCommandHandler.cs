using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;

namespace ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency
{
    internal sealed class UpdateCurrencyCommandHandler
        (ICurrenciesCommandsRepository _currencyCommandsRepository,
         IValidationPolicy<Currency> _validationPolicy,
         UpdateCurrencyCommandFlowDescribtor _updateCurrencyCommandFlowDescribtor)
        : IRequestHandler<UpdateCurrencyCommand, CurrencyDto>
    {
        public async Task<CurrencyDto> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var incoming = _updateCurrencyCommandFlowDescribtor.MapRequestToCurrencyAggregate(request);
            var validationResultIncoming = await _updateCurrencyCommandFlowDescribtor
                .ValidateIncomingCurrency(incoming, _validationPolicy);
            _updateCurrencyCommandFlowDescribtor.ThrowValidationExceptionIfIncomingInvalid(validationResultIncoming);

            var currency = await _updateCurrencyCommandFlowDescribtor
                .LoadExistingCurrency(request.currencyId, _currencyCommandsRepository, cancellationToken);

            var validationResultExisting = await _updateCurrencyCommandFlowDescribtor
                 .ValidateExistingCurrency(currency, _validationPolicy);
            _updateCurrencyCommandFlowDescribtor.ThrowValidationExceptionIfExistingInvalid(validationResultExisting);

            _updateCurrencyCommandFlowDescribtor.AssignNewCurrencyInformation(currency, incoming);
            _updateCurrencyCommandFlowDescribtor.UpdateCurrencyInRepository(currency, _currencyCommandsRepository);

            var currenciesHistory = _updateCurrencyCommandFlowDescribtor.CreateCurrencyHistoryEntry(currency);
            _updateCurrencyCommandFlowDescribtor.WriteHistoryToRepository(_currencyCommandsRepository, currenciesHistory);

            await _updateCurrencyCommandFlowDescribtor.SaveChanges(_currencyCommandsRepository, cancellationToken);
            return _updateCurrencyCommandFlowDescribtor.MapCurrencyToCurrencyDto(currency);
        }
    }
}
