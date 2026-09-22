using ProductCatalog.Domain.Common.Pagination;
using ProductCatalog.Domain.Validation.Abstract;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Domain.Validation.Concrete.Rules.Common
{
    public sealed class PaginationParametersValidationRule : IValidationRule<PaginationParameters>
    {
        private readonly List<ValidationError> _errors =
        [
            new() { Name = nameof(PaginationParametersValidationRule), Entity = nameof(PaginationParameters.PageNumber), Message = "Page number must be greater than zero." },
            new() { Name = nameof(PaginationParametersValidationRule), Entity = nameof(PaginationParameters.PageSize), Message = "Page size must be greater than zero." }
        ];

        public Task IsValid(PaginationParameters pagination, ValidationResult validationResult)
        {
            if (pagination.PageNumber <= 0) validationResult.AddValidationError(_errors[0]);
            if (pagination.PageSize <= 0) validationResult.AddValidationError(_errors[1]);
            return Task.CompletedTask;
        }

        public List<ValidationError> Describe() => _errors;
    }
}
