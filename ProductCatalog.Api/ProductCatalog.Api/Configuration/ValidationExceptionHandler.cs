using Microsoft.AspNetCore.Diagnostics;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration
{
    public sealed class ValidationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not ValidationException validationException)
            {
                return false;
            }

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Errors = validationException.ValidationResult.GetValidateErrors()
            }, cancellationToken);

            return true;
        }
    }
}
