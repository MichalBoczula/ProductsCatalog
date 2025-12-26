using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Currencies.Commands.UpdateCurrency
{
    internal sealed class UpdateCurrencyCommandHandler
        (ICurrenciesCommandsRepository _currencyCommandsRepository,
         IValidationPolicy<Currency> _validationPolicy)
        : IRequestHandler<UpdateCurrencyCommand, CurrencyDto>
    {
        public async Task<CurrencyDto> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var incoming = request.currency.Adapt<Currency>();
            var validationResultIncoming = _validationPolicy.Validate(incoming);
           
            if(!validationResultIncoming.IsValid)
            {
                throw new ValidationException(validationResultIncoming);
            }

            var currency = await _currencyCommandsRepository.GetCurrencyById(request.currencyId, cancellationToken);
            var validationResultExisting = _validationPolicy.Validate(currency);

            if (!validationResultExisting.IsValid)
            {
                throw new ValidationException(validationResultExisting);
            }

            currency.AssigneNewCurrencyInformation(incoming);
            _currencyCommandsRepository.Update(currency);

            var currenciesHistory = new CurrenciesHistory
            {
                CurrencyId = currency.Id,
                Code = currency.Code,
                Description = currency.Description,
                IsActive = currency.IsActive,
                ChangedAt = currency.ChangedAt,
                Operation = Operation.Updated
            };

            _currencyCommandsRepository.WriteHistory(currenciesHistory);

            await _currencyCommandsRepository.SaveChanges(cancellationToken);
            return currency.Adapt<CurrencyDto>();
        }
    }
}
