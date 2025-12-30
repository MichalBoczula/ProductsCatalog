using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration.Extensions
{
    public static class NotFoundExceptionHandlerExtension
    {
        public static async Task HandleNotFoundException(this HttpContext context, ResourceNotFoundException exception, CancellationToken cancellationToken)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new NotFoundProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found.",
                Detail = $"Resource {exception.ResourceType} identify by id {exception.ResourceId} cannot be found in databese during action {exception.ActionName}.",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Instance = context.Request.Path,
                Extensions =
                {
                    ["traceId"] = context.TraceIdentifier
                }
            }, cancellationToken);
        }
    }
}