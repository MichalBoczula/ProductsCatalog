using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = "One or more validation errors occurred.",
                Extensions =
                {
                    ["errors"] = validationException.ValidationResult.GetValidateErrors(),
                    ["traceId"] = context.TraceIdentifier

                }
            }, cancellationToken);

            return true;
        }
    }
}
