using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration.Extensions
{
    public static class ValidationExceptionHandlerExtension
    {
        public static async Task HandleValidationException(this HttpContext context, ValidationException validationException, CancellationToken cancellationToken)
        {
            await ApiProblemResponse.WriteAsync(
                context,
                StatusCodes.Status400BadRequest,
                "validation_failed",
                "Validation failed",
                "One or more validation errors occurred.",
                cancellationToken,
                errors: validationException.ValidationResult.GetValidatonErrors());
        }
    }
}
