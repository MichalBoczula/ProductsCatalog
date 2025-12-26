using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration.Extensions
{
    internal static class ValidationExceptionHandlerExtension
    {
        public static async Task HandleValidationException(this HttpContext context, ValidationException validationException, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(new ApiProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = "One or more validation errors occurred.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Instance = context.Request.Path,
                Extensions =
                {
                    ["errors"] = validationException.ValidationResult.GetValidatonErrors(),
                    ["traceId"] = context.TraceIdentifier

                }
            }, cancellationToken);
        }
    }
}
