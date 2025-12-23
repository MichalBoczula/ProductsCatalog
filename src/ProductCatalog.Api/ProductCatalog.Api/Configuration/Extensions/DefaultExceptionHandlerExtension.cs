using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Configuration.Extensions
{
    internal static class DefaultExceptionHandlerExtension
    {
        public static async Task HandleDefaultException(this HttpContext context, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error",
                Detail = "An unexpected error occurred.",
                Extensions =
                {
                    ["traceId"] = context.TraceIdentifier
                }
            }, cancellationToken);
        }
    }
}
