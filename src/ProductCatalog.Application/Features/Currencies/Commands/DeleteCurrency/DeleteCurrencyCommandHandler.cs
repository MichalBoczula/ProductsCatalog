using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Currencies.Commands.DeleteCurrency
{
    public sealed record DeleteCurrencyCommandHandler
        (ICurrenciesCommandsRepository _currencyCommandsRepository,
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
            _currencyCommandsRepository.Update(currency);

            var currenciesHistory = new CurrenciesHistory
            {
                CurrencyId = currency.Id,
                Code = currency.Code,
                Description = currency.Description,
                IsActive = currency.IsActive,
                ChangedAt = currency.ChangedAt,
                Operation = Operation.Deleted
            };

            _currencyCommandsRepository.WriteHistory(currenciesHistory);
            
            await _currencyCommandsRepository.SaveChanges(cancellationToken);
            return currency.Adapt<CurrencyDto>();
        }
    }
}
