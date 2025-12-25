using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency
{
    public sealed record DeleteCurrencyCommandHandler
        (ICurrencyCommandsRepository _currencyCommandsRepository,
         IValidationPolicy<Currency> validationPolicy)
        : IRequestHandler<DeleteCurrencyCommand, CurrencyDto>
    {
        public async Task<CurrencyDto> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await _currencyCommandsRepository.GetCurrencyById(request.currencyId, cancellationToken);
            var validationResult = validationPolicy.Validate(currency);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
            currency.Deactivate();
            await _currencyCommandsRepository.Update(currency, cancellationToken);
            return currency.Adapt<CurrencyDto>();
        }
    }
}
