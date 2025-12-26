using Mapster;
using MediatR;
using ProductCatalog.Application.Common.Dtos.Currencies;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.History;
using ProductCatalog.Domain.AggregatesModel.CurrencyAggregate.Repositories;
using ProductCatalog.Domain.Common.Enums;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Application.Features.Currencies.Commands.CreateCurrency
{
    internal sealed class CreateCurrencyCommandHandler
        (ICurrenciesCommandsRepository _currencyCommandsRepository,
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

            _currencyCommandsRepository.Add(currency);

            var currenciesHistory = new CurrenciesHistory
            {
                CurrencyId = currency.Id,
                Code = currency.Code,
                Description = currency.Description,
                IsActive = currency.IsActive,
                ChangedAt = currency.ChangedAt,
                Operation = Operation.Inserted
            };

            _currencyCommandsRepository.WriteHistory(currenciesHistory);

            await _currencyCommandsRepository.SaveChanges(cancellationToken);
            return currency.Adapt<CurrencyDto>();
        }
    }
}
