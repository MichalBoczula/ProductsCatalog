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
            var validationResult = await validationPolicy.Validate(currency);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }

            currency.Deactivate();
            _currencyCommandsRepository.Update(currency);

            var currenciesHistory = currency.BuildAdapter()
                .AddParameters("operation", Operation.Deleted)
                .AdaptToType<CurrenciesHistory>();

            _currencyCommandsRepository.WriteHistory(currenciesHistory);
            
            await _currencyCommandsRepository.SaveChanges(cancellationToken);
            return currency.Adapt<CurrencyDto>();
        }
    }
}
