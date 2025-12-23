using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration.Extensions
{
    internal static class ValidationExceptionHandlerExtension
    {
        public static async Task HandleValidationException(this HttpContext context, ValidationException validationException, CancellationToken cancellationToken)
        {
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
        }
    }
}
