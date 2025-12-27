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
            var validationResult = await _validationPolicy.Validate(currency);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            _currencyCommandsRepository.Add(currency);

            var currenciesHistory = currency.BuildAdapter()
                .AddParameters("operation", Operation.Inserted)
                .AdaptToType<CurrenciesHistory>();

            _currencyCommandsRepository.WriteHistory(currenciesHistory);

            await _currencyCommandsRepository.SaveChanges(cancellationToken);
            return currency.Adapt<CurrencyDto>();
        }
    }
}
