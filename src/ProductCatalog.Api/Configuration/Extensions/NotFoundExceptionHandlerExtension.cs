using ProductCatalog.Api.Configuration.Common;
using ProductCatalog.Domain.Validation.Common;

namespace ProductCatalog.Api.Configuration.Extensions
{
    public static class NotFoundExceptionHandlerExtension
    {
        public static async Task HandleNotFoundException(this HttpContext context, ResourceNotFoundException exception, CancellationToken cancellationToken)
        {
            var resourceIds = string.Join(", ", exception.ResourceIds);
            var detail = exception.ResourceIds.Count == 1
                ? $"Resource {exception.ResourceType} identify by id {resourceIds} cannot be found in databese during action {exception.ActionName}."
                : $"Resource {exception.ResourceType} identified by id(s) {resourceIds} cannot be found in database during action {exception.ActionName}.";

            await ApiProblemResponse.WriteAsync(
                context,
                StatusCodes.Status404NotFound,
                "resource_not_found",
                "Resource not found.",
                detail,
                cancellationToken);
        }
    }
}
