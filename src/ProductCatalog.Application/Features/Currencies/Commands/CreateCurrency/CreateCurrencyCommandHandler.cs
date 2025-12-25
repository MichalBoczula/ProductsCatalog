using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency
{
    internal sealed class CreateCurrencyCommandHandler 
        (ICurrencyCommandsRepository _currencyCommandsRepository,
         IValidationPolicy<Currency> _validationPolicy)
        : IRequestHandler<CreateCurrencyCommand, CurrencyDto>
    {
        public async Task<CurrencyDto> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = request.currency.Adapt<Currency>();
            var validationResult = _validationPolicy.Validate(currency);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            await _currencyCommandsRepository.Add(currency, cancellationToken);
            return currency.Adapt<CurrencyDto>();
        }
    }
}
